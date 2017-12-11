using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:树形菜单控制
*Author:作者
*
*/
namespace Assets.Scripts.Logic.UI.CommonUI
{
    public class MyTreeMenu : UIbase
    {
        private Dictionary<string, MyTreeMenuItem> parents;
        private Dictionary<string, MyTreeMenuItem> leafNodes;
        private Dictionary<string, MyTreeMenuItem> nodes;
        private Dictionary<string, MyTreeMenuItem> rootNodes;
        private MyTreeMenuItem currentItem;
        private MyTreeMenuItem currentLeafItem;

        public UIGrid treeGrid;
        public UIScrollView scrollView;
        public UIScrollBar scrollBar;
        public UIButton topBtn;
        public UIButton bottomBtn;

        private Action<string, bool> leafNodesClickEvent;
        private bool isParent = false;
        //private string[] treeItemFold = { "ui_synthesis_button_djhcwpxx_normal_01", "ui_synthesis_button_djhcwpxx_normal_02", "ui_synthesis_button_djhcwpxx_normal_03" };
        //private string[] treeItemNoFold = { "ui_synthesis_button_djhcwpxx_pressed_01", "ui_synthesis_button_djhcwpxx_pressed_02", "ui_synthesis_button_djhcwpxx_pressed_03" };

        private string[] myTreeItemFold = { "A_menu_right", "B_menu_right", "C_menu_right", "C_menu_right", "D_menu_right" };
        private string[] myTreeItemNoFold = { "A_menu_down", "B_menu_down", "C_menu_down", "C_menu_down", "D_menu_down" };
        private string[] myTreeItemChildNode = { "A_menu", "B_menu", "C_menu", "C_menu", "D_menu", "F_menu" };
        private string[] myTreeItemChildNodeSelect = { "A_menu_Selected", "B_menu_Selected", "C_menu_Selected", "C_menu_Selected", "D_menu_Selected" };
		private Color [] myTreeItemTextColor = { Color.white, Color.white, Color.white, Color.green, Color.white };
		private int[] myTreeItemFontSize = { 26, 23, 21, 19, 17 };
        public override int Depth
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        void Start()
        {
			//UIEventListener.Get(topBtn.gameObject).onClick = scrollTop;
			// UIEventListener.Get(bottomBtn.gameObject).onClick = scrollBottom;
			//init();
        }

        /// <summary>
        /// 初始化树形菜单，树形节点层级数量，所有父节点包含数据信息，子节点信息含义
        /// </summary>
        public void init()
        {
            parents = new Dictionary<string, MyTreeMenuItem>();
            leafNodes = new Dictionary<string, MyTreeMenuItem>();
            nodes = new Dictionary<string, MyTreeMenuItem>();
            rootNodes = new Dictionary<string, MyTreeMenuItem>();
			//if (treeGrid.transform.childCount!=0)
			//	treeGrid.transform.localPosition = new Vector2(treeGrid.transform.localPosition.x, treeGrid.transform.localPosition.y + 50);
			clearChildren(treeGrid.transform);
            treeGrid.onCustomSort = treeSort;
            treeGrid.sorting = UIGrid.Sorting.Custom;
            treeGrid.animateSmoothly = true;

            EventDelegate.Add(scrollBar.onChange, scrollBarChange);
            scrollView.UpdateScrollbars(false);
			//临时创建列表
			//createItem();
			treeGrid.repositionNow = true;
			treeGrid.Reposition();
		}

        private void setValue(List<MyTreeMenuItemVO> menuItemVO, MyTreeMenuItem parent, int level, string firstStr)
        {
            List<MyTreeMenuItem> items = new List<MyTreeMenuItem>();
            foreach (MyTreeMenuItemVO item in menuItemVO)
            {
                GameObject obj = getPrefab("MyUI/TreeMenuItem");
                GameObject menuItem = generate(treeGrid.transform, obj, Vector3.one, Vector3.zero, Vector3.zero);
                string name = "";
                if (firstStr != null)
                    name = firstStr + "_" + item.ItemName;
                else name = item.ItemName;
                menuItem.name = name;
                MyTreeMenuItem treeMenuItem = menuItem.GetComponent<MyTreeMenuItem>();
                treeMenuItem.setValue(item, parent);
                items.Add(treeMenuItem);
                menuItem.GetComponent<MyUIDragScrollView>().scrollView = scrollView;//不太懂
                if (parent != null)
                    parent.addNode(treeMenuItem);
                else rootNodes.Add(item.value, treeMenuItem);
                nodes.Add(item.value, treeMenuItem);
                if (item.treeMenuItemVO != null && item.treeMenuItemVO.Count > 0)//双重保险吗？
                {
                    treeMenuItem.sprite.spriteName = myTreeItemFold[level];//取。。。
                    treeMenuItem.setIsLeafNode(false, level);
                    treeMenuItem.lable.fontSize = myTreeItemFontSize[level];
					treeMenuItem.lable.color = myTreeItemTextColor[level];
					parents.Add(name, treeMenuItem);
                    setValue(item.treeMenuItemVO, treeMenuItem, level + 1, name);//递归
                }
                else
                {
                    treeMenuItem.sprite.spriteName = myTreeItemChildNode[level];
                    treeMenuItem.setIsLeafNode(true, level);
                    treeMenuItem.lable.fontSize = myTreeItemFontSize[level];
                    leafNodes.Add(name, treeMenuItem);
                }
            }
            foreach (MyTreeMenuItem item in items)
            {
                item.allFoldNode();
            }
            treeGrid.repositionNow = true;
            treeGrid.Reposition();
        }

        /// <summary>
        /// 初始化属性菜单，选择设置子菜单处理函数，以及父级菜单点击是否会被触发事件
        /// </summary>
        /// <param name="menuItemVo"></param>
        /// <param name="leafNodesClickEvent"></param>
        /// <param name="isParent"></param>
        public void setValue(List<MyTreeMenuItemVO> menuItemVo, Action<string, bool> leafNodesClickEvent = null, bool isParent = false)
        {
            if (leafNodesClickEvent != null)
                this.leafNodesClickEvent = leafNodesClickEvent;
            this.isParent = isParent;
            setValue(menuItemVo, null, 0, null);
            treeGrid.repositionNow = true;
            treeGrid.Reposition();

            scrollBarChange();
            scrollView.UpdateScrollbars(true);
        }

        private void createItem()
        {
            List<MyTreeMenuItemVO> vo = new List<MyTreeMenuItemVO>();
            for (int i = 0; i < 4; i++)
            {
                MyTreeMenuItemVO itemi = new MyTreeMenuItemVO();
                itemi.ItemName = "songyu" + i;
                itemi.value = itemi.ItemName;
                itemi.name = "宋玉0";
                vo.Add(itemi);
                List<MyTreeMenuItemVO> treeis = new List<MyTreeMenuItemVO>();
                itemi.treeMenuItemVO = treeis;
                for (int j = 0; j < 4; j++)
                {
                    MyTreeMenuItemVO itemj = new MyTreeMenuItemVO();
                    itemj.ItemName = "caidan" + j;
                    itemj.value = itemi.value + "_" + itemj.ItemName;
                    itemj.name = "宋玉1";
                    treeis.Add(itemj);
                    List<MyTreeMenuItemVO> treejs = new List<MyTreeMenuItemVO>();
                    itemj.treeMenuItemVO = treejs;
                    for (int k = 0; k < 4; k++)
                    {
                        MyTreeMenuItemVO itemk = new MyTreeMenuItemVO();
                        itemk.ItemName = "yu" + k;
                        itemk.value = itemj.value + "_" + itemk.ItemName;
                        itemk.name = "宋玉2";
                        treejs.Add(itemk);
                        List<MyTreeMenuItemVO> treeks = new List<MyTreeMenuItemVO>();
                        itemk.treeMenuItemVO = treeks;
                        for (int l = 0; l < 4; l++)
                        {
                            MyTreeMenuItemVO treek = new MyTreeMenuItemVO();
                            treek.ItemName = "yezi" + l;
                            treek.value = itemk.value + "_" + treek.ItemName;
                            treek.name = "宋玉3";
                            treeks.Add(treek);
                        }
                    }
                }
            }

            setValue(vo, null, true);
        }

        /// <summary>
        /// 菜单某个节点被点击时
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <param name="isTriggerEvent">是否出发点击事件</param>
        /// <param name="isFoldMenu">是否折叠被点击的非叶子节点菜单</param>
        public void menuClick(GameObject obj, bool isMenu, bool isTriggerEvent = true, bool isFoldMenu = true)
        {
            this.currentItem = obj.GetComponent<MyTreeMenuItem>();
			string value = currentItem.getItemVO().value;
            if (currentItem.getIsLeafNode())
            {
                //Debug.Log(obj.name);

                if (leafNodesClickEvent != null && isTriggerEvent)
                {
                    leafNodesClickEvent(value, isMenu);

                }
                if (currentLeafItem != null)
                {
                    currentLeafItem.sprite.spriteName = myTreeItemChildNode[currentLeafItem.getNodeLevel()];
                }

                currentLeafItem = obj.GetComponent<MyTreeMenuItem>();
                currentLeafItem.sprite.spriteName = myTreeItemChildNodeSelect[currentLeafItem.getNodeLevel()];

                if (currentItem.getParent() != null)
                {
                    currentItem.getParent().foldPartChildMenuNode(currentItem);
                }

                else
                {
                    foreach (MyTreeMenuItem item in rootNodes.Values)
                    {
                        if (item != currentItem)
                            item.foldMenuNode(true);
                    }
                }
            }
            else if (isParent)
            {
                if (isFoldMenu)
                {
                    currentItem.foldMenuNode();
                    if (currentItem.getParent() != null)
                        currentItem.getParent().foldPartChildMenuNode(currentItem);
                    else
                    {
                        foreach (MyTreeMenuItem item in rootNodes.Values)
                        {
                            if (item != currentItem)
                                item.foldMenuNode(true);
                            //currentItem.sprite.spriteName = myTreeItemNoFold[currentItem.getNodeLevel()];//添加的
                        }
                    }
                }
                if (currentLeafItem != null)
                    currentLeafItem.sprite.spriteName = myTreeItemChildNode[currentLeafItem.getNodeLevel()];

                if (leafNodesClickEvent != null && isTriggerEvent)
                    leafNodesClickEvent(value, isMenu);

            }
        }

        public void scrollBarChange()
        {
            //if (scrollView.movement == UIScrollView.Movement.Horizontal)
            //{
            //    if (scrollView.shouldMoveHorizontally)
            //        scrollView.UpdateScrollbars(true);
            //}
            //else if (scrollView.movement == UIScrollView.Movement.Vertical)
            //    if (scrollView.shouldMoveVertically)
            //        scrollView.UpdateScrollbars(true);
            if (scrollView.panel.GetViewSize().y > scrollView.bounds.size.y)
            {
                scrollBar.value = 0;
            }
            if (scrollBar.alpha == 1)
            {
                topBtn.GetComponent<UISprite>().alpha = 1;
                topBtn.GetComponent<BoxCollider>().enabled = true;
                bottomBtn.GetComponent<UISprite>().alpha = 1;
                bottomBtn.GetComponent<BoxCollider>().enabled = true;
            }
            else
            {
                topBtn.GetComponent<UISprite>().alpha = 0;
                topBtn.GetComponent<BoxCollider>().enabled = false;
                bottomBtn.GetComponent<UISprite>().alpha = 0;
                bottomBtn.GetComponent<BoxCollider>().enabled = false;
            }

        }

        /// <summary>
        /// 进度条定位到指定节点位置
        /// </summary>
        /// <param name="item"></param>
        public void Locate(string value, bool isMenu, bool isFoldMenu = true)
        {
            MyTreeMenuItem clickItem = nodes[value];
            currentItem = clickItem;
            List<MyTreeMenuItem> items = clickItem.getParents();
            if (!isFoldMenu)
                clickItem.foldAllChildMenuNode(true);
            //foreach (MyTreeMenuItem item in nodes.Values)
            //{
            //    if(!items.Contains(item))
            //        item.allFoldNode();
            //}
            //if (items.Count > 0)
            //{
            //    for (int i = items.Count - 1; i >= 0; i--)
            //    {
            //        items[i].foldMenuNode(false);
            //    }
            //}
            menuClick(clickItem.gameObject, isMenu, true, isFoldMenu);
            //menuClick(clickItem.gameObject, value, false);
            //clickItem.foldMenuNode(false);
            int count = treeGrid.GetChildList().Count;
            float progress = (float)treeGrid.GetIndex(currentItem.transform) / count;
            scrollBar.value = progress;

            scrollBarChange();
            treeGrid.repositionNow = true;
            treeGrid.Reposition();
        }

        public void allFold()
        {
            //foreach (MyTreeMenuItem item in items)
            //{
            //    item.allFoldNode();
            //}
            this.currentItem.foldMenuNode(true);

            scrollBarChange();
            treeGrid.repositionNow = true;
            treeGrid.Reposition();
        }

        public MyTreeMenuItem getItem(string value)
        {
            if (nodes.ContainsKey(value))
                return nodes[value];
            else return null;
        }

        public void scrollTop(GameObject obj)
        {
            float size = scrollBar.barSize;
            scrollBar.value -= size / 10;
        }

        public void scrollBottom(GameObject obj)
        {
            float size = scrollBar.barSize;
            scrollBar.value += size / 10;
        }

        public string getTreeItemSprite(bool isLeafNode, int level, bool State)
        {
            if (!isLeafNode)
            {
                if (State)
                    return myTreeItemFold[level];
                else return myTreeItemNoFold[level];
            }
            else
            {
                if (State)
                    return myTreeItemChildNodeSelect[level];
                else return myTreeItemChildNode[level];
            }
        }
	//	private Color[] foldFlowMenuColor = new Color[]
	//{
	//		new Color(182f / 255, 254f / 255, 214f / 255, 1),
	//		new Color(255f / 255, 216f / 255, 51f / 255, 1),
	//		new Color(165f / 255, 213f / 255, 249f / 255, 1),
	//};
		public void setCurrentFlow()
		{
			currentItem.lable.color = new Color(28f/255, 28f/ 255, 28f/ 255, 255f/255);
		}
		public void resetCurrentFlow()
		{
			foreach (var item in parents)
			{
				if (item.Value.getNodeLevel()==1)
					item.Value.lable.color = new Color(255f / 255, 255f / 255, 255f / 255, 255f / 255);
			}
			foreach (var item in leafNodes)
			{
				if (item.Value.getNodeLevel() == 1)
					item.Value.lable.color = new Color(255f / 255, 255f / 255, 255f / 255, 255f / 255);
			}
		}
		#region 菜单排序算法
		public int treeSort(Transform item1, Transform item2)
        {
            string[] item1s = item1.name.Split('_');
            string[] item2s = item2.name.Split('_');
            return itemNameEquals(item1s, item2s);
        }

        private int itemNameEquals(string[] item1, string[] item2)
        {
            int length = 0;
            if (item1.Length <= item2.Length)
                length = item1.Length;
            else length = item2.Length;
            for (int i = 0; i < length; i++)
            {
                int result = string.Compare(item1[i], item2[i]);
                if (result != 0)
                    return result;
            }
            if (item1.Length > item2.Length)
                return 1;
            else return -1;
        }
        #endregion

    }
}

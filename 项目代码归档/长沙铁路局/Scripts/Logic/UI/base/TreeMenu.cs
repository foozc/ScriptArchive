using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace Assets.Scripts.Logic.UI
{
    public class TreeMenu : UIbase
    {
        private Dictionary<string, TreeMenuItem> parents;
        private Dictionary<string, TreeMenuItem> leafNodes;
        private Dictionary<string, TreeMenuItem> nodes;
        private Dictionary<string, TreeMenuItem> rootNodes;
        private TreeMenuItem currentItem;
        private TreeMenuItem currentLeafItem;

        public UIGrid treeGrid;
        public UIScrollView scrollView;
        public UIScrollBar scrollBar;
        public UIButton topBtn;
        public UIButton bottomBtn;

        private Action<string> leafNodesClickEvent;
        private bool isParent = false;
        private string[] treeItemFold = { "ui_synthesis_button_djhcwpxx_normal_01", "ui_synthesis_button_djhcwpxx_normal_02", "ui_synthesis_button_djhcwpxx_normal_03" };
        private string[] treeItemNoFold = { "ui_synthesis_button_djhcwpxx_pressed_01", "ui_synthesis_button_djhcwpxx_pressed_02", "ui_synthesis_button_djhcwpxx_pressed_03" };

        private string[] myTreeItemFold = { "A_menu_right", "B_menu_right", "C_menu_right" };
        private string[] myTreeItemNoFold = { "A_menu_down", "B_menu_down", "C_menu_down" };
        private string[] myTreeItemChildNode = { "A_menu", "B_menu", "C_menu", "D_menu" };
        private string[] myTreeItemChildNodeSelect = { "A_menu_Selected", "B_menu_Selected", "C_menu_Selected", "D_menu_Selected" };
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
            UIEventListener.Get(topBtn.gameObject).onClick = scrollTop;
            UIEventListener.Get(bottomBtn.gameObject).onClick = scrollBottom;

            //init();

        }

        /// <summary>
        /// 初始化树形菜单，树形节点层级数量，所有父节点包含数据信息，子节点信息含义
        /// </summary>
        public void init()
        {
            parents = new Dictionary<string, TreeMenuItem>();
            leafNodes = new Dictionary<string, TreeMenuItem>();
            nodes = new Dictionary<string, TreeMenuItem>();
            rootNodes = new Dictionary<string, TreeMenuItem>();
            clearChildren(treeGrid.transform);

            treeGrid.onCustomSort = treeSort;
            treeGrid.sorting = UIGrid.Sorting.Custom;
            treeGrid.animateSmoothly = true;

            EventDelegate.Add(scrollBar.onChange, scrollBarChange);
            scrollView.UpdateScrollbars(false);
            //临时创建列表
            //createItem();
        }

        private void setValue(List<TreeMenuItemVO> menuItemVO, TreeMenuItem parent, int level, string firstStr)
        {
            List<TreeMenuItem> items = new List<TreeMenuItem>();
            foreach (TreeMenuItemVO item in menuItemVO)
            {
                GameObject obj = getPrefab("TreeMenuItem");
                GameObject menuItem = generate(treeGrid.transform, obj, Vector3.one, Vector3.zero, Vector3.zero);
                string name = "";
                if (firstStr != null)
                    name = firstStr + "_" + item.ItemName;
                else name = item.ItemName;
                menuItem.name = name;
                TreeMenuItem treeMenuItem = menuItem.GetComponent<TreeMenuItem>();
                treeMenuItem.setValue(item, parent);
                items.Add(treeMenuItem);
                menuItem.GetComponent<UIDragScrollView>().scrollView = scrollView;
                if (parent != null)
                    parent.addNode(treeMenuItem);
                else rootNodes.Add(item.value, treeMenuItem);
                nodes.Add(item.value, treeMenuItem);
                if (item.treeMenuItemVO != null && item.treeMenuItemVO.Count > 0)
                {
                    treeMenuItem.sprite.spriteName = treeItemFold[level];
                    treeMenuItem.setIsLeafNode(false, level);
                    parents.Add(name, treeMenuItem);
                    setValue(item.treeMenuItemVO, treeMenuItem, level + 1, name);
                }
                else
                {
                    treeMenuItem.sprite.spriteName = treeItemFold[2];
                    treeMenuItem.setIsLeafNode(true, level);
                    leafNodes.Add(name, treeMenuItem);
                }
            }
            foreach (TreeMenuItem item in items)
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
        public void setValue(List<TreeMenuItemVO> menuItemVo, Action<string> leafNodesClickEvent = null, bool isParent = false)
        {
            if (leafNodesClickEvent != null)
                this.leafNodesClickEvent = leafNodesClickEvent;
            this.isParent = isParent;
            setValue(menuItemVo, null, 0, null);
            scrollBarChange();
        }

        

        private void createItem()
        {
            List<TreeMenuItemVO> vo = new List<TreeMenuItemVO>();
            for (int i = 0; i < 4; i++)
            {
                TreeMenuItemVO itemi = new TreeMenuItemVO();
                itemi.ItemName = "songyu" + i;
                itemi.name = "宋玉0";
                vo.Add(itemi);
                List<TreeMenuItemVO> treeis = new List<TreeMenuItemVO>();
                itemi.treeMenuItemVO = treeis;
                for (int j = 0; j < 4; j++)
                {
                    TreeMenuItemVO itemj = new TreeMenuItemVO();
                    itemj.ItemName = "caidan" + j;
                    itemj.name = "宋玉1";
                    treeis.Add(itemj);
                    List<TreeMenuItemVO> treejs = new List<TreeMenuItemVO>();
                    itemj.treeMenuItemVO = treejs;
                    for (int k = 0; k < 4; k++)
                    {
                        TreeMenuItemVO itemk = new TreeMenuItemVO();
                        itemk.ItemName = "yezi" + k;
                        itemk.name = "宋玉2";
                        treejs.Add(itemk);
                    }
                }
            }
            setValue(vo);
        }

        /// <summary>
        /// 菜单某个节点被点击时
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <param name="isTriggerEvent">是否出发点击事件</param>
        /// <param name="isFoldMenu">是否折叠被点击的非叶子节点菜单</param>
        public void menuClick(GameObject obj, bool isTriggerEvent = true, bool isFoldMenu = true)
        {
            this.currentItem = obj.GetComponent<TreeMenuItem>();
            string value = currentItem.getItemVO().value;
            if (currentItem.getIsLeafNode())
            {
                Debug.Log(obj.name);
                if (leafNodesClickEvent != null && isTriggerEvent)
                {
                    leafNodesClickEvent(value);
                }
                if (currentLeafItem != null)
                    currentLeafItem.sprite.spriteName = treeItemFold[2];
                currentLeafItem = obj.GetComponent<TreeMenuItem>();
                currentLeafItem.sprite.spriteName = treeItemNoFold[2];

                if (currentItem.getParent() != null)
                    currentItem.getParent().foldPartChildMenuNode(currentItem);
                else
                {
                    foreach (TreeMenuItem item in rootNodes.Values)
                    {
                        if (item != currentItem)
                            item.foldMenuNode(true);
                    }
                }
            }
            else if(isParent)
            {
                Debug.Log(obj.name);
                if (isFoldMenu)
                {
                    currentItem.foldMenuNode();
                    if(currentItem.getParent() != null)
                        currentItem.getParent().foldPartChildMenuNode(currentItem);
                    else
                    {
                        foreach (TreeMenuItem item in rootNodes.Values)
                        {
                            if (item != currentItem)
                                item.foldMenuNode(true);
                        }
                    }
                }
                if (currentLeafItem != null)
                    currentLeafItem.sprite.spriteName = treeItemFold[2];


                if (leafNodesClickEvent != null && isTriggerEvent)
                    leafNodesClickEvent(value);
            }
        }

        public void scrollBarChange()
        {
            //if (scrollView.movement == UIScrollView.Movement.Horizontal)
            //{
            //    if (scrollView.shouldMoveHorizontally)
            //        scrollView.UpdateScrollbars(false);
            //}
            //else if(scrollView.movement == UIScrollView.Movement.Vertical)
            //    if (scrollView.shouldMoveVertically)
            //        scrollView.UpdateScrollbars(false);
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
        public void Locate(string value, bool isFoldMenu = true)
        {
            TreeMenuItem clickItem = nodes[value];
            currentItem = clickItem;
            List<TreeMenuItem> items = clickItem.getParents();
            if(!isFoldMenu)
                clickItem.foldAllChildMenuNode(true);
            //foreach (TreeMenuItem item in nodes.Values)
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
            menuClick(clickItem.gameObject, true, isFoldMenu);
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
            //foreach (TreeMenuItem item in items)
            //{
            //    item.allFoldNode();
            //}
            this.currentItem.foldMenuNode();

            scrollBarChange();
            treeGrid.repositionNow = true;
            treeGrid.Reposition();
        }

        public TreeMenuItem getItem(string value)
        {
            return nodes[value];
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
                    return treeItemFold[level];
                else return treeItemNoFold[level];
            }
            else
            {
                if(State)
                    return treeItemFold[2];
                else return treeItemNoFold[2];
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

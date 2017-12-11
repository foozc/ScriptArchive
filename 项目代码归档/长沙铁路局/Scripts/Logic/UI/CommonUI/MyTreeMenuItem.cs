using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:树形菜单单元按钮
*Author:作者
*
*/
namespace Assets.Scripts.Logic.UI.CommonUI
{
    public class MyTreeMenuItemVO
    {
        /// <summary>
        /// 子菜单集合，没有则为null
        /// </summary>
        public List<MyTreeMenuItemVO> treeMenuItemVO;
        /// <summary>
        /// 用于树形菜单gameobject的name依据
        /// </summary>
        public string ItemName;
        /// <summary>
        /// 树形菜单显示的信息
        /// </summary>
        public string name;
        /// <summary>
        /// 节点的值
        /// </summary>
        public string value;
    }
    public class MyTreeMenuItem : MonoBehaviour
    {
        private bool isLeafNode = true;
        private List<MyTreeMenuItem> Nodes;
        private bool isFold = false;
        private MyTreeMenu treeMenu = null;
        private int nodeLevel = 0;
        private Dictionary<MyTreeMenuItem, bool> itemState = null;
        private MyTreeMenuItemVO value;
        private MyTreeMenuItem parent;


		public UILabel lable;
        public UISprite sprite;

        void Awake()
        {

        }

        void Start()
        {
            //Nodes = new List<MyTreeMenuItem>();
            treeMenu = NGUITools.FindInParents<MyTreeMenu>(gameObject);
        }

        public void removeNode(MyTreeMenuItem item)
        {
            if (Nodes == null)
                Nodes = new List<MyTreeMenuItem>();
            if (Nodes.Contains(item))
                Nodes.Remove(item);
        }

        public void addNode(MyTreeMenuItem item)
        {
            if (Nodes == null)
            {
                Nodes = new List<MyTreeMenuItem>();
                itemState = new Dictionary<MyTreeMenuItem, bool>();
            }
            if (Nodes.Contains(item))
                Debug.LogWarning("已经存在该菜单节点");
            else
            {
                Nodes.Add(item);
                itemState.Add(item, false);
            }
        }

        /// <summary>
        /// 设置标签上显示的文字
        /// </summary>
        /// <param name="vo"></param>
        public void setValue(MyTreeMenuItemVO vo, MyTreeMenuItem parent)
        {
            treeMenu = NGUITools.FindInParents<MyTreeMenu>(gameObject);
            this.parent = parent;
            this.value = vo;
            lable.text = vo.name;
		}
        public void setIsLeafNode(bool isLeafNode, int nodeLevel)
        {
            this.isLeafNode = isLeafNode;
            this.nodeLevel = nodeLevel;
		}
        public void allFoldNode()
        {
            if (Nodes != null && Nodes.Count > 0)
            {
                this.isFold = true;
                foreach (MyTreeMenuItem item in Nodes)
                {
                    if (item.Nodes != null && item.Nodes.Count > 0)
                    {
                        item.allFoldNode();
                        item.setState(true);
                    }
                    item.gameObject.SetActive(false);
                }
            }
        }



        public void setState(bool isFold)
        {
            if (treeMenu == null)
                treeMenu = NGUITools.FindInParents<MyTreeMenu>(gameObject);
            sprite.spriteName = treeMenu.getTreeItemSprite(false, nodeLevel, isFold);
            this.isFold = isFold;
        }

        public List<MyTreeMenuItem> getParents()
        {
            List<MyTreeMenuItem> items = new List<MyTreeMenuItem>();
            if (parent != null)
            {
                items.Add(parent);
                MyTreeMenuItem item = null;
                while (true)
                {
                    if (item == null)
                        item = parent.getParent();
                    else
                        item = item.getParent();
                    if (item == null)
                        break;
                    items.Add(item);
                }
            }

            return items;
        }
        public MyTreeMenuItem getParent()
        {
            return parent;
        }

        public MyTreeMenuItemVO getItemVO()
        {
            return value;
        }

        public bool getIsLeafNode()
        {
            return isLeafNode;
        }

        /// <summary>
        /// 获取节点级别
        /// </summary>
        /// <returns></returns>
        public int getNodeLevel()
        {
            return nodeLevel;
        }

        public void setChildState(bool isShow)
        {
            if (isShow)
            {
                foreach (MyTreeMenuItem item in Nodes)
                {
                    if (item.Nodes != null && item.Nodes.Count > 0)
                        item.setChildState(isShow);
                    item.gameObject.SetActive(itemState[item]);
                }
            }
            else
            {
                foreach (MyTreeMenuItem item in Nodes)
                {
                    if (item.Nodes != null && item.Nodes.Count > 0)
                        item.setChildState(isShow);
                    item.gameObject.SetActive(isShow);
                }
            }
        }

        /// <summary>
        /// 折叠菜单，如果当前是折叠的则展开，当前是展开的则折叠
        /// </summary>
        public void foldMenuNode()
        {
            if (isFold)
            {
                foldMenuNode(false);
            }
            else
            {
                foldMenuNode(true);
            }
        }
        /// <summary>
        /// 设置菜单折叠状态
        /// </summary>
        /// <param name="isFold"></param>
        public void foldMenuNode(bool isFold)
        {
            if (!isLeafNode)
            {
                sprite.spriteName = treeMenu.getTreeItemSprite(false, nodeLevel, isFold);
                this.isFold = isFold;
            }
            else
            {
                //sprite.spriteName = treeMenu.getTreeItemSprite(true, nodeLevel, isFold);
            }
            if (Nodes != null)
            foreach (MyTreeMenuItem item in Nodes)
            {
                itemState[item] = !isFold;

                item.gameObject.SetActive(!isFold);
                if (item.Nodes != null && item.Nodes.Count > 0)
                {
                    //item.setChildState(!isFold);
                    item.setState(true);
                    item.setChildState(false);
                }
            }
            UIGrid grid = NGUITools.FindInParents<UIGrid>(gameObject);
            grid.repositionNow = true;
            treeMenu.scrollBarChange();
            NGUITools.FindInParents<UIScrollView>(gameObject).UpdateScrollbars(false);
        }

        /// <summary>
        /// 折叠该节点下的所有子节点
        /// </summary>
        /// <param name="isFold"></param>
        public void foldAllChildMenuNode(bool isFold)
        {
            foreach (MyTreeMenuItem item in Nodes)
            {
                item.foldMenuNode(isFold);
            }
        }
        public void foldPartChildMenuNode(MyTreeMenuItem exclude)
        {
            foreach (MyTreeMenuItem item in Nodes)
            {
                if (item == exclude)
                    continue;
                item.foldMenuNode(true);
            }
        }

       public void OnClick()
        {
            treeMenu.menuClick(gameObject, true);
        }
    }
}

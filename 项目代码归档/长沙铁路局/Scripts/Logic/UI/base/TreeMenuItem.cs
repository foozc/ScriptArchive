using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace Assets.Scripts.Logic.UI
{
    public class TreeMenuItemVO
    {
        /// <summary>
        /// 子菜单集合，没有则为null
        /// </summary>
        public List<TreeMenuItemVO> treeMenuItemVO;
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

    public class TreeMenuItem : MonoBehaviour
    {
        private bool isLeafNode = true;
        private List<TreeMenuItem> Nodes;
        private bool isFold = false;
        private TreeMenu treeMenu = null;
        private int nodeLevel = 0;
        private Dictionary<TreeMenuItem, bool> itemState = null;
        private TreeMenuItemVO value;
        private TreeMenuItem parent;


        public UILabel lable;
        public UISprite sprite;

        void Awake()
        {

        }

        void Start()
        {
            //Nodes = new List<TreeMenuItem>();
            treeMenu = NGUITools.FindInParents<TreeMenu>(gameObject);
        }

        public void removeNode(TreeMenuItem item)
        {
            if (Nodes == null)
                Nodes = new List<TreeMenuItem>();
            if (Nodes.Contains(item))
                Nodes.Remove(item);
        }

        public void addNode(TreeMenuItem item)
        {
            if (Nodes == null)
            {
                Nodes = new List<TreeMenuItem>();
                itemState = new Dictionary<TreeMenuItem, bool>();
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
        public void setValue(TreeMenuItemVO vo, TreeMenuItem parent)
        {
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
                foreach (TreeMenuItem item in Nodes)
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
            if(treeMenu == null)
                treeMenu = NGUITools.FindInParents<TreeMenu>(gameObject);
            sprite.spriteName = treeMenu.getTreeItemSprite(false, nodeLevel, isFold);
            this.isFold = isFold;
        }

        public List<TreeMenuItem> getParents()
        {
            List<TreeMenuItem> items = new List<TreeMenuItem>();
            if (parent != null)
            {
                items.Add(parent);
                while (true)
                {
                    TreeMenuItem item = parent.getParent();
                    if (item == null)
                        break;
                    items.Add(item);
                }
            }

            return items;
        }
        public TreeMenuItem getParent()
        {
            return parent;
        }

        public TreeMenuItemVO getItemVO()
        {
            return value;
        }

        public bool getIsLeafNode()
        {
            return isLeafNode;
        }

        public void setChildState(bool isShow)
        {
            if (isShow)
            {
                foreach (TreeMenuItem item in Nodes)
                {
                    if (item.Nodes != null && item.Nodes.Count > 0)
                        item.setChildState(isShow);
                    item.gameObject.SetActive(itemState[item]);
                }
            }
            else
            {
                foreach (TreeMenuItem item in Nodes)
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
                sprite.spriteName = treeMenu.getTreeItemSprite(true, 2, isFold);
            }
            if (Nodes != null)
                foreach (TreeMenuItem item in Nodes)
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
            foreach (TreeMenuItem item in Nodes)
            {
                item.foldMenuNode(isFold);
            }
        }
        public void foldPartChildMenuNode(TreeMenuItem exclude)
        {
            foreach (TreeMenuItem item in Nodes)
            {
                if (item == exclude)
                    continue;
                item.foldMenuNode(true);
            }
        }

        void OnClick()
        {
            //if (isLeafNode)
            //{
            //    treeMenu.menuClick(gameObject, value.value);
            //}
            //else
            //{
                treeMenu.menuClick(gameObject);
                //foldMenuNode();
            //}
        }
    }
}

using Assets.Scripts.Configs;
using Assets.Scripts.Controller;
using Assets.Scripts.VO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:桌面列表UI
*Author:作者
*
*/
namespace Assets.Scripts.Logic.UI.MainUIMenu
{
    public delegate void TableListItemClick();
    public class TableListUI : UIbase
    {

        public UIGrid treeGrid;
        public UIScrollView scrollView;
        public UIScrollBar scrollBar;
        public UIButton topBtn;
        public UIButton bottomBtn;

        private Dictionary<GameObject, object> itemDic;
        private GameObject currentItemGameObject;
        private TableListItemClick itemEventClick;

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

        public void init(TableListItemClick itemEventClick)
        {
            this.itemEventClick = itemEventClick;
        }

        void Awake()
        {
            itemDic = new Dictionary<GameObject, object>();
        }
        void Start()
        {
            UIEventListener.Get(topBtn.gameObject).onClick = scrollTop;
            UIEventListener.Get(bottomBtn.gameObject).onClick = scrollBottom;

            EventDelegate.Add(scrollBar.onChange, scrollBarChange);

        }

        public void scrollBarChange()
        {
            //scrollView.UpdateScrollbars();
            scrollView.UpdatePosition();
            if (scrollBar.value != 0)
                if (scrollBar.barSize > 0.98)
                    scrollBar.value = 0;
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

        public List<ListItemBase<T>> getCurrrentItemList<T>() where T: ListItemBase<T>
        {
            List<ListItemBase<T>> items = new List<ListItemBase<T>>();
            foreach(object obj in itemDic.Values)
            {
                items.Add((ListItemBase<T>)obj);
            }
            return items;
        }

        public Dictionary<GameObject, E> getCurrrentItemDic<T, E>() where E : ListItemBase<T>
        {
            Dictionary<GameObject, E> items = new Dictionary<GameObject, E>();
            foreach (GameObject obj in itemDic.Keys)
            {
                items.Add(obj, (E)itemDic[obj]);
            }
            return items;
        }

        public E getCurrentItem<T, E>() where E : ListItemBase<T>
        {
            if (currentItemGameObject != null)
                return (E)itemDic[currentItemGameObject];
            return null;
        }
        /// <summary>
        /// 当前选中的item的gameobject
        /// </summary>
        /// <returns></returns>
        public GameObject getCurrentItemGameObject()
        {
            return currentItemGameObject;
        }

        public void deleteCurrentItem()
        {
            itemDic.Remove(currentItemGameObject);
            ResourceManager.getInstance().getUIPool.Despawn(currentItemGameObject.transform);
            treeGrid.repositionNow = true;
            treeGrid.Reposition();
            scrollView.UpdateScrollbars(true);
            SingleTwinSprite(treeGrid.GetChildList());
        }
        
        public GameObject addItem<T>(ListItemBase<T> obj)
        {
            GameObject item = ResourceManager.getInstance().loadUISpawnPool(obj.getPrefab(), treeGrid.gameObject);
            UIEventListener.Get(item).onClick = myOnClick;
            obj.setValue(item);
            treeGrid.repositionNow = true;
            treeGrid.Reposition();
            scrollView.UpdateScrollbars(true);

            itemDic.Add(item, obj);
            SingleTwinSprite<T>(treeGrid.GetChildList());
            return item;
        }

        public void loadAllItem<T>(List<ListItemBase<T>> objs)
        {
            itemDic.Clear();
            
            List<Transform> childNodes = new List<Transform>();
            foreach (Transform tr in treeGrid.transform)
            {
                if (tr.gameObject.activeSelf)
                    childNodes.Add(tr);
            }

            if (objs.Count > childNodes.Count)
            {
                for (int i = 0; i < childNodes.Count; i++)
                {
                    Transform childNode = treeGrid.transform.GetChild(i);
                    objs[i].setValue(childNode.gameObject);
                    UIEventListener.Get(childNode.gameObject).onClick = myOnClick;
                    itemDic.Add(childNode.gameObject, objs[i]);
                }
                for (int i = childNodes.Count; i < objs.Count; i++)
                {
                    GameObject node = ResourceManager.getInstance().loadUISpawnPool(objs[i].getPrefab(), treeGrid.gameObject);
                    UIEventListener.Get(node).onClick = myOnClick;
                    objs[i].setValue(node);
                    itemDic.Add(node, objs[i]);
                }

            }
            else
            {
                for (int i = 0; i < objs.Count; i++)
                {
                    Transform childNode = childNodes[i];
                    objs[i].setValue(childNode.gameObject);
                    UIEventListener.Get(childNode.gameObject).onClick = myOnClick;
                    itemDic.Add(childNode.gameObject, objs[i]);
                }
                for (int i = objs.Count; i < childNodes.Count; i++)
                {
                    ResourceManager.getInstance().getUIPool.Despawn(childNodes[i]);
                }
            }
            SingleTwinSprite<T>(treeGrid.GetChildList());
            treeGrid.repositionNow = true;
            treeGrid.Reposition();
            StartCoroutine(changeBar());
        }

        /// <summary>
        /// 设置单双列表单双背景颜色
        /// </summary>
        private void SingleTwinSprite<T>(List<Transform> objs)
        {
            for (int i = 0; i < objs.Count; i++)
            {
                ListItemBase<T> item = (ListItemBase<T>)itemDic[objs[i].gameObject];
                if (i % 2 == 0)
                    item.setUISprite(getSpriteName(true));
                else
                    item.setUISprite(getSpriteName(false));
            }
        }
        private void SingleTwinSprite(List<Transform> objs)
        {
            for (int i = 0; i < objs.Count; i++)
            {
                UISprite sprite = objs[i].Find("Sprite").GetComponent<UISprite>();
                if (i % 2 == 0)
                    sprite.spriteName = getSpriteName(true);
                else
                    sprite.spriteName = getSpriteName(false);
            }
        }

        private string getSpriteName(bool isSingular)
        {
            switch (MySession.Role)
            {
                case Login.RoleType.adminCheckbox:
                    return UserListItem.getSpriteName(isSingular);
                case Login.RoleType.instructorCheckbox:
                    return ScoreListItem.getSpriteName(isSingular);
                case Login.RoleType.studentCheckbox:
                    return ScoreListItem.getSpriteName(isSingular);
            }
            return null;
        }
        public void loadAllItem1<T>(List<ListItemBase<T>> objs)
        {
            itemDic.Clear();

            List<Transform> childNodes = new List<Transform>();
            foreach (Transform tr in treeGrid.transform)
            {
                if (tr.gameObject.activeSelf)
                    childNodes.Add(tr);
            }

            if (objs.Count > childNodes.Count)
            {
                for (int i = 0; i < childNodes.Count; i++)
                {
                    Transform childNode = treeGrid.transform.GetChild(i);
                    objs[i].setValue(childNode.gameObject);
                    UIEventListener.Get(childNode.gameObject).onClick = myOnClick;
                    itemDic.Add(childNode.gameObject, objs[i]);
                }
                for (int i = childNodes.Count; i < objs.Count; i++)
                {
                    GameObject node = ResourceManager.getInstance().loadUISpawnPool(objs[i].getPrefab(), treeGrid.gameObject);
                    UIEventListener.Get(node).onClick = myOnClick;
                    objs[i].setValue(node);
                    itemDic.Add(node, objs[i]);
                }

            }
            else
            {
                List<Transform> childs = new List<Transform>();
                for (int i = 0; i < objs.Count; i++)
                {
                    Transform childNode = treeGrid.transform.GetChild(i);
                    objs[i].setValue(childNode.gameObject);
                    UIEventListener.Get(childNode.gameObject).onClick = myOnClick;
                    itemDic.Add(childNode.gameObject, objs[i]);
                }
                for (int i = objs.Count; i < childNodes.Count; i++)
                {
                    childs.Add(treeGrid.transform.GetChild(i));
                }
                for (int i = 0; i < childs.Count; i++)
                {
                    ResourceManager.getInstance().getUIPool.Despawn(childs[i]);
                }
            }
            SingleTwinSprite<T>(treeGrid.GetChildList());
            treeGrid.repositionNow = true;
            treeGrid.Reposition();
            StartCoroutine(changeBar());
        }

        IEnumerator changeBar()
        {
            int i = 20;
            while (i > 0)
            {
                i--;
                yield return null;
            }
            scrollView.UpdateScrollbars(true);
            scrollBar.value = 0;
        }

        public void myOnClick(GameObject obj)
        {
            if (currentItemGameObject != null)
                changeItemColor(currentItemGameObject, false);
            currentItemGameObject = obj;
            //改变选择的颜色
            changeItemColor(currentItemGameObject, true);

            if(itemEventClick != null)
                itemEventClick();

        }
        private void changeItemColor(GameObject obj, bool state)
        {
            if (MySession.Role == Login.RoleType.adminCheckbox)
            {
                UIWidget sprite = obj.transform.Find("Sprite").GetComponent<UIWidget>();
                if (state)
                    sprite.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                else sprite.color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Logic.UI
{
    public class FocusWindow:MonoBehaviour
    {
        /// <summary>
        /// 最大窗口叠加数
        /// </summary>
        public int maxOverlayLayer = 4;
        /// <summary>
        /// UIpanel深度的起始值
        /// </summary>
        public int panelStartDepthValue = 100;
        /// <summary>
        /// 存储对所有uipanel的引用
        /// </summary>
        private List<Transform> panels = new List<Transform>();
        /// <summary>
        /// 存储对所有添加到焦点控制器里的window对象的引用
        /// </summary>
        private List<GameObject> windows = new List<GameObject>();

        /// <summary>
        /// 构造单例类
        /// </summary>
        public static FocusWindow Instance = null;

        void Awake()
        {
            Instance = this;
            initPanelsList(maxOverlayLayer);
        }

        /// <summary>
        /// 唯一焦点窗口
        /// </summary>
        private GameObject focusOn = null;
        /// <summary>
        /// 设置唯一焦点窗口
        /// </summary>
        public GameObject FocusOn
        {
            get{return focusOn;}
            set 
            {
                if (value.transform.Find("titleSprite/FocusCollider").GetComponent<Collider>() == null)
                    Debug.LogError("窗口对象的指定位置【titleSprite/FocusCollider】上必须要有与窗口尺寸等大的collider，且处于激活状态！");
                else
                    reOrderWindowList(value);                    
            }
        } 

        /// <summary>
        /// 重置UIPanel的深度值
        /// </summary>
        /// <param name="startDepth"></param>
        public void resetPanelDepth(int startDepth)
        {
            panelStartDepthValue = startDepth;
            for (int i = 0; i < panels.Count; i++)
            {
                panels[i].GetComponent<UIPanel>().depth = panelStartDepthValue + i;
            }
            //重设完uipanel的深度之后刷新一下当前显示/
            if(windows.Count>0)
                reOrderWindowList(windows[windows.Count-1]);
        }

        /// <summary>
        /// 手动移除一个焦点列表中的项目
        /// </summary>
        /// <param name="go">要移除的项目</param>
        public void removeFocusItem(GameObject go)
        {
            if (windows.Contains(go))
            {
                windows.Remove(go);
            }
        }


        /// <summary>
        /// 初始化uipanel的列表,depth由低到高顺序存放的
        /// </summary>
        /// <param name="num"></param>
        private void initPanelsList(int num)
        {
            for (int i = 0; i < num; i++)
            {
                UIPanel panel = NGUITools.AddChild<UIPanel>(gameObject);
                panel.depth = panelStartDepthValue + i;
                panels.Add(panel.transform);
            }
        }

        /// <summary>
        /// 对window列表重新排序，放到depth最高的那个panel里,window对象按setfocus的先后顺序存放的，也就是说最后那个应该处于最高层
        /// </summary>
        /// <param name="go"></param>
        private void reOrderWindowList(GameObject go)
        {
            if (go.GetComponent<FocusWindowItem>() != null)
                Destroy(go.GetComponent<FocusWindowItem>());

            focusOn = go;   //当前处于焦点激活状态的对象/
            int curGoNum = clearInActiveGO(windows);  //先把list清理一下，把已经不显示在界面上的go清理出去/
            if (windows.Contains(go))   //说明是把list中已有的go放到最前面/
            {
                GameObject tempGO = go;
                windows.Remove(go);      //先从中间移除/
                windows.Add(tempGO);     //再添加到list尾部，相当于对list中的元素重新进行了一次排序/               
            }
            else   //现有list中不存在，说明go是新增对象，需要添加到list中然后重新排序/
            {
                if (curGoNum >= maxOverlayLayer)   //如果当前list中的元素个数不小于窗口最大可叠加数量，说明窗口已达到叠加上限，需要清理掉最底层的窗口，然后将新增窗口排到最顶层,如果当前list中的元素个数小于窗口最大可叠加数量，说明窗口还可以叠加/
                {
                    windows[0].SetActive(false);   //隐藏那个最底层的界面/
                    windows.Remove(windows[0]);
                }
                windows.Add(go);
            }
            showGoInTurn(windows);   //对window列表中的对象重新排序后，按顺序显示对象到panel里/
        }

        /// <summary>
        /// 清理当前window列表中处于隐藏状态的window
        /// </summary>
        /// <param name="curWindowsInList">要执行清理操作的列表</param>
        /// <returns>返回清理完成后的新list的长度值</returns>
        private int clearInActiveGO(List<GameObject> curWindowsInList)
        {
            List<GameObject> temp = new List<GameObject>();
            foreach (GameObject go in curWindowsInList)
            {
                if(go!=null)
                {
                    if(go.activeSelf)
                    {
                        if (go.GetComponent<FocusWindowItem>() != null)
                            Destroy(go.GetComponent<FocusWindowItem>());
                    }
                    else
                    {
                        temp.Add(go);
                    }
                }
                else
                {
                    temp.Add(go);
                }
            }
            foreach (GameObject inActiveGO in temp)
            {
                curWindowsInList.Remove(inActiveGO);
            }
            return curWindowsInList.Count;
        }

        /// <summary>
        /// 将最后添加的window对象显示到depth最大的panel下
        /// </summary>
        /// <param name="orderedList">经过重新排序的window列表</param>
        private void showGoInTurn(List<GameObject> orderedList)
        {
            int panelLastIndex = panels.Count - 1;
            for (int i = orderedList.Count -1 ; i >=0; i--)
            {
                if (panelLastIndex >= 0)
                {
                    if (panelLastIndex == panels.Count - 1)
                    {
                        setFocusOnState(orderedList[i], panels[panelLastIndex],true,i);
                    }
                    else
                    {
                        setFocusOnState(orderedList[i], panels[panelLastIndex], false,i);
                    }
                    panelLastIndex--;
                }
            }
        }


        /// <summary>
        /// 设置对象的激活状态
        /// </summary>
        /// <param name="go">要设置的对象</param>
        /// <param name="parentTrs">要挂载到的父节点</param>
        /// <param name="isFocusOn">是否为焦点</param>
        /// <param name="layerOrder">当前go对象在orderlist的索引</param>
        private void setFocusOnState(GameObject go, Transform parentTrs, bool isFocusOn,int layerOrder)
        {
            go.SetActive(false);
            go.transform.parent = parentTrs;
            go.SetActive(true);   //打开关闭的对象，让对象在新的panel下重新渲染/
            GameObject colliderGO = go.transform.Find("titleSprite/FocusCollider").gameObject;
            UIEventListener.Get(colliderGO).onClick = overlayOnClick;
            colliderGO.GetComponent<Collider>().enabled = !isFocusOn;  //如果是焦点窗口，就关闭collider，不是就打开collider/
            if (isFocusOn)
            {
                FocusWindowItem.getItem(go).init(itemEnable, itemDisable,itemDestroy);               
            }
            Vector3 pos = go.transform.localPosition;
            go.transform.localPosition = new Vector3(pos.x, pos.y, -500f*(layerOrder+1));
        }

        #region   处理最顶层的对象自己关闭（隐藏）时下面的对象重新排层次的问题

        private bool toReorder = false;
        private FocusWindowItem item = null;

        /// <summary>
        /// 窗口隐藏时执行的方法
        /// </summary>
        /// <param name="obj"></param>
        private void itemDisable(FocusWindowItem obj)
        {
            toReorder = true;
            StartCoroutine(waitBeforeReorder());
        }

        /// <summary>
        /// 进入不激活状态时执行
        /// </summary>
        //void OnDisable()
        //{
        //    Debug.Log("关闭所有协程");
        //    StopAllCoroutines();
        //}

        //void OnApplicationQuit()
        //{
        //    StopAllCoroutines();
        //}

        /// <summary>
        /// FocusWindowItem对象激活时的执行方法
        /// </summary>
        /// <param name="obj"></param>
        private void itemEnable(FocusWindowItem obj)
        {
            toReorder = false;
            item = obj;
        }

        /// <summary>
        /// FocusWindowItem对象被销毁时执行的方法
        /// </summary>
        private void itemDestroy()
        {
            toReorder = false;
            item = null;
            StopCoroutine(waitBeforeReorder());
        }

        /// <summary>
        /// 重新排序前等待一定帧数（这里用来区分会引起disable和enable变化的两种情况：一是顶层对象隐藏确实需要重新排序，二是顶层对象只是渲染时临时setactive(false)无须重新排序）
        /// </summary>
        /// <returns></returns>
        private IEnumerator waitBeforeReorder()
        {
            int frameNum = 0;
            while (frameNum<5)
            {
                if (!toReorder)   //不需要重新排序显示,因为层控制器中的对象重新渲染时都有一个先setactive(true)再马上setactive(false)的过程，如果只是渲染需要，不需要重新排序显示；如果是对象确实隐藏，则需要重新排序/
                {
                    if(item!=null)
                        Destroy(item);
                    break;
                }                   
                yield return null;
                frameNum++;
            }
            if(toReorder)
            {
                GameObject temp = null;
                for (int i = windows.Count-1; i >=0; i--)
			    {
			        if(windows[i].activeSelf)
                    {
                        temp = windows[i];
                        break;
                    }
			    }
                if(temp!=null)
                    reOrderWindowList(temp);       //重新对window列表进行排序，激活对象为list中倒数第二个对象/
            }
        }
        #endregion

        /// <summary>
        /// 处于叠加列表中的对象被点击时执行的方法
        /// </summary>
        /// <param name="go"></param>
        private void overlayOnClick(GameObject go)
        {
            reOrderWindowList(go.transform.parent.parent.gameObject);       //重新对window列表进行排序/
        }

    }
}

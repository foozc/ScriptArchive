using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Logic.UI
{
    public class BtnWithFX : UIbtn<GameObject, BtnWithFX>
    {
        private GameObject btnObj;    //按钮，带collier的游戏对象/
        private GameObject fxObj;     //特效对象/
        private UIWidget widget;      //widget对象/
        private UILabel label;        //label对象/
        private int mouseBtnIndex=0;  //鼠标按键对应的序号，与unity默认的一致：0-左键，1-右键，2-中键/
        private bool isMouseBtnDown = false;  //鼠标按键是否按下/
        private bool isCursorOver = false;    //光标是否在ui元素上方/

        private BtnWithFX evtMsg = null; //事件要传递出去的参数/  



        public override int Depth
        {
            get
            {
                Debug.Log("highestDepth=" + this.label.depth + ";  lowestDepth=" + this.widget.depth);
                return this.widget.depth;
            }
            set
            {
                //this.widget.depth = value;
                adjustDepth(value);
            }
        }
        private void adjustDepth(int lowestDepthValue)
        {
            this.widget.depth = lowestDepthValue;
            this.btnObj.GetComponent<UIWidget>().depth = ++lowestDepthValue;
            this.label.depth = ++lowestDepthValue;
        }


        public override event UIEventHandler<GameObject, BtnWithFX> SingleClick;
        public override event UIEventHandler<GameObject, BtnWithFX> MouseEnter;
        public override event UIEventHandler<GameObject, BtnWithFX> MouseExit;

        public BtnWithFX init()
        {
            btnObj = this.transform.Find("btn").gameObject;
            fxObj = this.transform.Find("btn/particle").gameObject;
            widget = this.GetComponent<UIWidget>();
            label = this.transform.Find("btn/Label").GetComponent<UILabel>();
         
            UIEventListener.Get(btnObj).onHover = btnHasMouseHover;   //onHover可以引发鼠标进入、退出、点击事件/
            evtMsg = this;

            return this;
        }


        /// <summary>
        /// 隐藏或显示按钮上的特效
        /// </summary>
        public void showFX(bool show)
        {
            fxObj.SetActive(show);
        }


        private void btnHasMouseHover(GameObject go, bool state)
        {
            if (state)
            {
                isCursorOver = true;

                if(isMouseBtnDown)
                {
                    switch(mouseBtnIndex)
                    {
                        case 0:
                            leftMouseBtnClicked(gameObject);
                            break;
                        case 1:
                            rightMouseBtnClicked(gameObject);
                            break;
                    }
                    isMouseBtnDown = false;
                }
                else
                {
                    btnHasMouseEnter(gameObject);
                }
            }
            else
            {
                isCursorOver = false;
                btnHasMouseExit(gameObject);
            }
        }

        private void leftMouseBtnClicked(GameObject go)
        {
            Debug.Log("左键点击  "+Time.time);
            if (SingleClick != null) SingleClick(go, evtMsg);
        }

        private void rightMouseBtnClicked(GameObject go)
        {
            Debug.Log("右键点击  " + Time.time);
        }

        private void btnHasMouseEnter(GameObject go)
        {
            Debug.Log("鼠标进入   "+Time.time);
            if (MouseEnter != null) MouseEnter(go, evtMsg);
        }

        private void btnHasMouseExit(GameObject go)
        {
            Debug.Log("鼠标退出   "+Time.time);
            if (MouseExit != null) MouseExit(go, evtMsg);
        }



        void Update()
        {
            if (!isCursorOver) return;
            if (Input.GetMouseButtonDown(0))
            {
                isMouseBtnDown = true;
                mouseBtnIndex = 0;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                isMouseBtnDown = true;
                mouseBtnIndex = 1;
            }
        }

    }
}

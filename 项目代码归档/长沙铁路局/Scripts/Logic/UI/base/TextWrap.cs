using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Log;

namespace Assets.Scripts.Logic.UI
{
    public class TextWrap
    {
 
        private bool isMouseOver = false;
        private Action<int,string> textCallBack;    //整数只有三个值：0-鼠标点击；1-鼠标进入；2-鼠标退出/

        /// <summary>
        /// 对label对象添加鼠标进入、退出、单击事件的监听
        /// </summary>
        /// <param name="label"></param>
        public void AddListener(UILabel label,Action<int,string> callback)
        {
            if (label.GetComponent<BoxCollider>() == null)
            {
                label.gameObject.AddComponent<BoxCollider>().isTrigger = true;
                label.autoResizeBoxCollider = true;
            }
            this.textCallBack = callback;
            UIEventListener.Get(label.gameObject).onHover = labelOnHover;
        }

        public void SetFont(UILabel label, UIFont uifont, int fontsize, int xSpace,int ySpace)
        {
            label.bitmapFont = uifont;
            label.fontSize = fontsize;
            label.effectStyle = UILabel.Effect.None;
            label.overflowMethod = UILabel.Overflow.ResizeHeight;
            label.alignment = NGUIText.Alignment.Left;
            label.applyGradient = false;
            label.spacingX = xSpace;
            label.spacingY = ySpace;
            label.pivot = UIWidget.Pivot.TopLeft;
        }



        /// <summary>
        /// 光标在指定的添加的boxcollier的uilabel之上/
        /// </summary>
        /// <param name="go"></param>
        /// <param name="state"></param>
        private void labelOnHover(GameObject go, bool state)
        {
            if(state)
            {
                if (isMouseOver)
                {
                    onClick(go);
                }
                else
                {
                    onEnter(go);
                }
                isMouseOver = true;
            }
            else
            {
                isMouseOver = false;
                onExit(go);
            }
        }

        /// <summary>
        /// 鼠标单击
        /// </summary>
        /// <param name="go"></param>
        private void onClick(GameObject go)
        {
            string str = getUrl(go.GetComponent<UILabel>());
            if(!string.IsNullOrEmpty(str))
            {
                if (textCallBack != null) textCallBack(0,str);
            }
        }

        /// <summary>
        /// 鼠标进入
        /// </summary>
        /// <param name="go"></param>
        private void onEnter(GameObject go)
        {
            if (textCallBack != null) textCallBack(1, null);
        }

        /// <summary>
        /// 鼠标退出
        /// </summary>
        /// <param name="go"></param>
        private void onExit(GameObject go)
        {
            if (textCallBack != null) textCallBack(2, null);
        }

        /// <summary>
        /// 获取鼠标点击处label文本中包含的url字符串
        /// </summary>
        /// <param name="lbl"></param>
        /// <returns></returns>
        private string getUrl(UILabel lbl)
        {
            if (lbl != null)
            {
                //Camera currentCamera = UICamera.currentCamera;
                //Vector3 point = currentCamera.ScreenToWorldPoint(Input.mousePosition);
                //Debug.Log(point);
                //return lbl.GetUrlAtPosition(point);
                return lbl.GetUrlAtPosition(UICamera.lastHit.point);
            }
            else
            {
                return null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Logic.UI
{
    public class FuncBtn : UIbase
    {
        private bool isInited = false;         //是否已经初始化过/
        private UIWidget widget;               //对当前对象widget的引用/
        private UIButton btn;                  //对当前UIbutton的引用/
        private GameObject newRemind;          //"新"提醒/
        private GameObject particalGO;         //粒子系统对象/
        private ParticleSystem ps;             //粒子系统/
        private Action<FuncBtn> btnIsClicked;  //按钮被点击/


        public override int Depth
        {
            get
            {
                Debug.Log("highestDepth=" + (this.widget.depth+1) + ";  lowestDepth=" + this.widget.depth);
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
            this.newRemind.GetComponent<UIWidget>().depth = ++lowestDepthValue;
        }


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="isShowHint">是否显示“新”提示</param>
        /// <param name="isShowPartical">是否显示粒子系统</param>
        /// <param name="btnOnClick">监听当前按钮对象被点击时间的委托</param>
        /// <returns></returns>
        public FuncBtn init(bool isShowHint = false,bool isShowPartical = false,string btnName = null)
        {
            if (!isInited)
            {
                this.widget = this.GetComponent<UIWidget>();
                this.btn = this.GetComponent<UIButton>();
                this.newRemind = this.transform.Find("newTag").gameObject;
                this.particalGO = this.transform.Find("particle").gameObject;
                this.ps = this.particalGO.GetComponent<ParticleSystem>();
                if (!string.IsNullOrEmpty(btnName)) this.transform.Find("Label").GetComponent<UILabel>().text = btnName;   //如果funcbtn全部用图片的话，此方法以后可能不会用/
                UIEventListener.Get(this.gameObject).onClick = FuncBtnClicked;   //按钮被点击事件/

                //设置粒子系统的显示层级，不被sprite遮挡/
                ps.GetComponent<Renderer>().sortingLayerName = "Partical";
                ps.GetComponent<Renderer>().sortingOrder = -1;

                isInited = true;
            }

            showHint(isShowHint);
            showPartical(isShowPartical);

            return this;
        }

        /// <summary>
        /// 当前按钮被点击
        /// </summary>
        /// <param name="go"></param>
        private void FuncBtnClicked(GameObject go)
        {
            if (btnIsClicked != null)
                btnIsClicked(this);
        }


        /// <summary>
        /// 设置按钮显示的图片及被点击后执行的方法
        /// </summary>
        /// <param name="bs"></param>
        /// <param name="btnOnClick"></param>
        public void setValue(BtnSprites bs,Action<FuncBtn> btnOnClick = null)
        {

            if (!string.IsNullOrEmpty(bs.normalSprite))
                this.btn.normalSprite = bs.normalSprite;
            if (!string.IsNullOrEmpty(bs.hoverSprite))
                this.btn.hoverSprite = bs.hoverSprite;
            if (!string.IsNullOrEmpty(bs.pressedSprite))
                this.btn.pressedSprite = bs.pressedSprite;
            if (!string.IsNullOrEmpty(bs.disabledSprite))
                this.btn.disabledSprite = bs.disabledSprite;

            btnIsClicked = btnOnClick;
            
        }


        /// <summary>
        /// 设置按钮被点击的回调方法
        /// </summary>
        /// <param name="btnOnClick"></param>
        public void setBtnClickCallBack(Action<FuncBtn> btnOnClick)
        {
            btnIsClicked = btnOnClick;
        }



        /// <summary>
        /// 显示或隐藏粒子系统
        /// </summary>
        /// <param name="showOrNot"></param>
        public void showPartical(bool showOrNot = true)
        {
            this.particalGO.SetActive(showOrNot);
        }

        /// <summary>
        /// 显示或隐藏“新”提示
        /// </summary>
        /// <param name="showOrNot"></param>
        public void showHint(bool showOrNot = true)
        {
            this.newRemind.SetActive(showOrNot);
        }



        /// <summary>
        /// 按钮sprite组，包含uibutton中的四个状态
        /// </summary>
        public struct BtnSprites
        {
            public string normalSprite;
            public string hoverSprite;
            public string pressedSprite;
            public string disabledSprite;

            public BtnSprites(string normal, string hover,string pressed,string disabled)
            {
                normalSprite = normal;
                hoverSprite = hover;
                pressedSprite = pressed;
                disabledSprite = disabled;
            }
        }

    }
}

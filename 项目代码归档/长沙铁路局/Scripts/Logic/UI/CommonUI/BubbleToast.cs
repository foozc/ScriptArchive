using Assets.Scripts.Controller;
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
*Desciption:功能说明:冒泡对象类
*Author:作者
*
*/
namespace Assets.Scripts.Logic.UI.CommonUI
{
    public class BubbleToast : UIbase
    {
		public AnimationCurve posCurve;    //位移曲线/
        public AnimationCurve scalCurve;   //缩放曲线/
        public AnimationCurve alphaCurve;  //透明度曲线/
        public Color textColor;            //文字颜色/
        public bool moveToRight = true;    //是否向右移动/
        public float x_factor = 100f;      //x轴偏移系数/
        public float y_factor = 10f;       //y轴偏移系数/
        private UILabel content;           //对uilabel的引用/
        private int frameCount = 0;        //位移动画曲线中的总关键帧数/
        private float duration = 0f;       //位移动画曲线中一次动画总时长/
        private bool startMove = false;    //动画是否开始执行/
        private float sign = 1f;           //动画向左还是向右移动的x轴的正负号/
        private Vector2 offSet = Vector2.zero;            //冒泡对象在屏幕上和中心坐标的偏移量/
        private UIWidget bubbleWidget;     //对冒泡对象UIWidget的引用/
        private bool isAutoDespawn;        //是否自动销毁气泡信息

        void Awake()
        {
            this.content = this.transform.Find("Content").GetComponent<UILabel>();
            this.bubbleWidget = this.GetComponent<UIWidget>();
        }

        public BubbleToast init(Vector2 offset)
        {
            //this.content = this.transform.Find("Content").GetComponent<UILabel>();
            //this.bubbleWidget = this.GetComponent<UIWidget>();

            this.offSet = offset;

            return this;
        }

        public override int Depth
        {
            get
            {
                Debug.Log("highestDepth=" + this.content.depth + ";  lowestDepth=" + this.bubbleWidget.depth);
                return this.bubbleWidget.depth;
            }
            set
            {
                //this.bubbleWidget.depth = value;
                adjustDepth(value);
            }
        }

        private void adjustDepth(int lowestDepthValue)
        {
            this.bubbleWidget.depth = lowestDepthValue;
            this.content.depth = ++lowestDepthValue;
        }

        public void setContent(string content, Color contentColor, bool isAutoDespawn = true, int fontSize = 35, Font font = null)
        {
            this.content.text = content;
            this.content.color = contentColor;
            this.content.fontSize = fontSize;
            this.isAutoDespawn = isAutoDespawn;
            if (font != null) this.content.trueTypeFont = font;

            bubbleMoveStart();
        }

        public void setContent(string content, bool isAutoDespawn = true, int fontSize = 35, Font font = null)
        {
            this.content.text = content;
            this.content.color = this.textColor;
            this.content.fontSize = fontSize;
            this.isAutoDespawn = isAutoDespawn;
            if (font != null) this.content.trueTypeFont = font;

            bubbleMoveStart();
        }


        private void bubbleMoveStart()
        {
            frameCount = posCurve.length;
            if (frameCount == 0)
            {
                Debug.Log("位移曲线是主动画曲线，关键帧数量必须大于等于0！！");
                return;
            }
            this.duration = posCurve[frameCount - 1].time;
            sign = moveToRight ? 1f : -1f;
            startMove = true;
            StartCoroutine(bubbleTween());
        }


        private IEnumerator bubbleTween()
        {
            while (startMove)
            {
                duration -= Time.deltaTime;
                if (duration > 0)
                {
                    float t = posCurve[frameCount - 1].time - duration;
                    Vector3 newPos = new Vector3(sign * t * x_factor, posCurve.Evaluate(t) * y_factor, 0f);

                    this.transform.localPosition = new Vector3(transform.localPosition.x + offSet.x, transform.localPosition.y + offSet.y, 0) + newPos;     //位移变化/
                    this.bubbleWidget.alpha = Mathf.Clamp01(alphaCurve.Evaluate(t));   //透明度变化/
                    float scl = scalCurve.Evaluate(t);
                    this.transform.localScale = new Vector3(scl, scl, 0f);       //大小变化/
                }
                else
                {
                    startMove = false;
                }
                yield return null;
            }
            //Destroy(this.gameObject);
            //UIDownLoad.Instance.getUIPool.Despawn(this.transform);
            if (isAutoDespawn)
                ResourceManager.getInstance().getUIPool.Despawn(transform);
        }
        
        public void hideBubbleToast()
        {
            this.startMove = false;
        }

    }
}

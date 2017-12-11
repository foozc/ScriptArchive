using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Logic.UI
{
    //必须全部显示在屏幕范围内
    //可居左显示、居中显示、居右显示
    //可显示一行或多行(默认多行，长度可控)
    //背景图片根据内容自适应
    //提供自动销毁和手动销毁两种方式
    public class SimpleTip : UIbase
    {
        private UIWidget widget;  //当前对象上的UIWidget/
        private UISprite bg;      //背景图片/
        private UILabel label;    //文本框/


        public override int Depth
        {
            get
            {
                Debug.Log("highestDepth=" + this.label.depth + ";  lowestDepth=" + this.widget.depth);
                return this.widget.depth;
            }
            set
            {
                adjustDepth(value);
            }
        }
        private void adjustDepth(int lowestDepthValue)
        {
            this.widget.depth = lowestDepthValue;
            this.bg.depth = ++lowestDepthValue;
            this.label.depth = ++lowestDepthValue;
        }

        /// <summary>
        /// 一个只有简单功能的小TIP,可设置文字颜色、大小、对齐方式、文本框宽度，自动多行，可控制显示时长
        /// </summary>
        /// <param name="content">要显示的文字内容</param>
        /// <param name="alignment">内容的对齐方式</param>
        /// <param name="labelWidth">用来显示内容的文本框的宽度</param>
        /// <param name="lifeTime">当前tip的生命周期，使用默认值则须手动控制对象的销毁，设置大于0的时间值则会在指定时间后销毁</param>
        /// <returns></returns>
        public SimpleTip init(string content, Color color, int fontsize = 16, NGUIText.Alignment alignment = NGUIText.Alignment.Center, int labelWidth = 150, float lifeTime = -1f)
        {
            this.widget = this.GetComponent<UIWidget>();
            this.bg = this.transform.Find("bg").GetComponent<UISprite>();
            this.label = this.transform.Find("content").GetComponent<UILabel>();

            this.label.text = content;  //给label赋值/

            setTextFormat(color,fontsize,alignment,labelWidth);
            if (lifeTime > 0) StartCoroutine(recycle(lifeTime));

            return this;
        }

        /// <summary>
        /// 设置文本格式
        /// </summary>
        private void setTextFormat(Color color,int fontsize,NGUIText.Alignment align,int width)
        {
            this.label.color = color;
            this.label.fontSize = fontsize;
            this.label.alignment = align;
            this.label.width = width;
            //根据文本框的长宽自动调整背景图片的宽和高
            this.bg.width = width + 30;
            this.bg.height = this.label.height * 4;
        }


        /// <summary>
        /// 回收
        /// </summary>
        /// <param name="lifetime">多少秒之后回收</param>
        /// <returns></returns>
        private IEnumerator recycle(float lifetime)
        {
            yield return new WaitForSeconds(lifetime);
            Destroy(this.gameObject);
        }


        /// <summary>
        /// 用与手动控制tip的回收
        /// </summary>
        public void destroyTip()
        {
            StartCoroutine(recycle(0f));
        }
    }
}

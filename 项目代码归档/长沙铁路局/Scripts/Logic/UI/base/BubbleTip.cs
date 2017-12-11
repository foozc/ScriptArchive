using Assets.Scripts.Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Logic.UI
{
    ///
    /// <summary>
    /// 所有动画曲线请从零开始编辑
    /// 文字上浮信息
    /// </summary>
    public class BubbleTip : UIbase
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
        private float start = 0f;          //位移动画开始执行时的时间点/ 
        private float duration = 0f;       //位移动画曲线中一次动画总时长/
        private bool startMove = false;    //动画是否开始执行/
        private float sign = 1f;           //动画向左还是向右移动的x轴的正负号/
        private Camera cam;                //用于转换世界坐标到屏幕坐标的摄像机/
        private Vector3 wldPos;            //冒泡对象生成点在世界坐标系中的位置点/
        private Vector2 offSet;            //冒泡对象在屏幕上的偏移量，根据需要调整偏移量/
        private UIWidget bubbleWidget;     //对冒泡对象UIWidget的引用/
        private bool isAutoDespawn;        //是否自动销毁气泡信息


        public BubbleTip init(Camera camOnRole, Vector3 worldPos, Vector2 offset)
        {
            this.content = this.transform.Find("Content").GetComponent<UILabel>();
            this.bubbleWidget = this.GetComponent<UIWidget>();

            this.cam = camOnRole;
            this.wldPos = worldPos;
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

        public void setContent(string content, Color contentColor, bool isAutoDespawn = true, int fontSize = 40, Font font = null)
        {
            this.content.text = content;
            this.content.color = contentColor;
            this.content.fontSize = fontSize;
            this.isAutoDespawn = isAutoDespawn;
            if (font!=null) this.content.trueTypeFont = font;   

            bubbleMoveStart();
        }

        public void setContent(string content, bool isAutoDespawn = true, int fontSize = 40, Font font = null)
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
            if (frameCount==0)
            {
                Debug.Log("位移曲线是主动画曲线，关键帧数量必须大于等于0！！");
                return;
            }
            start = Time.time;
            this.duration = posCurve[frameCount - 1].time;
            sign = moveToRight ? 1f : -1f;
            startMove = true;
            StartCoroutine(bubbleTween());
        }


        private IEnumerator bubbleTween()
        {
            while(startMove)
            {
                float t = Time.time - start;
                if (t < duration)
                {
                    Vector3 newPos = new Vector3(sign * t * x_factor, posCurve.Evaluate(t) * y_factor, 0f);
                    Vector3 spawnPoint = getBubbleSpawn(cam, wldPos, offSet);    //当前对象的初始产生点/
                    this.transform.localPosition = spawnPoint + newPos;     //位移变化/
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
                ResourceManager.getInstance().destroyUILoad(gameObject);
        }


        private Vector3 getBubbleSpawn(Camera camOnRole, Vector3 worldPos, Vector2 offset)
        {
            Vector3 spawnPos = Vector3.zero;
            if (worldPos != null)
            {
                Vector3 pos = camOnRole.WorldToScreenPoint(worldPos);
                //NGUI中元素在屏幕上的位置的本地坐标 = 屏幕坐标 - （screen.width/2,screen.height/2）/
                Vector3 newPos = pos - new Vector3(Screen.width / 2, Screen.height / 2, 0f);
                spawnPos = new Vector3(newPos.x + offset.x, newPos.y + offset.y, 0f);
            }
            else
            {
                Debug.Log("冒泡需要参照的世界坐标值不存在，请检查后重新输入！");
            }
            return spawnPos;
        }

        public void hideBubbleTip()
        {
            this.startMove = false;
        }
    }
}

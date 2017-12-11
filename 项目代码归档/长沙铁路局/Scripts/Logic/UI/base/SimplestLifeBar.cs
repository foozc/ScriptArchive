using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Logic.UI
{
    /// <summary>
    /// 极简血条，仅显示名字和血量,或者只显示名字
    /// </summary>
    public class SimplestLifeBar : UIbase
    {
        private bool isInited = false;               //是否已经初始化过/

        private Vector2 offSet = Vector2.zero;       //偏移量/
        private Camera roleCam;                      //显示血条的相机/
        private Transform modelTrs;                  //模型的transform/
        private UISlider lifeProgressBar;            //进度条/
        private UILabel nameLabel;                   //名字label/
        private bool startRun = false;               //为true时，血条开始每帧根据模型和相机变化transform/
        private UIWidget widget;     //对血条对象UIWidget的引用/

        private bool isModelVisibel = true;          //模型是否可见/

         
        public override int Depth
        {
            get
            {
                Debug.Log("highestDepth=" + this.nameLabel.depth + ";  lowestDepth=" + this.widget.depth);
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
            this.lifeProgressBar.GetComponent<UIWidget>().depth = ++lowestDepthValue;
            this.transform.Find("miniLifeBar/Foreground").GetComponent<UIWidget>().depth = ++lowestDepthValue;
            this.nameLabel.depth = ++lowestDepthValue;
        }


        void Update()
        {
            if (startRun)
            {
                if (modelTrs != null && isModelVisibel)
                {
                    this.lifeProgressBar.alpha = 1f;
                    this.nameLabel.gameObject.SetActive(true);

                    Vector3 screenPos = getBarSpawn(roleCam, modelTrs.position, offSet);
                    this.transform.localPosition = screenPos;
                }
                else
                {
                    this.lifeProgressBar.alpha = 0f;
                    this.nameLabel.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="camOnRole"></param>
        /// <param name="modeltrs"></param>
        /// <param name="offset"></param>
        /// <param name="name"></param>
        /// <param name="lifePercent"></param>
        /// <param name="onlyName"></param>
        /// <returns></returns>
        public SimplestLifeBar init(Camera camOnRole, Transform modeltrs, Vector2 offset)
        {
            if (!isInited)
            {
                this.widget = this.GetComponent<UIWidget>();
                this.lifeProgressBar = this.transform.Find("miniLifeBar").GetComponent<UISlider>();
                this.nameLabel = this.transform.Find("roleName").GetComponent<UILabel>();
            }
            this.roleCam = camOnRole;
            this.modelTrs = modeltrs;
            this.offSet = offset;

            RendererVisibility.Get(modeltrs.gameObject).OnVisibleState = modelVisibility;

            return this;
        }

        /// <summary>
        /// 模型是否可见（是否被渲染在摄像机中）
        /// </summary>
        /// <param name="obj"></param>
        private void modelVisibility(bool visibelState)
        {
            isModelVisibel = visibelState;
        }


        /// <summary>
        /// 显示当前生命百分比，可选择不显示生命条，仅显示名字
        /// </summary>
        /// <param name="name">角色名字</param>
        /// <param name="curLifeValue">当前生命值</param>
        /// <param name="maxLifeValue">最大生命值</param>
        /// <param name="showLifeBar">是否显示生命条</param>
		public void setValue(int curLifeValue,int maxLifeValue, string name = default(string), bool showLifeBar = true)
        {
			if(name!=default(string))
            this.nameLabel.text = name;
            this.startRun = true;

            if (showLifeBar)
            {
                if (!this.lifeProgressBar.gameObject.activeSelf) 
                    this.lifeProgressBar.gameObject.SetActive(true);

                maxLifeValue = Mathf.Abs(maxLifeValue);
                curLifeValue = Mathf.Clamp(curLifeValue, 0, maxLifeValue);
                float lifePercent = (float)curLifeValue / (float)maxLifeValue;

                this.lifeProgressBar.value = lifePercent;
            }
            else
            {
                if (this.lifeProgressBar.gameObject.activeSelf) 
                    this.lifeProgressBar.gameObject.SetActive(false);
            }
        }


        /// <summary>
        /// 设置字体
        /// </summary>
        /// <param name="fontColor"></param>
        /// <param name="fontSize"></param>
        /// <param name="font"></param>
        public void setFont(Color fontColor, int fontSize = 20, Font font = null)
        {
            this.nameLabel.color = fontColor;
            this.nameLabel.fontSize = fontSize;
            if (font != null) this.nameLabel.trueTypeFont = font;
        }


        /// <summary>
        /// 获取生命条的生成点
        /// </summary>
        /// <param name="camOnRole"></param>
        /// <param name="worldPos"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private Vector3 getBarSpawn(Camera camOnRole, Vector3 worldPos, Vector2 offset)
        {
            Vector3 spawnPos = Vector3.zero;
            if (worldPos != null)
            {
                Vector3 pos = camOnRole.WorldToScreenPoint(worldPos);
                Vector3 newPos = pos - new Vector3(Screen.width / 2, Screen.height / 2, 0f);
                spawnPos = new Vector3(newPos.x + offset.x, newPos.y + offset.y, 0f);
            }
            else
            {
                Debug.Log("血条需要参照的世界坐标值不存在，请检查后重新输入！");
            }
            return spawnPos;
        }

        /// <summary>
        /// 关闭当前窗口
        /// </summary>
        public void close()
        {
            reset();
            destorySelf();
        }


        /// <summary>
        /// 重置血条
        /// </summary>
        public void reset()
        {
            this.lifeProgressBar.alpha = 1f;
            this.nameLabel.gameObject.SetActive(true);
        }
    }
}
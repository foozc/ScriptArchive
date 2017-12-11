using Assets.Scripts.Controller;
using System;
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
*Desciption:功能说明:认知标签类
*Author:作者
*
*/
namespace Assets.Scripts.Logic.UI.CommonUI
{
	public class equipTipMode
	{
		public GameObject modeObj;
		public string name;
	}
	public class EquipTip : UIbase
    {
        public UIWidget widget;                     //对设备名称对象UIWidget的引用/
        public UILabel nameLabel;                   //名字label/

        private bool isInited = false;               //是否已经初始化过/

        private Vector2 offSet = Vector2.zero;       //偏移量/
        private Camera roleCam;                      //显示设备名称的相机/
        private Transform modelTrs;                  //模型的transform/
        private bool startRun = false;               //为true时，设备名称开始每帧根据模型和相机变化transform/
        

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
            this.nameLabel.depth = ++lowestDepthValue;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="camOnRole"></param>
        /// <param name="modeltrs"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public EquipTip init(Camera camOnRole, Transform modeltrs, Vector2 offset)
        {
            this.roleCam = camOnRole;
            this.modelTrs = modeltrs;
            this.offSet = offset;

            //RendererVisibility.Get(modeltrs.gameObject).OnVisibleState = modelVisibility;

            return this;
        }

        void Update()
        {
            if (startRun)
            {
                if (modelTrs != null && isModelVisibel)
                {
                    this.nameLabel.gameObject.SetActive(true);

                    Vector3 screenPos = getBarSpawn(modelTrs.position, offSet);
                    this.transform.localPosition = screenPos;
                }
                else
                {
                    this.nameLabel.gameObject.SetActive(false);
                }
            }
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
        /// 设置显示设备名称
        /// </summary>
        /// <param name="name">设备名称</param>
        public void setValue(string name)
        {
            if (name != default(string))
                this.nameLabel.text = name;
            this.startRun = true;
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
        /// 获取设备名称物体的生成点
        /// </summary>
        /// <param name="camOnRole"></param>
        /// <param name="worldPos"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        //private Vector3 getBarSpawn(Camera camOnRole, Vector3 worldPos, Vector2 offset)
        //{
        //    Vector3 spawnPos = Vector3.zero;
        //    if (worldPos != null)
        //    {
        //        //Vector3 pos = camOnRole.WorldToScreenPoint(worldPos);
        //        //Vector3 newPos = pos - new Vector3(Screen.width / 2, Screen.height / 2, 0f);
        //        //spawnPos = new Vector3(newPos.x + offset.x, newPos.y + offset.y, 0f);
        //        Debug.LogWarning(offset);
        //        Vector3 pos = camOnRole.WorldToScreenPoint(new Vector3( worldPos.x + offset.x, worldPos.y, worldPos.z));
        //        spawnPos = pos - new Vector3(Screen.width/2, Screen.height/2, 0f);
        //    }
        //    else
        //    {
        //        Debug.Log("设备名称需要参照的世界坐标值不存在，请检查后重新输入！");
        //    }
        //    return spawnPos;
        //}

        private Vector3 getBarSpawn(Vector3 worldPos, Vector2 offset)
        {
            Vector3 spawnPos = Vector3.zero;
            if (worldPos != null)
            {
                //Vector3 pos = camOnRole.WorldToScreenPoint(worldPos);
                //Vector3 newPos = pos - new Vector3(Screen.width / 2, Screen.height / 2, 0f);
                //spawnPos = new Vector3(newPos.x + offset.x, newPos.y + offset.y, 0f);
                UIRoot uiroot = GetComponentInParent<UIRoot>();
                //Camera came = null;
                //if (Camera.main != null)
                //    foreach(Camera icame in Camera.allCameras)
                //    {
                //        if (icame.name.Equals("Camera"))
                //            came = icame;
                //    }
                //else came = Camera.main;
                Vector3 pos = Camera.main.WorldToScreenPoint(new Vector3(worldPos.x + offset.x, worldPos.y + offset.y, worldPos.z));
                //pos *= (float)uiroot.manualHeight / NGUITools.screenSize.y;
                float xMultiple = (float)uiroot.manualWidth / NGUITools.screenSize.x;
                float yMultiple = (float)uiroot.manualHeight / NGUITools.screenSize.y;
                pos = new Vector3(pos.x * xMultiple, pos.y * yMultiple, pos.z);

                spawnPos = pos - new Vector3(uiroot.manualWidth / 2, uiroot.manualHeight / 2, 0f);
                spawnPos.y = spawnPos.y + 30;
                //spawnPos = pos - new Vector3(NGUITools.screenSize.x / 2, NGUITools.screenSize.y / 2, 0f);
                //spawnPos *= (float)uiroot.manualHeight / uiroot.activeHeight;
                //uiroot.GetPixelSizeAdjustment()
            }
            else
            {
                Debug.Log("设备名称需要参照的世界坐标值不存在，请检查后重新输入！");
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
		/// 得到显示状态
		/// </summary>
		/// <returns></returns>
		public bool getstate()
		{
			return this.nameLabel.gameObject.active;
		}

        /// <summary>
        /// 重置设备名称
        /// </summary>
        private void reset()
        {
            this.nameLabel.gameObject.SetActive(true);
        }

		private Dictionary<string, EquipTip> currentEquipTip;
		public void setEquipTipModes(List<equipTipMode> equips)
		{
			foreach (var item in equips)
				setEquipTipMode(item);
		}
		public void setEquipTipMode(equipTipMode item)
		{
			GameObject obj = ResourceManager.getInstance().loadUISpawnPool("Prefabs/UI/MyUI/EquipTip", null);
			obj.transform.localPosition = Vector3.zero;
			currentEquipTip.Add(item.name, obj.GetComponent<EquipTip>());
			Vector2 off = Vector2.zero;
			if (item.modeObj != null)
			{
				if (item.modeObj.GetComponent<Renderer>() != null)
					off = new Vector2(0, item.modeObj.GetComponent<Renderer>().bounds.max.y - item.modeObj.GetComponent<Renderer>().bounds.center.y);
				obj.GetComponent<EquipTip>().init(Camera.main, item.modeObj.transform, off);
			}
			obj.GetComponent<EquipTip>().setValue(item.name);
		}
		public void setEquipTipActive(bool isshow, List<string> name)
		{
			foreach (var item in name)
				setEquipTipActive(isshow, item);
		}
		public void setEquipTipActive(bool isshow, string name)
		{
			if (currentEquipTip[name] != null)
			{
				currentEquipTip[name].gameObject.SetActive(isshow);
			}
		}
	}
}

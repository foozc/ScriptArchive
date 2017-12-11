using Assets.Scripts.Configs;
using Assets.Scripts.util;
using Assets.Scripts.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Training;
using UnityEngine;
using System.Data;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明：设备认知配置文档初始化，培训配置文档初始化
*Author:作者
*
*/
namespace Assets.Scripts.Logic.Scenes
{
	public enum listType
	{
		treeMune=0,
		childrenBtn
	}
	public class InitConfigs
    {
		private static InitConfigs _instance;
        private InitConfigs()
        {
            initEquipKnowConfig();
            initReciprocalConfig();
        }
        public static InitConfigs Instance
        {
            get
            {
                if (_instance == null)
                    return _instance = new InitConfigs();
                else return _instance;
            }
        }
		private Dictionary<string, EquipKnow> childrenBtnDic;
		public Dictionary<string, EquipKnow> ChildrenBtnDic
		{
			get { return childrenBtnDic; }
		}
		private Dictionary<string, EquipKnow> equipKnowDic;

        public Dictionary<string, EquipKnow> EquipKnowDic
        {
            get { return equipKnowDic; }
        }
        private Dictionary<string, List<EquipKnow>> subjectDic;

        public Dictionary<string, List<EquipKnow>> SubjectDic
        {
            get { return subjectDic; }
        }

        private Dictionary<string, ReciprocalModel> reciprocalModelDic;

        public Dictionary<string, ReciprocalModel> ReciprocalModelDic
        {
            get { return reciprocalModelDic; }
        }

        #region 设备认知配置文档初始化
        void initEquipKnowConfig()
        {
            equipKnowDic = new Dictionary<string, EquipKnow>();
			childrenBtnDic = new Dictionary<string, EquipKnow>();
			subjectDic = new Dictionary<string, List<EquipKnow>>();
            XmlUtil xmlUtil = new XmlUtil(GlobalConfig.EquipKnowOneConfigPath, true);
            XmlNode node = xmlUtil.GetChildXPathNode("data")[0];
            subjectDic.Add("DeviceKnow1", readEquipKnowConfig(xmlUtil.GetChildNodes(node), xmlUtil,listType.treeMune));
            xmlUtil = new XmlUtil(GlobalConfig.EquipKnowTwoConfigPath, true);
            node = xmlUtil.GetChildXPathNode("data")[0];
            subjectDic.Add("DeviceKnow2", readEquipKnowConfig(xmlUtil.GetChildNodes(node), xmlUtil,listType.treeMune));

			XmlUtil BtnXmlUtil = new XmlUtil(GlobalConfig.EquipKnowButtonConfigPath, true);
			XmlNode BtnXmlnode = BtnXmlUtil.GetChildXPathNode("data")[0];
			readEquipKnowConfig(BtnXmlUtil.GetChildNodes(BtnXmlnode), BtnXmlUtil,listType.childrenBtn);
		}

        private List<EquipKnow> readEquipKnowConfig(XmlNodeList nodes, XmlUtil xmlUtil,listType listType, EquipKnow equip = null)
        {
            List<EquipKnow> items = new List<EquipKnow>();

            try
            {
                foreach (XmlNode node in nodes)
                {
                    EquipKnow item = new EquipKnow();
                    item.ParentEquipKnow = equip;
                    item.Name = xmlUtil.ReadAttrValue(node, "菜单名字");
                    item.Model = xmlUtil.ReadAttrValue(node, "模型名");
                    item.Detail = xmlUtil.ReadAttrValue(node, "详细信息");
                    item.Warm = xmlUtil.ReadAttrValue(node, "警告信息");
					string[] offs = xmlUtil.ReadAttrValue(node, "偏移").Split(',');
					if (xmlUtil.ReadAttrValue(node, "解说语音")!=null)
						item.audio = xmlUtil.ReadAttrValue(node, "解说语音");
		
					if (offs.Length == 3)
                        item.Offset = new Vector3(float.Parse(offs[0]), float.Parse(offs[1]), float.Parse(offs[2]));

                    if (xmlUtil.GetChildNodes(node).Count > 0)
                        item.EquipKnows.AddRange(readEquipKnowConfig(xmlUtil.GetChildNodes(node), xmlUtil, listType, item));

                    if (node.Attributes["范围"] != null)
                    {
                        string[] limit = xmlUtil.ReadAttrValue(node, "范围").Split(',');
                        float minyaw = float.Parse(limit[0]);
                        float maxyaw = float.Parse(limit[1]);
                        item.LimitYaw = new Vector2(maxyaw > minyaw ? minyaw : maxyaw, maxyaw > minyaw ? maxyaw : minyaw);
                        float minpitch = float.Parse(limit[2]);
                        float maxpitch = float.Parse(limit[3]);
                        item.LimitPitch = new Vector2(maxpitch > minpitch ? minpitch : maxpitch, maxpitch > minpitch ? maxpitch : minpitch);
                        float minDistance = float.Parse(limit[4]);
                        float maxDistance = float.Parse(limit[5]);
                        item.LimitDistance = new Vector2(maxDistance > minDistance ? minDistance : maxDistance, maxDistance > minDistance ? maxDistance : minDistance);
                    }
                    items.Add(item);
					if (listType == listType.treeMune)
					{
						if (equipKnowDic.ContainsKey(item.Model))
							Debug.LogWarning("已存在");
						else
							equipKnowDic.Add(item.Model, item);
					}
					else
					{
						item.showInfo= xmlUtil.ReadAttrValue(node, "显示名称");
						if (xmlUtil.ReadAttrValue(node, "隐藏模型").Split(',') != null)
						{
							string[] touming = xmlUtil.ReadAttrValue(node, "隐藏模型").Split(',');
							if (touming != null)
							{
								for (int i = 0; i < touming.Length; i++)
								{
									item.transparent.Add(touming[i]);
								}
							}
						}
						if (childrenBtnDic.ContainsKey(item.Model))
							Debug.LogWarning("已存在");
						else
							childrenBtnDic.Add(item.Name, item);
					}
                }
            }
            catch (Exception e)
            {
                Debug.LogError("读取xml文件错误"+e);
            }
            return items;
        }
        #endregion

        #region 培训配置文档初始化
        void initReciprocalConfig()
        {
            reciprocalModelDic = new Dictionary<string, ReciprocalModel>();
            XmlUtil xmlUtil = new XmlUtil(GlobalConfig.ReciprocalConfigPath, true);
            XmlNode nodeData = xmlUtil.GetChildXPathNode("data")[0];
            //Debug.Log("hello world");
            //Debug.Log(nodeData.InnerXml);
            foreach (XmlNode node in xmlUtil.GetChildNodesByName(nodeData, "交互"))
            {
                ReciprocalModel reciprocal = new ReciprocalModel();
                if (xmlUtil.GetChildNodes(node).Count != 0)
                {
                    foreach (XmlNode child in xmlUtil.GetChildNodesByName(node, "交互1"))
                    {
                        ReciprocalModel re = new ReciprocalModel();
                        setReciprocal(re, xmlUtil, child);
                        reciprocal.Reciprocal.Add(re.Model, re);
                        //reciprocalModelDic.Add(re.Model, re);
                    }
                }

                try
                {
                    setReciprocal(reciprocal, xmlUtil, node);
                }
                catch (Exception e)
                {
                    Debug.Log(e + xmlUtil.ReadAttrValue(node, "模型名"));
                }
                if (reciprocalModelDic.ContainsKey(reciprocal.Model))
                    Debug.LogWarning("已存在: " + reciprocal.Model);
                else
                    reciprocalModelDic.Add(reciprocal.Model, reciprocal);
            }
        }

        private void setReciprocal(ReciprocalModel reciprocal, XmlUtil xmlUtil, XmlNode node)
        {
            reciprocal.Model = xmlUtil.ReadAttrValue(node, "模型名");
            string[] offset = xmlUtil.ReadAttrValue(node, "偏移").Split(',');
            if (offset.Length == 3)
                reciprocal.Offset = new Vector3(float.Parse(offset[0]), float.Parse(offset[1]), float.Parse(offset[2]));

            if (node.Attributes["范围"] != null)
            {
                string[] limit = xmlUtil.ReadAttrValue(node, "范围").Split(',');
                float minyaw = float.Parse(limit[0]);
                float maxyaw = float.Parse(limit[1]);
                reciprocal.LimitYaw = new Vector2(maxyaw > minyaw ? minyaw : maxyaw, maxyaw > minyaw ? maxyaw : minyaw);
                float minpitch = float.Parse(limit[2]);
                float maxpitch = float.Parse(limit[3]);
                reciprocal.LimitPitch = new Vector2(maxpitch > minpitch ? minpitch : maxpitch, maxpitch > minpitch ? maxpitch : minpitch);
                float minDistance = float.Parse(limit[4]);
                float maxDistance = float.Parse(limit[5]);
                reciprocal.LimitDistance = new Vector2(maxDistance > minDistance ? minDistance : maxDistance, maxDistance > minDistance ? maxDistance : minDistance);
            }
        }
        #endregion

        
    }
}

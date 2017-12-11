using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Assets.Scripts.util;
using System.Xml;
using System.Collections;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:选项界面
*Author:作者
*
*/
namespace Assets.Scripts.Logic.UI.CommonUI
{
    public class Option
    {
        public Assets.Scripts.Logic.UI.CommonUI.SelectTopicUI.OptionType value = SelectTopicUI.OptionType.non;
        public string text;
	}

    public class SelectTopic
    {
        public int index = 0;
        public string content;
        public Assets.Scripts.Logic.UI.CommonUI.SelectTopicUI.OptionType result;
        public List<Option> options;
    }
    public class SelectTopicUI : SingletonUI<SelectTopicUI>
    {
        public UILabel TopicLabel;
        public UILabel ALabel;
        public UILabel BLabel;
        public UILabel CLabel;
        public UILabel DLabel;

        private UIToggle currentToggle;
        private SelectTopic currentTopic;
        private Action<SelectTopic, Option> action;
        private Dictionary<int, SelectTopic> selectTopicDic;
		public GameObject mask;
        public enum OptionType{
            non = 0,
            A = 1,
            B,
            C,
			D
        }
		private SelectTopic random(SelectTopic topic)
		{
			int resultIndex=0;
			for (int i = 0; i < topic.options.Count; i++)
			{
				if (topic.options[i].value==topic.result)
				{
					resultIndex = i;
				}
			}
			for (int i = topic.options.Count - 1; i >= 0; i--)
			{
				int index = UnityEngine.Random.Range(0, i+1);
				OptionType newType = topic.options[i].value;
				topic.options[i].value = topic.options[index].value;
				topic.options[index].value = newType;
			}
			topic.result = topic.options[resultIndex].value;
			return topic;
		}
		public void init(string xmlPath)
        {
            readConfig(xmlPath);
        }

        private void readConfig(string xmlPath)
        {
            this.selectTopicDic = new Dictionary<int, SelectTopic>();
            XmlUtil xmlUtil = new XmlUtil(xmlPath, true);
            XmlNode nodeDatas = xmlUtil.GetChildXPathNode("data")[0];
            foreach (XmlNode topicNode in xmlUtil.GetChildNodesByName(nodeDatas, "题目"))
            {
                SelectTopic topic = new SelectTopic();
                topic.index = Int32.Parse(xmlUtil.ReadAttrValue(topicNode, "编号"));
                topic.content = xmlUtil.ReadAttrValue(topicNode, "题目内容");
                topic.result = (OptionType)Enum.Parse(typeof(OptionType), xmlUtil.ReadAttrValue(topicNode, "答案"));
                topic.options = new List<Option>();
                foreach (XmlNode nodeOption in xmlUtil.GetChildNodesByName(topicNode, "选项"))
                {
                    Option option = new Option();
                    option.value = (OptionType)Enum.Parse(typeof(OptionType), xmlUtil.ReadAttrValue(nodeOption, "编号"));
                    option.text = xmlUtil.ReadAttrValue(nodeOption, "选项内容");
                    topic.options.Add(option);
                }
				this.selectTopicDic.Add(topic.index, topic);
            }
        }

        /// <summary>
        /// 初始化选项界面
        /// </summary>
        /// <param name="options"></param>
        public void showSelectTopicUI(int selectIndex, Action<SelectTopic, Option> action)
        {

            this.currentToggle = null;
            this.action = action;
            gameObject.SetActive(true);
            gameObject.transform.localPosition = Vector3.zero;
            this.currentTopic = selectTopicDic[selectIndex];
            TopicLabel.text = currentTopic.content;
			currentTopic=random(currentTopic);
            foreach (Option option in currentTopic.options)
            {
                switch (option.value)
                {
                    case OptionType.A:
                        ALabel.text = option.text;
                        NGUITools.FindInParents<UIToggle>(ALabel.gameObject).optionCanBeNone = true;
                        NGUITools.FindInParents<UIToggle>(ALabel.gameObject).value = false;
                        break;
                    case OptionType.B:
                        BLabel.text = option.text;
                        NGUITools.FindInParents<UIToggle>(BLabel.gameObject).optionCanBeNone = true;
                        NGUITools.FindInParents<UIToggle>(BLabel.gameObject).value = false;
                        break;
                    case OptionType.C:
                        CLabel.text = option.text;
                        NGUITools.FindInParents<UIToggle>(CLabel.gameObject).optionCanBeNone = true;
                        NGUITools.FindInParents<UIToggle>(CLabel.gameObject).value = false;
                        break;
					case OptionType.D:
						DLabel.text = option.text;
						NGUITools.FindInParents<UIToggle>(CLabel.gameObject).optionCanBeNone = true;
						NGUITools.FindInParents<UIToggle>(CLabel.gameObject).value = false;
						break;
				}
            }
        }


        public void change()
        {
            if (UIToggle.current.value)
                this.currentToggle = UIToggle.current;
        }

        /// <summary>
        /// 选择选项后提交
        /// </summary>
        public void Submit()
        {
            if (this.action != null)
            {
                if (currentToggle != null)
                {
                    OptionType type = (OptionType)Enum.Parse(typeof(OptionType), currentToggle.name);
                    Option option = currentTopic.options.Find(delegate(Option op)
                    {
                        if (op.value == type)
                            return true;
                        else return false;
                    });
                    this.action(this.currentTopic, option);
                }
                else
                {
                    this.action(this.currentTopic, new Option());
                }
            }
        }

        /// <summary>
        /// 判断对应选择和选项是否是答案
        /// </summary>
        /// <param name="selectIndex"></param>
        /// <param name="type"></param>
        public bool checkResult(int selectIndex, OptionType type)
        {
            if (selectTopicDic.ContainsKey(selectIndex))
            {
                if (selectTopicDic[selectIndex].result == type)
                    return true;
                else return false;
            }
            else
                return false;
        }
		public Dictionary<int, SelectTopic> getSelectTopicDic()
		{
			return selectTopicDic;
		}

	}
}

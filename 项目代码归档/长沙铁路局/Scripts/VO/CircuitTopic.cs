using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:电路原理属性
*Author:作者
*/
namespace Assets.Scripts.VO
{
    public class CircuitTopic
    {
        private int id;
        private string circuitName;
        private string topicContent;
        private string showValue;
        private bool onOff;
        private int subjectId;
        private float score;

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        /// <summary>
        /// 电路图名 
        /// </summary>
        public string CircuitName
        {
            get
            {
                return circuitName;
            }

            set
            {
                circuitName = value;
            }
        }

        /// <summary>
        /// 题目内容 
        /// </summary>
        public string TopicContent
        {
            get
            {
                return topicContent;
            }

            set
            {
                topicContent = value;
            }
        }
        /// <summary>
        /// 显示值
        /// </summary>
        public string ShowValue
        {
            get
            {
                return showValue;
            }

            set
            {
                showValue = value;
            }
        }
        
        /// <summary>
        /// 通断
        /// </summary>
        public bool OnOff
        {
            get
            {
                return onOff;
            }

            set
            {
                onOff = value;
            }
        }
        /// <summary>
        /// 科目Id 
        /// </summary>
        public int SubjectId
        {
            get
            {
                return subjectId;
            }

            set
            {
                subjectId = value;
            }
        }

        public float Score
        {
            get
            {
                return score;
            }

            set
            {
                score = value;
            }
        }
    }
}

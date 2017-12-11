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
*Desciption:功能说明:3维试题属性
*Author:作者
*/
namespace Assets.Scripts.VO
{
    public class Topic3D
    {
        private int id;
        private string topicContent;
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

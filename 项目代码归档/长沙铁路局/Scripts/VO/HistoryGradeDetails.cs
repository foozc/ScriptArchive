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
*Desciption:功能说明:历史成绩相关数据
*Author:作者
*/
namespace Assets.Scripts.VO
{
    public class HistoryGradeDetails
    {
        private int id;
        private int topicId;
        private string topicName;
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

        public int TopicId
        {
            get
            {
                return topicId;
            }

            set
            {
                topicId = value;
            }
        }

        public string TopicName
        {
            get
            {
                return topicName;
            }

            set
            {
                topicName = value;
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

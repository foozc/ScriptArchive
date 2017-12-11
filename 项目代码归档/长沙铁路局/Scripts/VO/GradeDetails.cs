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
*Desciption:功能说明:成绩相关数据
*Author:作者
*/
namespace Assets.Scripts.VO
{
    public class GradeDetails
    {
        private int id;
        private string gradeId;
        private int topic3DId;
        private string topic3DName;
        private int topic2DId;
        private string topic2DName;
        private int topicCircuitId;
        private string topicCircuitName;
        private float score;
        private string errorMsg;

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

        public string GradeId
        {
            get
            {
                return gradeId;
            }

            set
            {
                gradeId = value;
            }
        }

        public int Topic3DId
        {
            get
            {
                return topic3DId;
            }

            set
            {
                topic3DId = value;
            }
        }

        public string Topic3DName
        {
            get
            {
                return topic3DName;
            }

            set
            {
                topic3DName = value;
            }
        }

        public int Topic2DId
        {
            get
            {
                return topic2DId;
            }

            set
            {
                topic2DId = value;
            }
        }

        public string Topic2DName
        {
            get
            {
                return topic2DName;
            }

            set
            {
                topic2DName = value;
            }
        }

        public int TopicCircuitId
        {
            get
            {
                return topicCircuitId;
            }

            set
            {
                topicCircuitId = value;
            }
        }

        public string TopicCircuitName
        {
            get
            {
                return topicCircuitName;
            }

            set
            {
                topicCircuitName = value;
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

        public string ErrorMsg
        {
            get
            {
                return errorMsg;
            }

            set
            {
                errorMsg = value;
            }
        }
    }
}

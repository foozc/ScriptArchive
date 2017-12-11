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
*Desciption:功能说明:2维试题属性
*Author:作者
*/
namespace Assets.Scripts.VO
{
    public class Topic2D
    {
        private int id;
        private string topicContent;
        private string optionA;
        private string aImage;
        private string optionB;
        private string bImage;
        private string optionC;
        private string cImage;
        private string optionD;
        private string dImage;
        private string answers;
        private float score;
        private int subjectId;

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

        public string OptionA
        {
            get
            {
                return optionA;
            }

            set
            {
                optionA = value;
            }
        }

        public string AImage
        {
            get
            {
                return aImage;
            }

            set
            {
                aImage = value;
            }
        }

        public string OptionB
        {
            get
            {
                return optionB;
            }

            set
            {
                optionB = value;
            }
        }

        public string BImage
        {
            get
            {
                return bImage;
            }

            set
            {
                bImage = value;
            }
        }

        public string OptionC
        {
            get
            {
                return optionC;
            }

            set
            {
                optionC = value;
            }
        }

        public string CImage
        {
            get
            {
                return cImage;
            }

            set
            {
                cImage = value;
            }
        }

        public string OptionD
        {
            get
            {
                return optionD;
            }

            set
            {
                optionD = value;
            }
        }

        public string DImage
        {
            get
            {
                return dImage;
            }

            set
            {
                dImage = value;
            }
        }

        public string Answers
        {
            get
            {
                return answers;
            }

            set
            {
                answers = value;
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

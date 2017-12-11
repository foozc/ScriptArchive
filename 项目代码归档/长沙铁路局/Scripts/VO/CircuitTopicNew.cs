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
    public enum CircuitStudyMode {
        Image = 0,
        Movie,
        ImageMovie,
        Training,
    }

    public class CircuitTopicNew
    {
        private int id;
        private string circuitName;
        private string topicContent;
        private string optionA;
        private string optionB;
        private string optionC;
        private string answers;
        private float score;
        private int subjectId;
        private CircuitStudyMode studyMode;
        private string studyImage;
        private string studyMovie;
        private string studyText;
        private string trainMovie;

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
        /// <summary>
        /// 正确答案
        /// </summary>
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
        /// <summary>
        /// 分数
        /// </summary>
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
        /// <summary>
        /// 学习模式
        /// </summary>
        public CircuitStudyMode StudyMode
        {
            get
            {
                return studyMode;
            }

            set
            {
                studyMode = value;
            }
        }
        /// <summary>
        /// 图片名称
        /// </summary>
        public string StudyImage
        {
            get
            {
                return studyImage;
            }

            set
            {
                studyImage = value;
            }
        }
        /// <summary>
        /// 视频名称
        /// </summary>
        public string StudyMovie
        {
            get
            {
                return studyMovie;
            }

            set
            {
                studyMovie = value;
            }
        }
        /// <summary>
        /// 练习模式的视频名称
        /// </summary>
        public string TrainMovie
        {
            get
            {
                return trainMovie;
            }

            set
            {
                trainMovie = value;
            }
        }

        public string StudyText
        {
            get
            {
                return studyText;
            }

            set
            {
                studyText = value;
            }
        }
    }
}

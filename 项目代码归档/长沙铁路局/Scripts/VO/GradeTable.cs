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
*Desciption:功能说明:成绩显示界面显示的相关数据
*Author:作者
*/
namespace Assets.Scripts.VO
{
    public class GradeTable
    {
        private string id;
        private string examTime;
        private int countTime;
        private int userId;
        private string userName;
        private string userSex;
        private int subjectId;
        private string subjectName;
        private float grade;
        private string evaluate;

        public string Id
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

        public string ExamTime
        {
            get
            {
                return examTime;
            }

            set
            {
                examTime = value;
            }
        }

        public int UserId
        {
            get
            {
                return userId;
            }

            set
            {
                userId = value;
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

        public float Grade
        {
            get
            {
                return grade;
            }

            set
            {
                grade = value;
            }
        }

        public string Evaluate
        {
            get
            {
                return evaluate;
            }

            set
            {
                evaluate = value;
            }
        }

        public int CountTime
        {
            get
            {
                return countTime;
            }

            set
            {
                countTime = value;
            }
        }

        public string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                userName = value;
            }
        }

        public string UserSex
        {
            get
            {
                return userSex;
            }

            set
            {
                userSex = value;
            }
        }

        public string SubjectName
        {
            get
            {
                return subjectName;
            }

            set
            {
                subjectName = value;
            }
        }
    }
}

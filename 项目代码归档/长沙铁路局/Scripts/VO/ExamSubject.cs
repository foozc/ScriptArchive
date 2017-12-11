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
*Desciption:功能说明:考试科目属性
*Author:作者
*/
namespace Assets.Scripts.VO
{
    public class ExamSubject
    {
        private int id;
        private string name;
        private string parent;
        private int indoorOutdoor;
        private int equipId = 1;
        private bool isExam;
        private float examTime;

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

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Parent
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
            }
        }

        public int IndoorOutdoor
        {
            get
            {
                return indoorOutdoor;
            }

            set
            {
                indoorOutdoor = value;
            }
        }

        public int EquipId
        {
            get
            {
                return equipId;
            }

            set
            {
                equipId = value;
            }
        }

        public bool IsExam
        {
            get
            {
                return isExam;
            }

            set
            {
                isExam = value;
            }
        }

        public float ExamTime
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
    }
}

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
*Desciption:功能说明:分数列表元素设置
*Author:作者
*/
namespace Assets.Scripts.VO
{
    public class Score
    {
        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private User _user;

        public User User
        {
            get { return _user; }
            set { _user = value; }
        }
        private float _subOne;

        public float SubOne
        {
            get { return _subOne; }
            set { _subOne = value; }
        }
        private float _subTwo;

        public float SubTwo
        {
            get { return _subTwo; }
            set { _subTwo = value; }
        }
        private float _score;

        public float Score1
        {
            get { return _score; }
            set { _score = value; }
        }
        private DateTime _date;

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }
    }
}

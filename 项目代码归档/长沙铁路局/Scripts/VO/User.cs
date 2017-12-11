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
*Desciption:功能说明:用户属性
*Author:作者
*/
namespace Assets.Scripts.VO
{
    public class User
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _pwd;

        public string Pwd
        {
            get { return _pwd; }
            set { _pwd = value; }
        }
        private short _role;

        public short Role
        {
            get { return _role; }
            set { _role = value; }
        }

        private string _sex;

        public string Sex
        {
            get { return _sex; }
            set { _sex = value; }
        }

        private string _phone;

        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        private string _idCard;

        public string IdCard
        {
            get { return _idCard; }
            set { _idCard = value; }
        }

        private string _term;

        public string Term
        {
            get { return _term; }
            set { _term = value; }
        }

        private string _workshop;

        public string Workshop
        {
            get { return _workshop; }
            set { _workshop = value; }
        }
        private string _ids;

        public string Ids
        {
            get { return _ids; }
            set { _ids = value; }
        }
        private string _group;

        public string Group
        {
            get { return _group; }
            set { _group = value; }
        }

        private string _accountID;

        public string AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }
    }

    public class song
    {
        public string aa = "fawe";
    }
}

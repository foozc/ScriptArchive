using Assets.Scripts.VO;
using Mysql.Util;
using System;
using System.Collections.Generic;
using System.Data;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明：从数据库获得电路原理相关信息
*Author:作者
*
*/
namespace Assets.Scripts.DBLogic
{
    public class TopicCircuitDBHelper
    {
		public List<string> getCircuitNamesStudy()
        {
            string sql = "SELECT DISTINCT(CircuitName) FROM topicCircuit where studyMode < 3";
            MysqlHelper db = new MysqlHelper();
            DataTable tb = MysqlHelper.ExecuteDataTable(sql);
            List<string> pics = new List<string>();
            if (tb.Rows.Count > 0)
            {
                foreach (DataRow dr in tb.Rows)
                {
                    pics.Add(dr["CircuitName"].ToString());
                }
            }
            return pics;
        }

        public List<string> getCircuitNamesTrain()
        {
            string sql = "SELECT DISTINCT(CircuitName) FROM topicCircuit where studyMode = 3";
            MysqlHelper db = new MysqlHelper();
            DataTable tb = MysqlHelper.ExecuteDataTable(sql);
            List<string> pics = new List<string>();
            if (tb.Rows.Count > 0)
            {
                foreach (DataRow dr in tb.Rows)
                {
                    pics.Add(dr["CircuitName"].ToString());
                }
            }
            return pics;
        }

        public List<string> getCircuitNamesExam()
        {
            string sql = "SELECT DISTINCT(TopicContent) FROM topicCircuit where studyMode = 3";
            MysqlHelper db = new MysqlHelper();
            DataTable tb = MysqlHelper.ExecuteDataTable(sql);
            List<string> pics = new List<string>();
            if (tb.Rows.Count > 0)
            {
                foreach (DataRow dr in tb.Rows)
                {
                    pics.Add(dr["TopicContent"].ToString());
                }
            }
            return pics;
        }
        public List<float> getCircuitScoreExam()
        {
            string sql = "SELECT Score FROM topicCircuit where studyMode = 3";
            MysqlHelper db = new MysqlHelper();
            DataTable tb = MysqlHelper.ExecuteDataTable(sql);
            List<float> pics = new List<float>();
            if (tb.Rows.Count > 0)
            {
                foreach (DataRow dr in tb.Rows)
                {
                    pics.Add(float.Parse(dr["Score"].ToString()));
                }
            }
            return pics;
        }

        public CircuitTopicNew getCircuitTopicsNew(string CircuitName)
        {
            string sql = "SELECT * FROM topicCircuit WHERE CircuitName = '" + CircuitName + "'";
            MysqlHelper db = new MysqlHelper();
            DataTable tb = MysqlHelper.ExecuteDataTable(sql);
            CircuitTopicNew top = new CircuitTopicNew();
            if (tb.Rows.Count > 0)
            {
                foreach (DataRow dr in tb.Rows)
                {
                    top.Id = int.Parse(dr["Id"].ToString());
                    top.CircuitName = CircuitName;
                    top.TopicContent = dr["TopicContent"].ToString();
                    top.OptionA = dr["optionA"].ToString();
                    top.OptionB = dr["optionB"].ToString();
                    top.OptionC = dr["optionC"].ToString();
                    top.Answers = dr["Answers"].ToString();
                    top.Score = float.Parse(dr["Score"].ToString());
                    top.SubjectId = int.Parse(dr["SubjectId"].ToString());
                    top.StudyMode = (CircuitStudyMode)int.Parse(dr["studyMode"].ToString());
                    top.StudyImage = dr["studyImage"].ToString();
                    top.StudyMovie = dr["studyMovie"].ToString();
                    top.StudyText = dr["studyText"].ToString();
                    top.TrainMovie = dr["trainMovie"].ToString();
                }
            }
            return top;
        }

        public CircuitTopicNew getCircuitTopicsBack(string TopicContent)
        {
            string sql = "SELECT * FROM topicCircuit WHERE TopicContent = '" + TopicContent + "'";
            MysqlHelper db = new MysqlHelper();
            DataTable tb = MysqlHelper.ExecuteDataTable(sql);
            CircuitTopicNew top = new CircuitTopicNew();
            if (tb.Rows.Count > 0)
            {
                foreach (DataRow dr in tb.Rows)
                {
                    top.Id = int.Parse(dr["Id"].ToString());
                    top.TopicContent = TopicContent;
                    top.CircuitName = dr["CircuitName"].ToString();
                    top.OptionA = dr["optionA"].ToString();
                    top.OptionB = dr["optionB"].ToString();
                    top.OptionC = dr["optionC"].ToString();
                    top.Answers = dr["Answers"].ToString();
                    top.Score = float.Parse(dr["Score"].ToString());
                    top.SubjectId = int.Parse(dr["SubjectId"].ToString());
                    top.StudyMode = (CircuitStudyMode)int.Parse(dr["studyMode"].ToString());
                    top.StudyImage = dr["studyImage"].ToString();
                    top.StudyMovie = dr["studyMovie"].ToString();
                    top.StudyText = dr["studyText"].ToString();
                    top.TrainMovie = dr["trainMovie"].ToString();
                }
            }
            return top;
        }

        public List<CircuitTopic> getCircuitTopics(string CircuitName)
        {
            string sql = "SELECT * FROM topicCircuit WHERE CircuitName = '" + CircuitName + "'";
            MysqlHelper db = new MysqlHelper();
            DataTable tb = MysqlHelper.ExecuteDataTable(sql);
            List<CircuitTopic> circuits = new List<CircuitTopic>();
            if(tb.Rows.Count> 0)
            {
                foreach(DataRow dr in tb.Rows)
                {
                    CircuitTopic top = new CircuitTopic();
                    top.Id = int.Parse(dr["Id"].ToString());
                    top.CircuitName = CircuitName;
                    top.TopicContent = dr["TopicContent"].ToString();
                    top.ShowValue = dr["ShowValue"].ToString();
                    top.OnOff = dr["OnOff"].ToString().Equals("1") ? true : false;
                    top.SubjectId = int.Parse(dr["SubjectId"].ToString());
					top.Score = float.Parse(dr["Score"].ToString());
                    circuits.Add(top);
                }
            }
            return circuits;
        }

        
    }
}

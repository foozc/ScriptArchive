using Assets.Scripts.Logic.UI.Login;
using Assets.Scripts.VO;
using Mysql.Util;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Assets.Scripts.Tools;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明：从数据库获取分数信息
*Author:作者
*
*/
namespace Assets.Scripts.DBLogic
{
    public class ScoreDBHelper
    {
		public List<Score> getScore(int userID)
        {
            string sql = "select * from score where UserId = " + userID;
            MysqlHelper db = new MysqlHelper();
            DataTable tb = MysqlHelper.ExecuteDataTable(sql);
            List<Score> scores = new List<Score>();
            if (tb.Rows.Count > 0)
            {
                foreach (DataRow dr in tb.Rows)
                {
                    Score score = new Score();
                    score.Id = dr["ID"].ToString();
                    int userId = Int32.Parse(dr["UserId"].ToString());
                    //score.User = (new UserDBHelper()).getUser(userId, (short)RoleType.studentCheckbox);
                    score.SubOne = float.Parse(dr["SubOne"].ToString());
                    score.SubTwo = float.Parse(dr["SubTwo"].ToString());
                    score.Score1 = float.Parse(dr["Score"].ToString());
                    score.Date = DateTime.Parse(dr["date"].ToString());
                    scores.Add(score);
                }
            }
            MysqlHelper.CloseConn();
            return scores;
        }

        public List<Score> getScore(string Condition)
        {
            string sql;
            DataTable tb = null;
            List<Score> scores = new List<Score>();
            if (Condition == null || Condition.Equals(""))
            {
                sql = "SELECT * FROM score";
                tb = MysqlHelper.ExecuteDataTable(sql);
                if (tb.Rows.Count > 0)
                    foreach (DataRow dr in tb.Rows)
                    {
                        Score score = new Score();
                        score.Id = dr["ID"].ToString();
                        int userId = Int32.Parse(dr["UserId"].ToString());
                        //score.User = (new UserDBHelper()).getUser(userId, (short)RoleType.studentCheckbox);
                        score.SubOne = float.Parse(dr["SubOne"].ToString());
                        score.SubTwo = float.Parse(dr["SubTwo"].ToString());
                        score.Score1 = float.Parse(dr["Score"].ToString());
                        score.Date = DateTime.Parse(dr["date"].ToString());
                        scores.Add(score);
                    }
            }
            else
            {
                UserDBHelper db = new UserDBHelper();
                List<User> users = db.getUser(Condition, RoleType.studentCheckbox);
                foreach (User user in users)
                {
                    MySqlParameter[] pars = null;
                    sql = "SELECT * FROM score WHERE UserId = @Param1";
                    MySqlParameter p1 = new MySqlParameter("@Param1", MySqlDbType.String);
                    p1.Value = user.Id;
                    pars = new MySqlParameter[] { p1 };
                    tb = MysqlHelper.ExecuteDataTable(sql, pars);
                    if (tb.Rows.Count > 0)
                        foreach (DataRow dr in tb.Rows)
                        {
                            Score score = new Score();
                            score.Id = dr["ID"].ToString();
                            score.User = user;
                            score.SubOne = float.Parse(dr["SubOne"].ToString());
                            score.SubTwo = float.Parse(dr["SubTwo"].ToString());
                            score.Score1 = float.Parse(dr["Score"].ToString());
                            score.Date = DateTime.Parse(dr["date"].ToString());
                            scores.Add(score);
                        }
                }

            }
            MysqlHelper.CloseConn();
            return scores;
        }

        public bool updateScore(Score score)
        {
            string sql = "update score set SubOne = @Param1, SubTwo = @Param2, Score = @Param3, date = @Param4 where ID = @Param5";
            MySqlParameter p1 = new MySqlParameter("@Param1", MySqlDbType.Float);
            p1.Value = score.SubOne;
            MySqlParameter p2 = new MySqlParameter("@Param2", MySqlDbType.Float);
            p2.Value = score.SubTwo;
            MySqlParameter p3 = new MySqlParameter("@Param3", MySqlDbType.Float);
            p3.Value = score.Score1;
            MySqlParameter p4 = new MySqlParameter("@Param4", MySqlDbType.DateTime);
            p4.Value = score.Date;
            MySqlParameter p5 = new MySqlParameter("@Param5", MySqlDbType.String);
            p5.Value = score.Id;
            MySqlParameter[] pars = new MySqlParameter[] { p1, p2, p3, p4, p5 };
            int AffectNum = MysqlHelper.ExecuteNonQuery(sql, pars);
            
            if (AffectNum > 0)
                return true;
            else return false;
        }


        public Score addScore(Score score)
        {
            string sql = "insert into score values(@Param1, @Param2, @Param3, @Param4, @Param5, @Param6);";

            MySqlParameter p1 = new MySqlParameter("@Param1", MySqlDbType.String);
            score.Id = Utils.CreateId();
            p1.Value = score.Id;
            MySqlParameter p2 = new MySqlParameter("@Param2", MySqlDbType.Int32);
            p2.Value = score.User.Id;
            MySqlParameter p3 = new MySqlParameter("@Param3", MySqlDbType.Float);
            p3.Value = score.SubOne;
            MySqlParameter p4 = new MySqlParameter("@Param4", MySqlDbType.Float);
            p4.Value = score.SubTwo;
            MySqlParameter p5 = new MySqlParameter("@Param5", MySqlDbType.Float);
            p5.Value = score.Score1;
            MySqlParameter p6 = new MySqlParameter("@Param6", MySqlDbType.DateTime);
            score.Date = DateTime.Now;
            p6.Value = score.Date;
            MySqlParameter[] pars = new MySqlParameter[] { p1, p2, p3, p4, p5, p6 };
            int AffectNum = MysqlHelper.ExecuteNonQuery(sql, pars);
            if (AffectNum > 0)
                return score; 
            else return null;
        }
    }
}

using UnityEngine;
using Assets.Scripts.VO;
using System;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Text;
using Mysql.Util;
using System.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明：链接数据库
*Author:作者
*
*/
namespace Assets.Scripts.DBLogic
{
    public class InfoLinksDBHelper
    {
		public InfoLinksDBHelper()
        {

        }
        public int getGetTotalLinksNum(string type)
        {
            string sql = "select * from railway_infolinks where type = '" + type+"'";
            MysqlHelper db = new MysqlHelper();
            DataTable tb = MysqlHelper.ExecuteDataTable(sql);
            MysqlHelper.CloseConn();
            return tb.Rows.Count;
        }
        public bool deleteLink(int id)
        {
            string sql = "delete from railway_infolinks where ID = " + id;
            int AffectNum = MysqlHelper.ExecuteNonQuery(sql);
            if (AffectNum > 0)
                return true;
            else return false;
        }
        public bool updateLink(LinkInfo info)
        {
            string sql = "update railway_infolinks set address='" + info.Address + "' where id='" + info.Id + "';";
            int AffectNum = MysqlHelper.ExecuteNonQuery(sql);
            if (AffectNum > 0)
                return true;
            else return false;
        }

        public bool addLink(LinkInfo info)
        {
            string sql = "insert into railway_infolinks (type,address) values('" + info.Type + "', '" + info.Address +
                "');";
            // UnityEngine.Debug.Log(sql);
            int AffectNum = MysqlHelper.ExecuteNonQuery(sql);
            if (AffectNum > 0)
                return true;
            else return false;
        }
        public string getGetLinks(string type,int pageNum,int num)
        {
            string sql = "select * from railway_infolinks where type = '" + type + "' order by id desc limit "+ (pageNum-1) * num + "," + pageNum* num;
            string linksStr = "";
            MysqlHelper db = new MysqlHelper();
            DataTable tb = MysqlHelper.ExecuteDataTable(sql);
            List<LinkInfo> infolist = new List<LinkInfo>();
            if (tb.Rows.Count > 0)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    DataRow dr = tb.Rows[i];
                    LinkInfo info = new LinkInfo();
                    info.Address = dr["address"].ToString();
                    info.Id = int.Parse(dr["id"].ToString());
                    infolist.Add(info);

                }
                MysqlHelper.CloseConn();
                return JsonConvert.SerializeObject(infolist);
            }
            else
            {
                MysqlHelper.CloseConn();
                return JsonConvert.SerializeObject(infolist);
            }
        }
    }
}    

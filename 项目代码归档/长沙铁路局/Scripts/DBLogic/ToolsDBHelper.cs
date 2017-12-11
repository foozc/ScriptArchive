using UnityEngine;
using System.Collections;
using Assets.Scripts.VO;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;
using Mysql.Util;
using System.Data;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明：从数据库获得工具信息
*Author:作者
*
*/
public class ToolsDBHelper{
	public ToolsDBHelper()
    {

    }

    public List<ToolModel> getTools()
    {

        List<ToolModel> mList = new List<ToolModel>();
        string sql = "select * from railway_toolbase";
        DataTable tb = MysqlHelper.ExecuteDataTable(sql);
        if (tb.Rows.Count > 0)
        {
            for (int i = 0; i < tb.Rows.Count; i++)
            {
                DataRow dr = tb.Rows[i];
                ToolModel tool = new ToolModel();
                tool.Index = Int32.Parse(dr["seq"].ToString());
                tool.Name = dr["name"].ToString();
                tool.SpriteName = dr["pathname"].ToString();
				if (tool.SpriteName==null||tool.SpriteName.Equals(""))
				{
					continue;
				}
                tool.Id = dr["id"].ToString();
                mList.Add(tool);
            }

        }
        else
        {
            MysqlHelper.CloseConn();
            return null;
        }
        MysqlHelper.CloseConn();
        return mList;
    }

 

}

/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	HistoryUtil 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年11月23日14:55:23# 
 *Description: 		   	历史记录折线图工具类   
 *History: 				修改版本记录                 
*/

using assest;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class HistoryUtil
{

    private string Day;                 //当前日期
    private int Hour;                   //当前小时
    public string tableName;            //查询的设备表名
    public int columnIndex;             //查询的字段名
    public string columnName;           //字段的中文名
    public string brand;                //查询的品牌

    public HistoryUtil(string day, int hour, string tableName, int columnIndex, string brand)
    {
        this.Day = day;
        this.Hour = hour;
        this.tableName = tableName;
        this.columnIndex = columnIndex;
        this.brand = brand;
        GetColmunName();
    }

    /// <summary>
    /// 获取数据库字段名
    /// </summary>
    private void GetColmunName()
    {
        string[] items = { "all_device." + columnIndex.ToString() };
        string[] col = { "name" };
        string[] operation = { "=" };
        string[] values = { tableName };
        DataSet ds = Init._instance.GetSqlAccess().SelectWhere("all_device", items, col, operation, values);
        DataTable table = ds.Tables[0];
        DataRow dataRow = table.Rows[0];
        if (!string.IsNullOrEmpty(dataRow[0].ToString()))
        {
            this.columnName = dataRow[0].ToString();
        }
    }

    /// <summary>
    /// 获取小时的平均值坐标点
    /// </summary>
    public List<Vector2> GetHourPoints()
    {
        List<Vector2> points = new List<Vector2>();
        for (int i = 0; i < Hour; i++)
        {
            string startTime = Day + i.ToString().PadLeft(2, '0') + "00";
            string endTime = Day + i.ToString().PadLeft(2, '0') + "59";
            //以小时求取平均值，保留2位小数
            string sql = "SELECT FORMAT((SELECT AVG(`" + columnName + "`) FROM " + tableName + " WHERE realTime BETWEEN " + startTime
                       + " AND " + endTime + "),2)";
            Debug.Log(sql);
            DataSet ds = SqlAccess.ExecuteQuery(sql);
            if (ds != null)
            {
                DataTable table = ds.Tables[0];
                if (table.Rows.Count > 0)
                {
                    DataRow dataRow = table.Rows[0];
                    if (!string.IsNullOrEmpty(dataRow[0].ToString()))
                    {
                        float hourAvgValue = float.Parse(dataRow[0].ToString());
                        points.Add(new Vector2((i), hourAvgValue));
                    }
                }
            }
        }
        return points;
    }

    /// <summary>
    /// 获取每5分钟的平均值
    /// </summary>
    /// <param name="hour"></param>
    public List<Vector2> GetMinutePoints(int hour)
    {
        List<Vector2> points = new List<Vector2>();

        string hourTime = hour.ToString().PadLeft(2, '0');
        string startTime, endTime;
        for (int i = 0; i < 12; i++)
        {
            startTime = Day + hourTime + (i * 5).ToString().PadLeft(2, '0');
            endTime = Day + hourTime + (i * 5 + 4).ToString().PadLeft(2, '0');
            //以每5分钟求取平均值，保留2位小数
            string sql = "SELECT FORMAT((SELECT AVG(`" + columnName + "`) FROM " + tableName + " WHERE realTime BETWEEN " + startTime
                       + " AND " + endTime + "),2)";
            DataSet ds = SqlAccess.ExecuteQuery(sql);
            if (ds != null)
            {
                DataTable table = ds.Tables[0];
                if (table.Rows.Count > 0)
                {
                    DataRow dataRow = table.Rows[0];
                    if (!string.IsNullOrEmpty(dataRow[0].ToString()))
                    {
                        float minuteAvgValue = float.Parse(dataRow[0].ToString());
                        points.Add(new Vector2((i + 1) * 5, minuteAvgValue));
                    }
                }
            }
        }
        return points;
    }

    /// <summary>
    /// 获取标准值
    /// </summary>
    /// <returns></returns>
    public float GetStandardValue()
    {
        string tableName_limit = tableName.Replace("_realtime", "_limit");
        string[] items = { columnName };
        string[] col = { "brand", "options" };
        string[] operation = { "=", "=" };
        string[] values = { brand, "standard_value" };
        DataSet ds = Init._instance.GetSqlAccess().SelectWhere(tableName_limit, items, col, operation, values);
        if (ds != null)
        {
            DataTable table = ds.Tables[0];
            if (table.Rows.Count > 0)
            {
                DataRow dataRow = table.Rows[0];
                return float.Parse(dataRow[0].ToString());
            }
        }
        return 100;
    }

    /// <summary>
    /// 获取最大值
    /// </summary>
    /// <param name="points"></param>
    /// <param name="standardValue"></param>
    /// <returns></returns>
    public float GetMaxYValue(List<Vector2> points, float standardValue)
    {
        List<float> values = new List<float>();
        for (int i = 0; i < points.Count; i++)
        {
            values.Add(points[i].y);
        }
        values.Add(standardValue);

        float max = values[0];
        for (int i = 0; i < values.Count; i++)
        {
            if (values[i] > max)
            {
                max = values[i];
            }
        }
        return max;
    }

    /// <summary>
    /// 获取最小值
    /// </summary>
    /// <param name="points"></param>
    /// <param name="standardValue"></param>
    /// <returns></returns>
    public float GetMinYValue(List<Vector2> points, float standardValue)
    {
        List<float> values = new List<float>();
        for (int i = 0; i < points.Count; i++)
        {
            values.Add(points[i].y);
        }
        values.Add(standardValue);

        float min = values[0];
        for (int i = 0; i < values.Count; i++)
        {
            if (values[i] < min)
            {
                min = values[i];
            }
        }
        return min;
    }
}

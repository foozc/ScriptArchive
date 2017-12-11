/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	PointUtil 
 *Author:       	   	#guocaizhi# 
 *Date:         	   	#2017年11月14日15:23:54# 
 *Description: 		   	折线图坐标点工具类   
 *History: 				修改版本记录
 *                      2017年11月14日15:24:13  yulong  修改相关逻辑
*/

using System.Collections.Generic;
using UnityEngine;
using System.Data;

public class PointUtil
{
    private string Day;             //当前日期
    private int HourTime;           //当前小时
    public string tableName;        //查询的设备表名
    public int columnIndex;         //查询的字段名
    public string columnName;       //字段的中文名
    public string brand;               //查询的品牌
    public Dictionary<int, double> HourCoordinates;                                                       //小时的坐标信息
    public Dictionary<int, Dictionary<int, double>> MinuteCoordinates;                                    //分钟的坐标信息

    public PointUtil(string day,int hourTime, string tableName, int columnIndex, string brand)
    {
        this.Day = day;
        this.HourTime = hourTime;
        this.tableName = tableName;
        this.columnIndex = columnIndex;
        this.brand = brand;
        GetColmunName();
        InitCoordinates();
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
        string columnName = dataRow[0].ToString();
        this.columnName = columnName;
    }

    /// <summary>
    /// 初始化坐标数据
    /// </summary>
    private void InitCoordinates()
    {
        HourCoordinates = new Dictionary<int, double>();
        MinuteCoordinates = new Dictionary<int, Dictionary<int, double>>();

        for (int i = 0; i < HourTime; i++)
        {
            //日期后面加小时
            string hour = (Day + i.ToString().PadLeft(2, '0'));
            for (int j = 0; j < 60; j++)
            {
                //日期后面分钟
                string minute = (hour + j.ToString().PadLeft(2, '0'));
                EveryMinuteAverageValue(i, j, minute);
            }
        }
    }

    /// <summary>
    /// 每一分钟的平均值
    /// </summary>
    /// <returns></returns>
    private void EveryMinuteAverageValue(int hour, int minute, string time)
    {
        Dictionary<int, double> dicMinute;

        if (MinuteCoordinates.ContainsKey(hour))
        {
            dicMinute = MinuteCoordinates[hour];
        }
        else
        {
            dicMinute = new Dictionary<int, double>();
        }

        string[] items = { columnName };
        string[] col = { "brand", "realTime" };
        string[] operation = { "=", "=" };
        string[] values = { brand, time };
        DataSet ds = Init._instance.GetSqlAccess().SelectWhere(tableName, items, col, operation, values);
        if (ds != null)
        {
            DataTable table = ds.Tables[0];
            int count = table.Rows.Count;
            double MinuteValue = 0;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    DataRow dataRow = table.Rows[i];
                    MinuteValue += double.Parse(dataRow[0].ToString());
                }
                if (!dicMinute.ContainsKey(minute))
                {
                    dicMinute.Add(minute, MinuteValue / count);
                    if (!MinuteCoordinates.ContainsKey(hour))
                    {
                        MinuteCoordinates.Add(hour, dicMinute);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 获取小时的坐标点
    /// </summary>
    /// <returns></returns>
    public List<Vector2> GetHourPoints()
    {
        List<Vector2> points = new List<Vector2>();
        foreach (int hour in MinuteCoordinates.Keys)
        {
            double value = 0;
            int count = MinuteCoordinates[hour].Count;
            foreach (KeyValuePair<int, double> kvp in MinuteCoordinates[hour])
            {
                value += kvp.Value;
            }
            points.Add(new Vector2(hour, (float)value / count));
        }
        return points;
    }

    /// <summary>
    /// 获取分钟的坐标点
    /// </summary>
    /// <param name="hour"></param>
    /// <returns></returns>
    public List<Vector2> GetMinutePoints(int hour)
    {
        List<Vector2> points = new List<Vector2>();
        if (MinuteCoordinates.ContainsKey(hour))
        {
            Dictionary<int, double> dic = MinuteCoordinates[hour];
            foreach (KeyValuePair<int, double> kv in dic)
            {
                points.Add(new Vector2(kv.Key, (float)kv.Value));
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

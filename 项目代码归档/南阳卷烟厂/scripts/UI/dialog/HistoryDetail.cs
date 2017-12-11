/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	HistoryDetail 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年11月10日14:11:58# 
 *Description: 		   	设备信息历史记录折线图详情   
 *History: 				修改版本记录
*/

using System;
using UnityEngine;

public class HistoryDetail : MonoBehaviour
{
    [HideInInspector]
    public GameObject HistoryDetail_Hour;           //按小时为单位的折线图
    [HideInInspector]
    public GameObject HistoryDetail_Minute;         //按分钟为单位的折线图
    [HideInInspector]
    public HistoryDetailByHour hdbh;                //控制脚本
    [HideInInspector]
    public HistoryDetailByMinute hdbm;              //控制脚本

    public static HistoryDetail _instance;

    public GameObject hoverImage;
    public GameObject xAxisLinePrefab;
    public GameObject yAixsLinePrefab;
    public GameObject labelPrefab;                      //文本预制体、
    public GameObject pointPrefab;                      //坐标点预制体
    public GameObject linePrefab;                       //折线预制体

    [HideInInspector]
    public string brand;                            //当前品牌
    [HideInInspector]
    public string deviceName;                       //设备名
    [HideInInspector]
    public string titleName;                        //条目项名
    [HideInInspector]
    public string tableName;                        //数据库表名
    [HideInInspector]
    public int columnIndex;                         //信息栏目序号

    public HistoryUtil util;

    [HideInInspector]
    public string time;
    [HideInInspector]
    public int HourTime;
    private int skipDayNum = 0;

    void Awake()
    {
        _instance = this;
        hoverImage.SetActive(false);
        HistoryDetail_Hour = transform.Find("HistoryDetail_Hour").gameObject;
        HistoryDetail_Minute = transform.Find("HistoryDetail_Minute").gameObject;
        hdbh = HistoryDetail_Hour.GetComponent<HistoryDetailByHour>();
        hdbm = HistoryDetail_Minute.GetComponent<HistoryDetailByMinute>();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="tableName">数据库表明</param>
    /// <param name="index">条目序号</param>
    public void Init(string brand,string deviceName, string titleName, string tableName, int index)
    {
        this.brand = brand;
        this.deviceName = deviceName;
        this.titleName = titleName;
        this.tableName = tableName;
        this.columnIndex = index;

        time = DateTime.Now.ToString("yyyyMMdd");
        //time = "20171115";
        HourTime = int.Parse(DateTime.Now.ToString("HH"));
        //HourTime = 23;
        util = new HistoryUtil(time, HourTime, tableName, columnIndex, brand);

        HistoryDetail_Hour.SetActive(true);
        HistoryDetail_Minute.SetActive(false);

        hdbh.Init();
    }

    public void Destroy()
    {
        HistoryDetail_Hour.SetActive(false);
        HistoryDetail_Minute.SetActive(false);
        skipDayNum = 0;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 往前一天查询：天数间隔不超过7天
    /// </summary>
    public void LastDay()
    {
        if (Mathf.Abs(skipDayNum - 1) < 7)
        {
            HistoryDetail_Hour.SetActive(false);
            HistoryDetail_Minute.SetActive(false);
            skipDayNum--;
            DateTime skipDay = DateTime.Now.AddDays(skipDayNum);
            time = skipDay.ToString("yyyyMMdd");
            HourTime = 24;
            util = new HistoryUtil(time, HourTime, tableName, columnIndex, brand);
            HistoryDetail_Hour.SetActive(true);
            HistoryDetail_Minute.SetActive(false);
            hdbh.Init();
        }
    }

    /// <summary>
    /// 往后一天查询：不得超过今天
    /// </summary>
    public void NextDay()
    {
        if (skipDayNum + 1 <= 0)
        {
            HistoryDetail_Hour.SetActive(false);
            HistoryDetail_Minute.SetActive(false);
            skipDayNum++;
            DateTime skipDay = DateTime.Now.AddDays(skipDayNum);
            time = skipDay.ToString("yyyyMMdd");
            HourTime = 24;
            util = new HistoryUtil(time, HourTime, tableName, columnIndex, brand);
            HistoryDetail_Hour.SetActive(true);
            HistoryDetail_Minute.SetActive(false);
            hdbh.Init();
        }
    }
}

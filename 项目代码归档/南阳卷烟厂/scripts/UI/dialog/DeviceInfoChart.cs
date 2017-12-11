/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	DeviceInfoChart 
 *Author:       	   	#FuZhiChao# 
 *Date:         	   	#2017年10月26日15:09:38# 
 *Description: 		   	设备信息图表详情：点击详情弹出设备信息曲线   
 *History: 				修改版本记录
 *                      yulong--2017年11月7日18:51:42：去掉小时和天的切换，添加进入详情按钮
*/

using UnityEngine;
using UnityEngine.UI;
using assest;
using System.Data;


public class DeviceInfoChart : MonoBehaviour
{
    public Text infoTitle;
    public Button hide;
    public Button detail;
    public GameObject detailQuxian;

    //设置时间间隔
    private int TimeInterval = 1;

    /// <summary>
    /// 表格中七个点位
    /// </summary>
    public GameObject[] Sites;

    /// <summary>
    /// 垂直坐标值切换
    /// </summary>
    public GameObject[] VerticalSites;


    /// <summary>
    /// div line
    /// </summary>
    public GameObject[] PrintLines;

    private string TempDeviceName;
    private string TempTableName;
    private string TempTitle;
    private int TempIndex = 0;

    void Awake()
    {
        hide.onClick.AddListener(() =>          //隐藏
        {
            gameObject.SetActive(false);
        });
        detail.onClick.AddListener(() =>         //进入详情 
        {
            detailQuxian.SetActive(true);
            HistoryDetail hd = detailQuxian.GetComponent<HistoryDetail>();
            //hd.Init(TempDeviceName, TempTitle, TempTableName, TempIndex);
        });
    }

    /// <summary>
    /// 初始化折线图
    /// </summary>
    /// <param name="tableName">数据库表名</param>
    /// <param name="titleName">信息条目标题</param>
    /// <param name="index">标题序号</param>
    public void InitChart(string deviceName, string tableName, string titleName, int index)
    {
        TempDeviceName = deviceName;
        TempTableName = tableName;
        TempTitle = titleName;
        TempIndex = index;
        infoTitle.text = titleName;

        initSites();
        initLines();

        ///选项名
        string keyName = FindKey(tableName, index);

        ///获取当前时间
        DataSet ds = SqlAccess.ExecuteQuery("SELECT MAX(realTime) FROM " + tableName);
        DataTable dt = ds.Tables[0];
        ///Debug.Log(dt.Rows[0][0]);  201710232200

        ///单位小时
        if (TimeInterval == 1)
        {
            ///当前时间
            int nowHour = getHourNow(dt.Rows[0][0].ToString());
            ///总共需要显示的点数 - 1
            int allHour = nowHour / 4 + 1;
            ///当前时间与整四点差
            int remainingHour = nowHour % 4;
            ///当前时间最近的整四点时间
            int endHour = nowHour - remainingHour;
            string[] allSite = new string[allHour + 1];
            for (int i = 0; i < allHour; i++)
            {

                DataSet ds1 = SqlAccess.ExecuteQuery("SELECT `" + keyName + "` FROM " + tableName +
                    " WHERE realTime = " + CutSomeHour(changeHour(dt.Rows[0][0].ToString(), endHour), i * 4));
                DataTable dt1 = ds1.Tables[0];
                //Debug.Log("endhour:" + CutSomeHour(changeHour(dt.Rows[0][0].ToString(), endHour), i * 4));
                if (dt1.Rows.Count >= 1)
                {
                    //Debug.Log("CutOneHour(changeHour(dt.Rows[0][0].ToString(),endHour), i*4):  " + 
                    //    CutOneHour(changeHour(dt.Rows[0][0].ToString(), endHour), i * 4));
                    //Debug.Log("dt.Rows[0][0]:" + dt1.Rows[0][0].ToString());
                    allSite[allHour - i - 1] = dt1.Rows[0][0].ToString();
                }
                else
                {
                    allSite[allHour - i - 1] = "0";
                }
            }
            DataSet ds2 = SqlAccess.ExecuteQuery("SELECT `" + keyName + "` FROM " + tableName + " WHERE realTime = " + dt.Rows[0][0].ToString());
            DataTable dt2 = ds2.Tables[0];
            allSite[allHour] = dt2.Rows[0][0].ToString();

            ///-160 -106.67 -53.34 0 53.34 106.67
            ///
            double maxValue = findMaxString(allSite);
            double minValue = findMinString(allSite);

            ///修改纵轴坐标值
            VerticalSites[0].GetComponent<Text>().text = minValue.ToString();

            ////当值全一样时，只有一条直线
            if (maxValue == minValue)
            {
                VerticalSites[1].GetComponent<Text>().text = "";
                VerticalSites[2].GetComponent<Text>().text = "";
                VerticalSites[3].GetComponent<Text>().text = "";
                VerticalSites[4].GetComponent<Text>().text = "";
            }
            else
            {
                VerticalSites[1].GetComponent<Text>().text = ((maxValue - minValue) / 4 + minValue).ToString();
                VerticalSites[2].GetComponent<Text>().text = ((maxValue - minValue) / 2 + minValue).ToString();
                VerticalSites[3].GetComponent<Text>().text = ((maxValue - minValue) * 3 / 4 + minValue).ToString();
                VerticalSites[4].GetComponent<Text>().text = maxValue.ToString();
            }
            double[] Yvalues = new double[allSite.Length];
            double[] Xvalues = new double[allSite.Length];
            int Ykey = 0;
            //lineRenderer.positionCount = allSite.Length;
            foreach (string item in allSite)
            {
                Xvalues[Ykey] = -160 + Ykey * 53.33;
                if (maxValue == minValue)
                {
                    //Yvalues[Ykey++] = -53.5;
                    Yvalues[Ykey++] = -107;
                }
                else
                {
                    Yvalues[Ykey++] = 214 * (double.Parse(item) - minValue) / (maxValue - minValue) - 107;
                }
                Sites[Ykey - 1].SetActive(true);
                ///描点
                Sites[Ykey - 1].transform.localPosition = new Vector3((float)Xvalues[Ykey - 1], (float)Yvalues[Ykey - 1], -1f);

                ///描线
                if (Ykey == allSite.Length)
                {
                    ///修正最后一个点位坐标nowHour 与画线
                    if (nowHour % 4 == 0)
                    {
                        Sites[Ykey - 1].SetActive(false);
                        //  lineRenderer.SetPosition(Ykey - 1, Sites[Ykey - 2].transform.localPosition);

                    }
                    else
                    {
                        Sites[Ykey - 1].transform.localPosition = new Vector3(320 * nowHour / 24 - 160, Sites[Ykey - 1].transform.localPosition.y, -1f);
                        if (Ykey - 1 > 0)
                        {
                            printLine(Sites[Ykey - 2].transform.localPosition.x, Sites[Ykey - 2].transform.localPosition.y,
                                Sites[Ykey - 1].transform.localPosition.x, Sites[Ykey - 1].transform.localPosition.y, Ykey - 2);
                        }
                        //  lineRenderer.SetPosition(Ykey - 1, Sites[Ykey - 1].transform.localPosition);
                    }
                    //lineRenderer.SetPosition(Ykey - 1, Sites[Ykey - 2].transform.position);
                }
                else
                {
                    if (Ykey - 1 > 0)
                    {
                        printLine(Sites[Ykey - 2].transform.localPosition.x, Sites[Ykey - 2].transform.localPosition.y,
                            Sites[Ykey - 1].transform.localPosition.x, Sites[Ykey - 1].transform.localPosition.y, Ykey - 2);
                    }
                    //   lineRenderer.SetPosition(Ykey - 1, Sites[Ykey - 1].transform.localPosition);
                }

            }

        }

        ///单位日
        else if (TimeInterval == 24)
        {
            ///当前日期
            int nowDay = getDayNow(dt.Rows[0][0].ToString());
            Debug.Log("nowday: " + nowDay);
            ///总共需要显示的点数 -1
            int allDay = (nowDay - 1) / 5 + 1;
            Debug.Log("allDay: " + allDay);
            ///当前时间与整五+1天差 且初始为1 不为0
            int remainingDay = (nowDay - 1) % 5;
            ///当前时间最近的整五+1天时间
            int endDay = nowDay - remainingDay;

            ///all day + 1 正确需要显示点数
            string[] allSite = new string[allDay + 1];
            for (int i = 0; i < allDay; i++)
            {

                DataSet ds1 = SqlAccess.ExecuteQuery("SELECT `" + keyName + "` FROM " + tableName +
                    " WHERE realTime = " + CutSomeDay(changeDay(dt.Rows[0][0].ToString(), endDay), i * 5));
                DataTable dt1 = ds1.Tables[0];
                Debug.Log("endhour:" + CutSomeDay(changeDay(dt.Rows[0][0].ToString(), endDay), i * 5));
                if (dt1.Rows.Count >= 1)
                {
                    //Debug.Log("CutOneHour(changeHour(dt.Rows[0][0].ToString(),endHour), i*4):  " + 
                    //    CutOneHour(changeHour(dt.Rows[0][0].ToString(), endHour), i * 4));
                    //Debug.Log("dt.Rows[0][0]:" + dt1.Rows[0][0].ToString());
                    allSite[allDay - i - 1] = dt1.Rows[0][0].ToString();
                }
                else
                {
                    allSite[allDay - i - 1] = "0";
                }
            }
            DataSet ds2 = SqlAccess.ExecuteQuery("SELECT `" + keyName + "` FROM " + tableName + " WHERE realTime = " + dt.Rows[0][0].ToString());
            DataTable dt2 = ds2.Tables[0];
            allSite[allDay] = dt2.Rows[0][0].ToString();

            ///-160 -106.67 -53.34 0 53.34 106.67
            ///
            double maxValue = findMaxString(allSite);
            double minValue = findMinString(allSite);


            ///修改纵轴坐标值
            VerticalSites[0].GetComponent<Text>().text = minValue.ToString();
            ////当值全一样时，只有一条直线
            if (maxValue == minValue)
            {
                VerticalSites[1].GetComponent<Text>().text = "";
                VerticalSites[2].GetComponent<Text>().text = "";
                VerticalSites[3].GetComponent<Text>().text = "";
                VerticalSites[4].GetComponent<Text>().text = "";
            }
            else
            {
                VerticalSites[1].GetComponent<Text>().text = ((maxValue - minValue) / 4 + minValue).ToString();
                VerticalSites[2].GetComponent<Text>().text = ((maxValue - minValue) / 2 + minValue).ToString();
                VerticalSites[3].GetComponent<Text>().text = ((maxValue - minValue) * 3 / 4 + minValue).ToString();
                VerticalSites[4].GetComponent<Text>().text = maxValue.ToString();
            }

            double[] Yvalues = new double[allSite.Length];
            double[] Xvalues = new double[allSite.Length];
            int Ykey = 0;
            Debug.Log("allSite.Length" + allSite.Length);
            foreach (string item in allSite)
            {
                Xvalues[Ykey] = -160 + Ykey * 53.33;
                if (maxValue == minValue)
                {
                    //Yvalues[Ykey++] = -53.5;
                    Yvalues[Ykey++] = -107;
                }
                else
                {
                    Yvalues[Ykey++] = 214 * (double.Parse(item) - minValue) / (maxValue - minValue) - 107;
                }

                //Debug.Log("x: " + Xvalues[Ykey - 1]);
                //Debug.Log("y: " + Yvalues[Ykey - 1]);
                Debug.Log("value：" + item);
                Sites[Ykey - 1].SetActive(true);
                ///描点
                Sites[Ykey - 1].transform.localPosition = new Vector3((float)Xvalues[Ykey - 1], (float)Yvalues[Ykey - 1], -1f);

                ///描线
                if (Ykey == allSite.Length)
                {
                    ///修正最后一个点位坐标nowHour 与画线
                    if ((nowDay - 1) % 5 == 0)
                    {
                        Sites[Ykey - 1].SetActive(false);
                        //  lineRenderer.SetPosition(Ykey - 1, Sites[Ykey - 2].transform.position);

                    }
                    else
                    {
                        Sites[Ykey - 1].transform.localPosition = new Vector3(320 * (nowDay - 1) / 30 - 160, Sites[Ykey - 1].transform.localPosition.y, -1f);
                        if (Ykey - 1 > 0)
                        {
                            printLine(Sites[Ykey - 2].transform.localPosition.x, Sites[Ykey - 2].transform.localPosition.y,
                                Sites[Ykey - 1].transform.localPosition.x, Sites[Ykey - 1].transform.localPosition.y, Ykey - 2);
                        }
                        //     lineRenderer.SetPosition(Ykey - 1, Sites[Ykey - 1].transform.position);
                    }
                }
                else
                {
                    if (Ykey - 1 > 0)
                    {
                        printLine(Sites[Ykey - 2].transform.localPosition.x, Sites[Ykey - 2].transform.localPosition.y,
                            Sites[Ykey - 1].transform.localPosition.x, Sites[Ykey - 1].transform.localPosition.y, Ykey - 2);
                    }
                    //    lineRenderer.SetPosition(Ykey - 1, Sites[Ykey - 1].transform.position);
                }

            }
        }
        ///时间分钟
        else if (TimeInterval == 60)
        {
            ///当前分钟
            int nowMinute = getMinuteNow(dt.Rows[0][0].ToString());
            if (nowMinute == 0)
            {
                nowMinute = getMinuteNow(UpdateMinuteString(dt.Rows[0][0].ToString()));
            }
            Debug.Log("nowMinute: " + nowMinute);
            ///总共需要显示的点数 -1
            int allMinute = nowMinute / 10 + 1;
            Debug.Log("allDay: " + allMinute);
            ///当前时间与整十分差 且初始为1 不为0
            int remainingMinute = nowMinute % 10;
            ///当前时间最近的整五+1天时间
            int endMinute = nowMinute - remainingMinute;

            ///all day + 1 正确需要显示点数
            string[] allSite = new string[allMinute + 1];
            for (int i = 0; i < allMinute; i++)
            {

                DataSet ds1 = SqlAccess.ExecuteQuery("SELECT `" + keyName + "` FROM " + tableName +
                    " WHERE realTime = " + CutSomeMinite(changeDay(dt.Rows[0][0].ToString(), endMinute), i * 5));
                DataTable dt1 = ds1.Tables[0];
                Debug.Log("endhour:" + CutSomeMinite(changeDay(dt.Rows[0][0].ToString(), endMinute), i * 5));
                if (dt1.Rows.Count >= 1)
                {
                    //Debug.Log("CutOneHour(changeHour(dt.Rows[0][0].ToString(),endHour), i*4):  " + 
                    //    CutOneHour(changeHour(dt.Rows[0][0].ToString(), endHour), i * 4));
                    //Debug.Log("dt.Rows[0][0]:" + dt1.Rows[0][0].ToString());
                    allSite[allMinute - i - 1] = dt1.Rows[0][0].ToString();
                }
                else
                {
                    allSite[allMinute - i - 1] = "0";
                }
            }
            DataSet ds2 = SqlAccess.ExecuteQuery("SELECT `" + keyName + "` FROM " + tableName + " WHERE realTime = " + dt.Rows[0][0].ToString());
            DataTable dt2 = ds2.Tables[0];
            allSite[allMinute] = dt2.Rows[0][0].ToString();

            ///-160 -106.67 -53.34 0 53.34 106.67
            ///
            double maxValue = findMaxString(allSite);
            double minValue = findMinString(allSite);


            ///修改纵轴坐标值
            VerticalSites[0].GetComponent<Text>().text = minValue.ToString();
            ////当值全一样时，只有一条直线
            if (maxValue == minValue)
            {
                VerticalSites[1].GetComponent<Text>().text = "";
                VerticalSites[2].GetComponent<Text>().text = "";
                VerticalSites[3].GetComponent<Text>().text = "";
                VerticalSites[4].GetComponent<Text>().text = "";
            }
            else
            {
                VerticalSites[1].GetComponent<Text>().text = ((maxValue - minValue) / 4 + minValue).ToString();
                VerticalSites[2].GetComponent<Text>().text = ((maxValue - minValue) / 2 + minValue).ToString();
                VerticalSites[3].GetComponent<Text>().text = ((maxValue - minValue) * 3 / 4 + minValue).ToString();
                VerticalSites[4].GetComponent<Text>().text = maxValue.ToString();
            }

            double[] Yvalues = new double[allSite.Length];
            double[] Xvalues = new double[allSite.Length];
            int Ykey = 0;
            Debug.Log("allSite.Length" + allSite.Length);
            foreach (string item in allSite)
            {
                Xvalues[Ykey] = -160 + Ykey * 53.33;
                if (maxValue == minValue)
                {
                    //Yvalues[Ykey++] = -53.5;
                    Yvalues[Ykey++] = -107;
                }
                else
                {
                    Yvalues[Ykey++] = 214 * (double.Parse(item) - minValue) / (maxValue - minValue) - 107;
                }

                //Debug.Log("x: " + Xvalues[Ykey - 1]);
                //Debug.Log("y: " + Yvalues[Ykey - 1]);
                Debug.Log("value：" + item);
                Sites[Ykey - 1].SetActive(true);
                ///描点
                Sites[Ykey - 1].transform.localPosition = new Vector3((float)Xvalues[Ykey - 1], (float)Yvalues[Ykey - 1], -1f);

                ///描线
                if (Ykey == allSite.Length)
                {
                    ///修正最后一个点位坐标nowHour 与画线
                    if (nowMinute % 10 == 0)
                    {
                        Sites[Ykey - 1].SetActive(false);
                        //  lineRenderer.SetPosition(Ykey - 1, Sites[Ykey - 2].transform.position);

                    }
                    else
                    {
                        Sites[Ykey - 1].transform.localPosition = new Vector3(320 * nowMinute / 60 - 160, Sites[Ykey - 1].transform.localPosition.y, -1f);
                        if (Ykey - 1 > 0)
                        {
                            printLine(Sites[Ykey - 2].transform.localPosition.x, Sites[Ykey - 2].transform.localPosition.y,
                                Sites[Ykey - 1].transform.localPosition.x, Sites[Ykey - 1].transform.localPosition.y, Ykey - 2);
                        }
                        //     lineRenderer.SetPosition(Ykey - 1, Sites[Ykey - 1].transform.position);
                    }
                }
                else
                {
                    if (Ykey - 1 > 0)
                    {
                        printLine(Sites[Ykey - 2].transform.localPosition.x, Sites[Ykey - 2].transform.localPosition.y,
                            Sites[Ykey - 1].transform.localPosition.x, Sites[Ykey - 1].transform.localPosition.y, Ykey - 2);
                    }
                    //    lineRenderer.SetPosition(Ykey - 1, Sites[Ykey - 1].transform.position);
                }

            }
        }
    }

    /// <summary>
    /// 通过表名与index查找抬头
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public string FindKey(string tableName, int index)
    {
        //Debug.Log("tableName: " + tableName);
        DataSet ds = SqlAccess.ExecuteQuery("SELECT `" + index + "` FROM all_device WHERE name = '" + tableName + "'");//yeshi_baoban_realtime
        DataTable dt = ds.Tables[0];
        //Debug.Log("dt.Rows[0]: " + dt.Rows[0][0].ToString());
        return dt.Rows[0][0].ToString();
    }

    /// <summary>
    /// 时间减去i小时
    /// </summary>
    /// <param name="timeString"></param>
    /// <returns></returns>
    private string CutSomeHour(string timeString, int i)
    {
        string year = timeString.Substring(0, 4);
        string month = timeString.Substring(4, 2);
        string day = timeString.Substring(6, 2);
        string hour = timeString.Substring(8, 2);
        string minute = timeString.Substring(10, 2);
        if ((int.Parse(hour) - i) < 0)
        {
            if ((int.Parse(day) - 1) < 1)
            {
                if ((int.Parse(month) - 1) < 1)
                {
                    year = (int.Parse(year) - 1).ToString();
                    month = "12";
                    day = "31";
                    hour = (24 + int.Parse(hour) - i).ToString();
                }
                else
                {
                    month = (int.Parse(month) - 1).ToString();
                    hour = (24 + int.Parse(hour) - i).ToString();
                    switch (month)
                    {
                        case "1":
                            day = "31";
                            break;
                        case "2":
                            if (int.Parse(year.Substring(2, 2)) % 4 == 0)
                            {
                                day = "29";
                            }
                            else
                            {
                                day = "28";
                            }
                            break;
                        case "3":
                            day = "31";
                            break;
                        case "4":
                            day = "30";
                            break;
                        case "5":
                            day = "31";
                            break;
                        case "6":
                            day = "30";
                            break;
                        case "7":
                            day = "31";
                            break;
                        case "8":
                            day = "31";
                            break;
                        case "9":
                            day = "30";
                            break;
                        case "10":
                            day = "31";
                            break;
                        case "11":
                            day = "30";
                            break;
                        case "12":
                            day = "31";
                            break;
                    }
                }
            }
            else
            {
                day = (int.Parse(day) - 1).ToString();
                hour = (24 - i).ToString();
            }
        }
        else
        {
            hour = (int.Parse(hour) - i).ToString();
        }
        //Debug.Log("Year"+ year);
        //Debug.Log("month" + month);
        //Debug.Log("day" + day);
        //Debug.Log("hour" + hour);
        //Debug.Log("minute" + minute);
        year = year.PadLeft(4, '0');
        month = month.PadLeft(2, '0');
        day = day.PadLeft(2, '0');
        hour = hour.PadLeft(2, '0');
        minute = minute.PadLeft(2, '0');
        return year + month + day + hour + minute;
    }


    /// <summary>
    /// 时间减去天数
    /// </summary>
    /// <param name="timeString"></param>
    /// <param name="i"></param>
    /// <returns></returns>
    private string CutSomeDay(string timeString, int i)
    {
        string year = timeString.Substring(0, 4);
        string month = timeString.Substring(4, 2);
        string day = timeString.Substring(6, 2);
        string hour = timeString.Substring(8, 2);
        string minute = timeString.Substring(10, 2);
        if ((int.Parse(day) - i) < 1)
        {
            if ((int.Parse(month) - 1) < 1)
            {
                year = (int.Parse(year) - 1).ToString();
                month = "12";
                day = (31 + int.Parse(day) - i).ToString();
            }
            else
            {
                month = (int.Parse(month) - 1).ToString();
                switch (month)
                {
                    case "1":
                        day = (31 + int.Parse(day) - i).ToString();
                        break;
                    case "2":
                        if (int.Parse(year.Substring(2, 2)) % 4 == 0)
                        {
                            day = (29 + int.Parse(day) - i).ToString();
                        }
                        else
                        {
                            day = (28 + int.Parse(day) - i).ToString();
                        }
                        break;
                    case "3":
                        day = (31 + int.Parse(day) - i).ToString();
                        break;
                    case "4":
                        day = (30 + int.Parse(day) - i).ToString();
                        break;
                    case "5":
                        day = (31 + int.Parse(day) - i).ToString();
                        break;
                    case "6":
                        day = (30 + int.Parse(day) - i).ToString();
                        break;
                    case "7":
                        day = (31 + int.Parse(day) - i).ToString();
                        break;
                    case "8":
                        day = (31 + int.Parse(day) - i).ToString();
                        break;
                    case "9":
                        day = (30 + int.Parse(day) - i).ToString();
                        break;
                    case "10":
                        day = (31 + int.Parse(day) - i).ToString();
                        break;
                    case "11":
                        day = (30 + int.Parse(day) - i).ToString();
                        break;
                    case "12":
                        day = (31 + int.Parse(day) - i).ToString();
                        break;
                }
            }
        }
        else
        {
            day = (int.Parse(day) - i).ToString();
        }

        year = year.PadLeft(4, '0');
        month = month.PadLeft(2, '0');
        day = day.PadLeft(2, '0');
        hour = hour.PadLeft(2, '0');
        minute = minute.PadLeft(2, '0');
        return year + month + day + hour + minute;
    }


    private string CutSomeMinite(string timeString, int i)
    {
        string year = timeString.Substring(0, 4);
        string month = timeString.Substring(4, 2);
        string day = timeString.Substring(6, 2);
        string hour = timeString.Substring(8, 2);
        string minute = timeString.Substring(10, 2);


        if (int.Parse(minute) - i >= 0)
        {
            minute = (int.Parse(minute) - i).ToString();
        }

        year = year.PadLeft(4, '0');
        month = month.PadLeft(2, '0');
        day = day.PadLeft(2, '0');
        hour = hour.PadLeft(2, '0');
        minute = minute.PadLeft(2, '0');
        return year + month + day + hour + minute;
    }

    /// <summary>
    /// 通过时间字符串获取具体小时
    /// </summary>
    /// <param name="timeString"></param>
    /// <returns></returns>
    private int getHourNow(string timeString)
    {
        int result = 0;
        result = int.Parse(timeString.Substring(8, 2));
        return result;
    }

    /// <summary>
    /// 通过时间字符串获取具体日
    /// </summary>
    /// <param name="timeString"></param>
    /// <returns></returns>
    private int getDayNow(string timeString)
    {
        int result = 0;
        result = int.Parse(timeString.Substring(6, 2));
        return result;
    }

    /// <summary>
    /// 通过时间字符串获取具体分钟
    /// </summary>
    /// <param name="timeString"></param>
    /// <returns></returns>
    private int getMinuteNow(string timeString)
    {
        int result = 0;
        result = int.Parse(timeString.Substring(10, 2));
        return result;
    }

    private string UpdateMinuteString(string timeString)
    {

        string allString, String1, String2, String3;
        allString = CutSomeHour(timeString, 1);
        String1 = timeString.Substring(0, 8);
        String2 = timeString.Substring(8, 2);
        String3 = "60";
        return String1 + String2 + String3;
    }

    /// <summary>
    /// 修改时间字符串中的小时
    /// </summary>
    /// <param name="timeString"></param>
    /// <param name="hour"></param>
    /// <returns></returns>
    private string changeHour(string timeString, int hour)
    {
        string intervalHour1 = "";
        string intervalHour2 = "";
        string intervalHour3 = "";
        intervalHour1 = timeString.Substring(0, 8);
        intervalHour2 = hour.ToString().PadLeft(2, '0');
        intervalHour3 = timeString.Substring(10, 2);
        return intervalHour1 + intervalHour2 + intervalHour3;
    }


    /// <summary>
    /// 修改字符串中的日期
    /// </summary>
    /// <param name="timeString"></param>
    /// <param name="Day"></param>
    /// <returns></returns>
    private string changeDay(string timeString, int Day)
    {
        string intervalDay1 = "";
        string intervalDay2 = "";
        string intervalDay3 = "";
        intervalDay1 = timeString.Substring(0, 6);
        intervalDay2 = Day.ToString().PadLeft(2, '0');
        intervalDay3 = timeString.Substring(8, 4);
        return intervalDay1 + intervalDay2 + intervalDay3;
    }

    /// <summary>
    /// 寻找字符串数组中最大的double
    /// </summary>
    /// <param name="stringsArray"></param>
    /// <returns></returns>
    private double findMaxString(string[] stringsArray)
    {
        if (stringsArray != null)
        {
            double intervalDouble = double.Parse(stringsArray[0]);
            foreach (string item in stringsArray)
            {
                if (double.Parse(item) > intervalDouble)
                {
                    intervalDouble = double.Parse(item);
                }
            }
            return intervalDouble;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// 寻找字符串数组中最小的double
    /// </summary>
    /// <param name="stringsArray"></param>
    /// <returns></returns>
    private double findMinString(string[] stringsArray)
    {
        if (stringsArray != null)
        {
            double intervalDouble = double.Parse(stringsArray[0]);
            foreach (string item in stringsArray)
            {
                if (double.Parse(item) < intervalDouble)
                {
                    intervalDouble = double.Parse(item);
                }
            }
            return intervalDouble;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// 根据两点画线
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <param name="x2"></param>
    /// <param name="y2"></param>
    /// <param name="index"></param>
    private void printLine(float x1, float y1, float x2, float y2, int index)
    {
        PrintLines[index].SetActive(true);
        PrintLines[index].GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Sqrt((y2 - y1) * (y2 - y1) + (x2 - x1) * (x2 - x1)), 3.8f);
        PrintLines[index].transform.localPosition = new Vector3((x1 + x2) / 2, (y1 + y2) / 2, 0f);
        if (y2 > y1)
        {

            PrintLines[index].transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan((y2 - y1) / (x2 - x1)) * 180 / Mathf.PI);
        }
        else
        {
            //PrintLines[index].GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Sqrt((y1 - y2) * (y1 - y2) + (x2 - x1) * (x2 - x1)), 6.5f);
            PrintLines[index].transform.localEulerAngles = new Vector3(0, 0, -1 * Mathf.Atan((y1 - y2) / (x2 - x1)) * 180 / Mathf.PI);
        }
    }

    /// <summary>
    /// 擦除所有线条
    /// </summary>
    private void initLines()
    {
        foreach (GameObject item in PrintLines)
        {
            item.SetActive(false);
        }
    }


    private void initSites()
    {
        foreach (GameObject item in Sites)
        {
            item.SetActive(false);
        }
    }

}

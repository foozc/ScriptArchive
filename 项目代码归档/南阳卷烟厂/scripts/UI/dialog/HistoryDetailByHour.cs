/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	HistoryDetailByHour 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年11月7日18:54:39# 
 *Description: 		   	历史记录图表详情（小时）：以小时为单位的历史记录曲线  
 *History: 				修改版本记录
*/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HistoryDetailByHour : MonoBehaviour
{
    private Button back;                                             //返回
    private Button Last;                                             //前一天
    private Button Next;                                             //下一天
    private RectTransform chart;                                     //折线图区域
    private Vector2 chartSize;                                       //折线图大小
    private float widthUnit = 54;                                    //单位宽度
    private float heightUnit = 42;                                   //单位高度

    private GameObject xAxisLines;
    private GameObject yAxisLines;
    private GameObject xAxisLabels;                                  //x轴文本区域
    private GameObject yAxisLabels;                                  //y轴文本区域
    private GameObject StandardLine;

    private Transform Lines;
    private GameObject points;                                       //坐标点容器
    private Text Date, Brand, Device, Column;

    private int yRange = 100;                                       //y轴值跨度
    int maxValue;
    int minValue;
    private float standardValue = 0;                                    //标准值

    private int verticalLineCount = 24;                             //垂直线数量：x轴的划分
    private int horizontalLineCount = 10;                           //水平线数量：y轴的划分
    private int yValueUnit = 10;

    List<GameObject> pointObjects = new List<GameObject>();
    List<GameObject> linesObjects = new List<GameObject>();
    List<GameObject> xAxisLineObjects = new List<GameObject>();
    List<GameObject> yAxisLineObjects = new List<GameObject>();
    List<GameObject> yAxisLabelObjects = new List<GameObject>();

    Dictionary<string, Vector3> pointPostions = new Dictionary<string, Vector3>();
    //数据点坐标
    List<Vector2> pointCoordinates;
    GameObject selectPoint;

    void Awake()
    {
        back = transform.Find("Btn/Back").GetComponent<Button>();
        Last = transform.Find("Btn/Last").GetComponent<Button>();
        Next = transform.Find("Btn/Next").GetComponent<Button>();

        chart = transform.Find("Chart").GetComponent<RectTransform>();
        xAxisLines = chart.Find("XAxisLines").gameObject;
        yAxisLines = chart.Find("YAxisLines").gameObject;

        xAxisLabels = chart.Find("xAxisLabels").gameObject;
        yAxisLabels = chart.Find("yAxisLabels").gameObject;
        points = chart.Find("Points").gameObject;
        StandardLine = chart.Find("StandardLine").gameObject;
        Lines = chart.Find("Lines");

        Date = transform.Find("Text/Date/Text").GetComponent<Text>();
        Brand = transform.Find("Text/Brand/Text").GetComponent<Text>();
        Device = transform.Find("Text/Device/Text").GetComponent<Text>();
        Column = transform.Find("Text/Column/Text").GetComponent<Text>();

        Measure();
        DrawYAxis();
        DrawXAxis();
    }

    /// <summary>
    /// 测量
    /// </summary>
    private void Measure()
    {
        chartSize = chart.sizeDelta;
        widthUnit = chartSize.x / verticalLineCount;
    }

    /// <summary>
    /// 绘制y轴坐标轴
    /// </summary>
    private void DrawYAxis()
    {
        DrawXAxisLabels();
        for (int i = 0; i <= verticalLineCount; i++)
        {
            GameObject yAxisLine = Instantiate(HistoryDetail._instance.yAixsLinePrefab);
            yAxisLine.transform.parent = yAxisLines.transform;
            yAxisLine.transform.localPosition = Vector3.right * widthUnit * i;
        }
    }

    /// <summary>
    /// 绘制x轴坐标刻度
    /// </summary>
    private void DrawXAxisLabels()
    {
        for (int j = 0; j < verticalLineCount; j++)
        {
            GameObject label = Instantiate(HistoryDetail._instance.labelPrefab);
            Text txt = label.GetComponent<Text>();
            if (txt)
            {
                txt.text = (j + 1).ToString() + "h";
            }
            label.transform.parent = xAxisLabels.transform;
            label.transform.localPosition = Vector3.right * (j + 1) * widthUnit;
            label.transform.localScale = Vector3.one;
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="pointCoordinates"></param>
    public void Init()
    {
        InitObjects();
        InitClick();
        InitText();
        this.pointCoordinates = HistoryDetail._instance.util.GetHourPoints();
        if (pointCoordinates == null || pointCoordinates.Count == 0)
        {
            StandardLine.SetActive(false);
            Debug.Log("无数据");
            return;
        }
        GetValueRange();
        InitPoints();
        DrawXAxis();
        DrawStandardLine();
        DrawRealTimeLine();
    }

    /// <summary>
    /// 绘制x轴坐标轴
    /// </summary>
    private void DrawXAxis()
    {
        DrawYAxisLabels();
        for (int i = 0; i < xAxisLineObjects.Count; i++)
        {
            Destroy(xAxisLineObjects[i]);
        }
        xAxisLineObjects.Clear();
        for (int i = 0; i <= horizontalLineCount; i++)
        {
            GameObject xAxisLine = Instantiate(HistoryDetail._instance.xAxisLinePrefab);
            xAxisLine.transform.parent = xAxisLines.transform;
            xAxisLine.transform.localPosition = Vector3.up * heightUnit * i;
            xAxisLineObjects.Add(xAxisLine);
        }
    }

    /// <summary>
    /// 绘制y轴坐标刻度
    /// </summary>
    private void DrawYAxisLabels()
    {
        for (int i = 0; i < yAxisLabelObjects.Count; i++)
        {
            Destroy(yAxisLabelObjects[i]);
        }
        yAxisLabelObjects.Clear();
        for (int j = 0; j <= horizontalLineCount; j++)
        {
            GameObject label = Instantiate(HistoryDetail._instance.labelPrefab);
            Text txt = label.GetComponent<Text>();
            if (txt)
            {
                txt.text = (minValue + j * yValueUnit).ToString();
            }
            label.transform.parent = yAxisLabels.transform;
            label.transform.localPosition = Vector3.up * j * heightUnit;
            label.transform.localScale = Vector3.one;
            yAxisLabelObjects.Add(label);
        }
    }

    private void InitClick()
    {
        back.onClick.RemoveAllListeners();
        back.onClick.AddListener(() =>
        {
            HistoryDetail._instance.Destroy();
        });
        //前一天（不超过7天）
        Last.onClick.RemoveAllListeners();
        Last.onClick.AddListener(() =>
        {
            HistoryDetail._instance.LastDay();
        });
        //后一天(不超过今天)
        Next.onClick.RemoveAllListeners();
        Next.onClick.AddListener(() =>
        {
            HistoryDetail._instance.NextDay();
        });
    }

    /// <summary>
    /// 初始化界面上的显示文本
    /// </summary>
    private void InitText()
    {
        string time = HistoryDetail._instance.time;
        string year = time.Substring(0, 4);
        string month = time.Substring(4, 2);
        string day = time.Substring(6, 2);
        Date.text = year + " / " + month + " / " + day;
        Brand.text = Brands._instance.GetBrandName(HistoryDetail._instance.brand);
        Device.text = HistoryDetail._instance.deviceName;
        Column.text = HistoryDetail._instance.titleName;
    }

    /// <summary>
    /// 获取值的跨度范围
    /// </summary>
    private void GetValueRange()
    {
        standardValue = HistoryDetail._instance.util.GetStandardValue();
        float max = HistoryDetail._instance.util.GetMaxYValue(pointCoordinates, standardValue);
        float min = HistoryDetail._instance.util.GetMinYValue(pointCoordinates, standardValue);
        yRange = Mathf.CeilToInt(max) - Mathf.FloorToInt(min);

        yValueUnit = (int)Math.Pow(10, yRange.ToString().Length - 1);
        maxValue = Mathf.CeilToInt(max / yValueUnit) * yValueUnit;
        minValue = Mathf.FloorToInt(min / yValueUnit) * yValueUnit;
        yRange = maxValue - minValue;

        horizontalLineCount = yRange == 0 ? 1 : Mathf.CeilToInt(yRange / yValueUnit);
        yRange = horizontalLineCount * yValueUnit;

        heightUnit = chartSize.y / horizontalLineCount;
    }

    /// <summary>
    /// 初始化坐标点
    /// </summary>
    private void InitPoints()
    {
        for (int i = 0; i < pointCoordinates.Count; i++)
        {
            Vector2 vec = pointCoordinates[i];
            Debug.Log(vec);
            float y = yRange == 0 ? 0 : Mathf.Abs(vec.y - minValue) / yRange * chartSize.y;
            float x = (Vector3.right * widthUnit * vec.x).x;
            GameObject point = Instantiate(HistoryDetail._instance.pointPrefab);
            point.transform.parent = points.transform;
            point.transform.localPosition = new Vector3(x, y, 0);
            point.name = "Point" + (int)vec.x;
            Button btn = point.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                selectPoint = point;
                HistoryDetail._instance.hoverImage.SetActive(false);
                int hour = int.Parse((point.name).Substring(5));
                HistoryDetail._instance.HistoryDetail_Minute.SetActive(true);
                HistoryDetail._instance.hdbm.Init(hour);
                gameObject.SetActive(false);
            });
            AddHoverEvent(point);
            pointPostions.Add(point.name, point.transform.position);
            pointObjects.Add(point);
        }
    }
    /// <summary>
    /// 点击鼠标移入效果
    /// </summary>
    /// <param name="point"></param>
    private void AddHoverEvent(GameObject point)
    {
        EventTrigger trigger = point.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = point.AddComponent<EventTrigger>();
            //鼠标移入
            EventTrigger.Entry enter = new EventTrigger.Entry();
            enter.eventID = EventTriggerType.PointerEnter;
            enter.callback.AddListener((BaseEventData data) =>
            {
                HistoryDetail._instance.hoverImage.SetActive(true);
                HistoryDetail._instance.hoverImage.transform.GetComponent<RectTransform>().pivot = new Vector2(0, 1);//左上角为锚点
                HistoryDetail._instance.hoverImage.transform.position = point.transform.position;
            });
            //鼠标移出
            EventTrigger.Entry exit = new EventTrigger.Entry();
            exit.eventID = EventTriggerType.PointerExit;
            exit.callback.AddListener((BaseEventData data) =>
            {
                HistoryDetail._instance.hoverImage.SetActive(false);
            });
            trigger.triggers.Add(enter);
            trigger.triggers.Add(exit);
        }
    }

    /// <summary>
    /// 绘制标准值线
    /// </summary>
    private void DrawStandardLine()
    {
        StandardLine.SetActive(true);
        float offset = yRange == 0 ? 0 : Mathf.Abs(standardValue - minValue) / yRange * chartSize.y;
        StandardLine.transform.localPosition = new Vector3(0, -210, 0) + Vector3.up * offset;
    }

    /// <summary>
    /// 绘制数据折线
    /// </summary>
    private void DrawRealTimeLine()
    {
        Vector3[] linePoints = new Vector3[pointObjects.Count];
        for (int i = 0; i < linePoints.Length; i++)
        {
            linePoints[i] = pointObjects[i].transform.position;
        }
        for (int i = 0; i < linePoints.Length - 1; i++)
        {
            printLine(linePoints[i].x, linePoints[i].y, linePoints[i + 1].x, linePoints[i + 1].y, i);
        }
    }

    private void printLine(float x1, float y1, float x2, float y2, int index)
    {
        GameObject line = Instantiate(HistoryDetail._instance.linePrefab);
        line.transform.SetParent(Lines);
        line.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Sqrt((y2 - y1) * (y2 - y1) + (x2 - x1) * (x2 - x1)), 3.8f);
        line.transform.position = new Vector3((x1 + x2) / 2, (y1 + y2) / 2, 0f);
        if (y2 > y1)
        {
            line.transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan((y2 - y1) / (x2 - x1)) * 180 / Mathf.PI);
        }
        else
        {
            line.transform.localEulerAngles = new Vector3(0, 0, -1 * Mathf.Atan((y1 - y2) / (x2 - x1)) * 180 / Mathf.PI);
        }
        linesObjects.Add(line);
    }

    /// <summary>
    /// 清空变动的物体
    /// </summary>
    public void InitObjects()
    {
        pointPostions.Clear();
        for (int i = 0; i < linesObjects.Count; i++)
        {
            Destroy(linesObjects[i]);
        }
        linesObjects.Clear();
        for (int i = 0; i < pointObjects.Count; i++)
        {
            Destroy(pointObjects[i]);
        }
        pointObjects.Clear();
    }

    /// <summary>
    /// 前一个小时，给HistoryDetailByMinute调用的
    /// </summary>
    public void LastHour()
    {
        if (pointObjects.Count > 1)
        {
            int index = pointObjects.IndexOf(selectPoint);
            if (index > 0)
            {
                GameObject point = pointObjects[index - 1];
                selectPoint = point;
                int hour = int.Parse((point.name).Substring(5));
                HistoryDetail._instance.hdbm.Init(hour);
            }
        }
    }

    /// <summary>
    /// 下一个小时，给HistoryDetailByMinute调用的
    /// </summary>
    public void NextHour()
    {
        if (pointObjects.Count > 1)
        {
            int index = pointObjects.IndexOf(selectPoint);
            if (index + 1 < pointObjects.Count)
            {
                GameObject point = pointObjects[index + 1];
                selectPoint = point;
                int hour = int.Parse((point.name).Substring(5));
                HistoryDetail._instance.hdbm.Init(hour);
            }
        }
    }
}

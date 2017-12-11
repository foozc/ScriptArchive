/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	HistoryDetailByMinute 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年11月10日15:08:47# 
 *Description: 		   	历史记录图表详情（分钟）：以分钟为单位的历史记录曲线  
 *History: 				修改版本记录
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vectrosity;

public class HistoryDetailByMinute : MonoBehaviour
{
    private Button back;                                             //返回
    private Button Last;                                             //前一天
    private Button Next;                                             //下一天
    private RectTransform chart;                                      //折线图区域
    private Vector2 chartSize;                                      //折线图大小
    private float widthUnit, heightUnit;                            //单位宽度、单位高度

    private GameObject xAxisLines;
    private GameObject yAxisLines;
    private GameObject xAxisLabels;                                  //x轴文本区域
    private GameObject yAxisLabels;                                  //y轴文本区域
    private GameObject StandardLine;

    private Transform Lines;
    private GameObject points;                                       //坐标点容器
    private Vector3 zeroPoint, xMaxPoint, yMaxPoint;                //原点、x轴最大值点、y轴最大值点
    private Text Date, Brand, Device, Column;

    private int yRange = 100;                                       //y轴值跨度
    int maxValue;
    int minValue;
    private float standardValue=0;                                    //标准值

    private int verticalLineCount = 60;                             //垂直线数量：x轴的划分
    private int horizontalLineCount = 10;                           //水平线数量：y轴的划分
    private int yValueUnit = 10;

    List<GameObject> pointObjects = new List<GameObject>();
    List<GameObject> linesObjects = new List<GameObject>();
    List<GameObject> xAxisLineObjects = new List<GameObject>();
    List<GameObject> yAxisLabelObjects = new List<GameObject>();

    //数据点坐标
    List<Vector2> pointCoordinates;
    int hour;

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
    }

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
        for (int i = 0; i <= verticalLineCount / 5; i++)
        {
            GameObject yAxisLine = Instantiate(HistoryDetail._instance.yAixsLinePrefab);
            yAxisLine.transform.parent = yAxisLines.transform;
            yAxisLine.transform.localPosition = Vector3.right * widthUnit * 5 * i;
        }
    }

    /// <summary>
    /// 绘制x轴坐标刻度
    /// </summary>
    private void DrawXAxisLabels()
    {
        for (int j = 0; j < verticalLineCount / 5; j++)
        {
            GameObject label = Instantiate(HistoryDetail._instance.labelPrefab);
            Text txt = label.GetComponent<Text>();
            if (txt)
            {
                txt.text = ((j + 1) * 5).ToString() + "m";
            }
            label.transform.parent = xAxisLabels.transform;
            label.transform.localPosition = Vector3.right * widthUnit * (j + 1) * 5;
            label.transform.localScale = Vector3.one;
        }
    }

    public void Init(int hour)
    {
        InitObjects();
        this.hour = hour;
        this.pointCoordinates = HistoryDetail._instance.util.GetMinutePoints(hour);
        InitText();
        InitClick();
        GetValueRange();
        Measure();
        InitPoints();
        DrawXAxis();
        DrawStandardLine();
        DrawRealTimeLine();
    }

    /// <summary>
    /// 初始化界面上的显示文本
    /// </summary>
    private void InitText()
    {
        Date.text = DateTime.Now.ToString("yyyy / MM / dd") + "  " + hour.ToString() + " : 00";
        Brand.text = Brands._instance.GetBrandName(HistoryDetail._instance.brand);
        Device.text = HistoryDetail._instance.deviceName;
        Column.text = HistoryDetail._instance.titleName;
    }

    private void InitClick()
    {
        back.onClick.RemoveAllListeners();
        back.onClick.AddListener(() =>
        {
            HistoryDetail._instance.HistoryDetail_Hour.SetActive(true);
            gameObject.SetActive(false);
        });

        //前一小时
        Last.onClick.RemoveAllListeners();
        Last.onClick.AddListener(() =>
        {
            HistoryDetail._instance.hdbh.LastHour();
        });
        //后一小时
        Next.onClick.RemoveAllListeners();
        Next.onClick.AddListener(() =>
        {
            HistoryDetail._instance.hdbh.NextHour();
        });
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
        for (int i = 0; i < pointObjects.Count; i++)
        {
            Destroy(pointObjects[i]);
        }
        pointObjects.Clear();

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
            btn.enabled = false;
            pointObjects.Add(point);
        }
    }

    /// <summary>
    /// 绘制x轴坐标轴
    /// </summary>
    private void DrawXAxis()
    {
        DrawYAxisLabels();
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

    /// <summary>
    /// 画标准值线
    /// </summary>
    private void DrawStandardLine()
    {
        StandardLine.SetActive(true);
        float offset = yRange == 0 ? 0 : Mathf.Abs(standardValue - minValue) / yRange * chartSize.y;
        StandardLine.transform.localPosition = new Vector3(0, -210, 0) + Vector3.up * offset;
    }

    /// <summary>
    /// 画实时值折线
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
        for (int i = 0; i < xAxisLineObjects.Count; i++)
        {
            Destroy(xAxisLineObjects[i]);
        }
        xAxisLineObjects.Clear();
        for (int i = 0; i < yAxisLabelObjects.Count; i++)
        {
            Destroy(yAxisLabelObjects[i]);
        }
        yAxisLabelObjects.Clear();
    }
}

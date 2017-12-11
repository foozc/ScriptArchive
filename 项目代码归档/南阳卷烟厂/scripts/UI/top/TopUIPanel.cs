/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	TopUIPanel 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年10月10日17:34:02# 
 *Description: 		   	顶部UI面板：4个主标签切换、系统时间、退出
 *History: 				修改版本记录
*/

using System;
using UnityEngine;
using UnityEngine.UI;

public class TopUIPanel : MonoBehaviour
{
    public static TopUIPanel _instance;

    private void Awake()
    {
        _instance = this;
    }

    public GameObject topTabs, text_date, text_time;
    public Button exit;

    [HideInInspector]
    public int currentTab = 1;

    private string[] weeksCn =
    {
        "星期天",
        "星期一",
        "星期二",
        "星期三",
        "星期四",
        "星期五",
        "星期六"
    };

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < topTabs.transform.childCount; i++)
        {
            Toggle tab = topTabs.transform.GetChild(i).gameObject.GetComponent<Toggle>();
            if (i == 0) { tab.isOn = true; }
            tab.onValueChanged.AddListener(OnTopTabClick);
        }
        exit.onClick.AddListener(Exit);
    }

    // Update is called once per frame
    void Update()
    {
        //显示系统日期
        text_date.GetComponent<Text>().text =
            DateTime.Now.Year
            + "  " + DateTime.Now.Month
            + "  " + DateTime.Now.Day
            + "  " + weeksCn[Convert.ToInt16(DateTime.Now.DayOfWeek)];
        //显示系统时间
        text_time.GetComponent<Text>().text = DateTime.Now.ToLongTimeString().ToString();
    }

    /// <summary>
    /// 顶部UI面板tab点击事件
    /// </summary>
    /// <param name="isSelected"></param>
    private void OnTopTabClick(bool isSelected)
    {
        if (isSelected)
        {
            int index = ToggleUtil.CurrentToggleIndex(topTabs.gameObject);
            ListManager._instance.ShowInfoList(index + 1);
            currentTab = index + 1;
            //if (currentTab == 4)
            //{
            //    GlobalManager._instance.StartCheck();
            //}
            //else
            //{
            //    GlobalManager._instance.StopCheck();
            //}
        }
    }

    public void SetCurrentTab(int index)
    {
        ToggleUtil.SetCurrentTab(topTabs, index);
        currentTab = index + 1;
    }

    private void Exit()
    {
        if (Dialog._instance.dialog_exit)
        {
            Dialog._instance.dialog_exit.SetActive(true);
            Dialog._instance.exit.onClick.RemoveAllListeners();
            Dialog._instance.exit.onClick.AddListener(() =>
            {
                Application.Quit();
            });
            Dialog._instance.stay.onClick.RemoveAllListeners();
            Dialog._instance.stay.onClick.AddListener(() =>
            {
                Dialog._instance.dialog_exit.SetActive(false);
            });
        }
    }
}

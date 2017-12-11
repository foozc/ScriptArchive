/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	AlarmInfoForm 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年10月20日14:01:40# 
 *Description: 		   	故障警报信息表格：获取数据，分为未处理和已处理;未处理的弹出警告信息
 *History: 				修改版本记录
*/

using assest;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class AlarmInfoForm : MonoBehaviour
{
    public static AlarmInfoForm _instance;

    public GameObject tabs;
    public GameObject alarmInfo;                           //未处理部分
    public GameObject dragpanel_v_alarm;
    public GameObject itemPrefab;
    public Sprite tiao1;                                    //第一行背景
    public Sprite tiao;                                     //其他行背景

    public GameObject alarm_processed;                     //已处理部分
    public GameObject dragpanel_v_alarm_processed;
    public GameObject processedPrefab;
    public Sprite tiao_processed1;                          //第一行背景
    public Sprite tiao_processed;                           //其他行背景

    void Awake()
    {
        _instance = this;

        alarmInfo.SetActive(true);
        alarm_processed.SetActive(false);
        for (int i = 0; i < tabs.transform.childCount; i++)
        {
            Toggle tab = tabs.transform.GetChild(i).gameObject.GetComponent<Toggle>();
            if (i == 0) { tab.isOn = true; }
            tab.onValueChanged.AddListener(OnTabClick);
        }
    }

    private void OnTabClick(bool isSelected)
    {
        if (isSelected)
        {
            int index = ToggleUtil.CurrentToggleIndex(tabs);
            if (index == 0)
            {
                alarmInfo.SetActive(true);
                alarm_processed.SetActive(false);
            }
            if (index == 1)
            {
                alarmInfo.SetActive(false);
                alarm_processed.SetActive(true);
            }
        }
    }

    /// <summary>
    /// 初始化界面
    /// </summary>
    public void Init(List<AlarmInfo> alarmToDeal, List<AlarmInfo> alarmDealt)
    {
        InitAlarmInfoForm(alarmToDeal);
        InitProcessedInfoForm(alarmDealt);
    }

    /// <summary>
    /// 初始化未处理警报列表
    /// </summary>
    /// <param name="list"></param>
    private void InitAlarmInfoForm(List<AlarmInfo> list)
    {
        int itemCounts = list.Count;
        dragpanel_v_alarm.GetComponent<ScrollRect>().vertical = false;
        dragpanel_v_alarm.GetComponent<ScrollRect>().horizontal = false;
        Transform tr = dragpanel_v_alarm.transform.Find("content");
        if (tr)
        {
            GameObject contentPanelRoot = tr.gameObject;
            for (int i = 0; i < contentPanelRoot.transform.childCount; i++)
            {
                Destroy(contentPanelRoot.transform.GetChild(i).gameObject);
            }
            for (int i = 0; i < itemCounts; i++)
            {
                GameObject item = Instantiate(itemPrefab);
                AlarmInfo info = list[i];
                if (i == 0)
                {
                    Utils.FindGameObject(item, "bg").GetComponent<Image>().sprite = tiao1;
                }
                else
                {
                    Utils.FindGameObject(item, "bg").GetComponent<Image>().sprite = tiao;
                }
                FillDataInfoForm(info, item, i);
                Button confirm = Utils.FindGameObject(item, "Confirm").GetComponent<Button>();
                confirm.onClick.RemoveAllListeners();
                confirm.onClick.AddListener(() =>
                {
                    ShowConfirmDialog(info);
                });

                item.transform.SetParent(contentPanelRoot.transform);
                item.transform.localScale = Vector3.one;
            }
            VerticalLayoutGroup layout = contentPanelRoot.GetComponent<VerticalLayoutGroup>();
            layout.spacing = 0;
            Vector2 vec = contentPanelRoot.GetComponent<RectTransform>().sizeDelta;
            float itemHeight = itemPrefab.GetComponent<RectTransform>().sizeDelta.y;
            vec.y = itemCounts * (itemHeight + layout.spacing);
            contentPanelRoot.GetComponent<RectTransform>().sizeDelta = vec;
            dragpanel_v_alarm.GetComponent<ScrollRect>().vertical = true;
        }
    }

    /// <summary>
    /// 初始化已处理警报信息
    /// </summary>
    /// <param name="list"></param>
    private void InitProcessedInfoForm(List<AlarmInfo> list)
    {
        int itemCounts = list.Count;
        dragpanel_v_alarm_processed.GetComponent<ScrollRect>().vertical = false;
        dragpanel_v_alarm_processed.GetComponent<ScrollRect>().horizontal = false;
        Transform tr = dragpanel_v_alarm_processed.transform.Find("content");
        if (tr)
        {
            GameObject contentPanelRoot = tr.gameObject;
            for (int i = 0; i < contentPanelRoot.transform.childCount; i++)
            {
                Destroy(contentPanelRoot.transform.GetChild(i).gameObject);
            }
            for (int i = 0; i < itemCounts; i++)
            {
                GameObject item = Instantiate(processedPrefab);
                AlarmInfo info = list[i];
                if (i == 0)
                {
                    Utils.FindGameObject(item, "bg").GetComponent<Image>().sprite = tiao_processed1;
                }
                else
                {
                    Utils.FindGameObject(item, "bg").GetComponent<Image>().sprite = tiao_processed;
                }

                FillDataInfoForm(info, item, i);
                item.transform.SetParent(contentPanelRoot.transform);
                item.transform.localScale = Vector3.one;
            }
            VerticalLayoutGroup layout = contentPanelRoot.GetComponent<VerticalLayoutGroup>();
            layout.spacing = 0;
            Vector2 vec = contentPanelRoot.GetComponent<RectTransform>().sizeDelta;
            float itemHeight = processedPrefab.GetComponent<RectTransform>().sizeDelta.y;
            vec.y = itemCounts * (itemHeight + layout.spacing);
            contentPanelRoot.GetComponent<RectTransform>().sizeDelta = vec;
            dragpanel_v_alarm_processed.GetComponent<ScrollRect>().vertical = true;
        }
    }

    private void FillDataInfoForm(AlarmInfo info, GameObject item, int index)
    {
        Utils.FindGameObject(item, "Number").SetText((index + 1).ToString());
        Utils.FindGameObject(item, "Des").SetText(info.About);
        Utils.FindGameObject(item, "StartTime").SetText(info.StartTime);
        Utils.FindGameObject(item, "EndTime").SetText(info.EndTime);
        Utils.FindGameObject(item, "Status").SetText(info.Status);
        Utils.FindGameObject(item, "Value").SetText(info.Value);
    }

    /// <summary>
    /// 显示确认弹窗
    /// </summary>
    /// <param name="info"></param>
    private void ShowConfirmDialog(AlarmInfo info)
    {
        Dialog._instance.confirmDialog.SetActive(true);
        Dialog._instance.confirm.onClick.RemoveAllListeners();
        Dialog._instance.confirm.onClick.AddListener(() =>
        {
            info.Show = 1;                          //确认后不再显示警报弹窗
            info.IsConfirm = 1;
            info.Status = "已确认";
            UpdateAlarmInfoInDB(info);
            Dialog._instance.confirmDialog.SetActive(false);
        });
        Dialog._instance.cancel.onClick.RemoveAllListeners();
        Dialog._instance.cancel.onClick.AddListener(() =>
        {
            Dialog._instance.confirmDialog.SetActive(false);
        });
    }

    /// <summary>
    /// 更新警报信息到数据库
    /// </summary>
    /// <param name="info"></param>
    private void UpdateAlarmInfoInDB(AlarmInfo info)
    {
        string update = "UPDATE `alarm_log` SET `isconfirm`=" + info.IsConfirm + ",`isShow`=" + info.Show + ",`status`='" + info.Status
                      + "' WHERE `_id`=" + info.Id;
        SqlAccess.ExecuteQuery(update);
        GlobalManager._instance.InitAlarmInfoList();
    }
}

/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	ListManager 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年10月9日16:31:15# 
 *Description: 		   	列表管理器：列表切换、标签管理、参数传递   
 *History: 				修改版本记录
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ListManager : MonoBehaviour
{
    public static ListManager _instance;

    void Awake()
    {
        _instance = this;
    }

    public GameObject part1;

    public GameObject item_productionInfo;
    public GameObject pdtTabs;             //生产信息标签栏实体

    public GameObject item_deviceInfo;
    public GameObject deviceTabs;          //设备信息标签栏实体
    public GameObject brands;              //品牌列表

    public GameObject item_cameraInfo;
    public GameObject item_alarmInfo;

    public Transform[] devicePostions;
    public GameObject FPSController;
    public GameObject Character;
    /// <summary>
    /// 参数
    /// </summary>
    /// 
    [HideInInspector]
    public int productionFlow = 1;          //当前生产工序段
    [HideInInspector]
    public string brand;                       //当前品牌
    [HideInInspector]
    public int deviceFlow = 1;              //当前设备信息所在工序段
    [HideInInspector]
    public int deviceIndex = 0;             //当前工序段下的设备序号:设备可能有多个

    // Use this for initialization
    void Start()
    {
        //brand = Brands._instance.DefaultBrandNum();
        InitProductionTabs();
        InitDeviceInfoTabs();
        ShowInfoList(1);                    //默认显示生产信息列表
    }

    /// <summary>
    /// 切换列表显示
    /// </summary>
    /// <param name="index"></param>
    public void ShowInfoList(int index)
    {
        //初始化参数
        productionFlow = 1;
        deviceFlow = 1;
        deviceIndex = 0;

        //生产信息
        if (index == 1)
        {
            if (part1) { part1.SetActive(true); }
            if (item_deviceInfo) { item_deviceInfo.SetActive(false); }
            if (item_cameraInfo) { item_cameraInfo.SetActive(false); }
            if (item_alarmInfo) { item_alarmInfo.SetActive(false); }
            item_productionInfo.SetActive(true);
            ToggleUtil.SetCurrentTab(pdtTabs, 0);
            if (brands) { brands.SetActive(true); }
            ProductionInfoForm._instance.InitProductionInfoList(productionFlow, brand);
        }
        //设备信息
        if (index == 2)
        {
            if (part1) { part1.SetActive(true); }
            if (item_productionInfo) { item_productionInfo.SetActive(false); }
            if (item_cameraInfo) { item_cameraInfo.SetActive(false); }
            if (item_alarmInfo) { item_alarmInfo.SetActive(false); }
            item_deviceInfo.SetActive(true);
            ToggleUtil.SetCurrentTab(deviceTabs, 0);
            if (brands) { brands.SetActive(true); }
            SkipToDevice(0);
            DeviceInfoForm._instance.InitDeviceInfoList(deviceIndex);
        }
        //监控信息
        if (index == 3)
        {
            if (part1) { part1.SetActive(false); }
            if (item_alarmInfo) { item_alarmInfo.SetActive(false); }
            item_cameraInfo.SetActive(true);
            if (brands) { brands.SetActive(false); }
        }
        //故障报警
        if (index == 4)
        {
            if (part1) { part1.SetActive(false); }
            if (item_cameraInfo) { item_cameraInfo.SetActive(false); }
            item_alarmInfo.SetActive(true);
            if (brands) { brands.SetActive(false); }
            GlobalManager._instance.InitAlarmInfoList();
        }
    }

    /// <summary>
    /// 初始化生产信息标签栏
    /// </summary>
    private void InitProductionTabs()
    {
        for (int i = 0; i < pdtTabs.transform.childCount; i++)
        {
            Toggle tab = pdtTabs.transform.GetChild(i).gameObject.GetComponent<Toggle>();
            if (i == 0) { tab.isOn = true; }
            tab.onValueChanged.AddListener(OnProductionTabClick);
        }
    }

    /// <summary>
    /// 生产信息标签点击监听：改变当前工序段
    /// </summary>
    /// <param name="isSelected"></param>
    private void OnProductionTabClick(bool isSelected)
    {
        if (isSelected)
        {
            int i = ToggleUtil.CurrentToggleIndex(pdtTabs);
            productionFlow = i + 1;
            ProductionInfoForm._instance.InitProductionInfoList(productionFlow, brand);
        }
    }

    /// <summary>
    /// 设置当前选中的生产段
    /// </summary>
    /// <param name="index"></param>
    public void SetCurrentProductionTab(int index)
    {
        ToggleUtil.SetCurrentTab(pdtTabs,index);
        productionFlow = index + 1;
        ProductionInfoForm._instance.InitProductionInfoList(productionFlow, brand);
    }

    /// <summary>
    /// 初始化设备信息标签栏
    /// </summary>
    private void InitDeviceInfoTabs()
    {
        for (int i = 0; i < deviceTabs.transform.childCount; i++)
        {
            Toggle tab = deviceTabs.transform.GetChild(i).gameObject.GetComponent<Toggle>();
            if (i == 0) { tab.isOn = true; }
            tab.onValueChanged.AddListener(OnDeviceTabClick);
        }
    }

    /// <summary>
    /// 设备信息标签点击监听
    /// 按照顺序，分别是：叶片回潮段、叶片加料段、叶丝处理、混丝加香、梗预处理段、烘梗丝加香料
    /// </summary>
    /// <param name="isSelected"></param>
    private void OnDeviceTabClick(bool isSelected)
    {
        if (isSelected)
        {
            int index = ToggleUtil.CurrentToggleIndex(deviceTabs);
            deviceIndex = index;
            SkipToDevice(deviceIndex);
            DeviceInfoForm._instance.InitDeviceInfoList(deviceIndex);
        }
    }

    public void OnBrandChanged(string mBrand)
    {
        brand = mBrand;
        switch (TopUIPanel._instance.currentTab)
        {
            case 1:
                ProductionInfoForm._instance.InitProductionInfoList(productionFlow, brand);
                break;
            case 2:
                DeviceInfoForm._instance.InitDeviceInfoList(deviceIndex);
                break;
        }
    }

    /// <summary>
    /// 跳转到设备
    /// </summary>
    public void SkipToDevice(int index)
    {
        if (devicePostions.Length == 0 || !devicePostions[index]) { return; }
        Transform t = devicePostions[index];
        if (t == null) { return; }
        FPSController.transform.position = new Vector3(t.position.x, 0.45f, t.position.z);
        FPSController.transform.rotation = Quaternion.identity * t.localRotation;

        Character.transform.position = new Vector3(t.position.x, 0.45f, t.position.z);
        Character.transform.rotation = Quaternion.identity * t.localRotation;
    }

}

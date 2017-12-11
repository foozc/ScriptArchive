/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	DeviceObjects 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年10月31日10:41:58# 
 *Description: 		   	场景物体模型：设备、工序段   
 *History: 				修改版本记录
*/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneObjects : MonoBehaviour
{
    public GameObject objectNameUI;
    private Text objectNameText;
    public GameObject cameras;
    public GameObject twoFloor;
    public GameObject outWall;
    public GameObject twoFloorCollider;

    //设备
    public GameObject[] devices;
    private string[] DeviceColliders = { "Jwq318a-CF", "Jwq385a-CF", "Jsj225b-CF", "Jsh626-CF", "Jsh9-CF", "Jrcc-CF", "Jwq375-CF",
                                        "Jwq373-CF", "Jd500-CF", "Jfd-CF" };                         //设备碰撞体名
    private Dictionary<string, GameObject> DeviceObjects = new Dictionary<string, GameObject>();     //设备物体
    private Dictionary<string, string> DeviceCNNames = new Dictionary<string, string>();             //设备中文名

    //生产工序段
    public GameObject[] flows;
    //生产工序段碰撞体
    private string[] FlowColliders = { "zkhcD-0-CF", "ypclD-0-CF", "ysclD-0-CF", "cpclD-0-CF", "jxcsD-0-CF", "gclD-0-CF", "7-GSCLD-CF" };
    private Dictionary<string, GameObject> FlowObjects = new Dictionary<string, GameObject>();
    private Dictionary<string, string> FlowNames = new Dictionary<string, string>();

    //储物柜
    public GameObject[] lockers;
    private string[] LockersColliders;    //储物柜碰撞体
    private Dictionary<string, GameObject> LockersObjects = new Dictionary<string, GameObject>();
    private Dictionary<string, string> LockersName = new Dictionary<string, string>();

    //监控器
    public GameObject[] monitors;
    private string[] MonitorColliders;
    private Dictionary<string, GameObject> MonitorsObjects = new Dictionary<string, GameObject>();
    private Dictionary<string, string> MonitorNames = new Dictionary<string, string>();


    public static SceneObjects _instance;

    void Awake()
    {
        _instance = this;
        objectNameText = objectNameUI.transform.Find("objectName").GetComponent<Text>();

        //初始化设备的碰撞体和名称信息
        for (int i = 0; i < devices.Length; i++)
        {
            DeviceObjects.Add(DeviceColliders[i], devices[i]);
            DeviceCNNames.Add(DeviceColliders[i], GlobalData.Devices[i]);
        }

        //初始化生产工序段的碰撞体和名称信息
        for (int i = 0; i < flows.Length; i++)
        {
            FlowNames.Add(FlowColliders[i], GlobalData.Flows[i]);
            FlowObjects.Add(FlowColliders[i], flows[i]);
        }

        //初始化储物柜的碰撞体和名称信息
        LockersColliders = new string[lockers.Length];
        for (int i = 0; i < lockers.Length; i++)
        {
            LockersColliders[i] = lockers[i].name + "-CF";
            LockersName.Add(LockersColliders[i], "储柜" + (i + 1));
            LockersObjects.Add(LockersColliders[i], lockers[i]);
        }

        //初始化监控器的碰撞体和名称信息
        MonitorColliders = new string[monitors.Length];
        for (int i = 0; i < monitors.Length; i++)
        {
            MonitorColliders[i] = monitors[i].name;
            MonitorNames.Add(MonitorColliders[i], "监控摄像机" + (i + 1));
            MonitorsObjects.Add(MonitorColliders[i], monitors[i]);
        }
    }

    /// <summary>
    /// 显示物体名称
    /// </summary>
    /// <param name="target"></param>
    public void ShowObjectName(Transform target)
    {
        string targetName = target.name;
        if (DeviceObjects.ContainsKey(targetName))
        {
            objectNameUI.SetActive(true);
            objectNameUI.transform.position = Input.mousePosition + Vector3.left * 120;
            objectNameText.text = DeviceCNNames[targetName];
            GameObject go = DeviceObjects[targetName];
            Highlight(go);
        }
        if (FlowObjects.ContainsKey(targetName))
        {
            objectNameUI.SetActive(true);
            objectNameUI.transform.position = Input.mousePosition + Vector3.left * 120;
            objectNameText.text = FlowNames[targetName];
            GameObject go = FlowObjects[targetName];
            Highlight(go);
        }
        if (LockersObjects.ContainsKey(targetName))
        {
            objectNameUI.SetActive(true);
            objectNameUI.transform.position = Input.mousePosition + Vector3.left * 120;
            objectNameText.text = LockersName[targetName];
            GameObject go = LockersObjects[targetName];
            Highlight(go);
        }

        if (MonitorsObjects.ContainsKey(targetName))
        {
            objectNameUI.SetActive(true);
            objectNameUI.transform.position = Input.mousePosition + Vector3.left * 120;
            objectNameText.text = MonitorNames[targetName];
            GameObject go = MonitorsObjects[targetName];
            Highlight(go);
        }
    }

    /// <summary>
    /// 触发式高亮
    /// </summary>
    /// <param name="go"></param>
    private void Highlight(GameObject go)
    {
        HighlightObject ho = go.GetComponent<HighlightObject>();        //获取高亮组件
        if (ho != null) { ho.MouseOver(); }
    }

    /// <summary>
    /// 显示物体信息详情
    /// </summary>
    /// <param name="target"></param>
    public void ShowObjectInfo(Transform target)
    {
        string targetName = target.name;
        if (DeviceObjects.ContainsKey(targetName))
        {
            int index = Array.IndexOf(DeviceColliders, targetName);
            Dialog._instance.dialog_deviceInfo.SetActive(true);
            DeviceInfo di = Dialog._instance.item_deviceInfo.GetComponent<DeviceInfo>();
            di.InitDeviceInfo(index);
        }

        if (FlowObjects.ContainsKey(targetName))
        {
            int flow = Array.IndexOf(FlowColliders, targetName);
            Dialog._instance.dialog_pdtInfo.SetActive(true);
            ProductionInfo pdt = Dialog._instance.productionInfo.GetComponent<ProductionInfo>();
            pdt.InitFlowInfo(flow, ListManager._instance.brand);
        }

        if (LockersObjects.ContainsKey(targetName))
        {
            int index = Array.IndexOf(LockersColliders, targetName);
            Dialog._instance.dialog_lockersInfo.SetActive(true);
            LockersInfo li = Dialog._instance.lockersInfo.GetComponent<LockersInfo>();
            li.InitLockerInfo(index, ListManager._instance.brand);
        }

        if (MonitorsObjects.ContainsKey(targetName))
        {
            int index = Array.IndexOf(MonitorColliders, targetName);
            //切换到故障报警界面
            TopUIPanel._instance.SetCurrentTab(2);
            ListManager._instance.ShowInfoList(3);
            CameraInfo._instance.SelectCamera(index);
        }
    }

    /// <summary>
    /// 跳转到物体
    /// </summary>
    /// <param name="objectName"></param>
    public void SkipToObject(string objectName)
    {
        foreach (KeyValuePair<string, string> kvp in DeviceCNNames)
        {
            if (kvp.Value.Equals(objectName))
            {
                string key = kvp.Key;
                int index = Array.IndexOf(DeviceColliders, key);
                ListManager._instance.SkipToDevice(index);
                return;
            }
        }
    }

    /// <summary>
    /// 聚焦工序段
    /// </summary>
    /// <param name="index"></param>
    public void FocusFlow(int index)
    {
        if (cameras.transform.childCount < index + 1) { return; }
        GameObject flow = flows[index];
        Transform t = cameras.transform.GetChild(index);
        Camera.main.transform.position = t.position;
        Camera.main.transform.rotation = Quaternion.identity * t.rotation;
        Camera.main.GetComponent<CameraDistance>().Reposition();
        if (index == 3)
        {
            twoFloor.SetActive(false);
            outWall.SetActive(false);
            twoFloorCollider.SetActive(false);
        }
        else
        {
            twoFloor.SetActive(true);
            outWall.SetActive(true);
            twoFloorCollider.SetActive(true);
        }
        //切换到生产段信息
        TopUIPanel._instance.SetCurrentTab(0);
        ListManager._instance.ShowInfoList(1);
        ListManager._instance.SetCurrentProductionTab(index);
    }

    /// <summary>
    /// 禁用高亮闪烁：达到高亮切换的效果
    /// </summary>
    private void DisableFlashing()
    {
        foreach (GameObject go in devices)
        {
            HighlightObject ho = go.GetComponent<HighlightObject>();
            ho.FlashOff();
        }
    }
}

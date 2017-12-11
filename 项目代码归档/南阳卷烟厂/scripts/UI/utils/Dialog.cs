/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	Device 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年10月19日16:17:49# 
 *Description: 		   	弹窗：获取相关组件，方便引用
 *History: 				修改版本记录
*/

using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{

    public static Dialog _instance;
    //物体名称
    public GameObject ObjectNameUI;
    //设备信息
    public GameObject dialog_deviceInfo;
    public GameObject item_deviceInfo;
    public GameObject dragpanel_shebei;
    //历史记录
    public GameObject dialog_historyInfo;
    //生产信息
    public GameObject dialog_pdtInfo;
    public GameObject productionInfo;
    public GameObject dragpanel_pdt;
    //储柜信息
    public GameObject dialog_lockersInfo;
    public GameObject lockersInfo;
    public GameObject dragpanel_lockers;
    //故障报警
    public GameObject alarmDialog;
    public Button close;
    public Button tiaozhuan;
    public Text alarmText;
    //确认故障
    public GameObject confirmDialog;
    public Button confirm;
    public Button cancel;
    //确认退出
    public GameObject dialog_exit;
    public Button exit;
    public Button stay;

    void Awake()
    {
        _instance = this;
    }

}

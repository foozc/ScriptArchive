/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	VoiceManager 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年10月10日17:57:23# 
 *Description: 		   	视角管理器：控制视角切换面板、切换视角   
 *History: 				修改版本记录
*/

using HighlightingSystem;
using UnityEngine;
using UnityEngine.UI;

public class ViewManager : MonoBehaviour
{
    public static ViewManager _instance;

    [HideInInspector]
    public int currentView = 0;                //当前视角，默认人视角0;第三人称视角1；鸟瞰视角2

    public Button renView, sanView, niaoView;
    private Vector3 btn0Pos, btn1Pos, btn2Pos;
    private Vector3[] pos;
    public Transform bg;
    private bool isOpen = false;

    public GameObject FPSController;
    public GameObject TPSContrller;
    public GameObject Character;
    public GameObject dingbu;       //顶部
    public GameObject zhuzi;        //柱子
    public GameObject birdView;
    public GameObject birdViewMap;

    private Quaternion lastQuaternion;
    private Vector3 lastPos;

    void Awake()
    {
        _instance = this;
    }

    // Use this for initialization
    void Start()
    {
        btn0Pos = renView.transform.position;
        btn1Pos = sanView.transform.position;
        btn2Pos = niaoView.transform.position;
        pos = new Vector3[] { btn0Pos, btn1Pos, btn2Pos };
        bg.gameObject.SetActive(false);

        renView.onClick.AddListener(OnRenViewClick);
        sanView.onClick.AddListener(On3dViewClick);
        niaoView.onClick.AddListener(OnNiaokanViewClick);

        if (FPSController)
        {
            Camera.main.transform.localPosition = new Vector3(0, 0.9f, 0);
            Camera.main.transform.localRotation = Quaternion.identity;
        }
        if (TPSContrller) { TPSContrller.SetActive(false); }
        dingbu.SetActive(true);
        zhuzi.SetActive(false);
        Camera.main.gameObject.AddComponent<HighlightingRenderer>();
    }

    /// <summary>
    /// 人视角（第一人称视角）按钮点击事件
    /// </summary>
    private void OnRenViewClick()
    {
        switch (currentView)
        {
            case 0:
                if (isOpen) { bg.gameObject.SetActive(false); isOpen = false; }
                else { bg.gameObject.SetActive(true); isOpen = true; }
                break;
            case 1:
                ExChangePosition(sanView, renView);
                currentView = 0;
                SwitchView(1, 0);
                break;
            case 2:
                ExChangePosition(niaoView, renView);
                currentView = 0;
                SwitchView(2, 0);
                break;
        }
    }

    /// <summary>
    /// 3d视角（第三人称）按钮点击事件
    /// </summary>
    private void On3dViewClick()
    {
        switch (currentView)
        {
            case 0:
                ExChangePosition(renView, sanView);
                currentView = 1;
                SwitchView(0, 1);
                break;
            case 1:
                if (isOpen) { bg.gameObject.SetActive(false); isOpen = false; }
                else { bg.gameObject.SetActive(true); isOpen = true; }
                break;
            case 2:
                ExChangePosition(niaoView, sanView);
                currentView = 1;
                SwitchView(2, 1);
                break;
        }
    }

    /// <summary>
    /// 鸟瞰视角按钮点击事件
    /// </summary>
    private void OnNiaokanViewClick()
    {
        switch (currentView)
        {
            case 0:
                ExChangePosition(renView, niaoView);
                currentView = 2;
                SwitchView(0, 2);
                break;
            case 1:
                ExChangePosition(sanView, niaoView);
                currentView = 2;
                SwitchView(1, 2);
                break;
            case 2:
                if (isOpen) { bg.gameObject.SetActive(false); isOpen = false; }
                else { bg.gameObject.SetActive(true); isOpen = true; }
                break;
        }
    }

    /// <summary>
    /// 切换视角
    /// </summary>
    /// <param name="last"></param>
    /// <param name="current"></param>
    private void SwitchView(int last, int current)
    {
        if (last == 0 && current == 1)      //第一人称转第三人称
        {
            lastQuaternion = FPSController.transform.localRotation;
            Destroy(Camera.main.gameObject.GetComponent<HighlightingRenderer>());
            FPSController.SetActive(false);
            TPSContrller.SetActive(true);
            Camera.main.gameObject.AddComponent<HighlightingRenderer>();
            Character.transform.position = FPSController.transform.position;
            Character.transform.localRotation = lastQuaternion;
        }
        if (last == 0 && current == 2)      //第一人称转鸟瞰
        {
            Destroy(Camera.main.gameObject.GetComponent<HighlightingRenderer>());
            lastPos = FPSController.transform.position;
            lastQuaternion = FPSController.transform.localRotation;
            FPSController.SetActive(false);
            birdView.SetActive(true);
            dingbu.SetActive(false);
            zhuzi.SetActive(true);
            birdViewMap.SetActive(true);
            Camera.main.gameObject.AddComponent<HighlightingRenderer>();
        }

        if (last == 1 && current == 0)      //第三人称转第一人称
        {
            lastQuaternion = Character.transform.localRotation;
            Destroy(Camera.main.gameObject.GetComponent<HighlightingRenderer>());
            TPSContrller.SetActive(false);
            FPSController.SetActive(true);
            Camera.main.gameObject.AddComponent<HighlightingRenderer>();
            FPSController.transform.position = Character.transform.position;
            FPSController.transform.localRotation = lastQuaternion;
        }
        if (last == 1 && current == 2)      //第三人称转鸟瞰
        {
            Destroy(Camera.main.gameObject.GetComponent<HighlightingRenderer>());
            lastPos = Character.transform.position;
            lastQuaternion = Character.transform.localRotation;
            TPSContrller.SetActive(false);
            birdView.SetActive(true);
            dingbu.SetActive(false);
            zhuzi.SetActive(true);
            birdViewMap.SetActive(true);
            Camera.main.gameObject.AddComponent<HighlightingRenderer>();
        }

        if (last == 2 && current == 0)      //鸟瞰转第一人称
        {
            Destroy(Camera.main.gameObject.GetComponent<HighlightingRenderer>());
            birdViewMap.SetActive(false);
            birdView.SetActive(false);
            FPSController.SetActive(true);
            FPSController.transform.localRotation = lastQuaternion;
            FPSController.transform.position = lastPos;
            dingbu.SetActive(true);
            zhuzi.SetActive(false);
            Camera.main.gameObject.AddComponent<HighlightingRenderer>();
        }
        if (last == 2 && current == 1)      //鸟瞰转第三人称
        {
            Destroy(Camera.main.gameObject.GetComponent<HighlightingRenderer>());
            TPSContrller.SetActive(true);
            Character.transform.position = lastPos;
            Character.transform.localRotation = lastQuaternion;
            birdViewMap.SetActive(false);
            birdView.SetActive(false);
            dingbu.SetActive(true);
            zhuzi.SetActive(false);
            Camera.main.gameObject.AddComponent<HighlightingRenderer>();
        }
    }

    /// <summary>
    /// 交换按钮的位置
    /// </summary>
    /// <param name="current"></param>
    /// <param name="selected"></param>
    private void ExChangePosition(Button current, Button selected)
    {
        Vector3 crtPos = current.transform.position;
        Vector3 setPos = selected.transform.position;

        current.transform.parent = bg;
        current.transform.position = setPos;

        selected.transform.parent = transform;
        selected.transform.position = crtPos;

        bg.gameObject.SetActive(false);
        isOpen = false;
    }
}

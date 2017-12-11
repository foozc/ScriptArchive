/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	CameraInfo 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年10月20日17:33:10# 
 *Description: 		   	监控信息  
 *History: 				修改版本记录
*/

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class CameraInfo : MonoBehaviour
{
    public static CameraInfo _instance;

    void Awake()
    {
        _instance = this;
    }

    public GameObject cameraBtnPrefab;
    public WinformCameraUI CameraUI;
    public Transform startPoint;
    public GameObject cameraMask;
    public GameObject CameraShowPanel;
    public GameObject loadPic;
    public GameObject CloseButton;

    private string[] cameras = { "摄像机1", "摄像机2", "摄像机3", "摄像机4", "摄像机5", "摄像机6", "摄像机7", "摄像机8",
                                 "摄像机9", "摄像机10" };
    private const int spacing = 5;          //间距
    private const float width = 1360f;      //总宽度
    private float btnWidth;                 //单个宽度
    private bool isCameraOpen = false;      //监控画面开启状态


    // Use this for initialization
    void Start()
    {
        //.....这里可以加入摄像机信息初始化逻辑

        // CameraShowPanel = GameObject.Find("CameraShowPanel");                        //监控画面UI
        btnWidth = (width - (cameras.Length - 1) * spacing) / cameras.Length;       //计算单个宽度
        if (cameraBtnPrefab) InitCameraPanel();
    }

    /// <summary>
    /// 按照按钮个数摆放
    /// </summary>
    private void InitCameraPanel()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            GameObject btn = Instantiate(cameraBtnPrefab);
            //设置按钮宽度
            Vector2 vec = btn.GetComponent<RectTransform>().sizeDelta;
            vec.x = btnWidth;
            btn.GetComponent<RectTransform>().sizeDelta = vec;
            //设置文本内容和宽度
            Text btnText = btn.transform.Find("Text").GetComponent<Text>();
            btnText.text = cameras[i];
            Vector2 textVec = btnText.GetComponent<RectTransform>().sizeDelta;
            textVec.x = btnWidth;
            btnText.GetComponent<RectTransform>().sizeDelta = textVec;
            //设置父级组件
            btn.transform.SetParent(startPoint);
            //设置位置
            btn.transform.localPosition = new Vector3(i * spacing + i * btnWidth + 0.5f * btnWidth, 0, 0);
            //设置名称
            btn.name = "monitor"+i;
            //设置点击事件
            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                int index = int.Parse(btn.name.Substring(7));
                SelectCamera(index);       //传递摄像机序号
            });
        }
    }

    /// <summary>
    /// 选择摄像机：显示实时画面等
    /// </summary>
    /// <param name="name">摄像机名称</param>
    public void SelectCamera(int index)
    {
        if (cameraMask) { cameraMask.SetActive(true); }

        if (isCameraOpen)
        {
            KillProcess();
        }

        OpenCamera(index);
    }

    /// <summary>
    /// 关闭摄像头画面
    /// </summary>
    public void CloseCamera()
    {
        KillProcess();
        if (cameraMask) { cameraMask.SetActive(false); }
        CameraShowPanel.SetActive(false);
        isCameraOpen = false;
    }

    private void KillProcess()
    {
        CameraUI.closeCameraUI();
    }

    /// <summary>
    /// 通过摄像头id切换到不同摄像头
    /// </summary>
    private void OpenCamera(int index)
    {
        CameraShowPanel.SetActive(true);
        CameraUI.openCamera(index.ToString());
        isCameraOpen = true;
        Debug.Log("当前摄像机：" + cameras[index]);
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Xml;

using UnityEngine.UI;

/// <summary>
/// winform版本的摄像机界面控制器
/// </summary>
public class WinformCameraUI : MonoBehaviour {

    private Process kbpr;
    [DllImport("user32.dll")]
    public static extern int ShowWindow(int hwnd, int nCmdShow);
    [DllImport("user32.dll")]
    static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);
    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);//设置此窗体为活动窗体
    // Use this for initialization
    const int GWL_STYLE = -16;
    const int WS_BORDER = 1;
    IntPtr current;
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(kbpr != null)
        {
            ShowWindow(kbpr.MainWindowHandle.ToInt32(), 2);
            SetWindowLong(kbpr.MainWindowHandle, GWL_STYLE, WS_BORDER);
            SetForegroundWindow(kbpr.MainWindowHandle);
            SetWindowPos(kbpr.MainWindowHandle, -1, 880, 500, -5, 500, 1 | 2);
        }
	}

    private void OnDestroy()
    {

        ToolControlTaskBar.ShowTaskBar();
    }

    //private void OnGUI()
    //{
    //    if (GUILayout.Button("开始浏览"))
    //    {
    //        openCameraUI(Application.streamingAssetsPath+ @"\IPCameraAssets\IPCamera\PreviewDemo\bin\PreviewDemo.exe");
    //    }
    //    if (GUILayout.Button("结束浏览"))
    //    {
    //        closeCameraUI();
    //    }
    //}

    public void openCamera(string cameraNum)
    {
        updatexml(cameraNum);
        openCameraUI(Application.streamingAssetsPath + @"\IPCameraAssets\IPCamera\PreviewDemo\bin\PreviewDemo.exe");
    }

    private void updatexml(string cameraString)
    {
        UnityEngine.Debug.Log("cameraString is" + cameraString);
        string newIp = "";
        switch (cameraString)
        {
            case "0":
                newIp = "10.65.110.75";
                break;
            case "1":
                newIp = "10.65.110.66";
                break;
            case "2":
                newIp = "10.65.110.67";
                break;
            case "3":
                newIp = "10.65.110.68";
                break;
            case "4":
                newIp = "10.65.110.69";
                break;
            case "5":
                newIp = "10.65.110.70";
                break;
            case "6":
                newIp = "10.65.110.71";
                break;
            case "7":
                newIp = "10.65.110.72";
                break;
            case "8":
                newIp = "10.65.110.73";
                break;
            case "9":
                newIp = "10.65.110.74";
                break;
            

        }

        XmlDocument xmlDoc = new XmlDocument();
        //Application.streamingAssetsPath + @"\IPCameraAssets\IPCamera\PreviewDemo\bin\PreviewDemo.exe"
        // xmlDoc.Load("Config.xml");//加载xml文件，文件
        xmlDoc.Load(Application.streamingAssetsPath + @"\IPCameraAssets\IPCamera\PreviewDemo\bin\Config.xml");
        XmlNode xns = xmlDoc.SelectSingleNode("Config");//查找要修改的节点

        XmlNodeList xnl = xns.ChildNodes;//取出所有的子节点

        foreach (XmlNode xn in xnl)
        {
            XmlElement xe = (XmlElement)xn;//将节点转换一下类型
                                           //if (xe.GetAttribute("类别") == "文学")//判断该子节点是否是要查找的节点
                                           //{
                                           //    xe.SetAttribute("类别", "娱乐");//设置新值
                                           //}
                                           //else//为了有更明显的效果，所以不管是否是符合条件的子节点，我都给一个操作
                                           //{
                                           //    //xe.SetAttribute("类别", "文学");
                                           //}

            XmlNodeList xnl2 = xe.ChildNodes;//取出该子节点下面的所有元素
            foreach (XmlNode xn2 in xnl2)
            {
                XmlElement xe2 = (XmlElement)xn2;//转换类型
                if (xe2.Name == "Camera")//判断是否是要查找的元素
                {
                    XmlNodeList xnl3 = xe2.ChildNodes;
                    foreach (XmlNode xn3 in xnl3)
                    {
                        XmlElement xe3 = (XmlElement)xn3;
                        if (xe3.Name == "ipAddress")
                        {
                            xe3.InnerText = newIp;
                        }
                    }
                }
                //break;//这里为了明显效果 我注释了break,用的时候不用，这个大家都明白的哈
            }
            //break;
        }
        xmlDoc.Save(Application.streamingAssetsPath + @"\IPCameraAssets\IPCamera\PreviewDemo\bin\Config.xml");//再一次强调 ，一定要记得保存的该XML文件
        //readtext();//这个是为了更好的比对修改前和修改后的结果
    }

    /// <summary>
    /// 打开winform程序的摄像机界面
    /// </summary>
    /// <param name="path"></param>
    public void openCameraUI(string path)
    {
        kbpr = new Process();
        kbpr.StartInfo.FileName = path;
        kbpr.Start();
        SetWindowPos(kbpr.MainWindowHandle, -1, 880, 500, -5, 500, 1 | 2);
        SetWindowLong(kbpr.MainWindowHandle, GWL_STYLE, WS_BORDER);
        SetForegroundWindow(kbpr.MainWindowHandle);
        ToolControlTaskBar.HideTaskBar();
    }

    /// <summary>
    /// 关闭winform程序的摄像机界面
    /// </summary>
    public void closeCameraUI()
    {
        if (kbpr!=null &&!kbpr.HasExited)
        {
            kbpr.Kill();
            ToolControlTaskBar.ShowTaskBar();
        }
    }
}

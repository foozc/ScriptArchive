using UnityEngine;
using System.Collections;
using Assets.Scripts.Logic;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明：进度管理类
*Author:作者
*
*/
public class ProgressManager : SingletonUI<ProgressManager>
{
	public delegate void SetAllProgress(float progress);
    public delegate void SetCurrentDownloadFile(string url);
    public delegate void SetCurrentDownloadProgress(float progress);
    public SetAllProgress setAllProgressEvent;
    public SetCurrentDownloadFile setCurrentDownloadFileEvent;
    public SetCurrentDownloadProgress setCurrentDownloadProgressEvent;

    public UISlider allProgressBar;
    public UISlider singleProgressBar;
    public UILabel fileUrlLable;


    private UIPanel thisUIPanel;
    private float allProgress = 1;
    private float currentDownPro = 0;
	// Use this for initialization
	void Start () {
        thisUIPanel = GetComponent<UIPanel>();
        setAllProgressEvent = setAllProgress;
        setCurrentDownloadFileEvent = setCurrentDownloadFile;
        setCurrentDownloadProgressEvent = setCurrentDownloadProgress;

	}

	
	// Update is called once per frame
	void Update () {
        if (allProgress == 1f)
        {
            if(thisUIPanel.alpha != 0)
                thisUIPanel.alpha = Mathf.Lerp(1, 0, Time.time);
        }
        else
        {
            thisUIPanel.alpha = Mathf.Lerp(0, 1, Time.time);
        }
	}

    /// <summary>
    /// �����������ؽ���
    /// </summary>
    /// <param name="progress"></param>
    private void setAllProgress(float progress)
    {
        allProgressBar.value = progress;
        this.allProgress = progress;
    }

    /// <summary>
    /// ���õ�ǰ�����ļ���Ϣ
    /// </summary>
    /// <param name="fileName"></param>
    private void setCurrentDownloadFile(string fileName)
    {
        fileUrlLable.text = fileName;
    }

    /// <summary>
    /// ���õ�ǰ�����ļ������ؽ���
    /// </summary>
    /// <param name="progress"></param>
    private void setCurrentDownloadProgress(float progress)
    {
        currentDownPro = progress;
        singleProgressBar.value = currentDownPro;
    }

    public bool isReady()
    {
        if (allProgress == 1 && currentDownPro == 1)
            return true;
        return false;
    }
}

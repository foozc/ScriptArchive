/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	CameraMask 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年10月30日16:47:55# 
 *Description: 		   	监控画面遮罩层：点击外部隐藏监控画面
 *History: 				修改版本记录
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMask : MonoBehaviour, IPointerClickHandler
{
    public CameraInfo cameraInfo;
    public GameObject CameraShowPanel;

    public void OnPointerClick(PointerEventData eventData)
    {
        CameraShowPanel.SetActive(false);
        cameraInfo.CloseCamera();
        gameObject.SetActive(false);
    }
}

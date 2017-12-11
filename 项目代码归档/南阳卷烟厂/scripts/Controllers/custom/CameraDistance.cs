/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	CameraDistance 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年11月6日20:28:39# 
 *Description: 		   	鸟瞰视距（y轴坐标）控制：当前视距默认为最大视距，可缩小视距
 *History: 				修改版本记录
*/

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraDistance : MonoBehaviour
{
    private float yMaxLimit;                 //最大视距                    
    private float yMinLimit;                 //最小视距
    private int range = 18;                  //视距变化范围
    private float yCurrent;                  //当前视距（y坐标）
    private float scrollSensitivity = 3.0f;  //鼠标滚轮灵敏度

    // Use this for initialization
    void Start()
    {
        Reposition();                       
    }

    /// <summary>
    /// 定位，确定当前视距
    /// </summary>
    public void Reposition()
    {
        this.yMaxLimit = Camera.main.transform.position.y;
        this.yMinLimit = this.yMaxLimit - range;
        this.yCurrent = yMaxLimit;
    }

    /// <summary>
    /// 滚轮改变视距
    /// </summary>
    void LateUpdate()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            this.yCurrent = Mathf.Clamp(this.yCurrent - (Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity), this.yMinLimit, this.yMaxLimit);
            transform.position = new Vector3(transform.position.x, this.yCurrent, transform.position.z);
        }
    }

}

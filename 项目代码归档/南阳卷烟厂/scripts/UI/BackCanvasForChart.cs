/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	BackCanvasForChart 
 *Author:       	   	#FUZHICHAO# 
 *Date:         	   	#DATE# 
 *Description: 		   	功能描述或者使用说明   
 *History: 				修改版本记录
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackCanvasForChart : MonoBehaviour {



    //LineRenderer  
    private LineRenderer lineRenderer;
    //定义一个Vector3,用来存储鼠标点击的位置  
    private Vector3 position;
    //用来索引端点  
    private int index = 0;
    //端点数  
    private int LengthOfLineRenderer = 0;

    private void Awake()
    {
        //添加LineRenderer组件  
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        //设置材质  
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        //设置颜色  
        //lineRenderer.SetColors(Color.red, Color.yellow);

        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.yellow;
        //设置宽度  
        //lineRenderer.SetWidth(0.02f, 0.02f);
        lineRenderer.startWidth = 2f;
        lineRenderer.endWidth = 2f;

        lineRenderer = GetComponent<LineRenderer>();
        //GameObject.Find("HourButton").GetComponent<Toggle>().onValueChanged.AddListener
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

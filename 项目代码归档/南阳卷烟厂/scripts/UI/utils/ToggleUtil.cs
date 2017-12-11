/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	ToggleUtil 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年10月12日10:58:31# 
 *Description: 		   	Toggle工具类：获取ToggleGroup当前选中的Toggle下标  
 *History: 				修改版本记录
*/

using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUtil
{
    /// <summary>
    /// 获取ToggleGroup当前选中的Toggle下标
    /// </summary>
    /// <param name="group">ToggleGroup所在GameObject</param>
    /// <returns>当前选中的Toggle下标</returns>
    public static int CurrentToggleIndex(GameObject group)
    {
        int index = -1;
        ToggleGroup toggleGroup = group.GetComponent<ToggleGroup>();
        if (toggleGroup == null)
        {
            throw new Exception("ToggleGroup is necessary");
        }

        for (int i = 0; i < group.transform.childCount; i++)
        {
            Toggle tab = group.transform.GetChild(i).gameObject.GetComponent<Toggle>();
            if (tab)
            {
                if (tab.isOn) { index = i; return index; }
            }
            else { throw new Exception("Toggle is necessary"); }
        }
        return index;
    }

    /// <summary>
    /// 当前选中的高亮：设置文字颜色
    /// </summary>
    public static void CurrentToggleHighlight(GameObject group, Color normal, Color highlight)
    {
        ToggleGroup toggleGroup = group.GetComponent<ToggleGroup>();
        if (toggleGroup == null)
        {
            throw new Exception("ToggleGroup is necessary");
        }
        for (int i = 0; i < group.transform.childCount; i++)
        {
            Toggle tab = group.transform.GetChild(i).gameObject.GetComponent<Toggle>();
            Text label = group.transform.GetChild(i).Find("Label").GetComponent<Text>();
            if (tab.isOn) { label.color = highlight; }
            else { label.color = normal; }
        }
    }

    /// <summary>
    /// 设置当前选中的标签
    /// </summary>
    /// <param name="group"></param>
    public static void SetCurrentTab(GameObject group, int index)
    {
        ToggleGroup toggleGroup = group.GetComponent<ToggleGroup>();
        if (toggleGroup == null)
        {
            throw new Exception("ToggleGroup is necessary");
        }
        Toggle tab = group.transform.GetChild(index).gameObject.GetComponent<Toggle>();
        tab.isOn = true;
    }

}

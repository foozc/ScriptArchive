/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	HighlightObject 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年10月10日18:09:33# 
 *Description: 		    高亮物体：鼠标移入时物体高亮；可自定义高亮颜色
 *History: 				修改版本记录
*/

using UnityEngine;
using HighlightingSystem;

public class HighlightObject : MonoBehaviour
{
    public bool seeThrough = false;
    protected bool _seeThrough = false;

    protected Highlighter h;

    void Awake()
    {
        h = GetComponent<Highlighter>();
        if (h == null)
        {
            h = gameObject.AddComponent<Highlighter>();
        }
    }

    void OnEnable()
    {
        _seeThrough = seeThrough;
        if (seeThrough) { h.SeeThroughOn(); }
        else { h.SeeThroughOff(); }
    }

    void Update()
    {
        if (_seeThrough != seeThrough)
        {
            _seeThrough = seeThrough;
            if (_seeThrough) { h.SeeThroughOn(); }
            else
            { h.SeeThroughOff(); }
        }
    }

    /// <summary>
    /// 触发式高亮
    /// </summary>
    public void MouseOver()
    {
        h.On(new Color(0,121,182,255));
    }

    /// <summary>
    /// 自动式高亮
    /// </summary>
    public void HighlightOn()
    {
        h.ConstantOn(Color.red);
    }

    /// <summary>
    /// 高亮闪烁开启：设置高亮颜色和时间间隔
    /// </summary>
    public void FlashOn()
    {
        h.FlashingOn(Color.white, Color.red, 1);
    }

    /// <summary>
    /// 高亮闪烁关闭
    /// </summary>
    public void FlashOff()
    {
        h.FlashingOff();
    }

}
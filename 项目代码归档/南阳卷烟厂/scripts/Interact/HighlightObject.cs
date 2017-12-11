/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	HighlightObject 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017��10��10��18:09:33# 
 *Description: 		    �������壺�������ʱ������������Զ��������ɫ
 *History: 				�޸İ汾��¼
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
    /// ����ʽ����
    /// </summary>
    public void MouseOver()
    {
        h.On(new Color(0,121,182,255));
    }

    /// <summary>
    /// �Զ�ʽ����
    /// </summary>
    public void HighlightOn()
    {
        h.ConstantOn(Color.red);
    }

    /// <summary>
    /// ������˸���������ø�����ɫ��ʱ����
    /// </summary>
    public void FlashOn()
    {
        h.FlashingOn(Color.white, Color.red, 1);
    }

    /// <summary>
    /// ������˸�ر�
    /// </summary>
    public void FlashOff()
    {
        h.FlashingOff();
    }

}
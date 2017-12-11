/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	TabColor 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年10月18日14:06:12# 
 *Description: 		   	标签颜色：鼠标移入时高亮效果，移出时恢复默认   
 *History: 				修改版本记录
*/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Toggle toggle;
    private Text text;
    public Color normal;
    public Color highlight;

    // Use this for initialization
    void Start()
    {
        toggle = GetComponent<Toggle>();
        text = transform.Find("Label").GetComponent<Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (toggle.isOn) { return; }
        text.color = highlight;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (toggle.isOn) { return; }
        text.color = normal;
    }

}

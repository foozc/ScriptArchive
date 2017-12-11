/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	DialogStyle 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年10月19日17:14:29# 
 *Description: 		   	对话框风格：给组件加上全屏遮罩层，达到对话框风格的效果
 *History: 				修改版本记录
*/

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DialogStyle : MonoBehaviour, IPointerClickHandler
{
    private Image image;            //对话框风格半透明遮罩层
    public UnityEvent OnDismiss;    //对话框风格点击外部隐藏时回调事件

    // Use this for initialization
    void Start()
    {
        image = GetComponent<Image>();
        Sprite bg = Resources.Load("Panel_bg", typeof(Sprite)) as Sprite;
        image.sprite = bg;

        Vector2 vec = gameObject.GetComponent<RectTransform>().sizeDelta;
        vec.x = 1920;
        vec.y = 1080;
        gameObject.GetComponent<RectTransform>().sizeDelta = vec;

    }

    /// <summary>
    /// 鼠标点击事件监听,包括左键、右键、滚轮等:弹出框消失
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnDismiss != null)
        {
            OnDismiss.Invoke();
        }
        gameObject.SetActive(false);
    }

    void Update()
    {
        //移动时隐藏
        var directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (directionVector != Vector3.zero)
        {
            if (OnDismiss != null)
            {
                OnDismiss.Invoke();
            }
            gameObject.SetActive(false);
        }
        //按右键隐藏
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (OnDismiss != null)
            {
                OnDismiss.Invoke();
            }
            gameObject.SetActive(false);
        }
    }
}

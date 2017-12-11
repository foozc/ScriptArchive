/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	AlphaAnim 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年11月10日18:00:25# 
 *Description: 		   	鸟瞰平面UI闪烁效果：高亮的图片的透明度动画
 *History: 				修改版本记录
*/

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Animator))]
public class AlphaAnim : MonoBehaviour,IPointerClickHandler
{
    private GameObject map;
    private Image image;
    private Animator animator;

    // Use this for initialization
    void Start()
    {
        map = transform.parent.parent.parent.gameObject;
        image = GetComponent<Image>();
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 高亮开始
    /// </summary>
    public void HighlightPlay()
    {
        animator.SetBool("play", true);
    }

    /// <summary>
    /// 高亮停止
    /// </summary>
    public void HighlightStop()
    {
        animator.SetBool("play", false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int index = int.Parse(transform.parent.name.Substring(4));
        if (animator.GetBool("play"))
        {
            HighlightStop();
            SceneObjects._instance.FocusFlow(index-1);                        //跳转到工序段
            map.SetActive(false);
        }
    }
}

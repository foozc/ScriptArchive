/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	BirdViewMap 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年11月3日14:19:49# 
 *Description: 		   	鸟瞰平面图控制管理
 *History: 				修改版本记录
*/

using UnityEngine;
using UnityEngine.UI;

public class BirdViewMap : MonoBehaviour
{

    public Button openMap;
    public GameObject map;
    public Button closeMap;
    public Transform flows;
    public GameObject flowAreas;

    void Awake()
    {
        openMap.onClick.AddListener(() =>           //打开鸟瞰图
        {
            map.SetActive(true);
        });
        closeMap.onClick.AddListener(() =>          //返回
        {
            //InitHighlightAnim();
            map.SetActive(false);
        });

        //平面图上工序段的点击事件
        for (int i = 0; i < flows.childCount; i++)
        {
            Button btn = flows.transform.GetChild(i).gameObject.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                int index = int.Parse((btn.name).Substring(3));
                Highlight(index);
            });
        }
    }

    /// <summary>
    /// 高亮工序段UI
    /// </summary>
    /// <param name="index"></param>
    public void Highlight(int index)
    {
        InitHighlightAnim();
        AlphaAnim anim = flowAreas.transform.GetChild(7 - index).Find("Highlight").GetComponent<AlphaAnim>();
        if (anim) { anim.HighlightPlay(); }
    }

    /// <summary>
    /// 初始化工序段UI高亮
    /// </summary>
    private void InitHighlightAnim()
    {
        for (int i = 0; i < flowAreas.transform.childCount; i++)
        {
            AlphaAnim anim = flowAreas.transform.GetChild(i).Find("Highlight").GetComponent<AlphaAnim>();
            if (anim) { anim.HighlightStop(); }
        }
    }

}

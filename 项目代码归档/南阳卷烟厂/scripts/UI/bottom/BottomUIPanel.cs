/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	BottomUIPanel 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年10月10日09:31:46# 
 *Description: 		   	底部UI面板：控制底部面板动画、更新表格数据
 *History: 				修改版本记录
*/

using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class BottomUIPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject bottomBg;
    private bool isOpen = false;
    private Tweener tweener;
    public GameObject objectNameUI;

    // Use this for initialization
    void Start()
    {
        //初始化移入移出动画
        tweener = transform.DOLocalMoveY(287f, 0.5f);
        //设置移动类型
        tweener.SetEase(Ease.InOutBack);
        tweener.SetAutoKill(false);// 把autokill 自动销毁设置为false
        tweener.Pause();
        tweener.onComplete = () =>
        {
            isOpen = true;
            bool isPointHover = RectTransformUtility.RectangleContainsScreenPoint(bottomBg.GetComponent<RectTransform>(), Input.mousePosition);
            if (!isPointHover)
            {
                tweener.PlayBackwards();
            }
        };
        tweener.onRewind = () => { isOpen = false; };
    }

    /// <summary>
    /// 鼠标移入：显示表格、更新数据
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerEnter(PointerEventData data)
    {
        if (objectNameUI) objectNameUI.SetActive(false);
        if (!isOpen)
        {
            tweener.PlayForward();
            switch (TopUIPanel._instance.currentTab)
            {
                case 1:
                    ProductionInfoForm._instance.InitProductionInfoList(ListManager._instance.productionFlow,ListManager._instance.brand, 
                                                                        true);
                    break;
                case 2:
                    DeviceInfoForm._instance.InitDeviceInfoList(ListManager._instance.deviceIndex, true);
                    break;
            }
        }
    }

    /// <summary>
    /// 鼠标移出
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerExit(PointerEventData data)
    {
        if (isOpen) { tweener.PlayBackwards(); }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UIButton))]
public class ButtonHover : MonoBehaviour {
    
    /// <summary>
    /// 鼠标移入需要改变的对象
    /// </summary>
    private UIButton BgchangeButton;

    // Use this for initialization
    void Start()
    {
        BgchangeButton = GetComponent<UIButton>();
    }

    public void MouseMoveIn()
    {
        BgchangeButton.normalSprite = BgchangeButton.hoverSprite;
        
        //当鼠标光标移入该对象时触发
    }

    public void MouserMoveOut()
    {
        BgchangeButton.normalSprite = BgchangeButton.pressedSprite;
       
        //当鼠标光标移出该对象时触发
    }
    
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            MouseMoveIn();
        }
        if (Input.GetMouseButtonUp(0))
        {
            MouserMoveOut();
        }
	}
}

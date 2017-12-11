using UnityEngine;
using System.Collections;
using Assets.Scripts.VO;
using System.Collections.Generic;

/*
 * Copyright 2017-07-07
 * Desciption:功能说明:选择界面 
*/
public class BallotScreen : MonoBehaviour {

    // Use this for initialization
    public UILabel OptionA;//问题内容
    public UILabel OptionB;
    public UILabel OptionC;
    public UIToggle ButtonA;//选择按钮
    public UIToggle ButtonB;
    public UIToggle ButtonC;
    public string answer;//答案
    List<string> tempstrs= new List<string>();
    public UISprite Atrue;//选择结果对象
    public UISprite Btrue;
    public UISprite Ctrue;
    bool tempbool = false;//判断是否选择正确
    bool tempbools = false;//判断是否进入消失计时 
    float times = 0;
    float timess = 1f;
    public GameObject tempOBJ;//界面

    void Start ()
    {
       
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (tempbools == true)
        {
            times += Time.deltaTime;
            if (times >= timess)
            {
                tempOBJ.SetActive(false);
                tempbools = false;
                times = 0;
            }

        }
	
	}

    public void UIShowOption(CircuitTopicNew var)
    {
        Debug.Log(var.OptionA);
        ButtonA.Set(false, false);
        ButtonB.Set(false, false);
        ButtonC.Set(false, false);
        Atrue.spriteName = "transparent";
        Btrue.spriteName = "transparent";
        Ctrue.spriteName = "transparent";
        OptionA.text = "A. " + var.OptionA;
        OptionB.text = "B. " + var.OptionB;
        OptionC.text = "C. " + var.OptionC;
        answer = var.Answers;
    }

    public bool confirmResult(string result)
    { return false;    }
}

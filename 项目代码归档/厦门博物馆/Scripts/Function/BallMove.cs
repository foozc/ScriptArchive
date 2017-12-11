using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallMove : MonoBehaviour {

    public  TweenPosition TP;  //TweenPosition脚本
    public GameObject[] Obj;   //物体上的方法调度
    private static BallMove instance;   //自己的实例

    // Use this for initialization
    void Start () {
        TP =this.GetComponent<TweenPosition>();
        instance = this;
    }
	
	// Update is called once per frame
	void Update () {
        

    }

    public void AddFinished()
    {
        TP.onFinished.Clear();
        //给TweenPosition往前播放的添加事件
        //TP.AddOnFinished(Obj[0].GetComponent<LoadAVPro>().LoadAVProLinkSite1);
        TP.AddOnFinished(Obj[1].GetComponent<SetTexture>().setActTrue);
        TP.AddOnFinished(Obj[2].GetComponent<TweenColor>().PlayForward);
    }

    public void AddFinishedReverse()
    {
        //给TweenPosition往后播放的添加事件
        TP.AddOnFinished(Obj[2].GetComponent<TweenColor>().PlayReverse);
    }
    public static BallMove getInstance()
    {
        return instance;
    }



}

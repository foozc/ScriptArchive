using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using System;

public class LoadAVProLinkSite : MonoBehaviour {
    public string _path;  //视频播放地址
    private MovieManager _MovieManager; //MovieManager功能类名
    private BallMove _BM;              //小球移动类
    private SetTexture _setTex;        //设置纹理类

    // Use this for initialization
    void Start () {

        _MovieManager = MovieManager.getInstance();
     

    }
	
	// Update is called once per frame
	void Update () {
    }

    //播放视频
     public void LoadAVProLinkSite1()
    {
        _MovieManager.playMovie(_path, true);
        //播放完调用BallMoveReverse
        _MovieManager.setFinishedPlayingAction(BallMoveReverse);

    }
    //小球往回走
    private void BallMoveReverse(string obj)
    {
        _setTex = SetTexture.getInstance();
        _BM = BallMove.getInstance();
        //播放小球往回走
        _BM.TP.PlayReverse();
        //设置播放的纹理隐藏
        _setTex.setActFalse();
        //清空往前事件调度器
        _BM.TP.onFinished.Clear();
        //添加往回事件调度器方法
        _BM.AddFinishedReverse();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using System;

public class LoadAVPro : MonoBehaviour {
    private string[] _paths; //第一次播放的视频地址
    private MovieManager _MovieManager;
    private BallMove _BM;              //小球移动类
    private SetTexture _setTex;        //设置纹理类

    // Use this for initialization
    void Start () {
        //_MovieManager = MovieManager.getInstance();
        //_paths = new string[7];
        //_paths[0] = "MainUI.mp4";
        //_paths[1] = "MainUIToMenuUI.mp4";
        //_paths[2] = "MenuUI.mp4";
        //_paths[3] = "BigBuckBunny_720p30.mp4";
        //_paths[4] = "SampleCube.mp4";
        //_paths[5] = "SampleSphere.mp4";
        //_paths[6] = "shouye_BJ.mp4";
        //_MovieManager.SetLooping(false);
        //_MovieManager.playMovie(_paths[0],false);
    }
	
	// Update is called once per frame
	void Update () {
       
    }

    //public void LoadAVProLinkSite1()
    //{
    //    int rand = UnityEngine.Random.Range(0, 6);
    //    _MovieManager.playMovie(_paths[rand], true);
    //    //播放完调用BallMoveReverse
    //    _MovieManager.setFinishedPlayingAction(BallMoveReverse);

    //}
    ////小球往回走
    //private void BallMoveReverse(string obj)
    //{
    //    _setTex = SetTexture.getInstance();
    //    _BM = BallMove.getInstance();
    //    //播放小球往回走
    //    _BM.TP.PlayReverse();
    //    //设置播放的纹理隐藏
    //    _setTex.setActFalse();
    //    //清空往前事件调度器
    //    _BM.TP.onFinished.Clear();
    //    //添加往回事件调度器方法
    //    _BM.AddFinishedReverse();
    //}
}

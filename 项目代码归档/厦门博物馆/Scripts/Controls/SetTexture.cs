using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class SetTexture : MonoBehaviour {

    private MovieManager movieManager;   //MovieManager功能类名
    private static SetTexture instance; //自己的实例

    // Use this for initialization
    void Start () {
        movieManager = MovieManager.getInstance();
        //第一次播放完设置纹理
        movieManager.setFinishedPlayingAction(setTexture);

        instance = this;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    //设置播放的那张纹理
    public void setTexture(string obj)
    {
   
        this.gameObject.SetActive(false);

        Vector3 vec = new Vector3(0.5f, 0.5f, 0.5f);
        //设置它的大小
        this.transform.localScale = vec;
    }

    public void setActTrue()
    {
        this.gameObject.SetActive(true);
    }
    //获取setTexture实例
    public static SetTexture getInstance()
    {
        return instance;
    }
    public void setActFalse()
    {
        this.gameObject.SetActive(false);
    }
}

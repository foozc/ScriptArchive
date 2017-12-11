using UnityEngine;
using System.Collections;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:界面载入界面图标旋转
*Author:作者
*
*/
public class Rotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    public void myStart()
    {
        StartCoroutine(loadScene());
    }
    IEnumerator loadScene()
    {
        float time = 2f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;       
        }
        transform.rotation = Quaternion.Euler(0, 0, 360);
        GameObject load = GameObject.Find("load");
        Transform loading = load.transform.Find("loading");
        loading.gameObject.SetActive(false);
        //      yield return null;
        //UIManager.getInstance().loading.SetActive(false);
        //Application.LoadLevel(GlobalConfig.loadName);
    }
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 0, -0.8f);
	}
    /// <summary>
    /// 功能：1控制旋转速度  speed=-0.1f 最好
    ///       2控制是是否旋转
    ///       3实验，在需要的时候，把他们调取出来显示active=true
    ///              在不需要的时候，把他们影藏 active=false
    ///        问题：如果在他们被影藏了，由该如何获取？？
    /// </summary>
    public void transformRotate(float speed, bool start)
    {
        if (start)
        {
            transform.Rotate(0, 0, speed);
        }
    }
}

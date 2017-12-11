using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts.Configs;
using Assets.Scripts.Controller;
using UnityEngine.SceneManagement;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:底部UI的控制类
*Author:作者
*
*/
public class Loading : MonoBehaviour
{

    //异步对象
    AsyncOperation async;

    //读取场景的进度，它的取值范围在0 - 1 之间。
    int progress = 0;
    public Transform rotate;
    void Start()
    {
    }

    public void myStart()
    {
        StartCoroutine(loadScene());
    }

    //注意这里返回值一定是 IEnumerator
    IEnumerator loadScene()
    {
        //异步读取场景。
        //Globe.loadName 就是A场景中需要读取的C场景名称。
        rotate.rotation = Quaternion.Euler(0, 0, 0);
        async = SceneManager.LoadSceneAsync(GlobalConfig.loadName);
        async.allowSceneActivation = true;
        while (async.progress != 1f)
        {
            yield return null;
        }
        float time = 0.5f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            progress += (int)((0.5 - time) / 0.5 * 10);
            if (progress > 100)
                progress = 100;
            yield return null;
        }
        rotate.rotation = Quaternion.Euler(0, 0, 360);
        gameObject.SetActive(false);
    }

    int i = 0;
    void Update()
    {
        if (async != null)
            progress = (int)(async.progress * 100);
        rotate.Rotate(0, 0, -2f);

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
            rotate.Rotate(0, 0, speed);
        }
    }
}
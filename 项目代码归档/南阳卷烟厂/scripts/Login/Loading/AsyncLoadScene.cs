/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	Globe 
 *Author:       	   	#FUZHICHAO# 
 *Date:         	   	#DATE# 
 *Description: 		   	功能描述或者使用说明   
 *History: 				修改版本记录
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Globe
{
    public static string nextSceneName;
}

public class AsyncLoadScene : MonoBehaviour {

    public Slider loadingSlider;
    public Text loadingText;
    private float loadingSpeed = 1;
    private float targetValue;
    private AsyncOperation operation;
    private Tween t;
    public Image loadingPic;

	// Use this for initialization
	void Start () {
        loadingSlider.value = 0.0f;
        if (SceneManager.GetActiveScene().name == "Loading") 
        {
            //start coroutine
            StartCoroutine(AsyncLoading());
        }

      //  t = loadingPic.GetComponent<RectTransform>().DORotate(new Vector3(360, 360, 180), 2f);
        //(new Vector3(0,0,360), 2f);
       // t.SetLoops(-1, LoopType.Restart);
    }

    IEnumerator AsyncLoading()
    {
        operation = SceneManager.LoadSceneAsync(Globe.nextSceneName);
        yield return operation;
    }
	
	// Update is called once per frame
	void Update () {
        //targetValue = operation.progress;

        //if (operation.progress >= 0.9f)
        //{
        //    //operation.progress的值最大为0.9  
        //    targetValue = 1.0f;
        //}

        //if (targetValue != loadingSlider.value)
        //{
        //    //插值运算  
        //    loadingSlider.value = Mathf.Lerp(loadingSlider.value, targetValue, Time.deltaTime * loadingSpeed);
        //    if (Mathf.Abs(loadingSlider.value - targetValue) < 0.01f)
        //    {
        //        loadingSlider.value = targetValue;
        //    }
        //}

        loadingText.text = ((int)(loadingSlider.value * 100)).ToString() + "%";

        if ((int)(loadingSlider.value * 100) == 100)
        {
            //允许异步加载完毕后自动切换场景  
            operation.allowSceneActivation = true;
        }
    }
}


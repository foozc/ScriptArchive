/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	VoiceManager 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年10月10日17:57:23# 
 *Description: 		   	音量管理器：控制音量滑动条的显示和功能   
 *History: 				修改版本记录
*/

using UnityEngine;
using UnityEngine.UI;

public class VoiceManager : MonoBehaviour
{
    public Button btn;
    public GameObject voiceSlider;
    public Slider slider;
    private bool isOpen = false;

    public float defaultVolume = 0.6f;
    public AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        slider.value = defaultVolume;
        audioSource.volume = defaultVolume;
        slider.onValueChanged.AddListener(OnVoiceValueChange);
        voiceSlider.SetActive(false);
        btn.onClick.AddListener(OnVoiceClick);
    }

    private void OnVoiceClick()
    {
        if (!isOpen)
        {
            voiceSlider.SetActive(true);
            isOpen = true;
        }
        else
        {
            voiceSlider.SetActive(false);
            isOpen = false;
        }
    }

    /// <summary>
    /// 设置音量
    /// </summary>
    /// <param name="_value"></param>
    private void OnVoiceValueChange(float _value)
    {
        audioSource.volume = _value;
    }

    void OnDestroy()
    {
        slider.onValueChanged.RemoveAllListeners();
    }
}

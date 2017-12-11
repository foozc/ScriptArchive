using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Logic;
using Assets.Scripts.Configs;

/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明：音效控制类
*Author:作者
*
*/
public class AudioManager : SingletonUI<AudioManager>
{
	public enum MusicNumType
    {
        /// <summary>
        /// 背景音乐
        /// </summary>
        groundMusic = 1,
        /// <summary>
        /// 实时音乐
        /// </summary>
        realtimeMusic, 
        clicki,
        non
    }

    private AudioSource as1;
    private AudioSource as2;
    private Action<string> action;
    private bool PlayFlag1 = false;
    private bool PlayFlag2 = false;
    private string rootPath = "Audio/";//存放音乐的地址
    // Use this for initialization  

    /// <summary>
    /// 设置回调函数
    /// </summary>
    /// <param name="action"></param>
    public void setAction(Action<string> action)
    {
        this.action = action;
    }
    
    void Awake()
    {
        AudioSource[] audios = this.gameObject.GetComponents<AudioSource>();
        as1 = audios[0];
        as2 = audios[1];
        AudioPlay(GlobalConfig.backgroundMusic, AudioManager.MusicNumType.groundMusic);
    }

    // Update is called once per frame  
    void Update()
    {
        if (PlayFlag1)
        {
            if (!as1.isPlaying)
            {
                as1.Play();
            }
        }
        if (PlayFlag2)
        {
            if (!as2.isPlaying)
            {
                if (action != null)
                {
                    action("音乐2播放完毕");
                }
                PlayFlag2 = false;
            }
        }
        
    }

    /// <summary>
    /// 选择音轨1或者音轨2，用选择的音轨来播放音乐
    /// MusicNumTypem默认为1，设置为背景音乐。
    ///                    2，为切换音乐。
    /// </summary>
    /// <param name="MusicNumTypem"></param>
    /// <param name="musicName"></param>
    public void AudioPlay(string musicName, MusicNumType type = MusicNumType.groundMusic)
    {
        switch (type)
        {
            case MusicNumType.groundMusic:
                as1.clip = (AudioClip)Resources.Load(rootPath + musicName, typeof(AudioClip));
                AudioPlay(type);
                PlayFlag1 = true;
                break;

            case MusicNumType.realtimeMusic:
                as2.clip = (AudioClip)Resources.Load(rootPath + musicName, typeof(AudioClip));
                AudioPlay(type);
                PlayFlag2 = true;
                break;
            case MusicNumType.clicki:
                AudioSource cameraAudioClip = GameObject.Find("Camera").GetComponent<AudioSource>();
                cameraAudioClip.clip = (AudioClip)Resources.Load(rootPath + musicName, typeof(AudioClip));
                cameraAudioClip.Play();
                break;
        }
    }
    /// <summary>
    /// 只用于选择音轨
    /// </summary>
    /// <param name="MusicNum"></param>
    public void AudioPlay(MusicNumType MusicNum)
    {
        //播放音乐
        switch (MusicNum)
        {
            case MusicNumType.groundMusic:
                if (!as1.isPlaying)
                {
                    as1.Play();
                    groundScale = 1f;
                    //SetAudioVolume(MusicNumType.groundMusic, 0.2f, false);// 修改背景音乐的初始音量大小
                }
                break;
            case MusicNumType.realtimeMusic:
                if (!as2.isPlaying)
                {
                    as2.Play();
                    groundScale =1f;
                    //SetAudioVolume(MusicNumType.groundMusic, 0.1f, false);// 当有事实音乐时，把修改背景音乐的初始音量变得更小
                }
                break;
            default:
                if (!as1.isPlaying)
                {
                    as1.Play();
                }
                if (!as2.isPlaying)
                {
                    as2.Play();
                    //SetAudioVolume(MusicNumType.groundMusic, 0.2f, true);
                }
                break;
        }
    }
    /// <summary>
    /// MusciNum:1背景音乐暂停
    ///          2 插曲音乐暂停
    ///          3音乐都暂停
    /// </summary>
    /// <param name="MusicNum"></param>
    public void AudioStop(MusicNumType MusicNum)
    {
        switch (MusicNum)
        {
            case MusicNumType.groundMusic:
                if (as1.isPlaying)
                {
                    as1.Stop();
                    PlayFlag1 = false;
                }
                break;
            case MusicNumType.realtimeMusic:
                if (as2.isPlaying)
                {
                    as2.Stop();
                    PlayFlag2 = false;
                }
                break;
            default:
                if (as1.isPlaying)
                {
                    as1.Stop();
                }
                if (as2.isPlaying)
                {
                    as2.Stop();

                }
                break;
        }

    }


    public void AudioPause(MusicNumType MusicNum)
    {
        //暂停音乐
        //这里说一下音乐暂停以后
        //点击播放音乐为继续播放
        //而停止以后在点击播放音乐
        //则为从新播放
        //这就是暂停与停止的区别
        //music.Pause();
        switch (MusicNum)
        {
            case MusicNumType.groundMusic:
                if (as1.isPlaying)
                {
                    as1.Pause();
                    PlayFlag1 = false;
                }
                break;
            case MusicNumType.realtimeMusic:
                if (as2.isPlaying)
                {
                    as2.Pause();
                    PlayFlag2 = false;
                }
                break;
            default:
                if (as1.isPlaying)
                {
                    as1.Pause();
                }
                if (as2.isPlaying)
                {
                    as2.Pause();
                }
                break;
        }
    }

    private float groundScale = 1f;
    public void SetGroundScale(float Scale)
    {
        groundScale = Scale;
    }
    /// <summary>
    /// MusicNum:3 两个音乐声音大小同时加减
    /// </summary>
    /// <param name="MusicNum"></param>
    /// <param name="Volume"></param>
    public void SetAudioVolume(MusicNumType MusicNum, float Volume, bool scale = false)
    {
       // Debug.LogError("ttt:" + Volume);
        switch (MusicNum)
        {
            case MusicNumType.groundMusic:
                //if (as1.isPlaying)
                //{
                    if (scale)
                    {
                        groundScale = Volume;
                        as1.volume = as1.volume * groundScale;
                    }else
                        as1.volume = Volume * groundScale;
                //}
               
                break;
            case MusicNumType.realtimeMusic:
                //if (as2.isPlaying)
                //{
                    as2.volume = Volume;
                //}
                break;
            default:
                //if (as1.isPlaying)
                //{
                if (scale)
                {
                    groundScale = Volume;
                    as1.volume = as1.volume * groundScale;
                }
                else
                {
                    groundScale = 0.2f;
                    as1.volume = Volume * groundScale;
                    as2.volume = Volume;
                }
                break;
        }
    }

    public float getAudioVolume(MusicNumType type)
    {
        switch (type)
        {
            case MusicNumType.groundMusic:
                return as1.volume;
            case MusicNumType.realtimeMusic:
                return as2.volume;
        }
        return 0;
    }
    /// <summary>
    /// MusicNum:3 播放速度同时加快
    /// </summary>
    /// <param name="MusicNum"></param>
    /// <param name="Pitch"></param>
    public void SetAudioPitch(MusicNumType MusicNum, float Pitch)
    {
        switch (MusicNum)
        {
            case MusicNumType.groundMusic:
                if (as1.isPlaying)
                {
                    as1.pitch = Pitch;
                }
                break;
            case MusicNumType.realtimeMusic:
                if (as2.isPlaying)
                {
                    as2.pitch = Pitch;
                }
                break;
            default:
                if (as1.isPlaying)
                {
                    as1.pitch = Pitch;
                }
                if (as2.isPlaying)
                {
                    as2.pitch = Pitch;
                }
                break;
        }
    }
}

#if UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_5
#define UNITY_FEATURE_UGUI
#endif

using UnityEngine;
#if UNITY_FEATURE_UGUI
using Assets.Scripts.Logic;
using System;
using System.Collections.Generic;
using RenderHeads.Media.AVProVideo;
using Assets.Scripts.Controller;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明：影片控制类
*Author:作者
*
*/
namespace Assets.Scripts.Logic.Movies
{
    public class Trigger
    {
        private float time;
        private Vector3 pos;
        public float Time
        {
            get{ return time; }
        }
        public Vector3 Pos
        {
            get{ return pos; }
        }
        public Trigger(float time, Vector3 pos)
        {
            this.time = time;
            this.pos = pos;
        }
    }
    public class MovieManager : SingletonUI<MovieManager>
    {
		public MediaPlayer _mediaPlayer;
        public GameObject Show_button;

        /*
         * TriggerTime_C标记数组TriggerTime的下标
         */
        private int TriggerTime_C = 0;

        private float _setVideoSeekSliderValue;
        private bool _wasPlayingOnScrub;
        private GameObject s_Button;
        private string movieName;

        private bool autouDestroy = false;
        private Action<string> finishedPlayingAction;
        private Action<float> movieProgress;


        private List<Trigger> triggers = new List<Trigger>();

        public void InputTimeandPosition(List<Trigger> t_list)
        {
            triggers = t_list;
        }

        private void OnOpenVideoFile(string Videopath)
        {
            TriggerTime_C = 0;
            _mediaPlayer.m_VideoPath = string.Empty;
            _mediaPlayer.m_VideoPath = Videopath;
            if (string.IsNullOrEmpty(_mediaPlayer.m_VideoPath))
            {
                _mediaPlayer.CloseVideo();
            }
            else
            {
                _mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, _mediaPlayer.m_VideoPath);
                _mediaPlayer.Control.Pause();
            }
        }
#endif

        public void setFinishedPlayingAction(Action<string> action)
        {
			finishedPlayingAction = action;
        }
        
        /// <summary>
        /// 暂停视频
        /// </summary>
        public void OnPause()
        {
            if (_mediaPlayer)
            {
                _mediaPlayer.Control.Pause();
            }
        }

        public void OnResume()
        {
            if (_mediaPlayer)
                _mediaPlayer.Control.Play();
        }

        public void OnStop()
        {
            if (_mediaPlayer)
            {
                _mediaPlayer.Control.Stop();
				_mediaPlayer.Control.SetVolume(0);
			}
        }

        /// <summary>
        /// 调整视频时间
        /// </summary>
        public void OnVideoSeekSlider(float value)
        {
            if (_mediaPlayer && value != _setVideoSeekSliderValue)
            {
                float time = value * _mediaPlayer.Info.GetDurationMs();
                _mediaPlayer.Control.Seek(value * _mediaPlayer.Info.GetDurationMs());
                ReviseTT_C(time/1000.0f);
            }
        }

        /// <summary>
        /// 设置更新进度的回调
        /// </summary>
        /// <param name="movieProgress"></param>
        public void setMovieProgressAction(Action<float> movieProgress)
        {
            this.movieProgress = movieProgress;
        }

        /// <summary>
        /// 进度条按下事件，持续调用
        /// </summary>
        /// <param name="value"></param>
        public void OnVideoSliderDown(float value)
        {
            if (_mediaPlayer)
            {
                _wasPlayingOnScrub = _mediaPlayer.Control.IsPlaying();
                if (_wasPlayingOnScrub)
                {
                    _mediaPlayer.Control.Pause();
                }
                OnVideoSeekSlider(value);
            }
        }
        /// <summary>
        /// 进度条数据抬起事件
        /// </summary>
        public void OnVideoSliderUp()
        {
            if (_mediaPlayer && _wasPlayingOnScrub)
            {
                _mediaPlayer.Control.Play();
                _wasPlayingOnScrub = false;
            }
        }

        /// <summary>
        /// 调整音量
        /// </summary>
        public void setAudioVolume(float volume)
        {
            if (_mediaPlayer)
            {
                _mediaPlayer.Control.SetVolume(volume);
            }
        }
        /// <summary>
        /// 重播视频
        /// </summary>
        public void OnRewind()
        {
            if (_mediaPlayer)
            {
                _mediaPlayer.Control.Rewind();
                _mediaPlayer.Control.Play();
                TriggerTime_C = 0;
            }
        }

        /// <summary>
        /// 播放视频
        /// </summary>
        /// <param name="path">电影链接地址</param>
        public void playMovie(string path, bool autouDestroy = true)
        {
			_mediaPlayer.Control.SetVolume(0.3f);
			if (_mediaPlayer)
            {
                if (!path.Equals(""))
                {
                    if (!string.IsNullOrEmpty(_mediaPlayer.m_VideoPath))
                        _mediaPlayer.Control.Stop();
                    path = "Movies/" + path;
                    this.autouDestroy = autouDestroy;
                    movieName = path;
                    _mediaPlayer.Events.AddListener(OnVideoEvent);
                    OnOpenVideoFile(path);
                }
                else _mediaPlayer.Control.Stop();
            }
        }
        public void playMovie(string moviePath, Action<string> action, bool autouDestroy = true)
        {
            finishedPlayingAction = action;
            playMovie(moviePath, autouDestroy);
        }
        /// <summary>
        /// 播放视频
        /// </summary>
        /// <param name="moviePath">电影链接地址</param>
        /// <param name="action">电影播放结束回调方法</param>
        public void playMovie(string moviePath, Action<string> action)
        {
            finishedPlayingAction = action;
            playMovie(moviePath);
        }
        public GameObject PlayByPrefabs(string path, string PrefabsName, string F_PosName)
        {
            GameObject instance = Instantiate(Resources.Load<GameObject>("Prefabs/UI/MyUI/" + PrefabsName));
            Transform tf = GameObject.Find(F_PosName).transform;
            instance.transform.parent = tf;
            instance.transform.localScale = new Vector3(1, 1, 1);
            instance.transform.position = tf.position;
            _mediaPlayer = GetComponent<MediaPlayer>();
            _mediaPlayer.Control.SetLooping(true);
            SetLooping(true);
            playMovie(path);
			return instance;
        }
        /// <summary>
        /// 设置是否重复播放
        /// </summary>
        /// <param name="isLoop"></param>
        public void SetLooping(bool isLoop)
        {
            if (_mediaPlayer == null)
                _mediaPlayer = GetComponent<MediaPlayer>();
            _mediaPlayer.Control.SetLooping(isLoop);
        }

        void Update()
        {
            if (_mediaPlayer && _mediaPlayer.Info != null && _mediaPlayer.Info.GetDurationMs() > 0f)
            {
                float time = _mediaPlayer.Control.GetCurrentTimeMs();
                float d = time / _mediaPlayer.Info.GetDurationMs();
                _setVideoSeekSliderValue = d;
                if(movieProgress != null)
                    movieProgress(d);
                if (triggers.Count > 0 && time >= triggers[TriggerTime_C].Time * 1000.0f && time <= triggers[TriggerTime_C].Time * 1100.0f)
                {
                    OnTriggerTime(TriggerTime_C);
                }
            }
        }
        private void ReviseTT_C(float newtime)
        {
            for (int i = 0; i < triggers.Count; i++)
            {
                if (newtime < triggers[i].Time)
                {
                    TriggerTime_C = i;
                    break;
                }
            }        
        }
        
        private void OnTriggerTime(int TriggerTime_C)
        {
            s_Button = NGUITools.AddChild(Show_button);
            s_Button.transform.localPosition = triggers[TriggerTime_C].Pos;
            s_Button.transform.localScale = new Vector3(1, 1, 1);
            if (TriggerTime_C == triggers.Count - 1)
            {
                TriggerTime_C = 0;
            }
            else
            {
                TriggerTime_C++;
            }
            this.TriggerTime_C = TriggerTime_C;
            OnPause();
        }
        /// <summary>
         /// 继续播放
         /// </summary>
        public void ResumePlay()
        {
            if (_mediaPlayer)
            {
                _mediaPlayer.Control.Play();
            }
        }
        private void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
        {
            switch (et)
            {
                case MediaPlayerEvent.EventType.ReadyToPlay:
                    break;
                case MediaPlayerEvent.EventType.Started:
                    break;
                case MediaPlayerEvent.EventType.FirstFrameReady:
                    break;
                case MediaPlayerEvent.EventType.FinishedPlaying:
                    if (finishedPlayingAction != null)
                        finishedPlayingAction(movieName);
					else
						_mediaPlayer.Pause();
					break;
            }
            Debug.Log("Event: " + et.ToString());
        }
    }
}

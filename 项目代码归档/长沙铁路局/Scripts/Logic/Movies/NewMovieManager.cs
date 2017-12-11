#if UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_5
#define UNITY_FEATURE_UGUI
#endif

using UnityEngine;
#if UNITY_FEATURE_UGUI
using Assets.Scripts.Logic;
using System;
using System.Collections.Generic;
using RenderHeads.Media.AVProVideo;

namespace Assets.Scripts.Logic.Movies
{
    public class NewMovieManager :MonoBehaviour
    {
		public MediaPlayer _mediaPlayer;
        /*
         * TriggerTime_C标记数组TriggerTime的下标
         */
        private int TriggerTime_C = 0;

        private float _setVideoSeekSliderValue;
        private bool _wasPlayingOnScrub;
        private bool VideoIsPlay = false;
        private GameObject s_Button;
        private float _setAudioVolumeSliderValue;
        private string movieName;

        private bool autouDestroy = false;
        private Action<string> finishedPlayingAction;
        
        /*
         *  t_list储存事件的对象
         */
        private List<Trigger> t_list = new List<Trigger>();

        public void InputTimeandPosition(List<Trigger> t_list)
        {
            this.t_list = t_list;
        }
        private void OnOpenVideoFile(string Videopath)
        {
            TriggerTime_C = 0;
            _mediaPlayer.m_VideoPath = string.Empty;
            _mediaPlayer.m_VideoPath =Videopath;
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

		/// <summary>
		/// 调整视频时间
		/// </summary>
		public void OnVideoSeekSlider(float timeSeek)
		{
			if (_mediaPlayer!=null)
			{
				_mediaPlayer.Control.Seek(timeSeek);
			}
		}

		///// <summary>
		///// 调整音量
		///// </summary>
		public void audioVolume(float audioVolume)
		{
			if (_mediaPlayer!=null)
			{
				_mediaPlayer.Control.SetVolume(audioVolume);
			}
		}
		/// <summary>
		/// 重播视频
		/// </summary>
		public void OnRewindButton()
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
        public void playMovie( string path, bool autouDestroy = true)
        {
            if (_mediaPlayer)
            {
                path = "Movies/" + path;
                this.autouDestroy = autouDestroy;
                movieName = path;
                _mediaPlayer.Events.AddListener(OnVideoEvent);
                OnOpenVideoFile(path);
            }
            else
            {
                print(_mediaPlayer);
            }
        }
        /// <summary>
        /// 播放视频
        /// </summary>
        /// <param name="moviePath">电影链接地址</param>
        /// <param name="action">电影播放结束回调方法</param>
        public void playMovie(string moviePath, Action<string> action, bool autouDestroy = true)
        {
            finishedPlayingAction = action;
            playMovie(moviePath);
        }
        public void PlayByPrefabs(string path, string PrefabsName, string F_PosName)
        {
            GameObject instance = Instantiate(Resources.Load<GameObject>("Prefabs/UI/MyUI/" + PrefabsName));
            Transform tf = GameObject.Find(F_PosName).transform;
            instance.transform.parent = tf;
            instance.transform.localScale = new Vector3(1, 1, 1);
            instance.transform.position = tf.position;
            _mediaPlayer = GetComponent<MediaPlayer>();
            _mediaPlayer.Control.SetLooping(true);
            playMovie(path);
        }
        public void SetLooping(bool isLoop)
        {
            if (_mediaPlayer == null)
                _mediaPlayer = GetComponent<MediaPlayer>();
            _mediaPlayer.Control.SetLooping(isLoop);
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
						_mediaPlayer.CloseVideo();
                    break;
            }
            Debug.Log("Event: " + et.ToString());
        }
    }
}

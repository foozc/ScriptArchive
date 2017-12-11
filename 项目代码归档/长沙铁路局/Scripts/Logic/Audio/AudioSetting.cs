using Assets.Scripts.Configs;
using Assets.Scripts.Logic.UI;
using System;
using UnityEngine;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明：音效设置类
*Author:作者
*
*/
namespace Assets.Scripts.Logic.Audio
{
    public class AudioSetting : UIbase
    {
		private float oldBackgroundMusicVolume = 0.0f;
        private float oldSoundEffectVolume = 0f;
        public AudioManager soundSwitch;

        public UISprite backgroundMusicMute;
        public UISprite soundEffectMuteMute;
        public UISlider backgroundMusicSlider;
        public UISlider SoundEffectSlider;

        void Awake()
        {
            
        }
        void Start()
        {
            soundSwitch.AudioPlay(GlobalConfig.backgroundMusic, AudioManager.MusicNumType.groundMusic);
            //读取配置文件
            //initUI();
        }
        
        private bool isFirst = true;
        public void initUI()
        {
            if (GlobalConfig.GameSettingConfig.BackgroundMusicMute)
            {
                UIButton button = backgroundMusicMute.GetComponent<UIButton>();
                backgroundMusicMute.spriteName = "bannotes";
                button.normalSprite = "bannotes";
                button.pressedSprite = "bannotes_pressed";
                button.hoverSprite = "bannotes_hover";
                if (isFirst)
                {
                    backgroundMusicSlider.value = 0;
                    oldBackgroundMusicVolume = (float)GlobalConfig.GameSettingConfig.BackgroundMusicVolume;
                }
            }
            else
            {
                UIButton button = backgroundMusicMute.GetComponent<UIButton>();
                backgroundMusicMute.spriteName = "music";
                button.normalSprite = "music";
                button.pressedSprite = "music_pressed";
                button.hoverSprite = "music_hover";
                if (isFirst)
                {
                    backgroundMusicSlider.value = (float)GlobalConfig.GameSettingConfig.BackgroundMusicVolume;
                    oldBackgroundMusicVolume = (float)GlobalConfig.GameSettingConfig.BackgroundMusicVolume;
                }
            }

            if (GlobalConfig.GameSettingConfig.SoundEffectMute)
            {
                UIButton button = soundEffectMuteMute.GetComponent<UIButton>();
                soundEffectMuteMute.spriteName = "bannotes";
                button.normalSprite = "bannotes";
                button.pressedSprite = "bannotes_pressed";
                button.hoverSprite = "bannotes_hover";
                if (isFirst)
                {
                    SoundEffectSlider.value = 0;
                    oldSoundEffectVolume = (float)GlobalConfig.GameSettingConfig.SoundEffectVolume;
                }
            }else
            {
                UIButton button = soundEffectMuteMute.GetComponent<UIButton>();
                soundEffectMuteMute.spriteName = "aside";
                button.normalSprite = "aside";
                button.pressedSprite = "aside_pressed";
                button.hoverSprite = "aside_hover";
                if (isFirst)
                {
                    SoundEffectSlider.value = (float)GlobalConfig.GameSettingConfig.SoundEffectVolume;
                    oldSoundEffectVolume = (float)GlobalConfig.GameSettingConfig.SoundEffectVolume;
                }
            }
            if (isFirst)
            {
                isFirst = false;
                UIEventListener.Get(soundEffectMuteMute.gameObject).onClick = setMusicqt;
                UIEventListener.Get(backgroundMusicMute.gameObject).onClick = setMusicqt;
                EventDelegate eventDelegate = new EventDelegate(this, "setVolume");
                eventDelegate.parameters[0] = new EventDelegate.Parameter(soundEffectMuteMute.gameObject, "param");
                SoundEffectSlider.onChange.Add(eventDelegate);

                EventDelegate eventDelegate1 = new EventDelegate(this, "setVolume");
                eventDelegate1.parameters[0] = new EventDelegate.Parameter(backgroundMusicMute.gameObject, "param");
                backgroundMusicSlider.onChange.Add(eventDelegate1);
            }
        }
        
        public void setMusicqt(GameObject musicType)
        {
            if(musicType.name.Equals("SoundEffect"))
            {
                if (GlobalConfig.GameSettingConfig.SoundEffectMute)
                {
                    GlobalConfig.GameSettingConfig.SoundEffectMute = false;
                    GlobalConfig.GameSettingConfig.SoundEffectVolume = oldSoundEffectVolume;
                    //soundSwitch.SetAudioVolume(AudioManager.MusicNumType.groundMusic, oldBackgroundMusicVolume);
                    SoundEffectSlider.value = oldSoundEffectVolume;
                }
                else
                {
                    GlobalConfig.GameSettingConfig.SoundEffectMute = true;
                    oldSoundEffectVolume = (float)GlobalConfig.GameSettingConfig.SoundEffectVolume;
                    GlobalConfig.GameSettingConfig.SoundEffectVolume = 0;
                    //soundSwitch.SetAudioVolume(AudioManager.MusicNumType.groundMusic, 0);
                    SoundEffectSlider.value = 0;
                }
            }
            else
            {
                Debug.LogError("abc:"+GlobalConfig.GameSettingConfig.BackgroundMusicMute);
                if (GlobalConfig.GameSettingConfig.BackgroundMusicMute)
                {
                    GlobalConfig.GameSettingConfig.BackgroundMusicMute = false;
                    GlobalConfig.GameSettingConfig.BackgroundMusicVolume = oldBackgroundMusicVolume;
                    //soundSwitch.SetAudioVolume(AudioManager.MusicNumType.realtimeMusic, oldSoundEffectVolume);
                    backgroundMusicSlider.value = oldBackgroundMusicVolume;
                }
                else
                {
                    GlobalConfig.GameSettingConfig.BackgroundMusicMute = true;
                    oldBackgroundMusicVolume = (float)GlobalConfig.GameSettingConfig.BackgroundMusicVolume;
                    GlobalConfig.GameSettingConfig.BackgroundMusicVolume = 0;
                    //soundSwitch.SetAudioVolume(AudioManager.MusicNumType.realtimeMusic, 0);
                    backgroundMusicSlider.value = 0;
                }
            }
            initUI();
        }

        public void setVolume(GameObject musicType)
        {
            if (musicType.name.Equals("SoundEffect"))
            {
                if(GlobalConfig.GameSettingConfig.SoundEffectMute && SoundEffectSlider.value > 0)
                {
                    GlobalConfig.GameSettingConfig.SoundEffectMute = false;
                    initUI();
                }
                GlobalConfig.GameSettingConfig.SoundEffectVolume = SoundEffectSlider.value;
                soundSwitch.SetAudioVolume(AudioManager.MusicNumType.realtimeMusic, (float)(SoundEffectSlider.value));
            }
            else
            {
                if(GlobalConfig.GameSettingConfig.BackgroundMusicMute && backgroundMusicSlider.value > 0)
                {
                    GlobalConfig.GameSettingConfig.BackgroundMusicMute = false;
                    initUI();
                }
                GlobalConfig.GameSettingConfig.BackgroundMusicVolume = backgroundMusicSlider.value;
                soundSwitch.SetAudioVolume(AudioManager.MusicNumType.groundMusic, (float)(backgroundMusicSlider.value));
            }
        }

        public void setVolume(GameObject musicType,int volume)
        {
            if (musicType.name.Equals("SoundEffect"))
            {
                if (GlobalConfig.GameSettingConfig.SoundEffectMute && volume > 0)
                {
                    GlobalConfig.GameSettingConfig.SoundEffectMute = false;
                }
                GlobalConfig.GameSettingConfig.SoundEffectVolume = volume/20;
                soundSwitch.SetAudioVolume(AudioManager.MusicNumType.realtimeMusic, (float)(volume / 20));
            }
            else
            {
                if (GlobalConfig.GameSettingConfig.BackgroundMusicMute && volume > 0)
                {
                    GlobalConfig.GameSettingConfig.BackgroundMusicMute = false;
                }
                GlobalConfig.GameSettingConfig.BackgroundMusicVolume = volume/20;
                soundSwitch.SetAudioVolume(AudioManager.MusicNumType.groundMusic, (float)(volume / 20));
            }
        }

        public override int Depth
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}

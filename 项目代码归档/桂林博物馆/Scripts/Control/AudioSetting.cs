using Assets.Scripts.Configs;
using System;
using UnityEngine;

namespace Assets.Scripts.Logic.Audio
{
    public class AudioSetting : MonoBehaviour
    {
        public AudioManager soundSwitch;
        
        public UISlider backgroundMusicSlider;
        public UISlider SoundEffectSlider;
        
        void Start()
        {
            //读取配置文件
            initUI();
        }

        public void initUI()
        {
    
            SoundEffectSlider.value = (float)GlobalConfig.GameSettingConfig.SoundEffectVolume;
            EventDelegate eventDelegate = new EventDelegate(this, "volumeChange");
            backgroundMusicSlider.onChange.Add(eventDelegate);
        }
        
        public void volumeChange()
        {
            GlobalConfig.GameSettingConfig.SoundEffectVolume = backgroundMusicSlider.value;
            //soundSwitch.SetAudioVolume(AudioManager.MusicNumType.realtimeMusic, backgroundMusicSlider.value);
            GlobalConfig.GameSettingConfig.BackgroundMusicVolume = backgroundMusicSlider.value;
            soundSwitch.SetAudioVolume(AudioManager.MusicNumType.non, backgroundMusicSlider.value);
        }
        
        public void setVolume(GameObject musicType)
        {
            if (musicType.name.Equals("SoundEffect"))
            {
                GlobalConfig.GameSettingConfig.SoundEffectVolume = SoundEffectSlider.value;
                soundSwitch.SetAudioVolume(AudioManager.MusicNumType.non, (float)(SoundEffectSlider.value));
            }
            else
            {
                GlobalConfig.GameSettingConfig.BackgroundMusicVolume = backgroundMusicSlider.value;
                soundSwitch.SetAudioVolume(AudioManager.MusicNumType.non, (float)(backgroundMusicSlider.value));
            }
        }

        public void setVolume(GameObject musicType,int volume)
        {
            if (musicType.name.Equals("SoundEffect"))
            {
                GlobalConfig.GameSettingConfig.SoundEffectVolume = volume/20;
                soundSwitch.SetAudioVolume(AudioManager.MusicNumType.non, (float)(volume / 20));
            }
            else
            {
                GlobalConfig.GameSettingConfig.BackgroundMusicVolume = volume/20;
                soundSwitch.SetAudioVolume(AudioManager.MusicNumType.non, (float)(volume / 20));
            }
        }

        public void audioSliderActive()
        {
            if (backgroundMusicSlider.gameObject.activeSelf)
                backgroundMusicSlider.gameObject.SetActive(false);
            else
            {
                backgroundMusicSlider.gameObject.SetActive(true);
            }
        }
    }
}

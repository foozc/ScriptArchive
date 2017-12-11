using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Configs
{
	public class GameSetting
    {
        private double backgroundMusicVolume = 1;
        private double soundEffectVolume = 1;
        private bool soundEffectMute;
        private bool backgroundMusicMute;

        public TweenPosition hideMainTable;
        public TweenPosition showDescriptionTable;

        public GameSetting()
        {
            if(PlayerPrefs.GetString("GameSettingFlag", "-1").Equals("-1"))
            {
                BackgroundMusicVolume = 1;
                SoundEffectVolume = 1;
                PlayerPrefs.SetString("GameSettingFlag", "1");
            }else
            {
                backgroundMusicVolume = PlayerPrefs.GetFloat("backgroundMusicVolume", 1);
                soundEffectVolume = PlayerPrefs.GetFloat("soundEffectVolume", 1);
                soundEffectMute = PlayerPrefs.GetInt("soundEffectMute", 1) == 1 ? true : false;
                backgroundMusicMute = PlayerPrefs.GetInt("backgroundMusicMute", 1) == 1 ? true : false;
            }
        }

        public double BackgroundMusicVolume
        {
            get
            {
                return backgroundMusicVolume;
            }

            set
            {
                backgroundMusicVolume = value;
                PlayerPrefs.SetFloat("backgroundMusicVolume", (float)value);
            }
        }

        public double SoundEffectVolume
        {
            get
            {
                return soundEffectVolume;
            }

            set
            {
                soundEffectVolume = value;
                PlayerPrefs.SetFloat("soundEffectVolume", (float)value);
            }
        }

        public bool SoundEffectMute
        {
            get
            {
                return soundEffectMute;
            }

            set
            {
                soundEffectMute = value;
                PlayerPrefs.SetInt("soundEffectVolume", value ? 1 : 0);
            }
        }

        public bool BackgroundMusicMute
        {
            get
            {
                return backgroundMusicMute;
            }

            set
            {
                backgroundMusicMute = value;
                PlayerPrefs.SetInt("backgroundMusicMute", value ? 1 : 0);
            }
        }

        public void hideMain()
        {

        }

        public void showDesription()
        {

        }

    }  
}

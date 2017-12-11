using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明 ：程序音效相关设置
*Author:作者
*
*/
namespace Assets.Scripts.Configs
{
	public class GameSetting
    {
        private bool backgroundMusicMute = false;
        private double backgroundMusicVolume = 1;
        private bool soundEffectMute = false;
        private double soundEffectVolume = 1;

        public bool BackgroundMusicMute
        {
            get
            {
                return backgroundMusicMute;
            }

            set
            {
                backgroundMusicMute = value;
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
            }
        }
    }
}

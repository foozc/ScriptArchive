using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Timers;


namespace Assets.Scripts.Tools
{
    public class Timer
    {
        private static Timer instance;

        public static Timer getInstance()
        {
            if (instance == null)
            {
                instance = new Timer();
            }
            return instance;
        }

        private Timer()
        {
            tick();
        }


        private ulong time = 0;

        private string newTime = null;

        public void tick()
        {
            time = (ulong)(DateTime.Now.Minute);
            newTime = DateTime.Now.ToString("HH:mm:ss:ffff");
        }

        public ulong getTime()
        {
            return time;
        }
        public string getTimeStr()
        {
            return newTime;
        }
    }
}

using Assets.Scripts.Controller;
using Assets.Scripts.Logic;
using System;
using System.Collections.Generic;
using UnityEngine;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:计时控制器
*Author:作者
*/
namespace Assets.Scripts.Tools
{
    public class ClockManager : MonoBehaviour
    {
        private static ClockManager instance;
        private List<Clock> clocks = new List<Clock>();
        
        void Awake()
        {
            instance = this;
        }

        public static ClockManager getInstance()
        {
            if(instance == null)
            {
                instance = UIManager.getInstance().gameObject.AddComponent<ClockManager>();
                //instance = GameObject.FindObjectOfType<ClockManager>();
            }
            return instance;
        }
        
        void Update()
        {
            for(int i = 0; i < clocks.Count; i++)
            {
                if (clocks[i].getClockStage() == Clock.Stage.None)
                    clocks[i].start();
                else clocks[i].update(Time.deltaTime);
            }
        }

        public void addClock(Clock clock)
        {
            clocks.Add(clock);
        }

        public void RemoveClock(Clock clock)
        {
            if (clocks.Contains(clock))
            {
                clocks.Remove(clock);
            }
        }
    }
}

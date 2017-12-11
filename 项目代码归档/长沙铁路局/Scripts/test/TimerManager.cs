using UnityEngine;
using System.Collections;
using Assets.Scripts.Logic;
using System.Collections.Generic;

namespace TimerMaster
{
    public class TimerManager : SingletonUI<TimerManager>
    {
        private List<Timer> timers = new List<Timer>();
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            foreach(Timer timer in timers)
                if (timer != null)
                    timer.Update(Time.deltaTime);
        }

        void OnGUI()
        {
            if (GUILayout.Button("fwiojef"))
            {
                
            }
        }

        //Some time u may need this to avoid conflict when re-init something , just a tip .
        void OnDestory()
        {
            
        }

        void Test()
        {
            Debug.Log("1");
        }


        void Test2()
        {
            Debug.Log("2");
        }
    }
}
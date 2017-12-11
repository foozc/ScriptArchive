using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Event
{


    public class Task
    {

        private TaskIdentity identity;


        /// <summary>
        /// 任务标识
        /// </summary>
        public TaskIdentity Identity
        {
            get { return identity; }
            set { identity = value; }
        }

        private ulong activateTime;


        /// <summary>
        /// 任务激活时间
        /// </summary>
        public ulong ActivateTime
        {
            get { return activateTime; }
            set { activateTime = value; }
        }

        private float deltaTime;

        /// <summary>
        /// 任务间隔时间
        /// </summary>
        public float DeltaTime
        {
            get { return deltaTime; }
            set { deltaTime = value; }
        }

        private TaskEventHandler handler;


        /// <summary>
        /// 任务执行方法
        /// </summary>
        public TaskEventHandler Handler
        {
            get { return handler; }
            set { handler = value; }
        }

        /// <summary>
        /// 任务执行次数
        /// </summary>
        private int triggerTimes;

        public int TriggerTimes
        {
            get { return triggerTimes; }
            set { triggerTimes = value; }
        }


        private bool isPause;


        /// <summary>
        /// 任务是否被暂停
        /// </summary>
        public bool IsPause
        {
            get { return isPause; }
            set { isPause = value; }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;


namespace Assets.Scripts.Event
{
    public delegate void TaskEventHandler();

    public delegate void UpdateHandler(float deltaTime);


    /// <summary>
    /// 作业调度管理
    /// </summary>
    public class SchedulerMgt
    {
        private static SchedulerMgt instance;

        public static SchedulerMgt getInstance()
        {
            if (instance == null)
            {
                instance = new SchedulerMgt();
            }
            return instance;
        }


        public const ulong Factor = 1000000;
        public event UpdateHandler onUpdate;

        private List<Task> mTaskList = new List<Task>();

        private Dictionary<int, Task> mTaskDic = new Dictionary<int, Task>();

        private float mDeltaTime;

        private float mPreviousTime;

        private float mCurrentTime;

        private float mStartTime;

        private List<Task> mTaskNeedDel = new List<Task>();

        private List<Task> mTaskNeedAdd = new List<Task>();


        private SchedulerMgt()
        {
            mDeltaTime = 0;
            mPreviousTime = 0;
            mCurrentTime = 0;
            mStartTime = Time.realtimeSinceStartup;
        }

        public void Update()
        {
            doAddTasks();

            mCurrentTime = Time.realtimeSinceStartup;
            mDeltaTime = mCurrentTime - mPreviousTime;
            if(onUpdate != null)
            {
                onUpdate(mDeltaTime);
            }
            tryTrigger();
            doDelTask();
            mPreviousTime = mCurrentTime;
        }

        private void doDelTask()
        {
            foreach (Task task in mTaskNeedDel)
            {
                mTaskList.Remove(task);
            }
            mTaskNeedDel.Clear();
        }


        private void doAddTasks()
        {
            foreach (Task task in mTaskNeedAdd)
            {
                int index = mTaskList.Count;
                for (int i = 0; i < mTaskList.Count; i++)
                {
                    if (mTaskList[i].ActivateTime > task.ActivateTime)
                    {
                        index = i;
                    }
                }
                mTaskList.Insert(index,task);
            }
            mTaskNeedAdd.Clear();
        }

        public void tryTrigger()
        {
            if (mTaskList.Count == 0)
            {
                return;
            }
            ulong now = (ulong)(mCurrentTime * Factor);
            for (int i = 0; i < mTaskList.Count; i++)
            {
                if (mTaskList[i].IsPause)
                { 
                    mTaskList[i].ActivateTime += (ulong)(mDeltaTime * Factor);
                }
                if (mTaskList[i].ActivateTime > now)
                {
                    break;
                }
                if (mTaskList[i].Identity.IsValid)
                {
                    mTaskList[i].Handler();
                    mTaskList[i].ActivateTime += (ulong)(mTaskList[i].DeltaTime * Factor);
                    if (mTaskList[i].TriggerTimes != -1)
                    {
                        if (--mTaskList[i].TriggerTimes == 0)
                        {
                            DelTask(mTaskList[i].Identity);
                        }
                    }
                }
            }
        }

        public void DelTask(TaskIdentity taskIdentity)
        {
            if (taskIdentity.IsValid)
            {
                taskIdentity.IsValid = false;
                if (mTaskNeedAdd.Contains(mTaskDic[taskIdentity.Id]))
                {
                    mTaskNeedAdd.Remove(mTaskDic[taskIdentity.Id]);
                }
                else if (!mTaskNeedDel.Contains(mTaskDic[taskIdentity.Id]))
                {
                    mTaskNeedDel.Add(mTaskDic[taskIdentity.Id]);
                }
                mTaskDic.Remove(taskIdentity.Id);
            }
        }


        public TaskIdentity AddTask(float delayTime , TaskEventHandler handler , int triggerTimes = 1, float deltaTime = 0)
        {
            System.Diagnostics.Debug.Assert(delayTime > 0, "DelayTime Invalid");
            System.Diagnostics.Debug.Assert(triggerTimes > -1, "TriggerTimes Invalid");
            System.Diagnostics.Debug.Assert(deltaTime > 0, "DeltaTime Invalid");
            int id = UnityEngine.Random.Range(int.MinValue , int.MaxValue);
            while (mTaskDic.ContainsKey(id))
            {
                id = UnityEngine.Random.Range(int.MinValue,int.MaxValue);
            }
            TaskIdentity taskIdentity = new TaskIdentity();
            taskIdentity.Id = id;
            Task task = new Task();
            task.Identity = taskIdentity;
            task.ActivateTime = (ulong)(Factor * (mCurrentTime + delayTime));
            task.Handler = handler;
            task.DeltaTime = deltaTime;
            task.TriggerTimes = triggerTimes;
            mTaskDic.Add(task.Identity.Id,task);

            if (task.TriggerTimes == 0)
            {
                DelTask(taskIdentity);
            }
            return taskIdentity;
        }

        public void PauseTask(TaskIdentity taskIdentity)
        {
            if (taskIdentity.IsValid)
            {
                mTaskDic[taskIdentity.Id].IsPause = true;
            }
        }

        public void ContinueTaks(TaskIdentity taskIdentity)
        {
            if (taskIdentity.IsValid)
            {
                mTaskDic[taskIdentity.Id].IsPause = false;
            }
        }
    }
}

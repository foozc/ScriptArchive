using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Scripts.Log;

/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:计时类
*Author:作者
*/
namespace Assets.Scripts.Tools
{

   
    /// <summary>
    /// 计时器
    /// </summary>
    public class Clock
    {
        public enum Stage
        {
            None = -3,
            /// <summary>
            /// 开始计时，包括延迟计时的计算
            /// </summary>
            Start = -2,
            /// <summary>
            /// 计时完成
            /// </summary>
            Complete = -1,
            /// <summary>
            /// 准备开始计时
            /// </summary>
            Setup = 0,
            /// <summary>
            /// 计时状态
            /// </summary>
            Tick,
            /// <summary>
            /// 计时结束
            /// </summary>
            Finish,
            /// <summary>
            /// 
            /// </summary>
            COUNT
        }
        /// <summary>
        /// 0表示没有延迟
        /// </summary>
        private readonly float delay;
        /// <summary>
        /// 0表示没有间隔限制
        /// </summary>
        private readonly float interval;
        /// <summary>
        /// 0表示没有时间限制，包括延迟时间，计时器总共的运行时间限制
        /// </summary>
        private readonly float duration;
        /// <summary>
        /// 0表示没有次数限制
        /// </summary>
        private readonly uint limitation;
        /// <summary>
        /// 触发机率[0,100]
        /// </summary>
        private readonly byte probability;
        /// <summary>
        /// 触发最小时间间隔,0表示没有间隔限制
        /// </summary>
        private readonly float triggerInterval;

        public delegate void CallbackHandler(Clock c, Stage s, ulong counter, float clockTime);
        /// <summary>
        /// 触发次数
        /// </summary>
        private ulong counter;
        /// <summary>
        /// 计时器当前计时
        /// </summary>
        private float clockTime;
        /// <summary>
        /// 本次触发间隔时间
        /// </summary>
        private float intervalTime;
        private Stage stage;
        private CallbackHandler[] onTriggers = null;
        private Random random = null;
        private int minute;
        private int hour;
        private int second;

        private Clock()
        {
            this.onTriggers = new CallbackHandler[(int)Stage.COUNT];
            this.counter = 0;
            this.clockTime = 0;
            this.stage = Stage.None;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="duration">0表示没有时间限制，包括延迟时间，计时器总共的运行时间限制</param>
        /// <param name="delay">0表示没有延迟</param>
        /// <param name="interval">0表示没有间隔限制</param>
        /// <param name="limitation">0表示没有次数限制</param>
        /// <param name="probability">//触发机率[0,100]</param>
        /// <param name="triggerInterval">触发最小时间间隔,0表示没有间隔限制</param>
        public Clock(float duration, float delay, float interval, uint limitation, byte probability, float triggerInterval)
            : this()
        {
            if (probability > 100)
            {
                probability = 100;
                Logger.warn(Module.Status, "probability exceed 100:" + probability + ",use 100 instead!");
            }
            else
                random = new Random();
            this.duration = duration;
            this.delay = delay;
            this.interval = interval;
            this.limitation = limitation;
            this.probability = probability;
            this.triggerInterval = triggerInterval;

            Logger.debug(Module.Status, "instance clock:" + this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="duration">0表示没有时间限制，包括延迟时间，计时器总共的运行时间限制</param>
        /// <param name="delay">0表示没有延迟</param>
        /// <param name="interval">0表示没有间隔限制</param>
        /// <param name="limitation">0表示没有次数限制</param>
        public Clock(float duration, float delay, float interval, uint limitation)
            : this(duration, delay, interval, limitation, 100, 0)
        {
        }

        /// <summary>
        /// 1秒后触发一次
        /// </summary>
        /// <param name="tick"></param>
        public Clock(CallbackHandler tick)
            : this(0, 0, 0, 1)
        {
            setTick(tick);
        }

        public float getRemainingTime(float time)
        {
            if (time <= delay)
                return duration;
            if (time >= delay + duration)
                return 0;
            return delay + duration - time;
        }

        public Clock setSetup(CallbackHandler ch)
        {
            onTriggers[(int)Stage.Setup] = ch;
            return this;
        }
        public Stage getClockStage() { return this.stage; }
        public Clock setTick(CallbackHandler ch)
        {
            onTriggers[(int)Stage.Tick] = ch;
            return this;
        }
        public Clock setFinish(CallbackHandler ch)
        {
            onTriggers[(int)Stage.Finish] = ch;
            Logger.debug(Module.Status, "onTriggers[(int)Stage.Finish]=" + ch + "," + this);
            return this;
        }

        public void start()
        {
            if (stage != Stage.None)
                throw new Exception("start");
            this.counter = 0;
            this.clockTime = 0;
            stage = Stage.Start;
        }

        public void restart()
        {
            this.counter = 0;
            this.clockTime = 0;
            stage = Stage.Start;
        }

        public void reset()
        {
            this.clockTime = 0;
            this.counter = 0;
        }

        public void finish()
        {
            stage = Stage.Finish;
        }

        public void complete()
        {
            stage = Stage.Complete;
        }

        private bool hitOrMiss()
        {
            if (probability == 100)
                return true;
            if (intervalTime <= triggerInterval)
                return false;
            return random.Next(100) < probability;
        }

        private void tick()
        {
            if (hitOrMiss() == false)
                return;
            intervalTime = 0;
            ++counter;
            if (onTriggers[(int)Stage.Tick] != null)
                onTriggers[(int)Stage.Tick](this, Stage.Tick, counter, clockTime);
        }

        public bool update(float time)
        {
            if (stage == Stage.None || stage == Stage.Complete)
            {
                return true;
            }

            if (stage == Stage.Start)
            {
                clockTime += time;
                if (clockTime < delay)
                    return false;
                else
                    stage = Stage.Setup;
            }

            if (stage == Stage.Setup)
            {
                if (onTriggers[(int)Stage.Setup] != null)
                    onTriggers[(int)Stage.Setup](this, Stage.Setup, 1, clockTime);
                stage = Stage.Tick;
                clockTime = 0;
            }

            if (stage == Stage.Tick)
            {
                clockTime += time;
                intervalTime += time;
                if (interval == 0)
                {
                    tick();
                }
                else
                {
                    if ((limitation == 0 || counter < limitation) && intervalTime >= interval)
                    {
                        tick();
                    }
                }
            }

            if ((limitation == 0 || counter == limitation) && duration != 0 && clockTime + delay >= duration)
            {
                Logger.debug(Module.Status, "stage=Stage.Finish" + this);
                stage = Stage.Finish;
            }

            if (stage == Stage.Finish)
            {
                if (onTriggers[(int)Stage.Finish] != null)
                {
                    Logger.debug(Module.Status, "onTriggers[(int)Stage.Finish](...)" + this);
                    onTriggers[(int)Stage.Finish](this, Stage.Finish, 1, clockTime);
                }
                stage = Stage.Complete;
            }
            return false;
        }

        public void Destroys()
        {
            ClockManager.getInstance().RemoveClock(this);
        }

        public override string ToString()
        {
            return "{Stage:" + stage + "," + GetHashCode() + "}";
        }

        public string UnitConversion()
        {
			second = (int)counter % 60;
            minute = (int) counter / 60 % 60;
            hour =	(int)counter / 3600;
			return hour + "时" + minute + "分" + second + "秒";
        }
    }

}

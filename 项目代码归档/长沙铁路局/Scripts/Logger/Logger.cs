using UnityEngine;
using System.Collections;

using Assets.Scripts.Tools;


namespace Assets.Scripts.Log
{
    public enum Module
    {
        /// <summary>
        /// 总框架
        /// </summary>
        Framework = 0,
        /// <summary>
        /// 网络模块
        /// </summary>
        Network,
        /// <summary>
        /// UI框架
        /// </summary>
        UI,
        /// <summary>
        /// 状态机模块
        /// </summary>
        StateMachine,
        /// <summary>
        /// 资源模块
        /// </summary>
        Resource,
        /// <summary>
        /// 登录模块
        /// </summary>
        Login,
        /// <summary>
        /// 技能模块
        /// </summary>
        Skill,
        /// <summary>
        /// 状态模块
        /// </summary>
        Status,
        /// <summary>
        /// 行为模块
        /// </summary>
        Behaviour,
        /// <summary>
        /// 指令模块
        /// </summary>
        Command,
        /// <summary>
        /// 特效模块
        /// </summary>
        Effect,
        /// <summary>
        /// 动画模块
        /// </summary>
        Animation,
        /// <summary>
        /// 战斗模块
        /// </summary>
        Battle,
        /// <summary>
        /// 英雄
        /// </summary>
        Hero,
        /// <summary>
        /// 怪物
        /// </summary>
        Monster,
        /// <summary>
        /// 领地
        /// </summary>
        Town,
        /// <summary>
        /// 角色成长
        /// </summary>
        GrowUp,
        /// <summary>
        /// 主界面
        /// </summary>
        MainUI,
        /// <summary>
        /// 配置文件
        /// </summary>
        Config,
        COUNT
    }


    public class Logger
    {

        public enum Level
        {
            Debug = 0,
            Info,
            Warn,
            Exception,
            Error
        }


        private static Level[] levels = new Level[(int)Module.COUNT];

        public static void init()
        {
            for (int i = 0; i < (int)Module.COUNT; i++)
            {
                levels[i] = Level.Debug;
            }
        }

        public static void setLevel(Module module, Level lv)
        {
            levels[(int)module] = lv;
        }

        public static bool isEnable(Module module, Level lv)
        {
            return ((int)lv) >= ((int)levels[(int)module]);
        }



        public static void debug(Module module, object message)
        {
            if (isEnable(module, Level.Debug))
            {
                Debug.Log("[DEBG][" + module + "]" + message + "[" + Timer.getInstance().getTimeStr() + "]");
            }
        }

        public static void info(Module module, object message)
        {
            if (isEnable(module, Level.Info))
            {
                Debug.Log("[INFO][" + module + "]" + message + "[" + Timer.getInstance().getTimeStr() + "]");
            }
        }

        public static void exception(Module module, object message)
        { 
            if(isEnable(module, Level.Exception))
            {
                UnityException uex = new UnityException("[EXCEPTION] [" + module + "]" + message + "[" + Timer.getInstance().getTimeStr() + "]");
                Debug.LogException(uex);
            }
        }

        public static void warn(Module module, object message)
        {
            if (isEnable(module, Level.Warn))
            {
                Debug.LogWarning("[WARN][" + module + "]" + message + "[" + Timer.getInstance().getTimeStr() + "]");
            }
        }


        public static void error(Module module, object message)
        {
            if (isEnable(module, Level.Error))
            {
                Debug.LogError("[EROR][" + module + "]" + message + "[" + Timer.getInstance().getTimeStr() + "]");
            }
        }


        public static void log(Module module, object message)
        {
            if (isEnable(module, Level.Info))
            { 
                Debug.Log("[" + module + "]" + message);
            }
        }
    }
}
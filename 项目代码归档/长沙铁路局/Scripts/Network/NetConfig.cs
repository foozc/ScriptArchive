using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Network
{


    /// <summary>
    /// 网络配置
    /// </summary>
    public class NetConfig
    {

        public enum IEPType
        { 
            /// <summary>
            /// KAIFA 
            /// </summary>
            DEVELOPMENT = 0,  

            /// <summary>
            /// 测试
            /// </summary>
            TEST,

            /// <summary>
            /// 发行
            /// </summary>
            PRODUCTION
        }



        /// <summary>
        /// 更新服务器
        /// </summary>
        private static string[] mUpdateServerIEP;


        /// <summary>
        /// 用户验证服务器
        /// </summary>
        private static string[] mUserAuthIEP;


        /// <summary>
        /// 服务器列表
        /// </summary>
        private static string[] mServerListIEP;


        private static string version;

        /// <summary>
        /// 版本号
        /// </summary>
        public static string Version
        {
            get { return NetConfig.version; }
            set { NetConfig.version = value; }
        }

        /// <summary>
        /// 项目状态：开发中，还是应用中
        /// </summary>
        public static bool isDevilop = true;



        private static string gameKey;

        /// <summary>
        /// 游戏名
        /// </summary>
        public static string GameKey
        {
            get { return NetConfig.gameKey; }
            set { NetConfig.gameKey = value; }
        }



        private static string lang;

        /// <summary>
        /// 语言
        /// </summary>
        public static string Lang
        {
            get { return NetConfig.lang; }
            set { NetConfig.lang = value; }
        }



        private static string channel;

        /// <summary>
        /// 渠道
        /// </summary>

        public static string Channel
        {
            get { return NetConfig.channel; }
            set { NetConfig.channel = value; }
        }




        private static string gameID;

        /// <summary>
        /// 游戏ID
        /// </summary>
        public static string GameID
        {
            get { return NetConfig.gameID; }
            set { NetConfig.gameID = value; }
        }



        static NetConfig()
        {
            mUpdateServerIEP = new string[3];
            mUpdateServerIEP[(int)IEPType.DEVELOPMENT] = "183.62.9.66:20083";
            mUpdateServerIEP[(int)IEPType.TEST] = "test.update.com:30083";
            mUpdateServerIEP[(int)IEPType.PRODUCTION] = "update.com:20083";
            mUserAuthIEP = new string[3];
            mUserAuthIEP[(int)IEPType.DEVELOPMENT] = "183.62.9.66:20081";
            mUserAuthIEP[(int)IEPType.TEST] = "account.test.com:20081";
            mUserAuthIEP[(int)IEPType.PRODUCTION] = "account.com:20081";
            mServerListIEP = new string[3];
            mServerListIEP[(int)IEPType.DEVELOPMENT] = "183.62.9.66:20082";
            mServerListIEP[(int)IEPType.TEST] = "servers.test.com:20087";
            mServerListIEP[(int)IEPType.PRODUCTION] = "serverlist.com:20087";

            Version = "1";
            GameKey = "名字待定";
            GameID = "24";
            Lang = "zh";
            Channel = "37wan";
        }



        public static string GetServerListURL(IEPType type = IEPType.DEVELOPMENT)
        {
            return string.Format("http://{0}/37wan/servers",mServerListIEP[(int)type]);
        }

        public static string GetLoginURL(IEPType type = IEPType.DEVELOPMENT)
        {
            return string.Format("http://{0}/login", mUserAuthIEP[(int)type]);
        }

        public static string GetRegisterURL(IEPType type = IEPType.DEVELOPMENT)
        {
            return string.Format("http://{0}/register", mUserAuthIEP[(int)type]);
        }


    }
}

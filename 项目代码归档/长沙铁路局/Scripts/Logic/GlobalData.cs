using UnityEngine;
using System.Collections;


namespace Assets.Scripts.Logic
{

    /// <summary>
    /// 全局静态数据
    /// </summary>
    public sealed class GlobalData
    {
        #region 网络通信部分配置
        /// <summary>
        /// 登录服务器的url
        /// </summary>
        public const string LOGIN_URL_0 = "192.168.1.151:8018";

        /// <summary>
        /// 心跳命令码C-S
        /// </summary>
        public const short OC_HEART_CS = 513;

        /// <summary>
        /// 心跳命令码S-C
        /// </summary>
        public const short OC_HEART_SC = 514;

        /// <summary>
        /// 弹出对话框命令码
        /// </summary>
        public const short OC_DIALO = 545;
        #endregion

    }
}
using System;
using UnityEngine;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明 ：配置系统xml配置文件路径，数据库和服务器配置
*Author:作者
*
*/
namespace Assets.Scripts.Configs
{   
	public class GlobalConfig
    {
        //在这里记录当前切换场景的名称
        public static string loadName;
        /// <summary>
        /// 项目状态：开发中，还是应用中
        /// </summary>
        public static bool isDevilop = true;


        public static GameSetting GameSettingConfig = new GameSetting();
        public static string backgroundMusic = "backgroundmusic01";

        public static string toolTexturePath = Application.streamingAssetsPath + "/ToolImg";

        #region 系统xml配置文件路径配置
        //public static string EquipKnowConfigPath = Application.streamingAssetsPath + "/XFTrain/EquipKnowListTest.xml";
        public static string EquipKnowOneConfigPath = Application.streamingAssetsPath + "/XFTrain/EquipKnowListOne.xml";
		public static string EquipKnowButtonConfigPath = Application.streamingAssetsPath + "/XFTrain/EquipKnowButton.xml";
		public static string EquipKnowTwoConfigPath = Application.streamingAssetsPath + "/XFTrain/EquipKnowListTwo.xml";
        public static string TrainSelectTopicPath = Application.streamingAssetsPath + "/XFTrain/TrainSelectTopics.xml";

        public static string ReciprocalConfigPath = Application.streamingAssetsPath + "/XFTrain/ReciprocalModel.xml";
#if UNITY_EDITOR
        public static string SaveFileImgPath = Application.dataPath + "/WebPlayerTemplates/uiresources/LoginMenu/imgResource/";
#else
        public static string SaveFileImgPath = Application.dataPath + "/uiresources/LoginMenu/imgResource/";
#endif
        #endregion

        #region 数据库和服务器配置
        public static string DBHost = "192.168.2.110";
        public static string DBPort = "3306";
        public static string DBID = "root";
        public static string DBPWD = "123456";
        public static string DbName = "railwaydatabase";

        //public static string DBHost = "localhost";
        //public static string DBPort = "3356";
        //public static string DBID = "songyu";
        //public static string DBPWD = "123456";
        //public static string DbName = "railwaydatabase";



        public static string serverHost = DBHost + ":8080";
        #endregion
    }
}

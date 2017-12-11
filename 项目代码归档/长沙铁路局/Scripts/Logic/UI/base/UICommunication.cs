using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Logic.UI
{
    public class UICommunication
    {
        #region  单例类:Instance属性获取实例
        private static UICommunication instance = null;

        public static UICommunication Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UICommunication();
                }
                return instance;
            }
        }

        private UICommunication()
        {
            responseRegister();
        }
        #endregion

        /// <summary>
        /// 对服务器返回的消息进行响应的方法的注册监听
        /// </summary>
        private void responseRegister()
        {

        }

        /// <summary>
        /// 向UICommunication注册监听服务器发送过来的消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response">收到服务器回复的委托</param>
        /// <param name="ProtocolNumber">协议号</param>
        public void addListener<T>(Action<T> response,short ProtocolNumber)
        {
           
            response_strengthenCom = response as Action<int>;


        }



        #region  装备普通强化
        public void request_equipStrengthenCom(int equipID)
        {
            //CommonReq strengthen = new CommonReq();
            //strengthen.@int = equipID;
            ////封装包并发送到服务器/
            //Packet packet = Packet.getNewPacket();
            //packet.Opcode = GlobalData.EQUIP_STRENGTHEN_COM;
            //packet.put<CommonReq>(strengthen);    //向packet中压入消息类实例，通信实体/
            //MyGameNetwork.getInstance().send(packet); Debug.Log("发送普通强化装备请求====================");
        }

        private Action<int> response_strengthenCom = null;  //装备普通强化/
        //private void response_equipStrengthenCom(Packet packet)
        //{
        //    ShowEquipLvup lvup = new ShowEquipLvup();
        //    lvup = packet.get<ShowEquipLvup>();    //获取消息类实例/

        //    if (response_strengthenCom != null) response_strengthenCom(lvup);  //发布委托/
        //}
        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Network
{
    public delegate T PacketDecodeHandler<T>(MemoryStream stream, ref Packet packet);
    public delegate byte[] PacketEncodeHandler(Packet packet);
    public class PacketCodecManager<T, D, E>
    {
        private Dictionary<uint, D> packetDecoders = null;
        private Dictionary<uint, E> packetEncoders = null;

        private static PacketCodecManager<T, D, E> instance = null;


        public PacketCodecManager()
        {
            packetDecoders = new Dictionary<uint, D>();
            packetEncoders = new Dictionary<uint, E>();
        }
        public static PacketCodecManager<T, D, E> getInstance()
        {
            if (instance == null)
            {
                instance = new PacketCodecManager<T, D, E>();
            }
            return instance;
        }

        private static uint getHashID(string str)
        {
            uint hash = 0;
            for (int i = 0; i < str.Length; i++)
            {
                hash = ((hash * 16777619) & 0xffffffff) ^ (char.ToUpper(str[i]));
            }
            return hash;
        }


        public static uint getHashID(Type t)
        {
            return getHashID(t.Name);
        }

        /// <summary>
        /// 注册编码和解码委托函数
        /// </summary>
        /// <param name="t"></param>
        /// <param name="decoder"></param>
        /// <param name="encoder"></param>
        public void registerCoder(Type t, D decoder, E encoder)
        {
            uint id = getHashID(t);
            if (!packetDecoders.ContainsKey(id))
            {
                packetDecoders.Add(id, decoder);
            }
            else
            {
                Log.Logger.warn(Log.Module.Network, "already exist decoder:" + t.ToString() + ", ignore registration");
            }

            if (!packetEncoders.ContainsKey(id))
            {
                packetEncoders.Add(id, encoder);
            }
            else
            {
                Log.Logger.warn(Log.Module.Network, "already exist encoder:" + t.ToString() + ", ignore registration");
            }
        }



        /// <summary>
        /// 从注册的解码集合中获取packet的解码方法
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public T decode(uint id, MemoryStream stream, ref Packet packet)
        {
            if (packetDecoders.ContainsKey(id))
            {
                PacketDecodeHandler<T> decodeHandler = packetDecoders[id] as PacketDecodeHandler<T>;
                if (decodeHandler == null)
                {
                    //Log.Logger.warn(Log.Module.Network, "DecodeHandler cast from " + packetDecoders[id] + " is null");
                    Debug.Log("**************************************************************************1");
                    return default(T);
                }
                T p = decodeHandler.Invoke(stream, ref packet);           
                Debug.Log("**************************************************************************=" + packet.Opcode+"】【"+packet.Option);
                return p;
            }
            //Log.Logger.warn(Log.Module.Network, "do not exist decoder with msg type:" + id + ", ignore encode");
            Debug.Log("**************************************************************************2");
            return default(T);
        }

        /// <summary>
        /// 从注册的解码集合中获取packet的编码方法
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        public byte[] encode(Packet packet)
        {
            uint id = getHashID(typeof(Packet));
            if (packetEncoders.ContainsKey(id))
            {
                PacketEncodeHandler encodeHandler = packetEncoders[id] as PacketEncodeHandler;
                if (encodeHandler == null)
                {
                    Log.Logger.warn(Log.Module.Network, "EncodeHandler cast from " + packetEncoders[id] + " is null");
                    return null;
                }
                return encodeHandler.Invoke(packet);
            }
            Log.Logger.warn(Log.Module.Network, "do not exist encoder with msg type:" + typeof(Packet) + ", ignore encode");
            return null;
        }
    }
}

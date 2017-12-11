using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

using PBMsg = Google.ProtocolBuffers.IMessage;
using Assets.Scripts.Tools;

namespace Assets.Scripts.Network
{

    /// <summary>
    /// 网络连接和数据收发的入口
    /// </summary>
    public class GameNetwork : BaseNetwork<PBNetMessage>
    {
        public delegate void MsgProcessHandler(PBNetMessage msg);
        private Dictionary<uint, MsgProcessHandler> msgProcesses = null;
        private CodecManager<PBMsg, DecodeHandler<PBMsg>, EncodeHandler<PBMsg>> codecMgr = null;

        private static GameNetwork instance = null;

        public static GameNetwork getInstance()
        {
            if (instance == null)
            {
                instance = new GameNetwork();
            }
            return instance;
        }

        private string token = null;

        private GameNetwork()
        {
            msgProcesses = new Dictionary<uint, MsgProcessHandler>();
            codecMgr = CodecManager<PBMsg, DecodeHandler<PBMsg>, EncodeHandler<PBMsg>>.getInstance();
        }

        public void registerProcess(Type t, MsgProcessHandler process)
        {
            uint id = Utils.getHash(t.ToString());
            registerProcess(id, process);
        }

        public void registerProcess(uint id, MsgProcessHandler process)
        {
            msgProcesses.Add(id, process);
        }

        public bool send<T>(T msg) where T : PBMsg
        {
            if (msg == null || this.state != State.Connected)
            {
                Log.Logger.warn(Log.Module.Network, "send msg:" + msg.GetType() + " failed, for msg == null or no connection");
                return false;
            }
            PBNetMessage t = msgPool.rent();
            byte[] data = codecMgr.encode(typeof(T), msg);
            //byte[] data = Packet.encodeHttp(
            t.setID(CodecManager<PBMsg, DecodeHandler<PBMsg>, EncodeHandler<PBMsg>>.getHashID(typeof(T)));
            t.setData(data);
            t.setName(typeof(T).Name);
            lock (sendQueue)
            {
                sendQueue.Enqueue(t);
            }
            return true;
        }

        protected override void doSend()
        {
            while (senderRunnable)
            {
                PBNetMessage t = null;

                lock (sendQueue)
                {
                    if (sendQueue.Count > 0)
                    {
                        t = sendQueue.Dequeue();
                    }
                }
                if (t != null)
                {
                    int len = 0;
                    try
                    {
                        len = 4 + 4 + t.getData().Length;
                        if (len > sendBuffer.Length)
                        {
                            Log.Logger.error(Log.Module.Network, "sender buffer require length : " + len + "is exceed the sendBuffer is length");
                        }
                        uint totalSize = (uint)(len - 4);
                        byte[] nbs = Utils.toBigBytes(totalSize);
                        System.Array.Copy(nbs, 0, sendBuffer, 0, 4);
                        if (t.getData().Length > 0)
                        {
                            System.Array.Copy(t.getData(), 0, sendBuffer, 8, t.getData().Length);
                        }
                        Log.Logger.debug(Log.Module.Network, "start-send:" + t.getName() + "," + Utils.toHex(sendBuffer, 0, len));
                        socket.send(sendBuffer, len);
                        Log.Logger.debug(Log.Module.Network, "end-send:" + t.getName() + "," + Utils.toHex(sendBuffer, 0, len));
                    }
                    catch (SocketException e)
                    {
                        doDisconnect();
                    }
                    msgPool.recycle(t);
                }
            }
            Log.Logger.info(Log.Module.Network, "doSend exit");
        }




        protected override void doRecv()
        {
            try
            {
                while (recverRunnable)
                {
                    int length = 0;
                    try
                    {
                        length = socket.recv(recvBuffer, 0, 4);
                        Utils.swap(recvBuffer, 4);
                        int len = BitConverter.ToInt32(recvBuffer, 0);
                        length = socket.recv(recvBuffer, 0, len);
                        Utils.swap(recvBuffer, 4);
                        uint id = BitConverter.ToUInt32(recvBuffer, 0);
                        PBMsg msg = codecMgr.decode(id, recvBuffer, 4, length - 4);
                        if (msg != null)
                        {
                            PBNetMessage t = msgPool.rent();
                            t.setMessage(msg);
                            t.setID(id);
                            lock (recvQueue)
                            {
                                recvQueue.Enqueue(t);
                                Log.Logger.debug(Log.Module.Network, "enqueue recv:" + id);
                            }
                        }
                        else
                        {
                            Log.Logger.debug(Log.Module.Network, "recv unrecoginized message with id:" + id);
                        }
                    }
                    catch (SocketException e1)
                    {
                        doDisconnect();
                        Log.Logger.exception(Log.Module.Network,e1.StackTrace);
                    }
                    catch (EndOfStreamException e2)
                    {
                        Log.Logger.exception(Log.Module.Network,e2.StackTrace);
                    }
                }
                Log.Logger.info(Log.Module.Network, "doRecv exit");
            }
            catch (Exception e3)
            {
                Log.Logger.warn(Log.Module.Network, "doRecv exit, encounter exception:" + e3.ToString());
                doDisconnect();
            }
        }

        public override void update()
        {
            if (!dataProcessible)
            {
                return;
            }
            lock (recvQueue)
            {
                while (recvQueue.Count > 0)
                {
                    PBNetMessage t = null;
                    if (recvQueue.Count > 0)
                    {
                        t = recvQueue.Dequeue();
                    }
                    if (t != null)
                    {
                        msgProcesses[t.getID()](t);
                    }
                }
            }
        }

        public void setToken(string token)
        {
            this.token = token;
        }

        public string getToken()
        {
            return token;
        }
    }
}

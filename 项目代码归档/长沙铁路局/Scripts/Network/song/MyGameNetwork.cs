using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Assets.Scripts.Tools;
using Utility;
using System.Threading;
using Assets.Scripts.Logic;


namespace Assets.Scripts.Network
{
    public class MyGameNetwork : BaseNetwork<Packet>
    {
        public delegate void PacketMagProcessHandler(Packet packet);
        private Dictionary<short, PacketMagProcessHandler> packetMsgProcesses = null;
        private PacketCodecManager<Packet, PacketDecodeHandler<Packet>, PacketEncodeHandler> packetCodecMgr = null;

        private static MyGameNetwork instance = null;
        private int Heartbeat_Time_Interval = 5 * 1000;

        private bool isHeartRet = true;


        public new static MyGameNetwork getInstance()
        {
            if (instance == null)
            {
                instance = new MyGameNetwork();
            }
            return instance;
        }
        
        private string token = null;

        private MyGameNetwork()
        {
            cacheStream = new MemoryStream();
            packetMsgProcesses = new Dictionary<short, PacketMagProcessHandler>();
            packetCodecMgr = PacketCodecManager<Packet, PacketDecodeHandler<Packet>, PacketEncodeHandler>.getInstance();
            packetCodecMgr.registerCoder(typeof(Packet), Packet.createPBDecodeHandler<Packet>(Packet.decodeHttps), Packet.createPBEncodeHandler());
        }

        /// <summary>
        /// 注册操作码对应的消息处理函数
        /// </summary>
        /// <param name="t"></param>
        /// <param name="process"></param>
        public void registerProcess(Packet packet, PacketMagProcessHandler process)
        {
            registerProcess(packet.Opcode, process);
        }

        public Packet getPoolPacket()
        {
            return msgPool.rent();
            
        }

        public new void connect(string ip, int port, int timeout)
        {
            registerProcess(GlobalData.OC_HEART_CS, startHeartThread);

            Packet packet = Packet.getNewPacket();
            packet.Opcode = GlobalData.OC_HEART_CS;

            int len = 0;
            try
            {
                lock (sendBuffer)
                {
                    len = packet.PacketAllAvailable;
                    if (len > sendBuffer.Length)
                    {
                        Log.Logger.error(Log.Module.Network, "sender buffer require length : " + len + "is exceed the sendBuffer is length");
                    }
                    byte[] temp = packetCodecMgr.encode(packet);
                    Array.Copy(temp, sendBuffer, temp.Length);

                    Log.Logger.debug(Log.Module.Network, "start-send:" + packet.getName() + "," + Utils.toHex(sendBuffer, 0, len));
                    int song = socket.send(sendBuffer, len);
                    Log.Logger.debug(Log.Module.Network, "end-send:" + packet.getName() + "," + Utils.toHex(sendBuffer, 0, len));
                }
            }
            catch (SocketException e)
            {
                Log.Logger.debug(Log.Module.Network, "连接已断开，重新连接：" + e);
                doDisconnect();
            }
            msgPool.recycle(packet);

            base.connect(ip, port, timeout);

        }


        public void startHeartThread(Packet packet)
        {
            Heartbeat_Time_Interval = packet.Key * 1000;
            registerProcess(GlobalData.OC_HEART_SC, serverHeart);
            Thread heartbeatThread = new Thread(heartbeatPacket);
            heartbeatThread.IsBackground = true;
            heartbeatThread.Start();
        }
        /// <summary>
        /// 心跳包
        /// </summary>
        public void heartbeatPacket()
        {
            while (senderRunnable)
            {
                if (isHeartRet)
                {
                    //isHeartRet = false;
                    Packet packet = Packet.getNewPacket();
                    packet.Opcode = GlobalData.OC_HEART_CS;
                    send(packet);
                    Thread.Sleep(Heartbeat_Time_Interval);
                }
                else
                {
                    doDisconnect();
                    connectOffPopup();
                }
            }
        }

        public void serverHeart(Packet packet)
        {
            isHeartRet = true;
        }

        public void connectOffPopup()
        {
            Packet packet = Packet.getNewPacket();
            packet.Opcode = GlobalData.OC_DIALO;
            Dialog dialog = new Dialog();
            dialog.text = "心跳线程，服务器断开连接！！！";
            packet.put<Dialog>(dialog);
            lock (recvQueue)
            {
                recvQueue.Enqueue(packet);
            }
        }

        /// <summary>
        /// 注册操作码对应的消息处理函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="process"></param>
        public void registerProcess(short id, PacketMagProcessHandler process)
        {
            if (packetMsgProcesses.ContainsKey(id))
                packetMsgProcesses[id] = process;
            else
                packetMsgProcesses.Add(id, process);
        }

        /// <summary>
        /// 发送packet，packet发送之前已经封装好
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool send(Packet packet)
        {
            if (packet == null || this.state != State.Connected)
            {
                Log.Logger.warn(Log.Module.Network, "send msg:" + packet.GetType() + " failed, for msg == null or no connection");
                return false;
            }
            Packet t = msgPool.rent();
            t = packet;
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
                Packet packet = null;

                lock (sendQueue)
                {
                    if (sendQueue.Count > 0)
                    {
                        packet = sendQueue.Dequeue();
                    }
                }
                if (packet != null)
                {
                    int len = 0;
                    try
                    {
                        lock (sendBuffer)
                        {
                            len = packet.PacketAllAvailable;
                            if (len > sendBuffer.Length)
                            {
                                Log.Logger.error(Log.Module.Network, "sender buffer require length : " + len + "is exceed the sendBuffer is length");
                            }
                            byte[] temp = packetCodecMgr.encode(packet);
                            Array.Copy(temp, sendBuffer, temp.Length);

                            Log.Logger.debug(Log.Module.Network, "start-send,操作码：" + packet.Opcode + ",name:" + packet.getName());
                            int song = socket.send(sendBuffer, len);
                            if (song == -1)
                                doDisconnect();
                            Log.Logger.debug(Log.Module.Network, "end-send，操作码：" + packet.Opcode + ",name:" + packet.getName());
                        }
                    }
                    catch (SocketException e)
                    {
                        connectOffPopup();
                        doDisconnect();
                        Log.Logger.error(Log.Module.Network, "网络连接错误：" + e);
                    }
                    msgPool.recycle(packet);
                }
                Thread.Sleep(1);
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
                        //此处暂时不确定需要接受的包体的总共长度，使用BaseNetwork.BUFFER_SIZE长度
                        length = socket.recv(recvBuffer, 0, recvBuffer.Length);
                        if (length != 0)
                            SplitPackage(recvBuffer, length);
                        if (length == -1)
                            doDisconnect();
                    }
                    catch (SocketException e1)
                    {
                        doDisconnect();
                        Log.Logger.exception(Log.Module.Network, e1.StackTrace);
                    }
                    catch (EndOfStreamException e2)
                    {
                        Log.Logger.exception(Log.Module.Network, e2.StackTrace);
                    }
                    Thread.Sleep(1);
                }
                Log.Logger.info(Log.Module.Network, "doRecv exit");
            }
            catch (Exception e3)
            {
                Log.Logger.warn(Log.Module.Network, "doRecv exit, encounter exception:" + e3.ToString());
                doDisconnect();
            }
        }


        private MemoryStream cacheStream = null;
        private short decodeFaildNumber = 0;
        private const short decodeFaildNumberMax = 16;
        /// <summary>
        /// 分包,先将传入的bytes数据写入到cacheStream流的尾部，再将得到的cacheStream复制到新的流中
        /// 从新流中分包得到数据，如果当前流中还有数据则继续分包，分包到最后如果有多余的数据不能完成
        /// 分包操作，则保留这部分数据到cacheStream中等待下次的数据接收，在进行分包。
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="index"></param>
        private void SplitPackage(byte[] bytes, int count)
        {
            cacheStream = new MemoryStream();
            cacheStream.Write(bytes, 0, count);
            cacheStream.Seek(0, SeekOrigin.Begin);
            long cacheStreamPos = 0;
            //记录当前流的位置
            while (cacheStream.Position < cacheStream.Length)
            {
                Packet packet = msgPool.rent();
                uint id = PacketCodecManager<Packet, PacketDecodeHandler<Packet>, PacketEncodeHandler>.getHashID(typeof(Packet));
				packetCodecMgr.decode(id, cacheStream, ref packet);
				if(packet==null ) {
                    Log.Logger.warn(Log.Module.Network, "解析失败！" );
				}else if ( packet.Opcode==514){
                    //Log.Logger.warn(Log.Module.Network, "收到数据，操作码(Opcode)：" + packet.Opcode);

				}
                else
                {
                    Log.Logger.warn(Log.Module.Network, "==========================================收到514以外数据，操作码(Opcode)：" + packet.Opcode);
                }

				if (packet != null)
				{
                    lock (recvQueue)
                    {
                        recvQueue.Enqueue(packet);
                    }
                    decodeFaildNumber = 0;
                    cacheStreamPos = cacheStream.Position;
                }
                else
                {
                    decodeFaildNumber++;
                    Log.Logger.warn(Log.Module.Network, "解析失败:" + decodeFaildNumber + "次，重试" + decodeFaildNumberMax + "次后，清空接受缓存");
                    if (decodeFaildNumber == decodeFaildNumberMax)
                    {
                        cacheStream.Close();
                        cacheStream.Dispose();
                        cacheStream = new MemoryStream();
                    }
                    else
                    {
                        cacheStream.Seek(cacheStreamPos, SeekOrigin.Begin);
                        setCapacity();
                    }
                    break;
                }
                setCapacity();
            }
        }

        public void setCapacity()
        {
            MemoryStream stream = new MemoryStream();
            byte[] buffer = new byte[1024];
            int dateLength = 0;
            while ((dateLength = cacheStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                stream.Write(buffer, 0, dateLength);
                Log.Logger.debug(Log.Module.Network, "未处理字节数据量：" + dateLength);
            }
            cacheStream.Close();
            cacheStream.Dispose();
            cacheStream = stream;
            //cacheStream.Seek(tempPosition, SeekOrigin.Begin);
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
                    Packet t = null;
                    if (recvQueue.Count > 0)
                    {
                        t = recvQueue.Dequeue();
                    }
                    if (t != null)
                    {
                        if (packetMsgProcesses.ContainsKey(t.Opcode))
                            packetMsgProcesses[t.Opcode](t);
                        //packetMsgProcesses[t.getID()](t);
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

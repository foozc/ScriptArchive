using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Threading;
using System.Net;
using System.Net.Sockets;

using Assets.Scripts.Tools;

namespace Assets.Scripts.Network
{
    public class BaseNetwork <T> where T : INetMessage
    {
        private static BaseNetwork<T> instance = null;

        public static BaseNetwork<T> getInstance()
        {
            if (instance == null)
            {
                instance = new BaseNetwork<T>();
            }
            return instance;
        }

        public delegate void OnActiveDisconnectedHandler();

        public delegate void OnPassiveDisconnectedHandler();

        public delegate void OnConnectFailedHandler();

        public delegate void OnConnectSuccessedHandler();

        public enum State
        { 
            Disconnected,
            Connecting,
            Connected
        }

        public const int CLOSE_TIMEOUT = 1000;
        public const int BUFFER_SIZE = 32768;
        public const int POOL_INIT_SIZE = 32;
        public const int SR_INTERVAL_TIME = 200;

        protected ObjectPool<T> msgPool = null;
        protected Queue<T> sendQueue = null;
        protected Queue<T> recvQueue = null;
        //在主动断开连接的处理程序
        public OnActiveDisconnectedHandler onActiveDisconnectedHandler = null;
        //在被动的断开连接的处理程序
        public OnPassiveDisconnectedHandler onPassiveDisconnectedHandler = null;
        //在连接失败的处理程序
        public OnConnectFailedHandler onConnectFailedHandler = null;
        //在连接成功处理程序
        public OnConnectSuccessedHandler onConnectSuccessedHandler = null;

        protected byte[] sendBuffer = null;

        protected byte[] recvBuffer = null;

        protected Thread sender = null;

        protected Thread recver = null;

        protected Thread connector = null;

        protected bool senderRunnable = false;

        protected bool recverRunnable = false;

        protected NetSocket socket = null;

        protected State state = State.Disconnected;

        private bool activeClose = false;

        protected bool dataProcessible = true;

        protected BaseNetwork()
        {
            socket = NetSocket.getInstance();
            msgPool = new ObjectPool<T>(POOL_INIT_SIZE);

            sendQueue = new Queue<T>();
            recvQueue = new Queue<T>();
            sendBuffer = new byte[BUFFER_SIZE];
            recvBuffer = new byte[BUFFER_SIZE];
        }

        protected void connect(string ip, int port, int timeout)
        {
            Log.Logger.warn(Log.Module.Network, this.state);
            if (state != State.Disconnected)
            {
                Log.Logger.warn(Log.Module.Network, "connect : " + ip + ", " + port + "failed , for network is state is not disconnected !");
                return;
            }
            this.state = State.Connecting;
            this.connector = new Thread(
                    delegate()
                    {
                        if (socket.connect(ip, port, timeout))
                        {
                            activeClose = false;
                            senderRunnable = true;
                            recverRunnable = true;
                            sender = new Thread(doSend);
                            sender.IsBackground = true;
                            recver = new Thread(doRecv);
                            recver.IsBackground = true;
                            this.state = State.Connected;
                            Log.Logger.info(Log.Module.Network, "this.state = State.Connected(" + ip + " :  " + port + ")");
                            onConnectSuccessedHandler();
                            sender.Start();
                            recver.Start();
                        }
                        else
                        {
                            this.state = State.Disconnected;
                            Log.Logger.info(Log.Module.Network, "this.state = State.Disconnected(" + ip + ":" + port + ")");
                            onConnectFailedHandler();
                        }
                    }
                );
            connector.Start();
        }


        public bool send(byte[] data, int length)
        {
            if (data == null || length <= 0 || this.state != State.Connected)
            {
                return false;
            }
            T t = msgPool.rent();
            t.decode(data , 0 ,length);
            lock (sendQueue)
            {
                sendQueue.Enqueue(t);
            }
            return true;
        }


        protected void doDisconnect()
        {
            if (!activeClose)
            {
                this.state = State.Disconnected;
                onPassiveDisconnectedHandler();
                close(false);
            }
        }

        protected virtual void doSend()
        {
            while (senderRunnable)
            { 
                T t = default(T);
                lock (sendQueue)
                { 
                    if(sendQueue.Count > 0)
                    {
                        t = sendQueue.Dequeue();
                    }
                }
                if (t != null)
                {
                    int len = 0;
                    try
                    {
                        if (t.encode(sendBuffer, 0, out len))
                        {
                            socket.send(sendBuffer, len);
                        }
                    }
                    catch (SocketException e)
                    {
                        Log.Logger.warn(Log.Module.Network, "doSend exit, encounter exception:" + e.ToString());
                        doDisconnect();
                    }
                    msgPool.recycle(t);
                }
                Thread.Sleep(SR_INTERVAL_TIME);
            }
            Log.Logger.info(Log.Module.Network,"doSend exit");
        }


        protected virtual void doRecv()
        {
            try
            {
                while (recverRunnable)
                {
                    int length = 0;
                    try
                    {
                        length = socket.recv(recvBuffer,0 , 2);
                        Log.Logger.debug(Log.Module.Network, "recved:" + length + "(2)");
                        Utils.swap(recvBuffer,2);
                        ushort len = System.BitConverter.ToUInt16(recvBuffer, 0);
                        length = socket.recv(recvBuffer, 0, len);
                        Log.Logger.debug(Log.Module.Network, "recved:" + length + "(" + len + ")");
                        T t = msgPool.rent();
                        t.decode(recvBuffer, 0 , length);
                        lock (recvQueue)
                        {
                            recvQueue.Enqueue(t);
                        }
                    }
                    catch (SocketException e1)
                    {
                        Log.Logger.warn(Log.Module.Network, "doRecv exit, encounter exception:" + e1.ToString());
                        doDisconnect();
                    }
                    Thread.Sleep(SR_INTERVAL_TIME);
                }
            }
            catch (System.Exception e2)
            {
                Log.Logger.warn(Log.Module.Network, "doRecv exit, encounter exception:" + e2.ToString());
                doDisconnect();
            }
        }

        public void clear()
        {
            lock (sendQueue)
            {
                foreach (T t in sendQueue)
                {
                    msgPool.recycle(t);
                }
                sendQueue.Clear();  
            }
            lock (recvQueue)
            {
                foreach (T t in recvQueue)
                {
                    msgPool.recycle(t);
                }
                sendQueue.Clear();
            }
        }

        protected void close(bool useJoin)
        {
            if (connector != null && connector.IsAlive)
            {
                connector.Abort();
            }
            
            while (sendQueue.Count != 0)
                senderRunnable = true;
            senderRunnable = false;
            recverRunnable = false;
            if (useJoin && sender != null && sender.ThreadState == ThreadState.Background)
            {
                sender.Join();
            }
            if (useJoin && recver != null && recver.ThreadState == ThreadState.Background)
            {
                recver.Join();
            }
            clear();
            socket.close();
            
            if (activeClose && this.state != State.Disconnected)
            {
                this.state = State.Disconnected;
                onActiveDisconnectedHandler();
            }
        }

        public void close()
        {
            activeClose = true;
            close(true);
        }

        public virtual void update()
        {
            if (!dataProcessible)
            {
                return;
            }
        }

        public State getState()
        {
            return state;
        }

        public int getSendQueueSize()
        {
            return sendQueue.Count;
        }

        public int getRecvQueueSize()
        {
            return recvQueue.Count;
        }
    }
}


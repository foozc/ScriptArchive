using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Assets.Scripts.Tools;
using Assets.Scripts.Log;

namespace Assets.Scripts.Network
{

    /// <summary>
    /// socket 网络连接
    /// socket通信包含2部分：
    /// 1.网络连接
    /// 2.数据收发
    /// </summary>
    public class NetSocket
    {
        private static NetSocket instance = null;

        public static NetSocket getInstance()
        {
            if (instance == null)
            {
                instance = new NetSocket();
            }
            return instance;
        }

        private TcpClient client = null;

        private NetworkStream stream = null;

        private bool connectSuccessed = false;

        private ManualResetEvent timeoutObject = new ManualResetEvent(false);

        private int ReadTimeout = 5000;
        private int WritTimeout = 5000;

        public bool isConnected()
        {
            return client != null && client.Connected;
        }



        /// <summary>
        /// 网络连接
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool connect(string ip, int port, int timeout = 5000)
        {
            //将事件状态设置为非终止状态，从而导致线程受阻。 （继承自 EventWaitHandle。）
            timeoutObject.Reset();
            close();
            client = new TcpClient();
            try
            {
                client.BeginConnect(IPAddress.Parse(ip), port, new AsyncCallback(callbackMethod), client);
            }
            catch (Exception e)
            {
                Log.Logger.warn(Log.Module.Network, e);
                return false;
            }
            //阻止当前线程，直到当前 WaitHandle 收到信号。 （继承自 WaitHandle。）(直到timeout，或者timeoutObject.set())
            if (timeoutObject.WaitOne(timeout, false))
            {
                if (!connectSuccessed)
                {
                    return false;
                }
            }
            else
            {
                Log.Logger.info(Log.Module.Network, "Network connect to:" + ip + ":" + port + " timeout of:" + timeout + " at:" + DateTime.Now.TimeOfDay.TotalMilliseconds);
                client.Close();
                client = null;
                return false;
            }
            stream = client.GetStream();


            //stream.ReadTimeout = ReadTimeout;
            //stream.WriteTimeout = WritTimeout;


            return true;
        }


        /// <summary>
        /// 异步回调，做异步处理。
        /// </summary>
        /// <param name="asyncResult"></param>
        private void callbackMethod(IAsyncResult asyncResult)
        {
            try
            {
                connectSuccessed = false;
                TcpClient tcpClient = asyncResult.AsyncState as TcpClient;

                if (tcpClient.Client != null)
                {
                    tcpClient.EndConnect(asyncResult);//结束挂起的异步连接请求。
                    connectSuccessed = true;
                }
            }
            catch (Exception e)
            {
                Log.Logger.warn(Log.Module.Network, e);
            }
            finally
            {
                timeoutObject.Set();
            }
        }



        /// <summary>
        /// 网络关闭
        /// </summary>
        public void close()
        {
            if (!isConnected())
            {
                return;
            }
            stream.Close();
            client.Close();
            client = null;
            stream = null;
        }


        public int send(byte[] data, int length)
        {
            if (!isConnected())
            {
                Log.Logger.warn(Log.Module.Network, "send data failed, no connection made!");
                //throw new SocketException();
                return -1;
            }
            if (stream == null)
            {
                Log.Logger.warn(Log.Module.Network, "send data failed, make sure you have made the connection!");
                return -1;
            }
            if (data == null || data.Length == 0 || length == 0)
            {
                Log.Logger.warn(Log.Module.Network, "send data failed, data is empty");
                return 0;
            }
            if (data.Length < length)
            {
                Log.Logger.warn(Log.Module.Network, "send data failed, data's length less than expired");
                return 0;
            }
            if (stream.CanWrite && client.Connected)
            {
                try
                {
                    stream.Write(data, 0, length);
                    return length;
                }
                catch (IOException e)
                {
                    Log.Logger.warn(Log.Module.Network, "send failed, encounter exception:" + e.ToString());
                    throw e;
                }
            }
            return 0;
        }

        public int recv(byte[] data, int offset , int length )
        {
            if (!isConnected())
            {
                Log.Logger.warn(Log.Module.Network, "接受数据失败，请确认tcpclient已连接!");
                return -1;
            }
            if (stream == null)
            {
                Log.Logger.warn(Log.Module.Network, "接收数据失败，请确保stream已连接!");
                return -1;
            }
            if (data == null || data.Length - offset < length)
            {
                Log.Logger.warn(Log.Module.Network, "接收数据失败，数据长度不够!");
                return 0;
            }

            int remain = length;
            int result = 0;
            int pos = offset;
            while (stream.DataAvailable && remain > 0)
            {
                try
                {
                    result = stream.Read(data, pos, remain);
                    Log.Logger.debug(Log.Module.Network, "接受到的数据大小：" + result);
                    remain -= result;
                    pos += result;
                }
                catch (SocketException e)
                {
                    Logger.exception(Module.Network, e.StackTrace);
                }
                //if (result == 0)
                //{
                //    Logger.warn(Module.Network, "recv failed, reach EOF");
                //    throw new EndOfStreamException();
                //}
            }
            return length - remain;
        }

    }
}

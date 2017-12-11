using System;
using System.IO;
using System.Text;

namespace Assets.Scripts.Tools
{
    public class Utils
    {
        /// <summary>
        /// 端序，字节反转
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        public static void swap(byte[] data, int length)
        {
            for (int i = 0; i < length / 2; ++i)
            {
                byte tmp = data[length - i - 1];
                data[length - i - 1] = data[i];
                data[i] = tmp;
            }
        }

        /// <summary>
        /// 将stream 转换成byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary>
        /// 将byte[]转成Stream
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
        /// <summary>
        /// 将byte[]转成MemoryStream
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static MemoryStream BytesToMemoryStream(byte[] bytes)
        {
            return new MemoryStream(bytes);
        }


        internal static uint getHash(string name)
        {
            uint hash = 0;

            for (int i = 0; i < name.Length; i++)
            {
                hash = ((hash * 16777619) & 0xffffffff) ^ (char.ToUpper(name[i]));
            }
            return hash;
        }

        internal static byte[] toBigBytes(uint totalSize)
        {
            if (BitConverter.IsLittleEndian)
            {
                return reverse(totalSize);
            }
            else
            {
                return getBytes(totalSize);
            }
        }

        internal static byte[] getBytes(uint totalSize)
        {
            return BitConverter.GetBytes(totalSize);
        }

        internal static byte[] reverse(uint totalSize)
        {
            byte[] bs = getBytes(totalSize);
            for (int i = 0; i < bs.Length / 2; i++)
            {
                int tail_idx = bs.Length - i - 1;
                byte b = bs[i];
                bs[i] = bs[tail_idx];
                bs[tail_idx] = b;
            }
            return bs;
        }

        internal static string toHex(byte[] buffer, int offset, int length)
        {
            int len = length;
            StringBuilder hex = new StringBuilder(len * 2);
            for (int i = 0; i < len; i++)
            {
                hex.AppendFormat("{0:x2}",buffer[i + offset]);
            }
            return hex.ToString();
        }

        internal static string subtractBracket(string name, string suffix)
        {
            if (name.EndsWith(suffix))
            {
                return name.Substring(0,name.Length - suffix.Length);
            }
            return name;
        }
        
        /// <summary>
        /// 生成18位ID
        /// </summary>
        /// <returns></returns>
        public static string CreateId()
        {
            string strR = "";
            strR += DateTime.Now.ToString("yyyyMMddHHmmss");
            //生成一个1000-9999之间的随机数
            System.Random roo = new System.Random();
            strR += roo.Next(1000, 9999);
            return strR;
        
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;


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


        //将txt文件加密、改变后缀、隐藏文件及其父文件夹/
        public static void EncryptionFile(string filePath, string mixedExtension, string encryptKey)
        {
            string sourcefileName = filePath;

            if (!File.Exists(sourcefileName)) return;
            
            //FileEncrypt.EncryptFile(sourcefileName, encryptKey, (int max, int value) => { });
            //string destfileName = Path.ChangeExtension(sourcefileName, mixedExtension);
            //File.Move(sourcefileName, destfileName);
            //File.SetAttributes(destfileName, FileAttributes.Hidden | FileAttributes.System);

            string folderPath = new FileInfo(sourcefileName).DirectoryName;
            File.SetAttributes(folderPath, FileAttributes.System);//设置添加系统文件夹/
            File.SetAttributes(folderPath, FileAttributes.Hidden);//设置添加隐藏文件夹/
        }

        //找到隐藏文件，改回后缀，解密，恢复为隐藏的已解密的txt文件/
        public static FileStream DecryptionFile(string filePath, string encryptKey)
        {
            FileStream fs = null;
            string sourcefileName = filePath;

            if (!File.Exists(sourcefileName))  return fs;
           
            //string destfileName = Path.ChangeExtension(sourcefileName, ".txt");
            //File.Move(sourcefileName, destfileName);
            //File.SetAttributes(destfileName, FileAttributes.Normal);
            //FileEncrypt.DecryptFile(destfileName, encryptKey, (int max, int value) => { });
            //fs = new FileStream(destfileName,FileMode.Open);
            fs = new FileStream(sourcefileName, FileMode.Open);
            return fs;
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

/*
 * 作者：宋玉
 * 作用：数据类型转换工具类，文件、字节、流的工具转换类
 */ 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Utility
{
    public class FileByteStreamConvert
    {
        /// <summary>
        /// 将stream 转换成byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] streamToBytes(Stream stream)
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

        /// <summary>
        /// 将Stream写入文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        public static void StreamToFile(Stream stream, string fileName)
        {
            byte[] bytes = streamToBytes(stream);

            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }

        /// <summary>
        /// 将指定路径的文件转化为Stream
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Stream FileToStream(string fileName)
        {
            //打开文件
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            //读取文件的byte[]
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            //把byte[]转换成Stream
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        /// <summary>
        /// 判断配置文件是否存在
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsExists(string file)
        {
            string fileDic = file.Substring(0, file.LastIndexOf("/") + 1);
            if (!Directory.Exists(fileDic))
            {
                Directory.CreateDirectory(fileDic);
                return false;
            }
            return File.Exists(file);
        }
    }
}

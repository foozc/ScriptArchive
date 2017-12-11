﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;

namespace Utility
{
    public class FileUtils
    {
        /// <summary>
        /// 将字节数组bytes保存到指定路径文件
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="FilePath"></param>
        public static void saveFileFromStream(byte[] bytes, string filePath)
        {
            string folder = filePath.Substring(0, filePath.LastIndexOf("/") + 1);
            CreateNewFolder(folder);
            Stream stream = FileByteStreamConvert.BytesToStream(bytes);
            FileByteStreamConvert.StreamToFile(stream, filePath);
        }
        /// <summary>
        /// 判断文件夹是否存在 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool checkFolderExits(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.Exists)//文件夹存在 
            {
                return true;
            }
            else
            {
                //dir.Create();//不存在就创建一个 
                return false;
            }
        }
        /// <summary>
        /// 创建一个文件夹，存在就创建失败
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool CreateNewFolder(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            if (!dir.Exists)
            {
                dir.Create();
                return true;
            }
            else
                return false;
        }
        /// <summary> 
        /// 在指定目录下创建指定名称文件夹 
        /// </summary> 
        /// <param name="ParentsPath"></param> 
        /// <param name="NewFolderName"></param> 
        /// <returns></returns> 
        public static bool CreateNewFolder(string ParentsPath, string NewFolderName)
        {
            string CreatePath = ParentsPath + @"\" + NewFolderName;
            DirectoryInfo dir = new DirectoryInfo(CreatePath);

            if (!dir.Exists)
            {
                dir.Create();
                return true;
            }
            else
                return false;
        }

        /// <summary> 
        /// 把文件夹得大小转换成比较合适的表示单位 
        /// </summary> 
        /// <param name="size"></param> 
        /// <returns></returns> 
        public static string ViewSize(long size)
        {
            long m = size;
            string viewstr;

            if ((m / 1024) > 0)//表示可以转换成KB 
            {
                m = m / 1024;//转换成KB 

                if ((m / 1024) > 0)//表示可以转换成MB 
                {
                    m = m / 1024;//转换成MB了 

                    if ((m / 1024) > 0)//表示可以转换成GB 
                    {
                        m = m / 1024;//转换成GB了 
                        viewstr = m.ToString() + "GB";
                    }
                    else
                    {
                        viewstr = m.ToString() + "MB";
                    }
                }
                else
                {
                    viewstr = m.ToString() + "KB";
                }
            }
            else
            {
                viewstr = m.ToString() + "byte";
            }
            return viewstr;
        }

        public static T loadObjectFromJsonFile<T>(string path)
        {
            TextReader reader = new StreamReader(path, Encoding.UTF8);
            if (reader == null)
            {
                Debug.LogError("找不到配置文件： " + path);
                reader.Close();
                return default(T);
            }
            string aa = reader.ReadToEnd();
            T data = JsonMapper.ToObject<T>(aa);
            if (data == null)
            {
                Debug.Log("当前没有数据： " + path);
            }
            reader.Close();
            return data;
        }

        public static void saveObjectToJsonFile<T>(T data, string path)
        {
            TextWriter tw = new StreamWriter(path, false, Encoding.UTF8);
            if (tw == null)
            {
                Debug.LogError("无法写文件： " + path);
                return;
            }
            string jsonStr = JsonFormatter.PrettyPrint(JsonMapper.ToJson(data));
            tw.Write(jsonStr);

            tw.Flush();
            tw.Close();
        }

#if UNITY_STANDALONE_WIN
        /// <summary> 
        /// 返回目录下的所有文件名 
        /// </summary> 
        /// <param name="path"></param> 
        /// <returns></returns> 
        public static List<string> getAllFiles(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.Exists)
            {
                FileInfo[] fileinfo = dir.GetFiles();
                List<string> list = new List<string>();
                foreach (FileInfo f in fileinfo)
                {
                    list.Add(f.Name);
                }
                return list;
            }
            else
                return null;
        }

        /// <summary>
        /// 获取所有文件列表
        /// </summary>
        /// <param name="info"></param>
        public static void getAllFiles(ref List<string> fileinfo_list, string search_path)
        {
            if (search_path == null || search_path == "")
                return;

            try
            {
                DirectoryInfo dir = new DirectoryInfo(search_path);
                //不是目录
                if (dir == null) return;

                FileSystemInfo[] files = dir.GetFileSystemInfos();
                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo file = files[i] as FileInfo;
                    //是文件
                    if (file != null)
                    {
                        fileinfo_list.Add(file.FullName);

                    }
                    //对于子目录
                    else
                    {
                        dir = files[i] as DirectoryInfo;
                        if (dir != null)
                        {
                            getAllFiles(ref fileinfo_list, dir.FullName);
                        }
                    }
                }
            }
            catch (System.Exception err)
            {
                Debug.LogError("GetAllFiles failed: " + err.Message);
            }
        }

        /// <summary> 
        /// 计算文件夹的大小 
        /// </summary> 
        /// <param name="d"></param> 
        /// <returns></returns> 
        public static long DirSize(DirectoryInfo d)
        {
            long Size = 0;
            // Add file sizes. 
            FileInfo[] fis = d.GetFiles();//获得目录文件列表 
            foreach (FileInfo fi in fis)
            {
                Size += fi.Length;
            }
            // Add subdirectory sizes. 
            DirectoryInfo[] dis = d.GetDirectories();//获取目录子目录列表 
            foreach (DirectoryInfo di in dis)
            {
                Size += DirSize(di);
            }
            return Size;
        }

        /// <summary> 
        /// 删除指定目录和内容 
        /// </summary> 
        /// <param name="dir"></param> 
        /// <returns></returns> 
        public static bool delDir(string dir)
        {
            bool flag = false;
            DirectoryInfo d = new DirectoryInfo(dir);
            if (d.Exists)//判断目录是否存在 
            {
                try
                {
                    d.Delete();
                    flag = true;
                }
                catch (Exception e) { flag = false; }
            }
            return flag;
        }
        /// <summary> 
        /// 删除指定文件 
        /// </summary> 
        /// <param name="fil"></param> 
        /// <returns></returns> 
        public static bool delFile(string fil)
        {
            bool flag = false;
            FileInfo d = new FileInfo(fil);
            if (d.Exists)//判断目录是否存在 
            {
                try
                {
                    d.Delete();
                    flag = true;
                }
                catch (Exception e) { flag = false; }
            }
            return flag;
        }

        public static void CopyFile(string source, string target)
        {
            FileInfo file = new FileInfo(source);
            if (file.Exists)
            {
                file.CopyTo(target, true);
            }
        }
        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }
        /// <summary> 
        /// 复制目录及子文件到指定目录 
        /// </summary> 
        /// <param name="source"></param> 
        /// <param name="target"></param> 
        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Check if the target directory exists, if not, create it. 
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it's new directory. 
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            // Copy each subdirectory using recursion. 
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        /// <summary>
        /// 递归删除非空文件夹及文件
        /// </summary>
        /// <param name="dir"></param>
        public static void DeleteDirectory(string dir)
        {
            if (Directory.GetDirectories(dir).Length == 0 && Directory.GetFiles(dir).Length == 0)
            {
                Directory.Delete(dir);
                return;
            }
            foreach (string var in Directory.GetDirectories(dir))
            {
                DeleteDirectory(var);
            }
            foreach (string var in Directory.GetFiles(dir))
            {
                File.Delete(var);
            }
            Directory.Delete(dir);
        }

#endif
    }
}

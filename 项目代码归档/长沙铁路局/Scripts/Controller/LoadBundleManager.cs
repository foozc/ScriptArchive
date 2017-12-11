/*
 * ���ߣ�����
 * ���ã���������ļ��汾�������ļ�
 */
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.IO;
using LitJson;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明：捆绑加载管理类
*Author:作者
*
*/
namespace Utility
{
    /// <summary>
    /// ��������ļ��汾�������ļ�
    /// </summary>
    public class LoadBundleManager
	{
#if UNITY_STANDALONE_WIN
		private static LoadBundleManager Instace = null;
        /*BundleStates�����ļ�������
         */ 
        //private TextReader bundleStatesFileReader = null;
        //private TextWriter bundleStatesFileWriter = null;
        ///*BundleData�����ļ�������
        // */ 
        //private TextReader bundleDataFileReader = null;
        //private TextWriter bundleDataFileWriter = null;
        /*���������ļ�·��
         */ 
        private string bundleStatesLoadFilePath = DownloadManager.Instance.getDownloadRootUrl(true) + "/BuildStates.txt";
        private string bundleDataLoadFilePath = DownloadManager.Instance.getDownloadRootUrl(true) + "/BundleData.txt";
        /*���������ļ���Ӧ�ڴ�ӳ���б�
         */ 
        private List<BundleBuildState> buildStates = null;
        private List<BundleData> bundleDatas = null;
        private Dictionary<string, BundleData> bundleDataDict = new Dictionary<string, BundleData>();
        private Dictionary<string, BundleBuildState> buildStatesDict = new Dictionary<string, BundleBuildState>();

        public enum BundleOperate
        {
            Delete,
            Update,
            Add,
            Select
        }
        static object obj = new object();
        public static LoadBundleManager getInstace()
        {
            //lock (obj)
            //{
                if (Instace == null)
                    Instace = new LoadBundleManager();
                return Instace;
            //}
        }
        private LoadBundleManager()
        {
            infoData();
        }

        private void infoData()
        {
            //��ȡ����BundleStates�����ļ���list�б�
            string dicPath = bundleStatesLoadFilePath.Substring(0, bundleStatesLoadFilePath.LastIndexOf("/"));
            if (!FileUtils.checkFolderExits(dicPath))
                FileUtils.CreateNewFolder(dicPath);
            FileStream bundleStatesfileStream = new FileStream(bundleStatesLoadFilePath, FileMode.OpenOrCreate);
            bundleStatesfileStream.Close();
            //if (bundleStatesFileReader != null)
            //    bundleStatesFileReader.Close();
            //bundleStatesFileReader = new StreamReader(bundleStatesfileStream);
            //if (bundleStatesFileWriter != null)
            //    bundleStatesFileWriter.Close();
            //bundleStatesFileWriter = new StreamWriter(bundleStatesfileStream, Encoding.UTF8);
            loadBundleBuildStateFromJsonFile();
            //bundleStatesfileStream.Close();
            //��ȡ����BundleData�����ļ���list�б�
            dicPath = bundleDataLoadFilePath.Substring(0, bundleDataLoadFilePath.LastIndexOf("/"));
            if (!FileUtils.checkFolderExits(dicPath))
                FileUtils.CreateNewFolder(dicPath);
            FileStream bundleDataFileStream = new FileStream(bundleDataLoadFilePath, FileMode.OpenOrCreate);
            bundleDataFileStream.Close();
            //if (bundleDataFileReader != null)
            //    bundleDataFileReader.Close();
            //bundleDataFileReader = new StreamReader(bundleDataFileStream);
            //if (bundleDataFileWriter != null)
            //    bundleDataFileWriter.Close();
            //bundleDataFileWriter = new StreamWriter(bundleDataFileStream, Encoding.UTF8);
            loadBundleDataFromJsonFile();
            //bundleDataFileStream.Close();
        }

        public List<BundleData> getLoadBundleDataList()
        {
            return bundleDatas;
        }
        public Dictionary<string, BundleData> getLoadBundleDataDict()
        {
            return bundleDataDict;
        }
        public List<BundleBuildState> getLoadBundleStatesList()
        {
            return buildStates;
        }
        public Dictionary<string, BundleBuildState> getLoadBundleStatesDict()
        {
            return buildStatesDict;
        }

        /// <summary>
        /// �Ա���BundleData��Դ�ļ�����ɾ�Ĳ���
        /// </summary>
        /// <param name="newBundleData"></param>
        /// <param name="operate"></param>
        public void changeBundleData(List<BundleData> newBundleData, BundleOperate operate)
        {
            switch (operate)
            {
                case BundleOperate.Add:
                    foreach (BundleData bundleData in newBundleData)
                    {
                        int index = bundleDatas.FindIndex(x => x.name == bundleData.name);
                        if (index != -1)
                            bundleDatas[index] = bundleData;
                        else bundleDatas.Add(bundleData);
                        if (bundleDataDict.ContainsKey(bundleData.name))
                            bundleDataDict[bundleData.name] = bundleData;
                        else bundleDataDict.Add(bundleData.name, bundleData);
                    }
                    break;
                case BundleOperate.Delete:
                    foreach (BundleData bundleData in newBundleData)
                    {
                        if (bundleDatas.FindIndex(x => x.name == bundleData.name) != -1)
                            bundleDatas.Remove(bundleData);
                        if (bundleDataDict.ContainsKey(bundleData.name))
                            bundleDataDict.Remove(bundleData.name);
                    }
                    break;
                case BundleOperate.Update:
                    foreach (BundleData bundleData in newBundleData)
                    {
                        int index = bundleDatas.FindIndex(x => x.name == bundleData.name);
                        if (index != -1)
                            bundleDatas[index] = bundleData;
                        if (bundleDataDict.ContainsKey(bundleData.name))
                            bundleDataDict[bundleData.name] = bundleData;
                    }
                    break;
            }
            if(newBundleData.Count != 0)
                FileUtils.saveObjectToJsonFile(bundleDatas, bundleDataLoadFilePath);
        }

        /// <summary>
        /// �Ա���BundleBuildState��Դ�ļ�����ɾ�Ĳ���
        /// </summary>
        /// <param name="newBundleBuildState"></param>
        /// <param name="operate"></param>
        public void changeBundleStates(List<BundleBuildState> newBundleBuildStates, BundleOperate operate)
        {
            switch (operate)
            {
                case BundleOperate.Add:
                    foreach (BundleBuildState buildState in newBundleBuildStates)
                    {
                        int index = buildStates.FindIndex(x => x.bundleName == buildState.bundleName);
                        if (index != -1)
                            buildStates[index] = buildState;
                        else buildStates.Add(buildState);
                        if (buildStatesDict.ContainsKey(buildState.bundleName))
                            buildStatesDict[buildState.bundleName] = buildState;
                        else buildStatesDict.Add(buildState.bundleName, buildState);
                    }
                    break;
                case BundleOperate.Delete:
                    foreach (BundleBuildState buildState in newBundleBuildStates)
                    {
                        if (buildStates.FindIndex(x => x.bundleName == buildState.bundleName) != -1)
                            buildStates.Remove(buildState);
                        if (buildStatesDict.ContainsKey(buildState.bundleName))
                            buildStatesDict.Remove(buildState.bundleName);
                    }
                    break;
                case BundleOperate.Update:
                    foreach (BundleBuildState buildState in newBundleBuildStates)
                    {
                        int index = buildStates.FindIndex(x => x.bundleName == buildState.bundleName);
                        if (index != -1)
                            buildStates[index] = buildState;
                        if (buildStatesDict.ContainsKey(buildState.bundleName))
                            buildStatesDict[buildState.bundleName] = buildState;
                    }
                    break;
            }
            if(newBundleBuildStates.Count != 0)
                FileUtils.saveObjectToJsonFile(buildStates, bundleStatesLoadFilePath);
        }

        public bool isExistsForConfiger(string bundleName, int version, uint crc)
        {
            if (!bundleDataDict.ContainsKey(bundleName))
                return false;
            BundleBuildState bundleState = buildStatesDict[bundleName];
            if (bundleState.crc == crc && bundleState.version == version)
                return true;
            return false;
        }
        /// <summary>
        /// ����ָ��bundleName��BundleDataʵ��
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        public BundleData getBundleDataList(string bundleName)
        {
            int index = bundleDatas.FindIndex(x => x.name == bundleName);
            if (index != -1)
                return bundleDatas[index];
            return null;
        }
        /// <summary>
        /// ����ָ��bundleName��BundleDataʵ��
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        public BundleData getBundleDataDict(string bundleName)
        {
            if (bundleDataDict.ContainsKey(bundleName))
            {
                return bundleDataDict[bundleName];
            }
            return null;
        }

        private void loadBundleDataFromJsonFile()
        {
            bundleDatas = FileUtils.loadObjectFromJsonFile<List<BundleData>>(bundleDataLoadFilePath);
            if (bundleDatas == null)
                bundleDatas = new List<BundleData>();
            foreach (var bundleData in bundleDatas)
                bundleDataDict.Add(bundleData.name, bundleData);
        }


        /// <summary>
        /// ����ָ��bundleName��BundleBuildStateʵ��
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        public BundleBuildState getBundleBuildStateDict(string bundleName)
        {
            if (buildStatesDict.ContainsKey(bundleName))
            {
                return buildStatesDict[bundleName];
            }
            return null;
        }

        /// <summary>
        /// ����ָ��bundleName��BundleBuildStateʵ��
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        public BundleBuildState getBundleBuildStateList(string bundleName)
        {
            int index = buildStates.FindIndex(x => x.bundleName == bundleName);
            if (index != -1)
                return buildStates[index];
            return null;
        }

        private void loadBundleBuildStateFromJsonFile()
        {
            buildStates = FileUtils.loadObjectFromJsonFile<List<BundleBuildState>>(bundleStatesLoadFilePath);
            if(buildStates == null)
                buildStates = new List<BundleBuildState>();
            foreach (var buildState in buildStates)
                buildStatesDict.Add(buildState.bundleName, buildState);
        }

        /// <summary>
        /// �Ա����ص��ļ���CRC������������ļ���CRC���Ƿ�һ��
        /// </summary>
        /// <param name="FilePath">�Ա��ļ�·��</param>
        /// <returns></returns>
        public bool verifyFileExists(string filePath, uint crc)
        {
            if (!FileByteStreamConvert.IsExists(filePath))
            {
                return false;
            }
            uint crcNew = Crc32.GetFileCRC32(filePath);
            if (crcNew == crc)
                return true;
            return false;
        }

        /// <summary>
        /// �Ա����ص��ļ���CRC������������ļ���CRC���Ƿ�һ��
        /// </summary>
        /// <param name="FilePath">��Ҫ�Ա��ļ�����</param>
        /// <returns></returns>
        public bool verifyFileExists(Stream stream, uint crc)
        {
            uint crcNew = Crc32.GetStreamCRC32(stream);
            if (crcNew == crc)
                return true;
            return false;
        }

        

        /// <summary>
        /// ת����Ե�ַΪ���Ե�ַ
        /// </summary>
        /// <param name="urlstr"></param>
        /// <returns></returns>
        string formatUrl(string urlstr)
        {
            Uri url;
            if (!isAbsoluteUrl(urlstr))
            {
                url = new Uri(new Uri(DownloadManager.Instance.getDownloadRootUrl(false) + '/'), urlstr);
            }
            else
                url = new Uri(urlstr);
            return url.AbsoluteUri;
        }
        bool isAbsoluteUrl(string url)
        {
            Uri result;
            return Uri.TryCreate(url, System.UriKind.Absolute, out result);
        }
#endif
    }
}

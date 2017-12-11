using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Uri = System.Uri;
using LitJson;
using Utility;

/**
 * DownloadManager is a runtime class for asset steaming and WWW management.
 */ 
public class DownloadManager : MonoBehaviour 
{

    public List<string> CurrentDownloadAllBundle = new List<string>();
    private bool isProgressBackcallRunning = false;

    IEnumerator progressBackcall()
    {
        isProgressBackcallRunning = true;
        bool sceneBundleSucceed = false;
        do
        {
            sceneBundleSucceed = true;
            foreach (string sceneBundleName in CurrentDownloadAllBundle)
            {
                WWW www = DownloadManager.Instance.GetWWW(sceneBundleName);
                if (www != null)
                {
                    if (!www.isDone)
                        sceneBundleSucceed = false;
                }
                else sceneBundleSucceed = false;
            }

            float progressOfScenes = DownloadManager.Instance.ProgressOfBundles(CurrentDownloadAllBundle.ToArray());

            if (progressManager != null && progressManager.setAllProgressEvent != null)
                progressManager.setAllProgressEvent(progressOfScenes);
            if (!sceneBundleSucceed)
                yield return null;
        } while (!sceneBundleSucceed);
        if(sceneBundleSucceed)
            CurrentDownloadAllBundle.Clear();
        isProgressBackcallRunning = false;
    }

    /// <summary>
    /// ��ȡ��ǰ���̵���Դ��������Ŀ¼
    /// </summary>
    /// <returns></returns>
    public string getDownloadRootUrl(bool isLoad)
    {
        if (isLoad)
            return downloadLoadRootUrl;
        else
            return downloadRootUrl;
    }
	/// <summary>
	/// Get the error string of WWW request.
    /// ��ȡ�����WWW����
	/// @return The error string of WWW. Return null if WWW request succeed or still in processing.
    /// WWW�Ĵ����ַ���������null���WWW����ɹ����߻��ڴ����
	/// </summary>
	/// <param name="url"></param>
    /// <returns>WWW�Ĵ����ַ���������null���WWW����ɹ����߻��ڴ����</returns>
	public string GetError(string url)
	{
        url = bundlePacketResouse(url);
		if(!ConfigLoaded)
			return null;
		
		url = formatUrl(url, false);
        if (failedRequest.ContainsKey(url))
            return failedRequest[url].www.error;
        else
        {
            string url1 = formatUrl(url, true);
            if (failedRequest.ContainsKey(url1))
                return failedRequest[url1].www.error;
            else
                return null;
        }
	}
	
	/// <summary>
    /// Test if the url is already requested.
	/// ��url�Ƿ����Ѿ������
	/// </summary>
	/// <param name="url"></param>
	/// <returns></returns>
	public bool IsUrlRequested(string url)
	{
        url = bundlePacketResouse(url);
		if(!ConfigLoaded)
		{
			return isInBeforeInitList(url);
		}
		else
		{
			url = formatUrl(url, false);
            string url1 = formatUrl(url, true);
			bool isRequested = isInWaitingList(url) || processingRequest.ContainsKey(url) 
                || succeedRequest.ContainsKey(url) || failedRequest.ContainsKey(url)
                || isInWaitingList(url1) || processingRequest.ContainsKey(url1)
                || succeedRequest.ContainsKey(url1) || failedRequest.ContainsKey(url1);
			return isRequested;
		}
	}
	
    /// <summary>
    /// ��ȡ��Ӧurl��WWWʵ��
	/// @return ����null˵��������û�гɹ�
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
	public WWW GetWWW(string url)
    {
        url = bundlePacketResouse(url);
		if(!ConfigLoaded)
			return null;
		
		url = formatUrl(url, false);

        if (succeedRequest.ContainsKey(url))
        {
            WWWRequest request = succeedRequest[url];
            prepareDependBundles(stripBundleSuffix(request.requestString));
            return request.www;
        }
        else
        {
            string url1 = formatUrl(url, true);
            if (succeedRequest.ContainsKey(url1))
            {
                WWWRequest request = succeedRequest[url1];
                prepareDependBundles(stripBundleSuffix(request.requestString));
                return request.www;
            }
            else
                return null;
        }
	}

    /// <summary>
    /// Coroutine for download waiting. 
    /// Эͬ����ȴ�����
    /// You should use it like this,
    /// ʹ�÷���    yield return StartCoroutine(DownloadManager.Instance.WaitDownload("bundle1.assetbundle"));
    /// If this url didn't been requested, the coroutine will start a new request.
    /// �����url��Ҫ��Эͬ�������һ���µ�����
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
	public IEnumerator WaitDownload(string url)
    {
        url = bundlePacketResouse(url);
		yield return StartCoroutine( WaitDownload(url, -1) );
	}


    /// <summary>
    /// Coroutine for download waiting. 
    /// Эͬ����ȴ�����
    /// You should use it like this,
    /// ʹ�÷���    yield return StartCoroutine(DownloadManager.Instance.WaitDownload("bundle1.assetbundle"));
    /// If this url didn't been requested, the coroutine will start a new request.
    /// �����url��Ҫ��Эͬ�������һ���µ�����
    /// </summary>
    /// <param name="url"></param>
    /// <param name="priority"></param>
    /// <returns></returns>
	public IEnumerator WaitDownload(string url, int priority)
    {
        url = bundlePacketResouse(url);
        string newUrl = stripBundleSuffix(url) + ".assetBundle";

        CurrentDownloadAllBundle.Add(newUrl);
        if (!isProgressBackcallRunning)
        {
            StartCoroutine(progressBackcall());
            if (progressManager != null && progressManager.setAllProgressEvent != null)
                progressManager.setAllProgressEvent(0);
        }

		while(!ConfigLoaded)
			yield return null;
		
		WWWRequest request = new WWWRequest();
		request.requestString = url;
		request.url = formatUrl(url, false);
		request.priority = priority;
		download(request);
		
		while(isDownloadingWWW(request.url))
			yield return null;
	}
    /// <summary>
    /// Start a new download request.
    /// ��ʼһ���µ���������
    /// </summary>
    /// <param name="url">���ص�url������ʹһ�����Ի������url</param>
	public void StartDownload(string url)
    {
        url = bundlePacketResouse(url);
        StartDownload(url, -1);
	}

    
    /// <summary>
    /// Start a new download request.
    /// ��ʼһ���µ���������
	/// </summary>
    /// <param name="url">���ص�url������ʹһ�����Ի������url</param>
	/// <param name="priority">�������ȼ�</param>
	public void StartDownload(string url, int priority)
    {
        url = bundlePacketResouse(url);
        string newUrl = stripBundleSuffix(url) + ".assetBundle";
        CurrentDownloadAllBundle.Add(newUrl);
        if (!isProgressBackcallRunning)
        {
            StartCoroutine(progressBackcall());
            if (progressManager != null && progressManager.setAllProgressEvent != null)
                progressManager.setAllProgressEvent(0);
        }

		WWWRequest request = new WWWRequest();
		request.requestString = url;
		request.url = url;
		request.priority = priority;

		if(!ConfigLoaded)
		{
			if(!isInBeforeInitList(url))
				requestedBeforeInit.Add(request);
		}
		else
			download(request);
	}
	
	/// <summary>
    /// Stop a request.
	/// ��ͣ��ӦUrl������
	/// </summary>
	/// <param name="url"></param>
	public void StopDownload(string url)
    {
        url = bundlePacketResouse(url);
        string url1 = formatUrl(url, true);
		if(!ConfigLoaded)
		{
			requestedBeforeInit.RemoveAll(x => x.url == url);
            requestedBeforeInit.RemoveAll(x => x.url == url1);
		}
		else
		{
			url = formatUrl(url, false);
			
			waitingRequests.RemoveAll(x => x.url == url);
            waitingRequests.RemoveAll(x => x.url == url1);
			if(processingRequest.ContainsKey(url))
			{
				processingRequest[url].www.Dispose();
				processingRequest.Remove(url);
            }
            else if (processingRequest.ContainsKey(url1))
            {
                processingRequest[url1].www.Dispose();
                processingRequest.Remove(url1);
            }
		}
	}

    /// <summary>
    /// dispose a finished www request
    /// �������ڴ��е�WWW������ڴ���ж�أ�Ĭ�ϲ�����������ϵ
    /// </summary>
    /// <param name="url"></param>
    public void DisposeWWW(string url)
    {
        url = bundlePacketResouse(url);
        DisposeWWW(url, true);
    }
	
    /// <summary>
    /// �������ڴ��е�WWW������ڴ���ж��,ָ���Ƿ�����������ϵ
    /// </summary>
    /// <param name="url"></param>
    /// <param name="unloadAllLoadedObjects">�Ƿ�����������ϵ</param>
    public void DisposeWWW(string url, bool unloadAllLoadedObjects)
    {
        url = bundlePacketResouse(url);
		url = formatUrl(url, false);
        
		StopDownload(url);

        disposeDependListWWW(Path.GetFileNameWithoutExtension(url));

        
		if(succeedRequest.ContainsKey(url))
		{
            if (succeedRequest[url].dependCount == 0)
            {
                succeedRequest[url].www.Dispose();
                succeedRequest.Remove(url);
            }
            else
                succeedRequest[url].dependCount--;
        }
        else
        {
            string url1 = formatUrl(url, true);
            if (succeedRequest.ContainsKey(url1))
            {
                if (succeedRequest[url1].dependCount == 0)
                {
                    succeedRequest[url1].www.Dispose();
                    succeedRequest.Remove(url1);
                }
                else succeedRequest[url1].dependCount--;
            }
        }
		
		if(failedRequest.ContainsKey(url))
		{
            if (failedRequest[url].dependCount == 0)
            {
                failedRequest[url].www.Dispose();
                failedRequest.Remove(url);
            }
            else failedRequest[url].dependCount--;
        }
        else
        {
            string url1 = formatUrl(url, true);
            if (failedRequest.ContainsKey(url1))
            {
                if (failedRequest[url1].dependCount == 0)
                {
                    failedRequest[url1].www.Dispose();
                    failedRequest.Remove(url1);
                }
                else failedRequest[url1].dependCount--;
            }
        }
	}

    private void disposeDependListWWW(string bundle)
    {
        if (!ConfigLoaded)
        {
            Debug.LogError("getDependList() should be call after download manger inited");
            return;
        }


        if (!bundleDict.ContainsKey(bundle))
        {
            return;
        }

        if (bundleDict[bundle].parent != "")
        {
            bundle = bundleDict[bundle].parent;
            if (bundleDict.ContainsKey(bundle))
            {
                DisposeWWW(bundle, true);
            }
            else
            {
                Debug.LogWarning("Cannot find parent bundle [" + bundle + "], Please check your bundle config.");
            }
        }
    }

	
	/**
	 * This function will stop all request in processing.
	 */
	public void StopAll()
	{
		requestedBeforeInit.Clear();
		waitingRequests.Clear();
		
		foreach(WWWRequest request in processingRequest.Values)
			request.www.Dispose();
		
		processingRequest.Clear();
	}

    /// <summary>
    /// ��ȡ�������ذ������ؽ���.
    /// �������ֻ������ built bundles.
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns></returns>
    private float ProgressOfBundle(string bundleName)
    {
        string filename = Path.GetFileNameWithoutExtension(bundleName);
        long bundleSize = buildStatesDict[filename].size;
        if (bundleName.Split('.').Length != 2)
            bundleName = bundleName + "." + bmConfiger.bundleSuffix;
        if (!buildStatesDict.ContainsKey(filename))
        {
            Debug.LogWarning("Cannot get progress of [" + bundleName + "]. It's not such bundle in bundle build states list.");
            return 0f;
        }

        long currentSize = 0;

        string url = formatUrl(bundleName, false);
        string url1 = formatUrl(bundleName, true);

        WWW www = null;
        if (processingRequest.ContainsKey(url))
            www = processingRequest[url].www;
        else if (processingRequest.ContainsKey(url1))
            www = processingRequest[url1].www;
        currentSize += (long)(www.progress * bundleSize);
        if (succeedRequest.ContainsKey(url))
            currentSize += bundleSize;
        else if (succeedRequest.ContainsKey(url1))
            currentSize += bundleSize;
        float prog = ((float)currentSize) / bundleSize;
        if(prog == 1)
            if (www.isDone)
                return 1;
            else return 0.99f;
        else
            return prog;
    }

    /// <summary>
    /// ��ȡָ���������ؽ���.
    /// ���������İ����ᱻͳ��.
    /// �������ֻ������ built bundles.
    /// </summary>
    /// <param name="bundlefile"></param>
    /// <returns></returns>
    public float ProgressOfBundles(string bundlefile)
    {
        string[] strs = { bundlefile };
        return ProgressOfBundles(strs);
    }

    /// <summary>
    /// ��ȡָ���������ؽ���.
	/// ���������İ����ᱻͳ��.
	/// �������ֻ������ built bundles.
    /// </summary>
    /// <param name="bundlefiles"></param>
    /// <returns></returns>
	public float ProgressOfBundles(string[] bundlefiles)
	{
		if(!ConfigLoaded)
			return 0f;
        for (int i = 0; i < bundlefiles.Length; i++)
        {
            bundlefiles[i] = bundlePacketResouse(bundlefiles[i]);
        }
		List<string> bundles = new List<string>();
		foreach(string bundlefile in bundlefiles)
		{
            string bundlename = Path.GetFileName(bundlefile);
            if (bundlename.Split('.').Length == 2)
            {
                if (!bundlefile.EndsWith("." + bmConfiger.bundleSuffix, System.StringComparison.OrdinalIgnoreCase))
                {
                    Debug.LogWarning("ProgressOfBundles only accept bundle files. " + bundlefile + " is not a bundle file.");
                    continue;
                }
            }
            else
            {
                if (!bundleDict.ContainsKey(bundlename))
                {
                    Debug.LogWarning("ProgressOfBundles only accept bundle files. " + bundlefile + " is not a bundle file.");
                    continue;
                }
            }
			
			bundles.Add(Path.GetFileNameWithoutExtension(bundlefile));
		}
		
		HashSet<string> allInludeBundles = new HashSet<string>();
		foreach(string bundle in bundles)
		{
			foreach(string depend in getDependList(bundle))
			{
				if(!allInludeBundles.Contains(depend))
					allInludeBundles.Add(depend);
			}
			
			if(!allInludeBundles.Contains(bundle))
				allInludeBundles.Add(bundle);
		}
		
		long currentSize = 0;
		long totalSize = 0;
		foreach(string bundleName in allInludeBundles)
		{
			if(!buildStatesDict.ContainsKey(bundleName))
			{
                Debug.LogWarning("Cannot get progress of [" + bundleName + "]. It's not such bundle in bundle build states list.");		
				continue;
			}
				
			long bundleSize = buildStatesDict[bundleName].size;
			totalSize += bundleSize;
			
			string url = formatUrl( bundleName + "." + bmConfiger.bundleSuffix, false);
            string url1 = formatUrl(bundleName + "." + bmConfiger.bundleSuffix, true);
			
            WWW www = null;
            if (processingRequest.ContainsKey(url))
                www = processingRequest[url].www;
            else if (processingRequest.ContainsKey(url1))
                www = processingRequest[url1].www;
            if(www != null)
                if (((www.progress * bundleSize) / bundleSize) == 1)
                    if (www.isDone)
                        currentSize += bundleSize;
                    else currentSize += (long)0.99 * bundleSize;
                else
                    currentSize += (long)www.progress * bundleSize;

			if(succeedRequest.ContainsKey(url))
				currentSize += bundleSize;
            else if (succeedRequest.ContainsKey(url1))
                currentSize += bundleSize;
		}
		
		if(totalSize == 0)
			return 0;
		else
			return ((float)currentSize)/totalSize;
	}

	
    /// <summary>
    /// ��������ļ��Ƿ��������
    /// </summary>
	public bool ConfigLoaded
	{
		get
		{
			return bundles != null && buildStates != null && bmConfiger != null;
		}
	}

    public bool isIncludeBundleData(string url)
    {
        url = bundlePacketResouse(url);
        if (bundleDict.ContainsKey(url))
            return true;
        else return false;
    }

	/// <summary>
    /// Get list of the built bundles. 
	/// ��ȡ���б�
    /// * Before use this, please make sure ConfigLoaded is true.
    /// ʹ��ǰ������ȷ��configloaded��true
	/// </summary>
	public BundleData[] BuiltBundles
	{
		get
		{
			if(bundles == null)
				return null;
			else
				return bundles.ToArray();
		}
	}

	/// <summary>
    /// Get list of the BuildStates. 
	/// �õ���BuildStates�б�
    /// * Before use this, please make sure ConfigLoaded is true.
    /// ʹ��ǰ������ȷ��configloaded��true
	/// </summary>
	public BundleBuildState[] BuildStates
	{
		get
		{
			if(buildStates == null)
				return null;
			else
				return buildStates.ToArray();
		}
	}

	// Privats
    IEnumerator Start() 
	{

		// Initial download urls
		initRootUrl();

		// Try to get Url redirect file
		WWW redirectWWW = new WWW(formatUrl("BMRedirect.txt", false));
		yield return redirectWWW;

		if(redirectWWW.error == null)
		{
			// Redirect download
			string downloadPathStr = BMUtility.InterpretPath(redirectWWW.text, curPlatform);
			Uri downloadUri = new Uri(downloadPathStr);
			downloadRootUrl = downloadUri.AbsoluteUri;
		}

		redirectWWW.Dispose();


		// Download the initial data bundle
		const string verNumKey = "BMDataVersion";
		string bmDataUrl = formatUrl("BM.data", false);
		int lastBMDataVersion = 0;
		if(PlayerPrefs.HasKey(verNumKey))
			lastBMDataVersion = PlayerPrefs.GetInt(verNumKey);
		
		// Download and cache new data version
		WWW initDataWWW;

		if(bmUrl.offlineCache)
		{
			initDataWWW = WWW.LoadFromCacheOrDownload(bmDataUrl, lastBMDataVersion + 1);

			yield return initDataWWW;
			if(initDataWWW.error != null)
			{
				initDataWWW.Dispose();
				Debug.Log("Cannot load BMData from target url. Try load it from cache.");

				initDataWWW = WWW.LoadFromCacheOrDownload(bmDataUrl, lastBMDataVersion);
				yield return initDataWWW;

				if(initDataWWW.error != null)
				{
					Debug.LogError("Download BMData failed.\nError: " + initDataWWW.error);
					yield break;
				}
			}
			else
			{
				// Update cache version number
				PlayerPrefs.SetInt(verNumKey, lastBMDataVersion + 1);
			}
		}
		else
		{
			initDataWWW = new WWW(bmDataUrl);

			yield return initDataWWW;

			if(initDataWWW.error != null)
			{
				Debug.LogError("Download BMData failed.\nError: " + initDataWWW.error);
				yield break;
			}
		}

		// Init datas
		//
		// Bundle Data
#if UNITY_5
		TextAsset ta = initDataWWW.assetBundle.LoadAsset<TextAsset>("BundleData");
#else
		TextAsset ta = initDataWWW.assetBundle.Load("BundleData") as TextAsset;
#endif
		bundles = JsonMapper.ToObject< List< BundleData > >(ta.text);
		foreach(var bundle in bundles)
			bundleDict.Add(bundle.name, bundle);

		// Build States
#if UNITY_5
		ta = initDataWWW.assetBundle.LoadAsset<TextAsset>("BuildStates");
#else
		ta = initDataWWW.assetBundle.Load("BuildStates") as TextAsset;
#endif
		buildStates = JsonMapper.ToObject< List< BundleBuildState > >(ta.text);
		foreach(var buildState in buildStates)
			buildStatesDict.Add(buildState.bundleName, buildState);

		// BMConfiger
#if UNITY_5
		ta = initDataWWW.assetBundle.LoadAsset<TextAsset>("BMConfiger");
#else
		ta = initDataWWW.assetBundle.Load("BMConfiger") as TextAsset;
#endif
		bmConfiger = JsonMapper.ToObject< BMConfiger >(ta.text);

		initDataWWW.assetBundle.Unload(true);
		initDataWWW.Dispose();

		
		// Start download for requests before init
		foreach(WWWRequest request in requestedBeforeInit)
		{
			download(request);
		}
	}

	void Update () 
	{
		if(!ConfigLoaded)
			return;
		
		// Check if any WWW is finished or errored
        //����Ƿ����������WWW���󣬻��ߴ���
		List<string> newFinisheds = new List<string>();
		List<string> newFaileds = new List<string>();
		foreach(WWWRequest request in processingRequest.Values)
		{
            //�����������ļ��ص���Ϣ
            if(progressManager != null && progressManager.setCurrentDownloadFileEvent != null)
                progressManager.setCurrentDownloadFileEvent(request.url);
            if (progressManager != null && progressManager.setCurrentDownloadProgressEvent != null)
            {
                float progress = ProgressOfBundle(Path.GetFileName(request.url));
                progressManager.setCurrentDownloadProgressEvent(progress);
            }

			if(request.www.error != null)
			{
				if(request.triedTimes - 1 < bmConfiger.downloadRetryTime)
				{
					// Retry download
					request.CreatWWW();
				}
				else
				{
					newFaileds.Add( request.url );
					Debug.LogError("Download " + request.url + " failed for " + request.triedTimes + " times.\nError: " + request.www.error);
				}
			}
			else if(request.www.isDone)
			{
				newFinisheds.Add( request.url );
			}
		}

        List<BundleBuildState> saveBundleStates = new List<BundleBuildState>();
        List<BundleData> saveBundleData = new List<BundleData>();
        // �����سɹ��İ����б��processingRequest��ɾ�������뵽succeedRequest��
		foreach(string finishedUrl in newFinisheds)
		{
			succeedRequest.Add(finishedUrl, processingRequest[finishedUrl]);
			//var bundle = processingRequest[finishedUrl].www.assetBundle;
			processingRequest.Remove(finishedUrl);

#if UNITY_STANDALONE_WIN
            //�����سɹ������ݱ��浽����
            string bundleName = separateUrl(finishedUrl);
            if (bundleDict.ContainsKey(bundleName))
            {
                saveBundleData.Add(bundleDict[bundleName]);
                saveBundleStates.Add(buildStatesDict[bundleName]);
                string filePath = downloadLoadRootUrl + "/" + bundleDict[bundleName].bundleRelativePath + "/"
                    + bundleName + "." + bmConfiger.bundleSuffix;
                FileUtils.saveFileFromStream(succeedRequest[finishedUrl].www.bytes, filePath);
            }
#endif
		}
#if UNITY_STANDALONE_WIN
        LoadBundleManager.getInstace().changeBundleData(saveBundleData, LoadBundleManager.BundleOperate.Add);
        LoadBundleManager.getInstace().changeBundleStates(saveBundleStates, LoadBundleManager.BundleOperate.Add);
#endif

		// ������ʧ�ܵİ����б��processingRequest��ɾ�������뵽faileRequest��
		foreach(string finishedUrl in newFaileds)
		{
			if(!failedRequest.ContainsKey(finishedUrl))
				failedRequest.Add(finishedUrl, processingRequest[finishedUrl]);
			processingRequest.Remove(finishedUrl);
		}
		
		// ��ʼ�����°�
		int waitingIndex = 0;
		while( processingRequest.Count < bmConfiger.downloadThreadsCount && 
			   waitingIndex < waitingRequests.Count)
		{
			WWWRequest curRequest = waitingRequests[waitingIndex++];
			
			bool canStartDownload = curRequest.bundleData == null || isBundleDependenciesReady( curRequest.bundleData.name );
			if(canStartDownload)
			{
				waitingRequests.Remove(curRequest);
				curRequest.CreatWWW();
				processingRequest.Add(curRequest.url, curRequest);
			}
		}
	}
	
	bool isBundleDependenciesReady(string bundleName)
	{
		List<string> dependencies = getDependList(bundleName);
		foreach(string dependBundle in dependencies)
		{
			string url = formatUrl(dependBundle + "." + bmConfiger.bundleSuffix, true);
            string url1 = formatUrl(dependBundle + "." + bmConfiger.bundleSuffix, false);
			if(!succeedRequest.ContainsKey(url) && !succeedRequest.ContainsKey(url1))
				return false;
		}
		
		return true;
	}

	void prepareDependBundles(string bundleName)
	{
		List<string> dependencies = getDependList(bundleName);
		foreach(string dependBundle in dependencies)
		{
			string dependUrl = formatUrl(dependBundle + "." + bmConfiger.bundleSuffix, true);
            string dependUrl1 = formatUrl(dependBundle + "." + bmConfiger.bundleSuffix, false);
			if(succeedRequest.ContainsKey(dependUrl))
			{
				#pragma warning disable 0168
				var assetBundle = succeedRequest[dependUrl].www.assetBundle;
				#pragma warning restore 0168
			}
            else if (succeedRequest.ContainsKey(dependUrl1))
            {
                #pragma warning disable 0168
                var assetBundle = succeedRequest[dependUrl1].www.assetBundle;
                #pragma warning restore 0168
            }
		}
	}
	
	// This private method should be called after init
	void download(WWWRequest request)
	{
		request.url = formatUrl(request.url, false);
        string url = formatUrl(request.url, true);
		if(isDownloadingWWW(request.url) || succeedRequest.ContainsKey(request.url)
           || isDownloadingWWW(url) || succeedRequest.ContainsKey(url))
			return;
		
		if(isBundleUrl(request.url))
		{
			string bundleName = stripBundleSuffix(request.requestString);
			if(!bundleDict.ContainsKey(bundleName))
			{
				Debug.LogWarning("�������ذ�; [" + bundleName + "]. ���ļ����������ļ���.");

                CurrentDownloadAllBundle.Remove(bundleName + "." + bmConfiger.bundleSuffix);

				return;
			}
			
			List<string> dependlist = getDependList(bundleName);
			foreach(string bundle in dependlist)
			{
				string bundleRequestStr = bundle + "." + bmConfiger.bundleSuffix;
				string bundleUrl = formatUrl(bundleRequestStr, false);
                string bundleUrl1 = formatUrl(bundleRequestStr, true);

                bool flag = false;
                if (processingRequest.ContainsKey(bundleUrl))
                    processingRequest[bundleUrl].dependCount++;
                else if (succeedRequest.ContainsKey(bundleUrl))
                    succeedRequest[bundleUrl].dependCount++;
                else if (processingRequest.ContainsKey(bundleUrl1))
                    processingRequest[bundleUrl1].dependCount++;
                else if (succeedRequest.ContainsKey(bundleUrl1))
                    succeedRequest[bundleUrl1].dependCount++;
                else flag = true;

                //if(!processingRequest.ContainsKey(bundleUrl) && !succeedRequest.ContainsKey(bundleUrl)
                //    && !processingRequest.ContainsKey(bundleUrl1) && !succeedRequest.ContainsKey(bundleUrl1))
				if(flag)
                {
					WWWRequest dependRequest = new WWWRequest();
					dependRequest.bundleData = bundleDict[bundle];
					dependRequest.bundleBuildState = buildStatesDict[bundle];
					dependRequest.requestString = bundleRequestStr;
					dependRequest.url = bundleUrl;
					dependRequest.priority = dependRequest.bundleData.priority;
                    dependRequest.dependCount++;
					addRequestToWaitingList(dependRequest);
				}
			}
			
			request.bundleData = bundleDict[bundleName];
			request.bundleBuildState = buildStatesDict[bundleName];
			if(request.priority == -1)
				request.priority = request.bundleData.priority;  // User didn't change the default priority
			addRequestToWaitingList(request);
		}
		else
		{
			if(request.priority == -1)
				request.priority = 0; // User didn't give the priority
			addRequestToWaitingList(request);
		}
	}
	
	bool isInWaitingList(string url)
	{
		foreach(WWWRequest request in waitingRequests)
			if(request.url == url)
				return true;
		
		return false;
	}
	
	void addRequestToWaitingList(WWWRequest request)
	{
		if(succeedRequest.ContainsKey(request.url) || isInWaitingList(request.url))
			return;
		
		int insertPos = waitingRequests.FindIndex(x => x.priority < request.priority);

		insertPos = insertPos == -1 ? waitingRequests.Count : insertPos;
		waitingRequests.Insert(insertPos, request);
	}
	
	bool isDownloadingWWW(string url)
	{
		foreach(WWWRequest request in waitingRequests)
			if(request.url == url)
				return true;
		
		return processingRequest.ContainsKey(url);
	}
	
	bool isInBeforeInitList(string url)
	{
		foreach(WWWRequest request in requestedBeforeInit)
		{
			if(request.url == url)
				return true;
		}

		return false;
	}

	List<string> getDependList(string bundle)
	{
		if(!ConfigLoaded)
		{
			Debug.LogError("getDependList() should be call after download manger inited");
			return null;
		}
		
		List<string> res = new List<string>();
		
		if(!bundleDict.ContainsKey(bundle))
		{
			Debug.LogWarning("Cannot find parent bundle [" + bundle + "], Please check your bundle config.");
			return res;
		}
			
		while(bundleDict[bundle].parent != "")
		{
			bundle = bundleDict[bundle].parent;
			if(bundleDict.ContainsKey(bundle))
			{
				res.Add(bundle);
			}
			else
			{
                Debug.LogWarning("Cannot find parent bundle [" + bundle + "], Please check your bundle config.");
				break;
			}
		}
		
		res.Reverse();
		return res;
	}
	
	BuildPlatform getRuntimePlatform()
	{
		if(	Application.platform == RuntimePlatform.WindowsPlayer ||
			Application.platform == RuntimePlatform.OSXPlayer )
		{
			return BuildPlatform.Standalones;
		}
		else if(Application.platform == RuntimePlatform.OSXWebPlayer ||
				Application.platform == RuntimePlatform.WindowsWebPlayer)
		{
			return BuildPlatform.WebPlayer;
		}
		else if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return BuildPlatform.IOS;
		}
		else if(Application.platform == RuntimePlatform.Android)
		{
			return BuildPlatform.Android;
		}
		else
		{
			Debug.LogError("Platform " + Application.platform + " is not supported by BundleManager.");
			return BuildPlatform.Standalones;
		}
	}
	
	void initRootUrl()
	{
		TextAsset downloadUrlText = (TextAsset)Resources.Load("Urls");
		bmUrl = JsonMapper.ToObject<BMUrls>(downloadUrlText.text);

		if( Application.platform == RuntimePlatform.WindowsEditor ||
		   Application.platform == RuntimePlatform.OSXEditor )
		{
			curPlatform = bmUrl.bundleTarget;
		}
		else
		{
			curPlatform = getRuntimePlatform();
		}

		if(manualUrl == "")
		{
			string downloadPathStr;
			if(bmUrl.downloadFromOutput)
				downloadPathStr = bmUrl.GetInterpretedOutputPath(curPlatform);
			else
				downloadPathStr = bmUrl.GetInterpretedDownloadUrl(curPlatform);
				
			Uri downloadUri = new Uri(downloadPathStr);
			downloadRootUrl = downloadUri.AbsoluteUri;
		}
		else
		{
			string downloadPathStr = BMUtility.InterpretPath(manualUrl, curPlatform);
			Uri downloadUri = new Uri(downloadPathStr);
			downloadRootUrl = downloadUri.AbsoluteUri;
		}

        downloadLoadRootUrl = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("/")) + "/" + "assetBundle";

	}

    private string separateUrl(string urlstr)
    {
        urlstr = urlstr.Substring(urlstr.LastIndexOf("/") + 1, urlstr.Length - urlstr.LastIndexOf("/") - 1);
        string bundleName = urlstr.Split('.')[0];
        return bundleName;
    }

    /// <summary>
    /// ��ʽ��Url
    /// </summary>
    /// <param name="urlstr"></param>
    /// <returns></returns>
    string formatUrl(string urlstr, bool onlyServer)
	{
		Uri url;
		if(!isAbsoluteUrl(urlstr) || !Path.IsPathRooted(urlstr))
        {
            //urlstr = urlstr.Substring(urlstr.LastIndexOf("/") + 1, urlstr.Length - urlstr.LastIndexOf("/") - 1);
            urlstr = Path.GetFileName(urlstr);
            string[] bundleNmaeAndExtension = urlstr.Split('.');
            string bundleName = bundleNmaeAndExtension[0];
            if (bundleNmaeAndExtension.Length < 2)
                urlstr = urlstr + "." + bmConfiger.bundleSuffix;
            if (buildStatesDict.ContainsKey(bundleName))
            {
                BundleBuildState bundleState = buildStatesDict[bundleName];
                BundleData bd = bundleDict[bundleName];
#if UNITY_STANDALONE_WIN
                if (!onlyServer && LoadBundleManager.getInstace().isExistsForConfiger(bundleState.bundleName, bundleState.version, bundleState.crc))
                {
                    string rootpath = downloadLoadRootUrl + "/" + bd.bundleRelativePath;
                    url = new Uri(new Uri(rootpath + '/'), urlstr);
                    if (!File.Exists(url.AbsoluteUri))
                    {
                        rootpath = downloadRootUrl + "/" + bd.bundleRelativePath;
                        url = new Uri(new Uri(rootpath + '/'), urlstr);
                    }
                }
                else
                {
                    string rootpath = downloadRootUrl + "/" + bd.bundleRelativePath;
                    url = new Uri(new Uri(rootpath + '/'), urlstr);
                }
#else
                string rootpath = downloadRootUrl + "/" + bd.bundleRelativePath;
                url = new Uri(new Uri(rootpath + '/'), urlstr); 
#endif
            }
            else
            {
                url = new Uri(new Uri(downloadRootUrl + "/"), urlstr);
            }
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
	
	bool isBundleUrl(string url)
	{
		return string.Compare(Path.GetExtension(url), "." + bmConfiger.bundleSuffix, System.StringComparison.OrdinalIgnoreCase)  == 0;
	}

	string stripBundleSuffix(string requestString)
	{
        string[] bundleNameAndExtension = requestString.Split('.');
        if (bundleNameAndExtension.Length < 2)
            return requestString;
        else return requestString.Substring(0, requestString.Length - "assetBundle".Length - 1);
	}

    string bundlePacketResouse(string bundle)
    {
        string[] strs = bundle.Split('/');
        return strs[0];
    }
	
	// Members
	List<BundleData> bundles = null;
	List<BundleBuildState> buildStates = null;
	BMConfiger bmConfiger = null;
	BMUrls bmUrl = null;
	
	string downloadRootUrl = null;
    string downloadLoadRootUrl = null;
	BuildPlatform curPlatform;
	
	Dictionary<string, BundleData> bundleDict = new Dictionary<string, BundleData>();
	Dictionary<string, BundleBuildState> buildStatesDict = new Dictionary<string, BundleBuildState>();
	
	// Request members
	Dictionary<string, WWWRequest> processingRequest = new Dictionary<string, WWWRequest>();
	Dictionary<string, WWWRequest> succeedRequest = new Dictionary<string, WWWRequest>();
	Dictionary<string, WWWRequest> failedRequest = new Dictionary<string, WWWRequest>();
	List<WWWRequest> waitingRequests = new List<WWWRequest>();
	List<WWWRequest> requestedBeforeInit = new List<WWWRequest>();

	static DownloadManager instance = null;
	static string manualUrl = "";
    private static ProgressManager progressManager = null;
	/**
	 * Get instance of DownloadManager.
	 * This prop will create a GameObject named Downlaod Manager in scene when first time called.
	 */ 
	public static DownloadManager Instance
	{
		get
		{
			if (instance == null)
			{
                try
                {
                    progressManager = ProgressManager.getInstance();
                }
                catch (System.Exception)
                {
                    progressManager = null;
                }
				instance = new GameObject("Download Manager").AddComponent<DownloadManager> ();
				DontDestroyOnLoad(instance.gameObject);
			}
 
			return instance;
		}
	}

	public static void SetManualUrl(string url)
	{
		if(instance != null)
		{
			Debug.LogError("Cannot use SetManualUrl after accessed DownloadManager.Instance. Make sure call SetManualUrl before access to DownloadManager.Instance.");
			return;
		}

		manualUrl = url;
	}
	
	class WWWRequest
	{
		public string requestString = "";
		public string url = "";
		public int triedTimes = 0;
		public int priority = 0;
		public BundleData bundleData = null;
		public BundleBuildState bundleBuildState = null;
        public int dependCount = 0;
		public WWW www = null;
#if UNITY_WEBPLAYER
		public void CreatWWW()
		{	
			triedTimes++;
			
			if(DownloadManager.instance.bmConfiger.useCache && bundleBuildState != null)
			{
#if !(UNITY_4_2 || UNITY_4_1 || UNITY_4_0)
				if(DownloadManager.instance.bmConfiger.useCRC)
					www = WWW.LoadFromCacheOrDownload(url, bundleBuildState.version, bundleBuildState.crc);
                    
				else
#endif
					www = WWW.LoadFromCacheOrDownload(url, bundleBuildState.version);
               
			}
			else
				www = new WWW(url);
		}
#endif
#if UNITY_STANDALONE_WIN
        public void CreatWWW()
        {
            triedTimes++;
//            //������Դ����֤�����Ƿ���ڸ��ļ����Ƚϰ汾
//            if (DownloadManager.instance.bmConfiger.useCache && bundleBuildState != null)
//            {
//#if !(UNITY_4_2 || UNITY_4_1 || UNITY_4_0)
//                if (DownloadManager.instance.bmConfiger.useCRC)
//                    www = WWW.LoadFromCacheOrDownload(url, bundleBuildState.version, bundleBuildState.crc);

//                else
//#endif
//                    www = WWW.LoadFromCacheOrDownload(url, bundleBuildState.version);

//            }
//            else


            www = new WWW(url);
        }
#endif 
	}
}
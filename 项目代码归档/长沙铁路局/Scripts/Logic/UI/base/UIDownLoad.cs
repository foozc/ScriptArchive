using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Log;
using Assets.Scripts.Tools;

namespace Assets.Scripts.Logic.UI
{
    //此类使用的前提条件是场景中必须要有UIroot或者uiroot3D/
    public class UIDownLoad
    {
        #region  单例类:Instance属性获取实例
        private static UIDownLoad instance = null;

        public static UIDownLoad Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UIDownLoad();
                }
                return instance;
            }
        }
        #endregion

        #region  assetbundle资源和prefab资源名称配置

        //------------ AssetBundle资源名称列表 --------------//
        public const string Asset_Main = "Main";         //主界面中的资源,一般要优先实例化到场景中并显示出来，下载优先级：2 /
        public const string Asset_Window = "Window";     //一级界面的资源，主要是背包、装备等窗口，下载优先级：5 /
        public const string Asset_Cell = "Cells";        //一些组成其他界面的基本功能单元，下载优先级：1 /
        public const string Asset_UIPool = "ToPool";     //实例化后要添加到UI对象池中的资源，即可放在其他bundle中也可放在toPool中的资源优先放在pool中，下载优先级：4 /
        public const string Asset_UIAtlas = "Atlas";     //图集资源，下载优先级：0 /
        public const string Asset_Tips = "Tips";         //UI中的各种tip资源，下载优先级：3 /

        //------------ prefab预制物名称列表 --------------//
        public const string Prefab_BtnWithFX_Cell = "BtnWithFX";        
        public const string Prefab_BagCell_Cell = "BagCell";
        public const string Prefab_SkillCell_Cell = "SkillCell";
        public const string Prefab_TabPre_Cell = "TabPre";
        public const string Prefab_MapMarkIcon_Cell = "MapMarkIcon";
        public const string Prefab_FuncBtn_Cell = "FuncBtn";
        public const string Prefab_IconFolder_Cell = "IconFolder";
        public const string Prefab_TreeMenuItem = "TreeMenuItem";

        public const string Prefab_BubbleTip_UIPool = "BubbleTip";
        public const string Prefab_PopWindow_UIPool = "PopWindow";
        public const string Prefab_SimplestLifeBar_UIPool = "SimplestLifeBar";


        public const string Prefab_BackpackUI_Window = "BackpackUI";
        public const string Prefab_EquipmentUI_Window = "EquipmentUI";
        public const string Prefab_RoleDataUI_Window = "RoleDataUI";
        public const string Prefab_SkillUI_Window = "UI_Skill_Panel";
        public const string Prefab_MountUI_Window = "UIMountPanel";
        public const string Prefab_MapUI_Window = "MapUI";
        #endregion

        #region  变量声明
        /// <summary>
        /// 请求过的要下载的所有资源的名称,被释放的会被remove掉
        /// </summary>
        private List<string> allResNames = new List<string>();
        /// <summary>
        /// UI预制物名字对应的assetbundle名字的字典
        /// </summary>
        private Dictionary<string, string> prefabAssetsNamePairDic = new Dictionary<string, string>();
        /// <summary>
        /// 从assetbundle中load出来的资源，在内存中未实例化到场景中之前的形态【key：预制物名；value：预制物】
        /// </summary>
        private Dictionary<string, GameObject> UIPrefab_dic = new Dictionary<string, GameObject>();
        /// <summary>
        /// 要放到UI对象池中的UI预制物的名字【三类对象：放到对象池、实例到场景、暂不实例】/
        /// </summary>
        private List<string> prefabToPool = new List<string>();
        /// <summary>
        /// 要实例化到场景中的UI预制物的名字【三类对象：放到对象池、实例到场景、暂不实例】/
        /// </summary>
        private List<string> prefabToScene = new List<string>();
        /// <summary>
        /// 存储sprite名和对应的图集名
        /// </summary>
        private Dictionary<string, string> spriteAndAtlas_dic = new Dictionary<string, string>();
        /// <summary>
        /// 所有的图集存放在这个字典里(存储所有图集名和图集本身)
        /// </summary>
        private Dictionary<string, UIAtlas> atlas_dic = new Dictionary<string, UIAtlas>();
        /// <summary>
        /// 需要做自适应的UI元素的名字及其对应参数
        /// </summary>
        private Dictionary<string, Orientation> UILocateArgsDic = new Dictionary<string, Orientation>();
        /// <summary>
        /// 存储对所有实例化到场景中的UI界面的引用
        /// </summary>
        private Dictionary<string, GameObject> UIInScene = new Dictionary<string, GameObject>();
        /// <summary>
        /// UI元素要挂载的父节点
        /// </summary>
        private Dictionary<LocateType, Transform> UIParent = new Dictionary<LocateType, Transform>();
        /// <summary>
        /// 下载完成的委托
        /// </summary>
        public Action downLoadIsDone = null;  //下载完成/
        public bool downLoadOver = false;    //下载完成/
        /// <summary>
        /// 首次实例化UI到场景中完成的委托
        /// </summary>
        public Action instantiateIsDone = null;  //首次实例化完成/
        public bool instantiateOver = false;   //首次实例化完成/
        /// <summary>
        /// 场景中必须要首先显示的UI的名字的list
        /// </summary>
        private List<string> UINecessaryToShow = new List<string>();
        #endregion

        #region  各字典或list的初始化方法[下载信息配置]

        /// <summary>
        /// 场景中必须要首先显示的UI
        /// </summary>
        /// <returns></returns>
        private List<string> necessaryUIListInit()
        {
            List<string> UINecessary = new List<string>();


            return UINecessary;
        }

        /// <summary>
        /// 预制物和对应的资源包名字典初始化，前一个string是预制物名称，后一个string是资源包名称
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> UIResInfoDicInit()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();


            dic.Add(Prefab_SkillCell_Cell, Asset_Cell);
            dic.Add(Prefab_BagCell_Cell, Asset_Cell);
            dic.Add(Prefab_TabPre_Cell, Asset_Cell);
            dic.Add(Prefab_BtnWithFX_Cell, Asset_Cell);
            dic.Add(Prefab_MapMarkIcon_Cell, Asset_Cell);
            dic.Add(Prefab_FuncBtn_Cell,Asset_Cell);
            dic.Add(Prefab_IconFolder_Cell,Asset_Cell);
            dic.Add(Prefab_TreeMenuItem, Asset_Cell);

            dic.Add(Prefab_PopWindow_UIPool, Asset_UIPool);
            dic.Add(Prefab_BubbleTip_UIPool, Asset_UIPool);
            dic.Add(Prefab_SimplestLifeBar_UIPool,Asset_UIPool);


            dic.Add(Prefab_MapUI_Window, Asset_Window);
            dic.Add(Prefab_BackpackUI_Window, Asset_Window);
            dic.Add(Prefab_EquipmentUI_Window,Asset_Window);
            dic.Add(Prefab_RoleDataUI_Window,Asset_Window);
            dic.Add(Prefab_SkillUI_Window, Asset_Window);
            dic.Add(Prefab_MountUI_Window, Asset_Window);


            return dic;
        }


        /// <summary>
        /// 要放到ui对象池中的ui预制物的名字的list
        /// </summary>
        /// <returns></returns>
        private List<string> UIToPoolNameslistInit()
        {
            List<string> uiPrefabToPool = new List<string>();

            uiPrefabToPool.Add(Prefab_BubbleTip_UIPool);
            uiPrefabToPool.Add(Prefab_PopWindow_UIPool);
            uiPrefabToPool.Add(Prefab_SimplestLifeBar_UIPool);

            return uiPrefabToPool;
        }


        /// <summary>
        /// 要实例化到场景中的ui预制物的名字的list
        /// </summary>
        /// <returns></returns>
        private List<string> UIToSceneNameslistInit()
        {
            List<string> uiPrefabToScene = new List<string>();

            uiPrefabToScene.Add(Prefab_MapUI_Window);
            
            uiPrefabToScene.Add(Prefab_RoleDataUI_Window);
            uiPrefabToScene.Add(Prefab_BackpackUI_Window);
            uiPrefabToScene.Add(Prefab_EquipmentUI_Window);
            uiPrefabToScene.Add(Prefab_SkillUI_Window);
            uiPrefabToScene.Add(Prefab_MountUI_Window);

            return uiPrefabToScene;
        }

        /// <summary>
        /// 所有UI中用到的精灵图片名和对应的图集名（用于根据精灵名寻找图集）
        /// </summary>
        /// <returns></returns>
        private Dictionary<string,string> spritesAtlasDicInit()
        {
            Dictionary<string, string> spritesAtlas = new Dictionary<string, string>();



            return spritesAtlas;
        }

        /// <summary>
        /// 图集字典初始化，这里初始化主要目的是后面装入下载下来的atlas内容时比较key值,初始化时都赋值null/
        /// </summary>
        /// <returns></returns>
        private Dictionary<string,UIAtlas> atlasDicInit()
        {
            Dictionary<string, UIAtlas> atlas = new Dictionary<string, UIAtlas>();


            return atlas;
        }


        /// <summary>
        /// 需要定位的UI元素的定位信息字典初始化方法
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, Orientation> UINeedLocateDicInit()
        {
            Dictionary<string, Orientation> locateUIInfo = new Dictionary<string, Orientation>();


            locateUIInfo.Add(Prefab_MapUI_Window, new Orientation(LocateType.None, Vector3.zero));
            locateUIInfo.Add(Prefab_BackpackUI_Window, new Orientation(LocateType.None, Vector3.zero));
            locateUIInfo.Add(Prefab_RoleDataUI_Window, new Orientation(LocateType.None, new Vector3(0f, 275f, 0f)));

            return locateUIInfo;
        }

        /// <summary>
        /// 不同定位类型的ui元素生成时候对应的父节点
        /// </summary>
        /// <returns></returns>
        private Dictionary<LocateType, Transform> UIParentDicInit()
        {
            Dictionary<LocateType, Transform> parenTrs = new Dictionary<LocateType, Transform>();
            //Transform root = GameObject.FindObjectOfType<UIRoot>().transform;
            Transform root = GameObject.Find("UI Root").transform;
            parenTrs.Add(LocateType.None, root);
            parenTrs.Add(LocateType.TopLeft, root.Find("TopLeft"));
            parenTrs.Add(LocateType.TopCenter, root.Find("TopCenter"));
            parenTrs.Add(LocateType.TopRight, root.Find("TopRight"));
            parenTrs.Add(LocateType.BottomLeft, root.Find("BottomLeft"));
            parenTrs.Add(LocateType.BottomCenter, root.Find("BottomCenter"));
            parenTrs.Add(LocateType.BottomRight, root.Find("BottomRight"));
            parenTrs.Add(LocateType.TipLayer, root.Find("TipPanel"));
            return parenTrs;
        }

        #endregion


        #region 根据配置下载所有预制物，并加载到内存中，保存对象的应用在对应字典中，后续加入实时单个预制物下载或加载
        /// <summary>
        /// 下载所有UI资源
        /// </summary>
        public string[] loadAllUI()
        {
            List<string> uiBundles = new List<string>();

            //完成相应字典或列表的初始化/
            prefabAssetsNamePairDic = UIResInfoDicInit();  //预制物名与对应包名，此字典用于下载assetbundle及load界面资源/
            prefabToPool = UIToPoolNameslistInit();        //ui资源中要放到对象池中管理的那部分预制物的名字，名字存在此list中的预制物将被添加到UI对象池中/
            prefabToScene = UIToSceneNameslistInit();      //场景初始化时要首先实例化到场景中的那部分ui预制物的名字，名字在此list中的预制物将在场景加载时首先被实例化出来/
            spriteAndAtlas_dic = spritesAtlasDicInit();    //精灵uisprite的名字和对应的图集的名字的字典初始化/
            atlas_dic = atlasDicInit();                    //图集的名字和对应的图集的字典的初始化/
            UILocateArgsDic = UINeedLocateDicInit();       //要进行屏幕自适应的ui预制物的名字及对应的自适应参数值的字典/
            UIParent = UIParentDicInit();                  //UI元素父节点transform信息初始化/ 
            UINecessaryToShow = necessaryUIListInit();     //场景中必须要首先显示的UI初始化/

            //开始下载assetbundle/
            DownloadManager.Instance.StartDownload(Asset_UIAtlas); uiBundles.Add(Asset_UIAtlas);
            DownloadManager.Instance.StartDownload(Asset_Cell); uiBundles.Add(Asset_Cell);
            DownloadManager.Instance.StartDownload(Asset_Main); uiBundles.Add(Asset_Main);
            DownloadManager.Instance.StartDownload(Asset_Tips); uiBundles.Add(Asset_Tips);
            DownloadManager.Instance.StartDownload(Asset_UIPool); uiBundles.Add(Asset_UIPool);
            DownloadManager.Instance.StartDownload(Asset_Window); uiBundles.Add(Asset_Window);
            
            //从assetbundle中load资源/
            //getMonoInstance().StartCoroutine(loadAllUIRes(new List<string>() { Asset_UIAtlas,Asset_Cell, Asset_Main,Asset_Tips,Asset_UIPool, Asset_Window }));
            getMonoInstance().StartCoroutine(loadAllUIRes(uiBundles));

            return uiBundles.ToArray();
        }

        /// <summary>
        /// 从assetbundle中load出ui资源并对资源按管理方式不同进行分配【是否使用对象池管理】
        /// </summary>
        /// <param name="bundleResStrings"></param>
        /// <returns></returns>
        private IEnumerator loadAllUIRes(List<string> bundleResStrings)
        {
            bool isErr = false;
            while(DownloadManager.Instance.ProgressOfBundles(bundleResStrings.ToArray())<1)
            {
                foreach (string str in bundleResStrings)
                {
                    if (!string.IsNullOrEmpty(DownloadManager.Instance.GetError(str)))
                    {
                        isErr = true;
                        break;
                    }
                }
                if (isErr) break;
                yield return null;
            }

            if (!isErr)
            {
                foreach(KeyValuePair<string,string> pair in prefabAssetsNamePairDic)
                {
                    WWW UIRes = DownloadManager.Instance.GetWWW(pair.Value);
                    yield return UIRes;

                    if (atlas_dic.ContainsKey(pair.Key))
                    {
                        GameObject atlasPrefab = UIRes.assetBundle.LoadAsset(pair.Key, typeof(GameObject)) as GameObject;  //这里下载的时候记得要限定类型，因为有同名的不同类型的资源存在/
                        yield return atlasPrefab;
                        atlas_dic[pair.Key] = atlasPrefab.GetComponent<UIAtlas>();  //把原先初始化时赋的null值赋上实际有的uiatlas值/
                    }
                    else
                    {
                        GameObject uiPrefab = UIRes.assetBundle.LoadAsset(pair.Key, typeof(GameObject)) as GameObject;  //这里下载的时候记得要限定类型，因为有同名的不同类型的资源存在/
                        yield return uiPrefab;
                        if (!UIPrefab_dic.ContainsKey(pair.Key))
                        {
                            UIPrefab_dic.Add(pair.Key, uiPrefab);
                        }
                    }
                }
                //所有资源下载完成/
                if (downLoadIsDone != null) downLoadIsDone();
                downLoadOver = true;
                //分配下载出来的资源/
                allocateRes(UIPrefab_dic);  //分配load出来的uiGameObject资源，该实例到场景中的实例出来，该添加到对象池的放到池子里/
            }
            else
            {
                Debug.Log("下载UI资源出现错误！！！！");
            }
        }


        /// <summary>
        /// 分配load出来的ui资源，该实例到场景中的实例出来，该添加到对象池的放到池子里/
        /// </summary>
        /// <param name="prefabsDic"></param>
        private void allocateRes(Dictionary<string, GameObject> prefabsDic)
        {
            foreach(KeyValuePair<string,GameObject> pair in prefabsDic)
            {
                if (prefabToPool.Contains(pair.Key))
                {
                    addToUIPool(pair.Value);   //添加到对象池/
                }
                else if (prefabToScene.Contains(pair.Key))
                {
                    InitToScene(pair.Value);   //实例化到场景/
                }
            }

            //首次实例化完成/
            if (instantiateIsDone != null) instantiateIsDone();
            instantiateOver = true;
        }


        /// <summary>
        /// 实例化到场景并隐藏(这个方法主要用于实例化不销毁的单例UI或将会挂在UIRoot下面六个定位容器中的一级子物体)
        /// </summary>
        private GameObject InitToScene(GameObject uiPrefab)
        {
            GameObject obj = MonoBehaviour.Instantiate(uiPrefab) as GameObject;
            if (obj.name.EndsWith("(Clone)"))
            {
                obj.name = obj.name.Substring(0, obj.name.Length - 7).TrimEnd();
            }
            //这里加入UI定位判断/
            if (UILocateArgsDic.ContainsKey(obj.name))
            {
                Orientation locateInfo = UILocateArgsDic[obj.name];
                Transform parentNode = UIParent[locateInfo.Type];
                obj.transform.parent = parentNode;
                obj.transform.localPosition = locateInfo.LocalPos;
            }
            else
            {
                obj.transform.parent = UIParent[LocateType.None];
                obj.transform.localPosition = Vector3.zero;
            }
            obj.transform.localScale = Vector3.one;
            obj.transform.localEulerAngles = Vector3.zero;

            //MonoBehaviour.DontDestroyOnLoad(obj);   //UI对象不销毁/

            if (!UIInScene.ContainsKey(obj.name))
            {
                UIInScene.Add(obj.name, obj);  //添加到字典中/
            }

            obj.SetActive(false);  //隐藏实例化出来的物体/

            return obj;
        }
        #endregion

        #region  提供给外部调用的public方法（未包含UI对象池的方法）

        /// <summary>
        /// 通过UI对象的名称获取定位数据
        /// </summary>
        /// <param name="UIName"></param>
        /// <returns></returns>
        public Orientation getUIOrientation(string UIName)
        {
            if (UILocateArgsDic.Count == 0) UILocateArgsDic = UINeedLocateDicInit();
            if (UILocateArgsDic.ContainsKey(UIName))
            {
                return UILocateArgsDic[UIName];
            }
            else
                return null;
        }

        /// <summary>
        /// 通过定位类型获取父物体
        /// </summary>
        /// <param name="locate"></param>
        /// <returns></returns>
        public Transform getUIParent(LocateType locate)
        {
            if (UIParent.Count == 0) UIParent = UIParentDicInit();  //UI元素父节点transform信息初始化/
            return UIParent[locate];
        }

        /// <summary>
        /// 根据配置显示UI
        /// </summary>
        public void showUIByCfg()
        {
            if (UINecessaryToShow.Count == 0) UINecessaryToShow = necessaryUIListInit();
            foreach (string uiname in UINecessaryToShow)
            {
                getUI(uiname).SetActive(true);
            }
        }


        /// <summary>
        /// 根据spritename获取对应图集
        /// </summary>
        /// <param name="spriteName"></param>
        /// <returns>可能会返回空值</returns>
        public UIAtlas getAtlas(string spriteName)
        {
            string atlasName = "";
            if(spriteAndAtlas_dic.ContainsKey(spriteName))
            {
                atlasName = spriteAndAtlas_dic[spriteName];
            }
            if (!string.IsNullOrEmpty(atlasName) && atlas_dic.ContainsKey(atlasName))
            {
                return atlas_dic[atlasName];
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 获取UI界面预制物(从assetbundle中load出来的预制物都存在这里)
        /// </summary>
        /// <param name="UIResName"></param>
        /// <param name="parentNode"></param>
        /// <returns></returns>
        public GameObject getUIPrefab(string UIResName)
        {
            if (UIPrefab_dic.ContainsKey(UIResName))
            {
                return UIPrefab_dic[UIResName];
            }
            else
            {
                if (prefabAssetsNamePairDic.Count == 0) prefabAssetsNamePairDic = UIResInfoDicInit();  //如果名字对字典中无数据，执行字典初始化/
                WWW bundle = getWWW(prefabAssetsNamePairDic[UIResName], null);  //获取UI的www资源，如果不为空就进行实例化，如果为空就发出下载请求/
                if (bundle != null)   //如果bundle当前没有获取到资源，那么请求获取资源/
                {
                    GameObject preObj = bundle.assetBundle.LoadAsset(UIResName) as GameObject;

                    UIPrefab_dic.Add(UIResName, preObj); //将UI预制物对象添加到字典中/
                    return preObj;
                }
                else
                {
                    Debug.Log("您要获取的预制物不存在！可能是查找名字错误或资源未下载完！暂时提供一个本地下载预制物方法提供预制物！");
                    GameObject pre = Resources.Load("Prefabs/UI/" + UIResName, typeof(GameObject)) as GameObject;
                    return pre;
                }
            }
        }



        /// <summary>
        /// 获取实例到场景中的UI对象（主要用来获取单例UI或一级UI，同名的UI对象只能获取到一个实例）
        /// </summary>
        /// <param name="UIName"></param>
        /// <param name="parentNode"></param>
        /// <returns></returns>
        public GameObject getUI(string UIName,bool active = false)
        {
            if(UIInScene.ContainsKey(UIName))
            {
                if(active) UIInScene[UIName].SetActive(true);   //显示出来/
                return UIInScene[UIName];
            }
            else
            {
                GameObject preObj = getUIPrefab(UIName);
                if (preObj != null)
                {
                    GameObject UIgo = InitToScene(preObj);
                    if(active) UIgo.SetActive(true);
                    return UIgo;
                }
                else
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// 销毁实例化到场景中的UI对象
        /// </summary>
        /// <param name="UIName"></param>
        /// <param name="isHide"></param>
        public bool destroyUI(string UIName,bool isHide = true)
        {
            if(UIInScene.ContainsKey(UIName))
            {
                if(isHide)
                {
                    UIInScene[UIName].SetActive(false);
                }
                else
                {
                    MonoBehaviour.Destroy(UIInScene[UIName]);
                    UIInScene.Remove(UIName);
                }
                return true;
            }
            else
            {
                Debug.Log("无法在场景中找到指定名字的要销毁的UI对象");
                return false;
            }
        }

        /// <summary>
        /// 销毁实例化在对象池中的UI对象
        /// </summary>
        /// <param name="objTrsInPool"></param>
        public void destroyUI(Transform objTrsInPool,bool isHide = true)
        {
            if(getUIPool.IsSpawned(objTrsInPool))
            {
                getUIPool.Despawn(objTrsInPool);
            }
            else
            {
                if (isHide)
                {                   
                    objTrsInPool.gameObject.SetActive(false);
                    Debug.Log("您要销毁的元素在UI对象池中不存在，已选择将其隐藏不显示！");
                }
                else
                {
                    MonoBehaviour.Destroy(objTrsInPool.gameObject);
                    Debug.Log("您要销毁的元素在UI对象池中不存在，已选择直接destroy掉！");
                }
            }
        }

        #endregion


        #region  下载assetbundle资源
        /// <summary>
        /// 获取一个monobehaviour的实例
        /// </summary>
        /// <returns></returns>
        private MonoBehaviour getMonoInstance()
        {
            UIRoot NGUIroot = GameObject.FindObjectOfType<UIRoot>();
            if (NGUIroot == null)
            {
                NGUIroot = new GameObject("_root").AddComponent<UIRoot>();
            }
            return NGUIroot as MonoBehaviour;
        }

        /// <summary>
        /// 获取一个www资源
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="downLoadDone"></param>
        /// <param name="progress"></param>
        private WWW getWWW(string bundleName,Action<WWW> downLoadDone,Action<float> progress = null)
        {
            if (DownloadManager.Instance.IsUrlRequested(bundleName))
            {
                string[] str = new string[1] { bundleName };
                if (DownloadManager.Instance.ProgressOfBundles(str) == 1)
                {
                    if (downLoadDone != null) downLoadDone(DownloadManager.Instance.GetWWW(bundleName));
                    if (progress != null) progress(1f);
                    return DownloadManager.Instance.GetWWW(bundleName);
                }
                else
                {
                    DownloadManager.Instance.StartDownload(bundleName);
                    MonoBehaviour behaviour = getMonoInstance();
                    behaviour.StartCoroutine(getRes(bundleName, downLoadDone, progress));
                    if (!allResNames.Contains(bundleName)) allResNames.Add(bundleName);
                    return null;
                }
            }
            else
            {
                DownloadManager.Instance.StartDownload(bundleName);
                MonoBehaviour behaviour = getMonoInstance();
                behaviour.StartCoroutine(getRes(bundleName, downLoadDone, progress));
                allResNames.Add(bundleName);
                return null;
            }
        }

        /// <summary>
        /// 协程获取www资源
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="downLoadDone"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        private IEnumerator getRes(string bundleName, Action<WWW> downLoadDone, Action<float> progress = null)
        {
            bool progressDone = false;
            WWW bundle = null;
            do
            {
                if (string.IsNullOrEmpty(DownloadManager.Instance.GetError(bundleName)))
                {
                    progressDone = true;
                    bundle = DownloadManager.Instance.GetWWW(bundleName);     //获取下载的www资源/
                    if (bundle == null) progressDone = false;
                    if (progress != null)
                    {
                        string[] strs = new string[1] { bundleName };
                        progress(DownloadManager.Instance.ProgressOfBundles(strs));   //下载进度/
                    }
                }
                else
                {
                    break;
                }
                yield return null;
            } while (!progressDone);
            if (downLoadDone!=null) downLoadDone(bundle);
            if (progress != null && !progressDone) progress(-1f);
        }


        /// <summary>
        /// 获取进度值，如果list中只有一个元素，则查单个元素下载进度，如果有一组元素，则查这一组元素的整体进度，如果输入参数为空，则查当前请求过的所有文件的下载进度
        /// </summary>
        /// <param name="bundleNameStrs"></param>
        /// <returns></returns>
        private float getProgress(List<string> bundleNameStrs = null)
        {
            if (bundleNameStrs == null)
            {
                return DownloadManager.Instance.ProgressOfBundles(allResNames.ToArray());
            }
            else
            {
                return DownloadManager.Instance.ProgressOfBundles(bundleNameStrs.ToArray());
            }
        }

        /// <summary>
        /// 释放内存中的assetbundle资源
        /// </summary>
        /// <param name="bundleName"></param>
        private void disposeWWW(string bundleName)
        {
            DownloadManager.Instance.DisposeWWW(bundleName);
            if (allResNames.Contains(bundleName)) allResNames.Remove(bundleName);
            //if (bundleResDic.ContainsKey(bundleName)) bundleResDic.Remove(wwwResName);
        }

        #endregion


        #region  PoolManager 管理需要重复生成与销毁的ui对象

        private SpawnPool spawnPool = null;
        private List<string> UIprefabsNames = new List<string>();


        public bool isUIfabExist(string UIprefabName)
        {
            return UIprefabsNames.Contains(UIprefabName);
        }

        public SpawnPool getUIPool
        {
            get
            {
                SpawnPool[] pools = GameObject.FindObjectsOfType<SpawnPool>();
                foreach (SpawnPool sp in pools)
                {
                    if (sp.transform.name.Equals("UIPool"))
                    {
                        this.spawnPool = sp;
                    }
                }
                if (this.spawnPool == null)
                {
                    this.spawnPool = PoolManager.Pools.Create("UI");
                    this.spawnPool.transform.parent = GameObject.Find("UI Root").transform;
                    this.spawnPool.transform.localScale = Vector3.one;
                    this.spawnPool.matchPoolScale = true;
                    this.spawnPool._dontDestroyOnLoad = true;
                }
                return this.spawnPool;
            }
        }

        /// <summary>
        /// 添加到UI池子里进行管理（主要针对伤害值显示等UI控件）
        /// </summary>
        /// <param name="prefabRes">这里prefabResTrs是从assetbundle里load出来后准备Instantiate要使用的资源</param>
        private void addToUIPool(GameObject prefabRes,int preloadCount = 10)   
        {
            if (UIprefabsNames.Contains(prefabRes.name)) return;   //已添加过的同名预制物不在添加，这里要求所有不同的预制物名称不同/

            if (this.spawnPool == null)
            {
                SpawnPool[] pools = GameObject.FindObjectsOfType<SpawnPool>();
                foreach (SpawnPool sp in pools)
                {
                    if (sp.transform.name.Equals("UIPool"))
                    {
                        this.spawnPool = sp;
                    }
                }
                if (this.spawnPool == null)
                {
                    this.spawnPool = PoolManager.Pools.Create("UI");
                    this.spawnPool.transform.parent = GameObject.Find("UI Root").transform; 
                    this.spawnPool.transform.localScale = Vector3.one;
                    this.spawnPool.matchPoolScale = true;
                    this.spawnPool._dontDestroyOnLoad = true;
                }                
            }

            PrefabPool refabPool = new PrefabPool(prefabRes.transform);   

            if (!spawnPool._perPrefabPoolOptions.Contains(refabPool))
            {
                refabPool = new PrefabPool(prefabRes.transform);
                //默认初始化一个Prefab
                refabPool.preloadAmount = preloadCount;
                //开启限制
                refabPool.limitInstances = true;
                //关闭无限取Prefab
                refabPool.limitFIFO = false;
                //限制池子里最大的Prefab数量
                refabPool.limitAmount = 50;
                //开启自动清理池子
                refabPool.cullDespawned = true;
                //最终保留
                refabPool.cullAbove = 5;
                //多久清理一次
                refabPool.cullDelay = 60;
                //每次清理几个
                refabPool.cullMaxPerPass = 5;
                //初始化内存池
                this.spawnPool._perPrefabPoolOptions.Add(refabPool);
                this.spawnPool.CreatePrefabPool(refabPool); 
                //spawnPool.CreatePrefabPool(spawnPool._perPrefabPoolOptions[spawnPool.Count]);

                UIprefabsNames.Add(prefabRes.name);  //已成功添加到池子里的预制物的名字也添加到预制物名字的list里/
            }
        }

        #endregion


        #region  ui元素在屏幕上定位要用到的枚举和参数类

        /// <summary>
        /// UI元素定位的类型，上左、上中、上右，下左、下中、下右/
        /// </summary>
        public enum LocateType
        {
            None = 0,
            TopLeft,
            TopCenter,
            TopRight,
            BottomLeft,
            BottomCenter,
            BottomRight,
            TipLayer
        }

        /// <summary>
        /// UI元素在屏幕中的方位，定位类型加相对坐标值
        /// </summary>
        public class Orientation
        {
            private LocateType type;
            private Vector3 localPos;

            public Orientation(LocateType type, Vector3 localPos)
            {
                this.type = type;
                this.localPos = localPos;
            }

            public LocateType Type
            {
                get { return this.type; }
            }

            public Vector3 LocalPos
            {
                get { return this.localPos; }
            }
        }

        #endregion
    }
}

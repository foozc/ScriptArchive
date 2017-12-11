using UnityEngine;
using System.Collections.Generic;

using Assets.Scripts.Logic;
using Assets.Scripts.Logic.UI;
using Assets.Scripts.Configs;
using Assets.Scripts.Logic.UI.CommonUI;
using Assets.Scripts.Logic.UI.Training;
using Assets.LSE.XF;
using Assets.Scripts.Logic.UI.MainUIMenu;
using UnityEngine.SceneManagement;
using Assets.Scripts.Tools;
using Coherent.UI;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明：UI管理类
*Author:作者
*
*/
namespace Assets.Scripts.Controller
{
    public enum UILayer
    {
        ModelUI = 0,          //UI模型层
        NormalUI,             //UI常规层
        PopupUI,              //UI弹出层
        ModalUI,              //UI模态层
        ApplicationUI         //UI程序层
    }

    public enum UIType
    {
        MainMenu,
        EquipKnow,
        TrainUI,
		CircuitUI,
		GuideUI
    }
    /**
     * 主要功能：
     *  1.对所有UI界面的管理
     *  2.各组UI界面的显示、隐藏、销毁控制
     *  3.对不同组UI界面的层级控制，防止界面重叠
     */
    public class UIManager : SingletonUI<UIManager>
    {
		#region 需要初始化的UI组
		public MainMenuUI mainMenuUI;
        public EquipKnowUI equipKnowUI;
        public TrainUI trainUI;
		public CircuitUINew circuitUI;
		public CoherentUIView H5View;
        public SelectTopicUI selectTopicUI;
        public GuideHelperUI guideUI;
        public GameObject loading;
        public UIPanel exitSystemUI;
		public string modename = null;
		// public SoftKey ytsoftkey;//引用由域天工具随机生成的加密类模块
		//public ExamUI examUI;

		#endregion

		private Dictionary<UIType, UILayer> uiMap;
        private UIType currentUIType = UIType.MainMenu;
        private List<GameObject> _viewMenuQueue;
        private bool ScrollViewIsActive = false;
        private Clock globalTimer;

        public List<GameObject> viewMenuQueue
        {
            get { return _viewMenuQueue; }
        }
        void Awake()
        {
            uiMap = new Dictionary<UIType, UILayer>();
            _viewMenuQueue = new List<GameObject>();
            initAllUI();
        }

        private void initAllUI()
        {
            globalTimer = new Clock(0, 0, 1, 0, 100, 0);
            ClockManager.getInstance().addClock(globalTimer);

            uiMap.Add(UIType.EquipKnow, new UILayer(equipKnowUI));
            uiMap.Add(UIType.TrainUI, new UILayer(trainUI));
			uiMap.Add(UIType.CircuitUI, new UILayer(circuitUI));
            uiMap.Add(UIType.GuideUI, new UILayer(guideUI));
            //uiMap.Add(UIType.ExamUI, new UILayer(examUI));
        }

        private bool isFirstTrain = false;
        public void changeScene(string sceneName, UIType type, string flow)
        {
            if (!isFirstTrain)
            {
                //记录LOADING场景中需要读取的场景名称
                GlobalConfig.loadName = sceneName;
                //先进入LoadingScene场景
                SceneManager.LoadScene("LoadingScene");
                loading.SetActive(true);

                ((TrainUI)setActiveUI(type)).initTrainUI(null, flow, TrainUI.TrainMode.Training);
            }
            else
            {
                SceneManager.LoadScene("Guide");
                ((GuideHelperUI)setActiveUI(UIType.GuideUI)).setNextScene(sceneName, type, flow);
            }
            isFirstTrain = false;
        }

        public void changeScene(string sceneName, string equipName, UIType type, string flow, TrainUI.TrainMode trainMode)
        {
            if (!isFirstTrain)
            {
                //记录LOADING场景中需要读取的场景名称
                GlobalConfig.loadName = sceneName;
                //先进入LoadingScene场景
                SceneManager.LoadScene("LoadingScene");
                loading.SetActive(true);

				((TrainUI)setActiveUI(type)).initTrainUI(equipName, flow, trainMode);
			}
			else
            {
                SceneManager.LoadScene("Guide");
                ((GuideHelperUI)setActiveUI(UIType.GuideUI)).setNextScene(sceneName, type, flow);
            }
            isFirstTrain = false;
        }

        public UIbase setActiveUI(UIType type)
        {
            UIbase ui = null;
            foreach (UIType layer in uiMap.Keys)
            {
                if (layer == type)
                {
                    uiMap[layer].uiModel.gameObject.SetActive(true);
                    uiMap[layer].state = true;
                    ui = uiMap[layer].uiModel;
                }
                else
                {
                    uiMap[layer].uiModel.gameObject.SetActive(false);
                    uiMap[layer].state = false;
                }
            }
            this.currentUIType = type;
            initUIGroup(type);
            return ui;
        }

        /// <summary>
        /// 当打开对应UI组时需要执行的界面初始化操作
        /// </summary>
        /// <param name="type"></param>
        private void initUIGroup(UIType type)
        {
            switch (type)
            {
                case UIType.EquipKnow:
                    AudioManager.getInstance().AudioPause(AudioManager.MusicNumType.groundMusic);
                    MyMouseFocus.Instance.reset();
                    globalTimer.setTick(equipKnowUI.updateRunTime);
                    break;
                case UIType.MainMenu:
                    AudioManager.getInstance().AudioPlay(AudioManager.MusicNumType.groundMusic);
                    MyMouseFocus.Instance.reset();
                    globalTimer.setTick(updateManMenuRunTime);
                    mainMenuUI.roleTypeInit();
                    break;
                case UIType.TrainUI:
                    AudioManager.getInstance().AudioPause(AudioManager.MusicNumType.groundMusic);
                    MyMouseFocus.Instance.reset();
                    globalTimer.setTick(trainUI.updateRunTime);
                    break;
				case UIType.CircuitUI:
					AudioManager.getInstance().AudioPause(AudioManager.MusicNumType.groundMusic);
					MyMouseFocus.Instance.reset();
					globalTimer.setTick(circuitUI.updateRunTime);
					break;
            }
        }

        public void updateManMenuRunTime(Clock c, Clock.Stage s, ulong counter, float clockTime)
        {
            string time = c.UnitConversion();
            string[] hour = time.Split('时');
            string[] min = hour[1].Split('分');
            string[] seconds = min[1].Split('秒');
            string message = "{\"Action\":\"TimeCallback\",\"param1\":\""+hour[0]+ "\",\"param2\":\"" + min[0] + "\",\"param3\":\"" + seconds[0] + "\"}";
            H5View.GetComponent<UICommunicationsScript>().UnityToWeb(message);
            //Debug.Log(time);
           
            //UnityToWeb("{\"Action\":\"TimeCallback\",\"param1\":\"result_hour\",\"param2\":\"result_miniter\",\"param3\":\"result_seconds\"}");
        }

        /// <summary>
        /// 进入设备认知的方法
        /// </summary>
        /// <param name="equipKnowName"></param>
        public void enterEquipKnow(string equipKnowName, TrainUI.TrainMode mode)
        {
            CoherentUISystem.Instance.UIViews.Remove(H5View);
            H5View.GetComponent<Camera>().depth = -40;

            GlobalConfig.loadName = "DeviceKnow1";
            //先进入LoadingScene场景
            SceneManager.LoadScene("LoadingScene");
            loading.SetActive(true);
            EquipKnowUI equipUI = (EquipKnowUI)UIManager.getInstance().setActiveUI(UIType.EquipKnow);
            if (mode == TrainUI.TrainMode.Training)
                mode = TrainUI.TrainMode.Study;
            equipUI.initTreeMenu(equipKnowName, mode);
			equipUI.setName(equipUI.H5Camera.GetComponent<UICommunicationsScript>().getUsetName());
		}

        /// <summary>
        /// 进入流程操作场景
        /// </summary>
        /// <param name="equipName">设备名称</param>
        /// <param name="trainName">流程名：如：设备拆解</param>
        /// <param name="trainMode">学习模式</param>
        public void enterTrain(string equipName, string trainName, TrainUI.TrainMode trainMode)
        {
			modename = trainName;
			switch (trainName)
            {
                case "设备拆解":
                    setActiveUI(UIType.TrainUI);
                    CoherentUISystem.Instance.UIViews.Remove(H5View);
                    H5View.GetComponent<Camera>().depth = -40;
                    changeScene("Train", equipName, UIType.TrainUI, trainName, trainMode);
					trainUI.ExamNumber = 3;
					trainUI.setName(trainUI.H5Camera.GetComponent<UICommunicationsScript>().getUsetName());
					break;
                case "设备组装":
                    setActiveUI(UIType.TrainUI);
                    CoherentUISystem.Instance.UIViews.Remove(H5View);
                    H5View.GetComponent<Camera>().depth = -40;
                    changeScene("Train1", equipName, UIType.TrainUI, trainName, trainMode);
					trainUI.ExamNumber = 4;
					trainUI.setName(trainUI.H5Camera.GetComponent<UICommunicationsScript>().getUsetName());
					break;
				case "电路原理":
					setActiveUI(UIType.CircuitUI);
                    CoherentUISystem.Instance.UIViews.Remove(H5View);
                    H5View.GetComponent<Camera>().depth = -40;
                    circuitUI.initUI(trainMode);
					circuitUI.setName(circuitUI.H5Camera.GetComponent<UICommunicationsScript>().getUsetName());
					break;
                case "设备巡视":
                    setActiveUI(UIType.TrainUI);
                    CoherentUISystem.Instance.UIViews.Remove(H5View);
                    H5View.GetComponent<Camera>().depth = -40;
                    if (trainMode == TrainUI.TrainMode.Exam)
                        trainMode = TrainUI.TrainMode.Study;
                    changeScene("Train_sbxs", equipName, UIType.TrainUI, trainName, trainMode);
					trainUI.ExamNumber = 5;
					trainUI.setName(trainUI.H5Camera.GetComponent<UICommunicationsScript>().getUsetName());
					break;
                case "设备检修":
                    setActiveUI(UIType.TrainUI);
                    CoherentUISystem.Instance.UIViews.Remove(H5View);
                    H5View.GetComponent<Camera>().depth = -40;
                    changeScene("Train_sbjx", equipName, UIType.TrainUI, trainName, trainMode);
					trainUI.ExamNumber = 6;
					trainUI.setName(trainUI.H5Camera.GetComponent<UICommunicationsScript>().getUsetName());
					break;
                case "设备故障":
                    setActiveUI(UIType.TrainUI);
                    CoherentUISystem.Instance.UIViews.Remove(H5View);
                    H5View.GetComponent<Camera>().depth = -40;
                    changeScene("YJ_DBQ_0307", equipName, UIType.TrainUI, trainName, trainMode);
                    trainUI.ExamNumber = 6;
					trainUI.setName(trainUI.H5Camera.GetComponent<UICommunicationsScript>().getUsetName());
                    break;
            }
        }

        /// <summary>
        /// 场景中鼠标滑过某个物体或者点击某个物体
        /// </summary>
        /// <param name="Event"></param>
        /// <param name="obj"></param>
        public void mouseoverClickEvent(string Event, GameObject obj)
        {
            if (Event.Equals("mouseover"))
                equipKnowUI.mouseoverSceneEvent(obj);
            else if (Event.Equals("mouseout"))
                equipKnowUI.mouseoutSceneEvent(obj);
            else if(Event.Equals("click"))
                equipKnowUI.clickSceneEvent(obj);
        }
        
        void OnApplicationQuit()
        {
            CoherentUISystem.Instance.OnApplicationQuit();
        }

        /// <summary>
        /// 当前是否点击在UI上
        /// </summary>
        /// <returns></returns>
        public bool isClickUI()
        {
            return UICamera.isOverUI;
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        public void exitLogin()
        {
            MySession.Clear();
            viewMenuQueue.Clear();
            //Application.LoadLevel("Login");
            setActiveUI(UIType.MainMenu);
        }

        /// <summary>
        /// 初始化消防培训模板所有选择题
        /// </summary>
        public void initXFTraining()
        {
            selectTopicUI.init(GlobalConfig.TrainSelectTopicPath);
        }

        public void showTrainWarn(string text)
        {
            trainUI.showAlertMSG(text);
        }

        /// <summary>
        /// 步骤提示信息
        /// </summary>
        /// <param name="msg"></param>
        public void addHintMSG(string msg)
        {
            trainUI.addHintMSG(msg);
        }
        public void updateHintMSG(string msg)
        {
            trainUI.updateHintMSG(msg);
        }

        public void showSelectTopic(int index)
        {
            trainUI.showSelectTopic(index);
        }

        public UIType getCurrentUIType()
        {
            return currentUIType;
        }
        
        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="musicName"></param>
        public void playMusic(string musicName)
        {
            AudioManager.getInstance().setAction(trainUI.musicCallback);
            AudioManager.getInstance().AudioPlay(musicName, AudioManager.MusicNumType.realtimeMusic);
        }

        public void playMusic(string musicName, AudioManager.MusicNumType type)
        {
            if(type == AudioManager.MusicNumType.realtimeMusic)
                AudioManager.getInstance().setAction(trainUI.musicCallback);
            AudioManager.getInstance().AudioPlay(musicName, type);
        }
        
        public void exitSystem()
        {
            Application.Quit();
        }
        public void showExitSystem()
        {
            exitSystemUI.gameObject.SetActive(true);
        }
        public void cancelExitSystem()
        {
            exitSystemUI.gameObject.SetActive(false);
        }


        private class UILayer
        {
            /// <summary>
            /// 当前ui界面显示状态
            /// </summary>
            public bool state = false;
            /// <summary>
            /// ui界面对象
            /// </summary>
            public UIbase uiModel;

            public UILayer(UIbase ui)
            {
                this.uiModel = ui;
            }
        }
     
        public void closeScrollView()
        {
            GameObject root = GameObject.Find("Scroll View").transform.Find("UIGrid").gameObject;
            root.SetActive(ScrollViewIsActive);
            ScrollViewIsActive=!ScrollViewIsActive;
        }
    }
}
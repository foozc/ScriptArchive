using Assets.LSE.TL;
using Assets.LSE.XF;
using Assets.Scripts.Configs;
using Assets.Scripts.Controller;
using Assets.Scripts.DBLogic;
using Assets.Scripts.Logic.Movies;
using Assets.Scripts.Logic.Scenes;
using Assets.Scripts.Logic.UI.CommonUI;
using Assets.Scripts.Tools;
using Assets.Scripts.VO;
using Newtonsoft.Json;
using PluginEvent;
using System;
using System.Collections.Generic;
using System.Xml;
using Training;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:S700K电路原理界面
*Author:作者
*
*/
namespace Assets.Scripts.Logic.UI.Training
{
    public class CircuitUINew : UIbase
    {
        public UILabel info;
        public CoherentUIView H5Camera;
        public UILabel runTime;
        public NextProcess popWindow;
        public GameObject[] coverTab = null;
        public UISprite Title;
        public UILabel titleLabel;
        public FlowOperateMenu flowOperateMenu;
        public MovieManager movieManager;
        public ToolMenu toolMenu;
        public UIWidget alertMSGParent;

        /// <summary>
        /// mask 
        /// </summary>
        public GameObject mask;
        public GameObject maskLeft;
        public GameObject maskTop;

        public VideoDisplay studyMovie;//视频
        public VideoDisplayCharacter studyMovieText;//视频加文字
        public GameObject studyImage;//图片
        public VideoDisplay traningMovie;//练习视频
        public VideoDisplay examMovie;//考核视频

        /// <summary>
        /// select module
        /// </summary>
        public BallotScreen selectObj;//select pop window
        public TrainUI.TrainMode currentTrainMode;
        public Dictionary<string, CircuitTopicNew> circuitTopics;
        public CircuitTopicNew topic;
        public CoherentUIView examGrade;
        public CountDown examCountDown;
        public GameObject circuitUIStart;
        public bool circuitUIIsStart = false;
        public UILabel nameLable;
        public GameObject ouebutton;
        GameObject tempOBJ = null;
        public TrainUI.TrainMode tempMode;
        public List<GradeDetails> details;

        private List<string> movieName = new List<string>() { "dianlutu1.mp4" };
        //public mode nowMode;
        private List<string> circuitFlows;
        private List<float> circuitScoreFlows;
        private Array AScore;
        private TopicCircuitDBHelper topicDB;
        private Dictionary<string, List<XmlNode>> circuitDic = new Dictionary<string, List<XmlNode>>();
        private BubbleToast alertMSG;
        private int index;
        private int Score = 0;
        private string errorMsg;
        private TLGradeManager gradeManager;
        public void setName(string name)
        {
            nameLable.text = name;
        }
        public override int Depth
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        public void initUI(TrainUI.TrainMode mode)
        {
            if (gradeManager == null)
                gradeManager = new TLGradeManager();
            examCountDown.setTotalSeconds((int)gradeManager.getExamTimeById(2));
            examCountDown.setTimeEndEvent(timeOver);
            if (topicDB == null)
            {
                topicDB = new TopicCircuitDBHelper();
                readXml(Application.streamingAssetsPath + "/XFTrain/MovieTime.xml");
            }
            initCircuitFlows(mode);
            flowOperateMenu.initFlowMenu(circuitFlows, flowOperateMenuClick, mode);
            currentTrainMode = mode;
            popWindow.setYesSwitchClick(changeMode);
            popWindow.setNoSwitchClick(noClick);
            circuitUIStart.SetActive(true);
            info.text = "学员" + H5Camera.GetComponent<UICommunicationsScript>().getUsetName() + "，欢迎来到S700K的学习模式";
            circuitUIIsStart = false;
            tempMode = mode;
            changeMode();
            toolMenu.initToolMenu(toolMenuItemClick);

            if (circuitTopics[circuitFlows[0]].StudyMode == CircuitStudyMode.Image)
                studyImage.gameObject.SetActive(true);
            else if (circuitTopics[circuitFlows[0]].StudyMode == CircuitStudyMode.Movie)
            {
                studyMovie.movie.SetActive(true);
                studyMovie.init();
            }
            else if (circuitTopics[circuitFlows[0]].StudyMode == CircuitStudyMode.ImageMovie)
            {
                studyMovieText.uiPanel.SetActive(true);
                studyMovieText.init();
            }
            else if(circuitTopics[circuitFlows[0]].StudyMode == CircuitStudyMode.Training)
            {
                if (mode == TrainUI.TrainMode.Training)
                {
                    traningMovie.movie.SetActive(true);
                    traningMovie.init();
                }
                else if(mode == TrainUI.TrainMode.Exam)
                {
                    examMovie.movie.SetActive(true);
                    examMovie.init();
                }
            }
        }
        private void initCircuitFlows(TrainUI.TrainMode mode)
        {
            if (mode == TrainUI.TrainMode.Study)
            {
                circuitFlows = topicDB.getCircuitNamesStudy();
                circuitTopics = new Dictionary<string, CircuitTopicNew>();
                foreach (string flow in circuitFlows)
                {
                    circuitTopics.Add(flow, topicDB.getCircuitTopicsNew(flow));
                }
            }
            else if (mode == TrainUI.TrainMode.Training)
            {
                circuitFlows = topicDB.getCircuitNamesTrain();
                circuitTopics = new Dictionary<string, CircuitTopicNew>();
                foreach (string flow in circuitFlows)
                {
                    circuitTopics.Add(flow, topicDB.getCircuitTopicsNew(flow));
                }
            }
            else if (mode == TrainUI.TrainMode.Exam)
            {
                circuitFlows = topicDB.getCircuitNamesExam();
                circuitScoreFlows = topicDB.getCircuitScoreExam();
                circuitTopics = new Dictionary<string, CircuitTopicNew>();
                foreach (string flow in circuitFlows)
                {
                    circuitTopics.Add(flow, topicDB.getCircuitTopicsBack(flow));
                }
            }
           // CircuitTopicNew topic = circuitTopics[circuitFlows[0]];
        }

        /// <summary>
        /// 电路原理开始
        /// </summary>
        public void circuitStart()
        {
            circuitUIStart.SetActive(false);
            if (tempMode == TrainUI.TrainMode.Exam)
            {
                examCountDown.gameObject.SetActive(true);
                examCountDown.onRestart();
            }
            circuitUIIsStart = true;
            
        }
        
        /// <summary>
        /// 切换学习内容
        /// </summary>
        /// <param name="flowIndex"></param>
		public void flowOperateMenuClick(int flowIndex)
        {
            selectObj.gameObject.SetActive(false);
            topic = circuitTopics[circuitFlows[flowIndex]];
                if (topic.StudyMode == CircuitStudyMode.Movie)
                {
                    MovieManager.getInstance().OnStop();
                    studyMovie.movie.SetActive(true);
                    studyMovie.init();
                    studyMovieText.uiPanel.SetActive(false);
                    studyImage.SetActive(false);
                    traningMovie.movie.SetActive(false);
                    examMovie.movie.SetActive(false);
                    studyMovie.playMovie(topic.StudyMovie);
                }
                else if (topic.StudyMode == CircuitStudyMode.ImageMovie)
                {
                    MovieManager.getInstance().OnStop();
                    studyMovie.movie.SetActive(false);
                    studyImage.SetActive(false);
                    traningMovie.movie.SetActive(false);
                    examMovie.movie.SetActive(false);
                    studyMovieText.uiPanel.SetActive(true);
                    studyMovieText.init();
                    studyMovieText.play(topic.StudyMovie, topic.StudyText);

                }
                else if (topic.StudyMode == CircuitStudyMode.Image)
                {
                    studyMovieText.uiPanel.SetActive(false);
                    studyMovie.movie.SetActive(false);
                    traningMovie.movie.SetActive(false);
                    examMovie.movie.SetActive(false);
                    studyImage.SetActive(true);

                }
                else if (topic.StudyMode == CircuitStudyMode.Training)
                {
                if (tempMode == TrainUI.TrainMode.Exam)
                {
                    MovieManager.getInstance().OnStop();
                    studyMovie.movie.SetActive(false);
                    studyImage.SetActive(false);
                    studyMovieText.uiPanel.SetActive(false);
                    traningMovie.movie.SetActive(false);
                    examMovie.movie.SetActive(true);
                    examMovie.init();
                    //Action<CircuitTopicNew> action = examScreen.UIxuanzhejiekous;
                    Action<string> action = showExam;
                    examMovie.playMovie(topic.TrainMovie, action);
                }
                else
                {
                    MovieManager.getInstance().OnStop();
                    studyMovie.movie.SetActive(false);
                    studyImage.SetActive(false);
                    studyMovieText.uiPanel.SetActive(false);
                    examMovie.movie.SetActive(false);
                    traningMovie.movie.SetActive(true);
                    traningMovie.init();                    
                    traningMovie.playMovie(topic.StudyMovie);

                }
            }
            cleanUI();
            index = flowIndex;
            flowOperateMenu.switchFlow(flowIndex);
        }
        
        /// <summary>
        /// show Select Exam
        /// </summary>
        /// <param name="movieName"></param>
        public void showExam(string movieName)
        {
            if (tempMode == TrainUI.TrainMode.Exam)
            {
                ///single count
                /*examCountDown.gameObject.SetActive(true);
                examCountDown.onRestart();
                examCountDown.setTotalSeconds(10);
                int length = circuitFlows.Count;
                if (index < length - 1)
                {
                    examCountDown.setTimeEndEvent(flowOperateMenuClickWithout);
                }
                else
                {
                    gradeDispose();
                }*/
                selectObj.gameObject.SetActive(true);
                selectObj.UIShowOption(topic);
            }
        }

        /// <summary>
        /// choices 
        /// </summary>
        public void selectOption()
        {
            if (UIToggle.current.value)
            {
                UISprite[] result = UIToggle.current.GetComponentsInChildren<UISprite>();
                int length = circuitFlows.Count;
                /*
                foreach (GradeDetails item in details)
                {
                    item.ErrorMsg = errorMsg;
                    item.Topic3DName = item.TopicCircuitName;
                    item.Score = 0;
                }*/
                if (index < length-1)
                {
                    if (("Button" + selectObj.answer) == UIToggle.current.name)
                    {
                        details[index].ErrorMsg = "无";
                        details[index].Score = circuitScoreFlows[index];
                        //details[index].Topic3DName = details[index].TopicCircuitName;
                        result[0].spriteName = "kaohe-xuanzhezhengque";
                        enableButtonOption();
                        Invoke("flowOperateMenuClickWithout", 1.0f);
                        
                    }
                    else
                    {
                        details[index].ErrorMsg = "选择错误";
                        details[index].Score = 0;
                        //details[index].Topic3DName = details[index].TopicCircuitName;
                        result[0].spriteName = "kaohe-xuanzhecuowu";
                        enableButtonOption();
                        Invoke("flowOperateMenuClickWithout", 1.0f);
                    }
                }
                else if(index < length)
                {
                    if (("Button" + selectObj.answer) == UIToggle.current.name)
                    {
                        details[index].ErrorMsg = "无";
                        details[index].Score = circuitScoreFlows[index];
                        //details[index].Topic3DName = details[index].TopicCircuitName;
                        result[0].spriteName = "kaohe-xuanzhezhengque";
                        enableButtonOption();
                        Invoke("gradeDispose", 1.0f);
                    }
                    else
                    {
                        details[index].ErrorMsg = "选择错误";
                        details[index].Score = 0;
                        //details[index].Topic3DName = details[index].TopicCircuitName;
                        result[0].spriteName = "kaohe-xuanzhecuowu";
                        enableButtonOption();
                        Invoke("gradeDispose", 1.0f);
                    }
                    
                }
                else
                {
                    gradeDispose();
                }
            }
            else
            {
                UISprite[] result = UIToggle.current.GetComponentsInChildren<UISprite>();
                result[0].spriteName = "";           
            }
        }

        public void enableButtonOption()
        {
            selectObj.ButtonA.enabled = false;
            selectObj.ButtonB.enabled = false;
            selectObj.ButtonC.enabled = false;
        }

        public void ableButtonOption()
        {
            selectObj.ButtonA.enabled = true;
            selectObj.ButtonB.enabled = true;
            selectObj.ButtonC.enabled = true;
        }


        public void flowOperateMenuClickWithout()
        {
            //examCountDown.gameObject.SetActive(false);
            flowOperateMenuClick(++index);
            ableButtonOption();
        }

        /// <summary>
        /// set time out
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="action"></param>
        public void SetTimeout(double interval, Action<int> action,int index)
        {
            System.Timers.Timer timer = new System.Timers.Timer(interval);
            timer.Elapsed += delegate (object sender, System.Timers.ElapsedEventArgs e)
            {
                timer.Enabled = false;
                action(index);
            };
            timer.Enabled = true;
        }

        public void updateRunTime(Clock c, Clock.Stage s, ulong counter, float clockTime)
        {
            runTime.text = c.UnitConversion();
        }

        public void onTabCilck(GameObject obj)
        {
            popWindow.setActiveSwitchMode(true);
            if (obj.name.Equals("study_tab"))
            {
                tempMode = TrainUI.TrainMode.Study;
            }
            else if (obj.name.Equals("practice_tab"))
            {
                tempMode = TrainUI.TrainMode.Training;
            }
            else if (obj.name.Equals("exam_tab"))
            {
                tempMode = TrainUI.TrainMode.Exam;
            }
        }

        /// <summary>
        /// mode change
        /// </summary>        
        public void changeMode()
        {

            selectObj.gameObject.SetActive(false);
            ouebutton.SetActive(false);
            info.gameObject.SetActive(false);
            examCountDown.onStop();
            cleanUI();
            movieManager.OnStop();
            circuitUIStart.SetActive(true);
            circuitUIIsStart = false;
            initCircuitFlows(tempMode);
            flowOperateMenu.initFlowMenu(circuitFlows, flowOperateMenuClick, tempMode);
            switch (tempMode)
            {
                case TrainUI.TrainMode.Study:
                    info.gameObject.SetActive(true);
                    if (alertMSG != null)
                        Destroy(alertMSG.gameObject);
                    examCountDown.gameObject.SetActive(false);
                    currentTrainMode = TrainUI.TrainMode.Study;
                    
                    coverTab[2].SetActive(false);
                    coverTab[1].SetActive(false);
                    Title.spriteName = "xitongshijianlan-xuexii-bj";
                    titleLabel.text = "学习模式>室外设备>S700K电路原理";
                    coverTab[0].SetActive(true);
                    mask.SetActive(true);
                    maskTop.SetActive(false);
                    maskLeft.SetActive(false);
                    break;
                case TrainUI.TrainMode.Training:
                    currentTrainMode = TrainUI.TrainMode.Training;
                    coverTab[2].SetActive(false);
                    coverTab[0].SetActive(false);
                    Title.spriteName = "xitongshijianlan-lianxi-bj";
                    titleLabel.text = "练习模式>室外设备>S700K电路原理";
                    coverTab[1].SetActive(true);
                    mask.SetActive(true);
                    maskTop.SetActive(false);
                    maskLeft.SetActive(false);
                    break;
                case TrainUI.TrainMode.Exam:
                    ouebutton.SetActive(false);
                    examCountDown.gameObject.SetActive(true);
                   // examCountDown.onRestart();
                    //examCountDown.startTimer();
                    currentTrainMode = TrainUI.TrainMode.Exam;
                    coverTab[0].SetActive(false);
                    coverTab[1].SetActive(false);
                    Title.spriteName = "xitongshijianlan-kaoshi-bj";
                    titleLabel.text = "考试模式>室外设备>S700K电路原理";
                    coverTab[2].SetActive(true);
                    mask.SetActive(true);
                    maskTop.SetActive(true);
                    maskLeft.SetActive(true);
                    TrainingManager.initCircuitUI();
                    details = new List<GradeDetails>(TrainingManager.circuitUIDetails.Values);
                    
                    break;
                default:
                    break;
            }
            flowOperateMenuClick(0);
            flowOperateMenu.initFlowMenu(circuitFlows, flowOperateMenuClick, currentTrainMode);
        }


        public void toolMenuItemClick(string toolName)
        {

        }
        
        /// <summary>
        /// back Button Click
        /// </summary>
        public void backMenu()
        {
            ouebutton.SetActive(false);
            popWindow.setActiveSwitchMode(false);
            circuitUIIsStart = false;
            examGrade.GetComponent<Camera>().depth = -50;
            CoherentUISystem.Instance.UIViews.Remove(examGrade);
            examCountDown.onStop();
            examCountDown.gameObject.SetActive(false);
            movieManager.OnStop();
            UIManager.getInstance().setActiveUI(UIType.MainMenu);
            cleanUI();
        }

        /// <summary>
        /// 显示警告信息
        /// </summary>
        /// <param name="msg"></param>
        public void showAlertMSG(string msg)
        {
            if (msg == null || msg.Equals(""))
            {
                if (!(alertMSG == null || !alertMSG.gameObject.activeSelf))
                    ResourceManager.getInstance().getUIPool.Despawn(alertMSG.transform);
                return;
            }
            if (alertMSG == null || !alertMSG.gameObject.activeSelf)
            {
                GameObject bubbleTip = ResourceManager.getInstance().loadUISpawnPool("Prefabs/UI/MyUI/BubbleToast1", alertMSGParent.gameObject);
                bubbleTip.transform.localPosition = new Vector3(0, 150, 0);
                BubbleToast tip = bubbleTip.GetComponent<BubbleToast>();
                tip.init(Vector2.zero);
                this.alertMSG = tip;
                tip.setContent(msg);
            }
            else
            {
                alertMSG.setContent(msg, true);
            }
        }

        private void readXml(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNodeList movieNodes = doc.SelectSingleNode("root").SelectNodes("circuit");
            foreach (XmlNode node in movieNodes)
            {
                XmlNodeList targets = node.SelectNodes("target");
                List<XmlNode> nodes = new List<XmlNode>();
                foreach (XmlNode no in targets)
                {
                    nodes.Add(no);
                }
                circuitDic.Add(node.Attributes["name"].Value, nodes);
            }
        }

        public void noClick()
        {

        }
        public void pauseMovie()
        {
            if (movieManager._mediaPlayer.Control.IsPlaying())
            {
                movieManager.OnPause();
            }
            else
            {
                movieManager.ResumePlay();
            }
        }
        public void rotateClick()
        {
            flowOperateMenuClick(index);
        }

        /// <summary>
        /// refresh exam 
        /// </summary>
        public void afreshExam()
        {
            examGrade.GetComponent<Camera>().depth = -50;
            CoherentUISystem.Instance.UIViews.Remove(examGrade);
            flowOperateMenuClick(0);
            examCountDown.onRestart();
            TrainingManager.initCircuitUI();
        }

        public void onPopWindowClick(GameObject obj)
        {
            flowOperateMenuClick(Convert.ToInt32(obj.name) - 1);
        }

        public void cleanUI()
        {
            if (alertMSG != null)
                Destroy(alertMSG.gameObject);
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                toolMenu.cancelToolFollow();
            }
        }
        /// <summary>
        /// show the result
        /// </summary>
        private void gradeDispose()
        {
            ableButtonOption();
            if (examCountDown.gameObject.activeSelf)
                examCountDown.gameObject.SetActive(false);
            foreach (GradeDetails item in details)
            {
                if (item.ErrorMsg == null) { 
                    item.ErrorMsg = "未选择";
                    item.Score = 0;
                }
                item.Topic3DName = item.TopicCircuitName;
            }
            for (int k = 0;k < details.Count;k++)
            {
                details[k].Topic3DId = k;
            }
            TLGradeManager tl = gradeManager;
            GradeTable grade = tl.examCircuitUI(examCountDown.getCurrentTime());
            grade.Grade = 0;
            for (int k = 0; k < details.Count; k++)
            {
                grade.Grade += details[k].Score;
            }
             
            string json = "{\"Grade\":" + JsonConvert.SerializeObject(grade) + ",\"Details\":" + JsonConvert.SerializeObject(details) + "}";
            examGrade.GetComponent<Camera>().depth = 50;
            CoherentUISystem.Instance.UIViews.Add(examGrade);
            examGrade.GetComponent<UICommunicationsScript>().UnityToWeb("gotoScoreQuery", json);
        }

        /// <summary>
        /// 考试时间到
        /// </summary>
        public void timeOver()
        {
            gradeDispose();
        }


        /// <summary>
        /// from result to back traning
        /// </summary>
        /// <param name="Sindex"></param>
        public void cause(int Sindex)
        {
            examGrade.GetComponent<Camera>().depth = -50;
            CoherentUISystem.Instance.UIViews.Remove(examGrade);
            tempMode = TrainUI.TrainMode.Training;
            changeMode();
            flowOperateMenuClick(Sindex);            
        }
    }
}

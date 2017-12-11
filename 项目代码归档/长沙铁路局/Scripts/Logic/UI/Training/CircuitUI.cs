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
	public class CircuitUI : UIbase
	{
		public UILabel info;
		public CoherentUIView H5Camera;
		public UILabel runTime;
		public NextProcess popWindow;
		public GameObject[] coverTab = null;
		public GameObject[] Title = null;
		public UITool uitool;
		public FlowOperateMenu flowOperateMenu;
		public GameObject moviePanel;
		public MovieManager movieManager;
		public ToolMenu toolMenu;
		public GameObject hintBtn;
		public MultimeterUI multimeter;
		public UIWidget alertMSGParent;
		public GameObject skipButton;
		public GameObject mask;
		public UILabel movieText;
		private List<string> movieName = new List<string>() { "dianlutu1.mp4" };
		//public mode nowMode;
		public TrainUI.TrainMode currentTrainMode;
		private List<string> circuitFlows = new List<string>() { "图一", "图二", "图三", "图四", "图五", "图六" };
		public Dictionary<string, List<CircuitTopic>> circuitTopics;
		private TopicCircuitDBHelper topicDB;
		private Dictionary<string, List<XmlNode>> circuitDic = new Dictionary<string, List<XmlNode>>();
		private string currentCircuit;
		private BubbleToast alertMSG;
		private int index;
		private bool OnOff;
		private int errorCount = 0;
		private Action errorAction;
		private int Score = 0;
		public CoherentUIView examGrade;
		private string errorMsg;
		public CountDown examCountDown;
		public GameObject circuitUIStart;
		public bool circuitUIIsStart=false;
		private TLGradeManager gradeManager;
		public UILabel nameLable;
		public GameObject ouebutton;
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
			if (mode == TrainUI.TrainMode.Training)
				mode = TrainUI.TrainMode.Study;
			if (topicDB == null)
			{
				topicDB = new TopicCircuitDBHelper();
				readXml(Application.streamingAssetsPath + "/XFTrain/MovieTime.xml");
			}
			initCircuitFlows();
			uitool.setReturnBtnClick(backMenu);
			uitool.setPreBtnClick(OnPreClick);
			uitool.setNextBtnClick(OnNextClick);
			uitool.setPauseBtnClick(pauseMovie);
			uitool.setRatateBtnClick(rotateClick);
			//uitool.setSkipBtnClick(onSkipBtnClick);
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
			hintBtn.SetActive(false);
			multimeter.gameObject.SetActive(false);
			uitool.setShiwaiSliderValue(1.0f / (float)circuitFlows.Count, 1, circuitFlows.Count);

			////test
			//UICommunicationsScript t = examGrade.GetComponent<UICommunicationsScript>();
			////t.ReceiveWeb("{\"Action\":\"updateAllTopic3DScore\",\"param1\":\"20\",\"param2\":\"1\",\"param3\":\"3\"}");
			////t.ReceiveWeb("{\"Action\":\"updateAllTopic2DScore\",\"param1\":\"20\",\"param2\":\"1\",\"param3\":\"7\"}");
			//t.ReceiveWeb("{\"Action\":\"GetStuInfo\",\"param1\":\"1\",\"param2\":\"4\"}");
			//t.ReceiveWeb("{\"Action\":\"DeleteBackup\",\"param1\":\"0\"}");
			//t.ReceiveWeb("{\"Action\":\"SubmitReportDate\",\"param1\":\"3\"}");
		}
		public void playMovieStart()
		{
			playMovie(0);
			circuitUIStart.SetActive(false);
			if (tempMode==TrainUI.TrainMode.Exam)
			{
				examCountDown.gameObject.SetActive(true);
				examCountDown.startTimer();
			}
			circuitUIIsStart = true;
		}
		private void initCircuitFlows()
		{
			circuitTopics = new Dictionary<string, List<CircuitTopic>>();
			foreach (string flow in circuitFlows)
			{
				circuitTopics.Add(flow, topicDB.getCircuitTopics(flow));
			}
		}

		public void flowOperateMenuClick(int flowIndex)
		{
			changgeMenuMovie();
			cleanUI();
			playMovie(flowIndex);
			index = flowIndex;
			flowOperateMenu.switchFlow(index);
			uitool.setShiwaiSliderValue((float)(index + 1) / (float)circuitFlows.Count, index + 1, circuitFlows.Count);
		}
		public void updateRunTime(Clock c, Clock.Stage s, ulong counter, float clockTime)
		{
			runTime.text = c.UnitConversion();
		}

		public void playMovie(int index)
		{
			moviePanel.SetActive(true);
			if (index == 0)
				setMovie(movieName[index], circuitFlows[index]);
			else setMovie("", "");
		}

		private void setMovie(string movieName, string circuit)
		{
			uitool.controlPanel.SetActive(false);
			currentCircuit = circuit;
			movieManager.playMovie(movieName, movieEndAction, false);
			movieManager.SetLooping(false);
		}
		/// <summary>
		/// 视频播放结束回调
		/// </summary>
		/// <param name="name"></param>
		public void movieEndAction(string name)
		{
			if (currentTrainMode == TrainUI.TrainMode.Exam)
			{
				hintBtn.SetActive(true);
			}
		}

		public void onTabCilck(GameObject obj)
		{
			if (multimeter != null)
				multimeter.GetComponent<UIPanel>().enabled = false;
			popWindow.setActiveSwitchMode(true);
			if (obj.name.Equals("study_tab"))
				tempMode = TrainUI.TrainMode.Study;
			else if (obj.name.Equals("exam_tab"))
				tempMode = TrainUI.TrainMode.Exam;
		}
		public TrainUI.TrainMode tempMode;
		public void changeMode()
        {
			ouebutton.SetActive(false);
			info.gameObject.SetActive(false);
			errorCount = 0;
            examCountDown.onStop();
			uitool.setShiwaiSliderValue((float)(1) / (float)circuitFlows.Count, 1, circuitFlows.Count);
			cleanUI();
			movieManager.OnStop();
			circuitUIStart.SetActive(true);
			circuitUIIsStart = false;
			switch (tempMode)
			{
				case TrainUI.TrainMode.Study:
						info.gameObject.SetActive(true);
					if (alertMSG != null)
                        Destroy(alertMSG.gameObject);
                    examCountDown.gameObject.SetActive(false);
					currentTrainMode = TrainUI.TrainMode.Study;
					if (coverTab[coverTab.Length - 1].activeSelf)
					{
						coverTab[coverTab.Length - 1].SetActive(false);
						Title[Title.Length - 1].SetActive(false);
					}
					Title[0].SetActive(true);
					coverTab[0].SetActive(true);
					skipButton.SetActive(false);
					mask.SetActive(true);
					uitool.gameObject.SetActive(true);
					movieText.text = "二维电路图原理动画";
					break;
				case TrainUI.TrainMode.Exam:
					ouebutton.SetActive(true);
					//examCountDown.gameObject.SetActive(true);
					//examCountDown.startTimer();
					currentTrainMode = TrainUI.TrainMode.Exam;
					if (coverTab[0].activeSelf)
					{
						coverTab[0].SetActive(false);
						Title[0].SetActive(false);
					}
					Title[Title.Length - 1].SetActive(true);
					coverTab[coverTab.Length - 1].SetActive(true);
					skipButton.SetActive(true);
					mask.SetActive(false);
					uitool.gameObject.SetActive(false);
					TrainingManager.initCircuitUI();
					movieText.text = "二维电路图考试";
					break;
				default:
					break;
			}
			flowOperateMenu.initFlowMenu(circuitFlows, flowOperateMenuClick, currentTrainMode);
		}

		public void toolMenuItemClick(string toolName)
		{

		}

		public void hintClick()
		{
			toolMenu.cancelToolFollow();
			List<string> valueString = new List<string>(TrainingManager.GetVaraiableStringValue("工具").ToString().Split(','));
			if (valueString.Count == 1)
			{
				string toolName = valueString[0];
				if (toolName.Equals("wanyongbiao"))
				{
                    //if (alertMSG != null)
                    //    Destroy(alertMSG.gameObject);
					multimeter.gameObject.SetActive(true);
					multimeter.setShowNumber(circuitTopics[currentCircuit][0].ShowValue);
					hintBtn.GetComponent<UIButton>().enabled = false;
				}
				else
				{
					error();
					showAlertMSG("操作错误！");
				}
			}
			else
			{
				error();
				showAlertMSG("操作错误！");
			}
			PluginEventManager.FireEvent("RecordTools", "");
		}
		public void backMenu()
		{
			ouebutton.SetActive(false);
			popWindow.setActiveSwitchMode(false);
			circuitUIIsStart = false;
			examGrade.GetComponent<Camera>().depth = -50;
            CoherentUISystem.Instance.UIViews.Remove(examGrade);
            examCountDown.onStop();
			examCountDown.gameObject.SetActive(false);
			multimeter.gameObject.SetActive(false);
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
			if (multimeter != null)
				multimeter.GetComponent<UIPanel>().enabled = true;
		}
		public void OnPreClick()
		{
			index--;
			index = index >= 0 ? index : 0;
			flowOperateMenuClick(index);
		}
		public void OnNextClick()
		{
			index++;
			index = index <= circuitFlows.Count - 1 ? index : circuitFlows.Count - 1;
			flowOperateMenuClick(index);
		}
		public void changgeMenuMovie()
		{
			movieManager.ResumePlay();
			uitool.pauseBtn.spriteName = "shiwaishebei-zanting";
			uitool.pauseBtn.GetComponent<UIButton>().normalSprite = "shiwaishebei-zanting";
		}
		public void pauseMovie()
		{
			if (movieManager._mediaPlayer.Control.IsPlaying())
			{
				movieManager.OnPause();
				uitool.pauseBtn.spriteName = "shiwaishebei-bofang";
				uitool.pauseBtn.GetComponent<UIButton>().normalSprite = "shiwaishebei-bofang";
			}
			else
			{
				movieManager.ResumePlay();
				uitool.pauseBtn.spriteName = "shiwaishebei-zanting";
				uitool.pauseBtn.GetComponent<UIButton>().normalSprite = "shiwaishebei-zanting";
			}
		}
		public void rotateClick()
		{
			flowOperateMenuClick(index);
		}
        public void afreshExam()
		{
			skipButton.SetActive(true);
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
		public void onSkipBtnClick()
		{
			skipButton.SetActive(false);
			if (movieManager._mediaPlayer.Control.IsPlaying())
				movieManager._mediaPlayer.Control.Seek(0.99f * movieManager._mediaPlayer.Info.GetDurationMs());
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		public void getOrCloseClick(GameObject obj)
		{
			if (multimeter.getIsOhm())
			{
				switch (obj.name)
				{
					case "tong":
						circuitTopics[currentCircuit][0].OnOff = true;
						//foreach (GradeDetails grades in TrainingManager.circuitUIDetails.Values)
						//{
						//	Score=int.Parse(grades.Score.ToString());
						//}
						errorMsg = "通断选择正确";
						break;
					case "duan":
						circuitTopics[currentCircuit][0].OnOff = false;
						errorMsg = "通断选择错误";
						errorInfo("通断选择错误");
						break;
					default:
						break;
				}
				gradeDispose();
                if (alertMSG != null)
                    Destroy(alertMSG.gameObject);
				hintBtn.GetComponent<UIButton>().enabled = true;
				multimeter.gameObject.SetActive(false);
				hintBtn.SetActive(false);
			}
			else
			{
				error();
				errorMsg = "万用表档位错误！";
				showAlertMSG("万用表档位错误！");
			}
		}
		private void error()
		{
			Debug.Log("错误了！！！！！！！！！！！");
			errorCount++;
			if (errorCount % 3 == 0 && errorCount > 0)
			{
				errorEvent();
				//if (errorAction != null)
				//{
				//	errorAction();
				//}
				Debug.Log("错误3次了！！！！！！！！！！！");
				gradeDispose();
			}
		}
		//public void setErrorAction(Action errorAction)
		//{
		//	this.errorAction = errorAction;
		//}
		public void errorEvent()
		{
			errorMsg = "使用工具错误";
			errorInfo("使用工具错误");
			multimeter.gameObject.SetActive(true);
			multimeter.setShowNumber(circuitTopics[currentCircuit][0].ShowValue);
			hintBtn.GetComponent<UIButton>().enabled = false;
			multimeter.setCurrentStall(3);
			multimeter.stallSwitch();
			//if (alertMSG != null)
				//Destroy(alertMSG.gameObject);
		}
		public void cleanUI()
		{
			if (multimeter != null)
			{
				multimeter.GetComponent<UIPanel>().enabled = true;
				multimeter.gameObject.SetActive(false);
			}
			if (hintBtn != null)
			{
				hintBtn.SetActive(false);
				hintBtn.GetComponent<UIButton>().enabled = true;
			}
			if (alertMSG != null)
				Destroy(alertMSG.gameObject);
		}
		private void Update()
		{
			if (Input.GetMouseButtonDown(0)|| Input.GetMouseButtonDown(1))
			{
				toolMenu.cancelToolFollow();
			}
		}
		public void outExam()
		{
			errorMsg = "未作答";
			Debug.Log(TrainingManager.circuitUIDetails.Keys);
			TrainingManager.circuitUIDetails[circuitTopics[currentCircuit][0].TopicContent].Score = 0;
			gradeDispose();
		}
		private void setScore()
		{
			foreach (GradeDetails grades in TrainingManager.circuitUIDetails.Values)
			{
				grades.Score = Score;
			}
		}
		private void gradeDispose()
		{
			if (examCountDown.gameObject.activeSelf)
				examCountDown.gameObject.SetActive(false);
			TLGradeManager tl = new TLGradeManager();
			GradeTable grade = tl.examCircuitUI(examCountDown.getCurrentTime());
			List<GradeDetails> details = new List<GradeDetails>(TrainingManager.circuitUIDetails.Values);
			foreach (GradeDetails item in details)
			{
				item.ErrorMsg = errorMsg;
				item.Topic3DName = item.TopicCircuitName;
			}
			string json = "{\"Grade\":" + JsonConvert.SerializeObject(grade) + ",\"Details\":" + JsonConvert.SerializeObject(details) + "}";
			Debug.Log(json);
			examGrade.GetComponent<Camera>().depth = 50;
			CoherentUISystem.Instance.UIViews.Add(examGrade);
			examGrade.GetComponent<UICommunicationsScript>().UnityToWeb("gotoScoreQuery", json);
		}
		private void errorInfo(string errorMsg)
		{
			this.errorMsg = errorMsg;
			TrainingManager.circuitUIDetails[circuitTopics[currentCircuit][0].TopicContent].Score = 0;
		}
		private void timeOver()
		{
			errorInfo("考试时间结束");
			gradeDispose();
		}
		public void cause()
		{
			examGrade.GetComponent<Camera>().depth = -50;
			CoherentUISystem.Instance.UIViews.Remove(examGrade);
			flowOperateMenuClick(0);
			//examCountDown.onRestart();
			tempMode = TrainUI.TrainMode.Study;
			changeMode();
			playMovieStart();
		}
	}
}

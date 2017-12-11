using Assets.LSE.TL;
using Assets.LSE.XF;
using Assets.Scripts.Configs;
using Assets.Scripts.Controller;
using Assets.Scripts.Logic.Movies;
using Assets.Scripts.Logic.Scenes;
using Assets.Scripts.Logic.UI.CommonUI;
using Assets.Scripts.Tools;
using Assets.Scripts.VO;
using Coherent.UI;
using LitJson;
using Newtonsoft.Json;
using PluginEvent;
using System;
using System.Collections;
using System.Collections.Generic;
using Training;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:S700K设备拆解、设备组装、设备巡视、设备检修界面UI逻辑
*Author:作者
*/
namespace Assets.Scripts.Logic.UI.Training
{
	public class TrainUI : UIbase
	{
		public enum TrainMode
		{
			Study = 0,
			Training = 1,
			Exam = 2
		}
		public UITool uitool;
		public UISprite topNavigateBackground;
		public UILabel topNavigateLabel;
		public UISprite userAvatar;
		public UILabel userName;
		public UILabel userOnLineTime;
		public UISprite userTrainMode;
		public TabStrip tabStrip;
		public FlowOperateMenu flowOperateMenu;
		public DynamicListUI dynamiclistUI;
		public UISprite toolMenuTitle;
		public ToolMenu toolMenu;

		public UIWidget alertMSGParent;
		public UIButton backSuperNodeBtn;
		public UIButton helpBtn;
		public UISprite trainStartUI;
		public UIButton startBtn;
		public UIButton startOkBtn;
		public UISprite trainEndUI;
		public UIPanel dialog;
		public NextProcess nextProcess;
		public CoherentUIView examGrade;
		public CoherentUIView H5Camera;
		public CountDown examCountDown;
		public MovieManager movieManager;
		public GameObject flowOperateImage;
		public GameObject flowOperateMovie;
		public GameObject flowMovie;
		public GameObject buttomUI;

		private BubbleToast alertMSG;
		private string currentTrainFlow;
		private bool isWalkCamea = false;
		private MyMouseFocus _mouseFocus;
		private ReciprocalModel currentReciprocal;

		private int errorCount;
		public TrainMode currentTrainMode;
		private string currentEquipName;
		private int indexNumber;
		private int count;
		public UISprite[] coverTabs = null;
		private int changeCount;
		public int index;
		private bool isBackMenu = false;
		public GameObject showIcon;
		private TLGradeManager gradeManager;
		public UILabel info;
		public PopWindow popwindow;
		private int selectTopicIndex;
		private int examNumber;
		public UILabel nameLable;
		public GameObject outExamBtn;
		public void setName(string name)
		{
			nameLable.text = name;
		}
		#region 初始化操作
		/// <summary>
		/// 初始化培训场景UI
		/// </summary>
		public void initTrainUI(string equipName, string flow, TrainMode trainMode)
		{
			//tabStrip.init(1, tabStripClick);
			//tabStrip.setTabActiveState(1);
			currentEquipName = equipName;
			this.currentTrainFlow = flow;
			this.currentTrainMode = trainMode;
			index = (int)currentTrainMode;
			dynamiclistUI.reset();
			dynamiclistUI.resetPostion();
			flowOperateMenu.initFlowMenu(flowOperateMenuClick, currentTrainMode);
			initToolMenu();
			toolMenu.canelHindTool();
			uitool.setPauseBtnClick(pauseMovie);
			uitool.setReturnBtnClick(backMenu);
			uitool.setRatateBtnClick(afreshTrain);
			uitool.setNextBtnClick(setNextClick);
			uitool.setPreBtnClick(setBackClick);
			nextProcess.setNoSwitchClick(noBtnClick);
			nextProcess.setYesSwitchClick(yesBtnClick);
			count = flowOperateMenu.getflows().Count;
			if (gradeManager == null)
				gradeManager = new TLGradeManager();
			ExamSubject subject = gradeManager.getExamSubject(flow);
			if (!subject.IsExam)
				tabStrip.setTabDisableState(2);
			//isExam = false;
			//initTrainMode(trainMode);
			popwindow.init();
		}
		public void initUI(string equipName, string flow, TrainMode trainMode)
		{
			currentEquipName = equipName;
			this.currentTrainFlow = flow;
			this.currentTrainMode = trainMode;
			flowOperateMenu.initFlowMenu(flowOperateMenuClick, currentTrainMode);
		}

		public void initTrainMode(TrainMode trainMode)
		{
			string topNavigateStr = "";
			switch (trainMode)
			{
				case TrainMode.Study:
					topNavigateStr += "学习模式 > ";
					topNavigateBackground.spriteName = "xitongshijianlan-xuexii-bj";
					break;
				case TrainMode.Training:
					topNavigateStr += "练习模式 > ";
					topNavigateBackground.spriteName = "xitongshijianlan-lianxi-bj";
					break;
				case TrainMode.Exam:
					topNavigateStr += "考试模式 > ";
					topNavigateBackground.spriteName = "xitongshijianlan-kaoshi-bj";
					break;
			}
			topNavigateStr += "室外模式 > ";
			topNavigateStr += currentEquipName + currentTrainFlow;
			topNavigateLabel.text = topNavigateStr;
		}

		private void initToolMenu()
		{
			tabStrip.init(1, tabStripClick, Convert.ToInt32(currentTrainMode));
			tabStripClick((int)currentTrainMode);
			toolMenu.initToolMenu(toolMenuItemClick);
		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		public void tabStripClick(int index)
		{
			nextProcess.setActiveForNextProcessPanel(false);
			foreach (var item in coverTabs)
			{
				item.gameObject.SetActive(false);
			}
			if (isBackMenu)
			{
				if (changeCount == 0)
				{
					coverTabs[(int)currentTrainMode].gameObject.SetActive(true);
					switchTabs(index);
					changeCount++;
				}
				else
				{
					coverTabs[(int)currentTrainMode].gameObject.SetActive(true);
					this.index = index;
					ShowSwitchMode();
					changeCount++;
					isBackMenu = false;
				}
			}
			else if (!isBackMenu)
			{
				if (changeCount <= 1)
				{
					coverTabs[(int)currentTrainMode].gameObject.SetActive(true);
					switchTabs(index);
					changeCount++;
				}
				else
				{
					coverTabs[(int)currentTrainMode].gameObject.SetActive(true);
					this.index = index;
					ShowSwitchMode();
				}
			}
		}

		public void updateRunTime(Clock c, Clock.Stage s, ulong counter, float clockTime)
		{
			userOnLineTime.text = c.UnitConversion();
		}

		/// <summary>
		/// 流程菜单流程选项点击事件
		/// </summary>
		/// <param name="flowIndex"></param>
		public void flowOperateMenuClick(int flowIndex)
		{
			uitool.controlPanel.SetActive(true);
			changgeMenuMovie();
			UIManager.getInstance().selectTopicUI.mask.SetActive(false);
			errorCount = 0;
			if (flowMovie.activeSelf)
			{
				flowMovie.SetActive(false);
				movieManager.OnStop();
			}
			flowOperateImage.SetActive(false);
			flowOperateMovie.SetActive(false);
			UIManager.getInstance().selectTopicUI.gameObject.SetActive(false);
			indexNumber = flowIndex;
			uitool.setShiwaiSliderValue(Convert.ToSingle(indexNumber + 1) / Convert.ToSingle(count), indexNumber + 1, count);
			if (this.currentTrainMode == TrainMode.Training)
				PluginEventManager.FireEvent("StartPlayFlow", this.CurrentTrainFlow + ",半自动浏览," + flowIndex);
			else if (this.currentTrainMode == TrainMode.Study)
				PluginEventManager.FireEvent("StartPlayFlow", this.CurrentTrainFlow + ",自动浏览," + flowIndex);
			else if (this.currentTrainMode == TrainMode.Exam)
				PluginEventManager.FireEvent("StartPlayFlow", this.CurrentTrainFlow + ",考核," + flowIndex);
			trainEndUI.gameObject.SetActive(false);
		}
		/// <summary>
		/// 工具栏工具点击事件
		/// </summary>
		/// <param name="toolName"></param>
		public void toolMenuItemClick(string toolName)
		{

		}

		/// <summary>
		/// 返回上级节点
		/// </summary>
		public void backSuperNode()
		{
			if (this.currentReciprocal.Reciprocal.Count > 0)
				this.currentReciprocal.ModelObject.GetComponent<BoxCollider>().enabled = true;

			this.currentReciprocal = null;
			backSuperNodeBtn.gameObject.SetActive(false);
			mouseFocus.ChangeCamera(false);
			this.isWalkCamea = true;
		}

		public void cancelBackMenu()
		{
			dialog.gameObject.SetActive(false);
			isFirstBack = true;
		}
		bool isFirstBack = true;
		/// <summary>
		/// 退出培训流程，返回到菜单界面
		/// </summary>
		public void backMenu()
		{
			outExamBtn.SetActive(false);
			uitool.controlPanel.SetActive(true);
			UIManager.getInstance().selectTopicUI.mask.SetActive(false);
			errorCount = 0;
			if (!(alertMSG == null || !alertMSG.gameObject.activeSelf))
				ResourceManager.getInstance().getUIPool.Despawn(alertMSG.transform);
			if (flowMovie.activeSelf)
			{
				flowMovie.SetActive(false);
				movieManager.OnStop();
			}
			flowOperateImage.SetActive(false);
			flowOperateMovie.SetActive(false);
			examCountDown.onStop();
			examCountDown.gameObject.SetActive(false);
			trainEndUI.gameObject.SetActive(false);
			isBackMenu = true;
			//tabStrip.tabs[(int)currentTrainMode].GetComponent<UIToggle>().value = false;
			for (int i = 0; i < coverTabs.Length; i++)
			{
				tabStrip.tabs[i].Set(false);
			}
			if (nextProcess.switchMode.activeSelf)
			{
				nextProcess.switchMode.SetActive(false);
			}
			changeCount = 0;
			uitool.setTextl(null);
			uitool.setPopText(null);
			tabStrip.restTabStrip();
			indexNumber = -1;
			uitool.setShiwaiSliderValue(Convert.ToSingle(indexNumber + 1) / Convert.ToSingle(count), indexNumber + 1, count);
			TrainingManager.ExitFlow();

			//音乐调节
			AudioManager.getInstance().SetGroundScale(1f);
			AudioManager.getInstance().AudioPlay(GlobalConfig.backgroundMusic, AudioManager.MusicNumType.groundMusic);
			AudioManager.getInstance().AudioStop(AudioManager.MusicNumType.realtimeMusic);//返回的时候停止实时音乐

			SceneManager.LoadScene("MainUI");
			trainEndUI.gameObject.SetActive(false);
			trainStartUI.gameObject.SetActive(false);
			isShowTrainStart = false;
			isStartTrain = false;
			this.currentReciprocal = null;

			this.isWalkCamea = true;
			backSuperNodeBtn.gameObject.SetActive(false);
			this.isStartTrain = false;
			UIManager.getInstance().selectTopicUI.gameObject.SetActive(false);
			ScreenRaycaster raycaster = GameObject.Find("FingerGestures").GetComponent<ScreenRaycaster>();
			raycaster.Cameras = new Camera[1];
			raycaster.Cameras[0] = GameObject.FindObjectOfType<Main>().GetComponent<Camera>();
			UIManager.getInstance().setActiveUI(UIType.MainMenu);
			examGrade.GetComponent<Camera>().depth = -50;
			CoherentUISystem.Instance.UIViews.Remove(examGrade);
		}

		/// <summary>
		/// 定位物体
		/// </summary>
		public void LocatObjects()
		{
			if (TrainingComponent.waitForUserInteraction && TrainingManager.getTrainingOperateState() != null)
			{
				GameObject obj = null;
				if (!InitConfigs.Instance.ReciprocalModelDic.ContainsKey(TrainingManager.getTrainingOperateState().target.targets[0]))
				{
					bool flag = false;
					foreach (ReciprocalModel model in InitConfigs.Instance.ReciprocalModelDic.Values)
					{
						if (model.Reciprocal.ContainsKey(TrainingManager.getTrainingOperateState().target.targets[0]))
						{
							obj = GameObject.Find(model.Model);
							flag = true;
							break;
						}
					}
					if (!flag)
						return;
				}
				else
					obj = GameObject.Find(TrainingManager.getTrainingOperateState().target.targets[0]);
				if (this.currentReciprocal == null || !obj.name.Equals(currentReciprocal.Model))
					clickEvent(obj);
			}
		}

		void Update()
		{
			if (currTime != -1)
			{
				currTime += Time.deltaTime;
				if (currTime % 0.6f < 0.3f)
					PluginEventManager.FireEvent("ModelEffect", TrainingManager.getTrainingOperateState().target.targets[0] + ",0,0,0");
				else if (currTime % 0.6f < 0.6f)
					PluginEventManager.FireEvent("ModelEffect", TrainingManager.getTrainingOperateState().target.targets[0] + ",1,0,0");

				if (currTime > 2)
				{
					PluginEventManager.FireEvent("ModelEffect", TrainingManager.getTrainingOperateState().target.targets[0] + ",1,0,0");
					currTime = -1;
					myLocatObject(true);
				}
			}
		}

		float currTime = -1;
		public void myLocatObject(bool isClick = false)
		{
			GameObject obj = null;
			if (!InitConfigs.Instance.ReciprocalModelDic.ContainsKey(TrainingManager.getTrainingOperateState().target.targets[0]))
			{
				bool flag = false;
				foreach (ReciprocalModel model in InitConfigs.Instance.ReciprocalModelDic.Values)
				{
					if (model.Reciprocal.ContainsKey(TrainingManager.getTrainingOperateState().target.targets[0]))
					{
						obj = GameObject.Find(model.Model);
						flag = true;
						break;
					}
				}
				if (!flag)
					return;
			}
			else
				obj = GameObject.Find(TrainingManager.getTrainingOperateState().target.targets[0]);
			if (this.currentReciprocal == null || !obj.name.Equals(currentReciprocal.Model))
			{
				clickEvent(obj);
				currTime = 0;
			}
			else
			{
				if (isClick)
					clickEvent(GameObject.Find(TrainingManager.getTrainingOperateState().target.targets[0]));
				else currTime = 0;
			}
		}

		/// <summary>
		/// 显示流程结束
		/// </summary>
		public void showTrainEnd()
		{
			if (currentTrainMode == TrainMode.Exam)
			{
				gradeDispose();
			}
			else
				trainEndUI.gameObject.SetActive(true);
		}

		public void hideTrainEnd()
		{
			trainEndUI.gameObject.SetActive(false);
			//退出流程
			backMenu();
		}

		bool isShowTrainStart = false;
		public void showTrainStart()
		{
			mouseFocus.cancelHighLightObj();
			mouseFocus.reset();
			if (!isShowTrainStart)
			{
				isShowTrainStart = true;
				trainStartUI.gameObject.SetActive(true);
				//播放帮助文档的语音：
				//AudioManager.getInstance().AudioPlay("XF_yuyin/kemu1/" + "yuyin_bangzhu", AudioManager.MusicNumType.realtimeMusic);
				if (currentTrainMode == TrainMode.Study)
					info.text = "学员" + H5Camera.GetComponent<UICommunicationsScript>().getUsetName() + "，欢迎来到S700K的学习模式";
				if (currentTrainMode == TrainMode.Exam)
				{
					examCountDown.gameObject.SetActive(true);
					examCountDown.setTotalSeconds((int)gradeManager.getExamTimeById(examNumber)); //this is exam Time
					examCountDown.setTimeEndEvent(timeOver);
				}
				else examCountDown.gameObject.SetActive(false);

				if (!isStartTrain)
				{
					mouseFocus.ForbiddenWalk(false);
					startBtn.gameObject.SetActive(true);
					startOkBtn.gameObject.SetActive(false);
				}
				else
				{
					startBtn.gameObject.SetActive(false);
					startOkBtn.gameObject.SetActive(true);
				}
			}
		}

		public bool isStartTrain = false;
		public void hideTrainStart()
		{
			if (!dynamiclistUI.getIsShow())
				dynamiclistUI.showSpInfoUI();
			mouseFocus.cancelHighLightObj();
			mouseFocus.reset();
			isShowTrainStart = false;
			trainStartUI.gameObject.SetActive(false);
			if (!isStartTrain)
			{
				mouseFocus.ForbiddenWalk(true);

				TrainingBigFlow trainingFlow = TrainingManager.flows[this.CurrentTrainFlow];
				addHintMSG(trainingFlow.operateHint);
				if (currentTrainMode == TrainMode.Study)
					PluginEventManager.FireEvent("StartPlayFlow", this.CurrentTrainFlow + ",自动浏览,0");//自动浏览
				else if (currentTrainMode == TrainMode.Training)
					PluginEventManager.FireEvent("StartPlayFlow", this.CurrentTrainFlow + ",半自动浏览,0");//半自动浏览
				else if (currentTrainMode == TrainMode.Exam)
				{
					PluginEventManager.FireEvent("StartPlayFlow", this.CurrentTrainFlow + ",考核,0");//考核
					examCountDown.startTimer();
				}
				isStartTrain = true;
			}
		}

		/// <summary>
		/// 场景中点击事件
		/// </summary>
		/// <param name="obj"></param>
		public void clickEvent(GameObject obj)
		{
			if (currentReciprocal == null)
				return;
			if (this.isWalkCamea)
			{
				if (!InitConfigs.Instance.ReciprocalModelDic.ContainsKey(obj.name))
					return;

				ReciprocalModel reciprocal = InitConfigs.Instance.ReciprocalModelDic[obj.name];
				//设置当前聚焦对象，如果是组对象，取消碰撞
				if (InitConfigs.Instance.ReciprocalModelDic[obj.name].Reciprocal.Count > 0)
					obj.GetComponent<BoxCollider>().enabled = false;

				this.currentReciprocal = reciprocal;
				this.currentReciprocal.ModelObject = obj;

				this.isWalkCamea = false;
				//backSuperNodeBtn.gameObject.SetActive(true);
				mouseFocus.ChangeCamera(true);


				if (reciprocal.LimitYaw != Vector2.zero)
					mouseFocus.setLimit(reciprocal.LimitYaw, reciprocal.LimitPitch, reciprocal.LimitDistance);
				mouseFocus.ChangeTarget(reciprocal.Model, reciprocal.Offset.x, reciprocal.Offset.y, reciprocal.Offset.z);
			}
			else
			{
				///判断点击的物体是否是当前组对象，是组内对象则直接点击操作
				///不是组内对象，则恢复当前组对象的碰撞，摄像机聚焦该对象，返回键时
				///如果当前是聚焦的是组对象时，恢复组碰撞

				//PluginEventManager.FireEvent("RecordTools", TrainingOperateRequirement.conditions[TrainingManager.getTrainingOperate().states[0].requirement.formulars[0]]);

				if (obj == this.currentReciprocal.ModelObject || this.currentReciprocal.Reciprocal.ContainsKey(obj.name))
				{
					if (TrainingComponent.waitForUserInteraction && TrainingManager.getTrainingOperateState() != null)
					{
						if (!TrainingManager.MouseLeftClick(obj.name))
							clickError();
						else
						{
							errorCount = 0;
							if (currentTrainMode == TrainMode.Exam)
								TrainingManager.gradeDetails[TrainingManager.getTrainingOperate().name].ErrorMsg = "操作正确！";
						}
					}
					else if (currentTrainMode != TrainMode.Study)
					{
						showAlertMSG("未到交互时间");
						Debug.LogError("未到交互时间");
					}
				}
				else
				{
					if (!InitConfigs.Instance.ReciprocalModelDic.ContainsKey(obj.name))
						return;

					if (this.currentReciprocal.Reciprocal.Count > 0)
						this.currentReciprocal.ModelObject.GetComponent<BoxCollider>().enabled = true;

					ReciprocalModel reciprocal = InitConfigs.Instance.ReciprocalModelDic[obj.name];
					this.currentReciprocal = reciprocal;
					this.currentReciprocal.ModelObject = obj;

					if (InitConfigs.Instance.ReciprocalModelDic[obj.name].Reciprocal.Count > 0)
						obj.GetComponent<BoxCollider>().enabled = false;

					if (reciprocal.LimitYaw != Vector2.zero)
						mouseFocus.setLimit(reciprocal.LimitYaw, reciprocal.LimitPitch, reciprocal.LimitDistance);
					mouseFocus.ChangeTarget(reciprocal.Model, reciprocal.Offset.x, reciprocal.Offset.y, reciprocal.Offset.z);
				}
			}
		}
		/// <summary>
		/// 高亮物体
		/// </summary>
		/// <param name="obj"></param>
		public void mouseoverSceneEvent(GameObject obj)
		{
			if (this.currentReciprocal != null && this.currentReciprocal.Reciprocal.ContainsKey(obj.name))
			{
				HighLightObj(obj);
			}
			else
			{
				if (!InitConfigs.Instance.ReciprocalModelDic.ContainsKey(obj.name))
					return;
				HighLightObj(obj);
			}
		}

		private GameObject currentChangeColor;
		public void mouseDownEvent(GameObject obj)
		{
			if (this.currentReciprocal != null && this.currentReciprocal.Reciprocal.ContainsKey(obj.name))
			{
				currentChangeColor = obj;
				currentChangeColor.GetComponent<HighlightableObject>().ConstantParams(new Color(10f / 255f, 10f / 255f, 10f / 255f));
			}
		}

		public void mouseUpEvent()
		{
			if (currentChangeColor != null)
			{
				currentChangeColor.GetComponent<HighlightableObject>().ConstantParams(Color.red);
			}
		}
		/// <summary>
		/// 选择题回答之后的回调方法
		/// </summary>
		/// <param name="option"></param>
		public void selectHandle(SelectTopic selectTopic, Option option)
		{
			if (UIManager.getInstance().selectTopicUI.checkResult(TrainingManager.getTrainingOperateState().target.selectTopicsIndex, option.value))
			{
				errorCount = 0;
				if (currentTrainMode == TrainMode.Exam)
					TrainingManager.gradeDetails[TrainingManager.getTrainingOperate().name].ErrorMsg = "操作正确！";
				TrainingManager.selectTopicResult(selectTopic.index, true);
				UIManager.getInstance().selectTopicUI.gameObject.SetActive(false);
				showAlertMSG("");
			}
			else
			{
				//选择错误   区分培训和考核的不同处理方式
				showAlertMSG("选择错误");
				TrainingManager.selectTopicResult(selectTopic.index, false);
				clickError();
				TrainingManager.gradeDetails[TrainingManager.getTrainingOperate().name].ErrorMsg = "选择错误！";
			}
		}

		/// <summary>
		/// 步骤提示信息
		/// </summary>
		/// <param name="msg"></param>
		public void addHintMSG(string msg)
		{
			dynamiclistUI.addItem(msg);
		}
		public void updateHintMSG(string msg)
		{
			dynamiclistUI.updateItemLast(msg);
		}
		public void showSelectTopic(int index)
		{
			selectTopicIndex = index;
			UIManager.getInstance().selectTopicUI.showSelectTopicUI(index, selectHandle);
		}

		/// <summary>
		/// 电影视频动画播放完毕回调函数
		/// </summary>
		/// <param name="msg"></param>
		public void movieCallback(string msg)
		{
			Debug.Log("电影视频动画播放完毕回调函数" + msg);
			TrainingManager.getTrainingOperate().video.movieOver();
		}

		/// <summary>
		/// 音乐播放完毕回调函数
		/// </summary>
		/// <param name="msg"></param>
		public void musicCallback(string msg)
		{
			Debug.Log("音乐播放完毕回调函数" + msg);
		}

		public string CurrentTrainFlow
		{
			get { return currentTrainFlow; }
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
				alertMSG.setContent(msg, false);
			}
		}

		public void HighLightObj(GameObject equip, MyMouseFocus.LightMode light = MyMouseFocus.LightMode.light)
		{
			List<string> equips = new List<string>();

			if (equip.transform.childCount == 0)
				equips.Add(equip.name);
			else
				getAllChildTransform(equip.transform, equips);

			string names = equips[0];
			for (int i = 1; i < equips.Count; i++)
			{
				names += "," + equips[i];
			}
			mouseFocus.HighLightObj(names, light);
		}

		private void getAllChildTransform(Transform tr, List<string> trs)
		{
			if (tr.GetComponent<Renderer>() == null)
			{
				if (tr.childCount == 0)
					trs.Add(tr.name);
				else
				{
					foreach (Transform mytr in tr.transform)
					{
						getAllChildTransform(mytr, trs);
					}
				}
			}
			else
				trs.Add(tr.name);
		}

		private MyMouseFocus mouseFocus
		{
			get
			{
				if (_mouseFocus == null)
				{
					_mouseFocus = GameObject.FindObjectOfType<MyMouseFocus>();
					return _mouseFocus;
				}
				else return _mouseFocus;
			}
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

		public void changeCamera(string deviceName)
		{
			if (!InitConfigs.Instance.ReciprocalModelDic.ContainsKey(deviceName))
				return;
			ReciprocalModel reciprocal = InitConfigs.Instance.ReciprocalModelDic[deviceName];
			//设置当前聚焦对象，如果是组对象，取消碰撞
			GameObject obj = GameObject.Find(deviceName);
			if (InitConfigs.Instance.ReciprocalModelDic[deviceName].Reciprocal.Count > 0)
				obj.GetComponent<BoxCollider>().enabled = false;

			this.currentReciprocal = reciprocal;
			this.currentReciprocal.ModelObject = obj;
			this.isWalkCamea = false;
			mouseFocus.ChangeCamera(true);

			if (reciprocal.LimitYaw != Vector2.zero)
				mouseFocus.setLimit(reciprocal.LimitYaw, reciprocal.LimitPitch, reciprocal.LimitDistance);
			mouseFocus.ChangeTarget(reciprocal.Model, reciprocal.Offset.x, reciprocal.Offset.y, reciprocal.Offset.z);
		}

		//更新菜单栏信息，更新弹出菜单栏信息
		public void updateTrainingFlow(int index)
		{
			indexNumber = index;
			uitool.setShiwaiSliderValue(Convert.ToSingle(indexNumber + 1) / Convert.ToSingle(count), indexNumber + 1, count);
			flowOperateMenu.switchFlow(index);
			dynamiclistUI.reset();
			dynamiclistUI.updateTitle((index + 1).ToString().PadLeft(2, '0'), TrainingManager.getCurrentTrainingBigFow().getTrainingFlow(index).name);
		}
		public void afreshTrain()
		{
			examGrade.GetComponent<Camera>().depth = -50;
			CoherentUISystem.Instance.UIViews.Remove(examGrade);
			flowOperateMenuClick(indexNumber);
			Debug.Log(indexNumber);
		}

		public void afreshExam()
		{
			examGrade.GetComponent<Camera>().depth = -50;
			CoherentUISystem.Instance.UIViews.Remove(examGrade);
			flowOperateMenuClick(0);
			examCountDown.onRestart();
			TrainingManager.resetGradeDetails();
		}

		private void setNextClick()
		{
			if (indexNumber == count)
			{
				return;
			}
			flowOperateMenuClick(++indexNumber);
		}
		private void setBackClick()
		{
			if (indexNumber == 0)
			{
				return;
			}
			flowOperateMenuClick(--indexNumber);
		}

		/// <summary>
		/// 错误操作处理
		/// </summary>
		private void clickError()
		{
			errorCount++;
			if (errorCount == 3)
			{
				errorCount = 0;
				if (currentTrainMode == TrainMode.Exam)
				{
					TrainingManager.gradeDetails[TrainingManager.getTrainingOperate().name].Score = 0;
					TrainingManager.gradeDetails[TrainingManager.getTrainingOperate().name].ErrorMsg = "工具或物体交互操作错误！";
				}
				Debug.Log("错三次了！！");
				StartCoroutine(errorDispose());
			}
			else
			{
				showAlertMSG("操作错误！");
			}
		}

		IEnumerator errorDispose()
		{
			float time = 5;
			float errorTime = 3;
			changeCamera(TrainingManager.getTrainingOperateState().target.targets[0]);
			//界面显示正确的工具物品
			showAlertMSG("操作错误三次，自动操作！");
			if (!TrainingComponent.currState.target.isSelectTopics)
			{
				setActiveIcon(true);
			}
			if (UIManager.getInstance().selectTopicUI.gameObject.activeSelf)
			{
				UIManager.getInstance().selectTopicUI.mask.SetActive(true);
				showAlertMSG("正确答案为" + UIManager.getInstance().selectTopicUI.getSelectTopicDic()[selectTopicIndex].result);
				while (errorTime > 0)
				{
					errorTime -= Time.deltaTime;
					yield return null;
				}
				UIManager.getInstance().selectTopicUI.gameObject.SetActive(false);
				UIManager.getInstance().selectTopicUI.mask.SetActive(false);
				setActiveIcon(false);
				showAlertMSG("");
			}
			while (time > 0)
			{
				time -= Time.deltaTime;
				yield return null;
			}
			if (TrainingManager.getTrainingOperateState() != null)
			{
				if (TrainingManager.getTrainingOperateState().requirement.formulars.Count > 0)
					PluginEventManager.FireEvent("RecordTools", TrainingManager.getTrainingOperateState().requirement.formulars[0].formulars[0]);
				else PluginEventManager.FireEvent("RecordTools", "");
			}
			TrainingManager.MouseLeftClick(TrainingManager.getTrainingOperateState().target.targets[0]);
			TrainingComponent.PlayNextComponent();
		}
		/// <summary>
		/// 考试时间到
		/// </summary>
		public void timeOver()
		{
			foreach (GradeDetails grade in TrainingManager.gradeDetails.Values)
			{
				if (grade.ErrorMsg == null || grade.ErrorMsg == "")
				{
					grade.Score = 0;
					grade.ErrorMsg = "考试时间已到！";
				}
			}
			gradeDispose();
		}
		public void outExam()
		{
			foreach (GradeDetails item in TrainingManager.gradeDetails.Values)
			{
				if (item.ErrorMsg==null)
				{
					item.ErrorMsg = "未作答";
				}
			}
			gradeDispose();
		}
		private void gradeDispose()
		{
			if (examCountDown.gameObject.activeSelf)
				examCountDown.gameObject.SetActive(false);
			TLGradeManager tl = new TLGradeManager();
			GradeTable grade = tl.exam3DOver(examCountDown.getCurrentTime());
			examCountDown.onStop();
			List<GradeDetails> details = new List<GradeDetails>(TrainingManager.gradeDetails.Values);
			string json = "{\"Grade\":" + JsonConvert.SerializeObject(grade) + ",\"Details\":" + JsonConvert.SerializeObject(details) + "}";

			examGrade.GetComponent<Camera>().depth = 50;
			CoherentUISystem.Instance.UIViews.Add(examGrade);
			examGrade.GetComponent<UICommunicationsScript>().UnityToWeb("gotoScoreQuery", json);
		}

		public void yesBtnClick()
		{
			foreach (var item in coverTabs)
			{
				item.gameObject.SetActive(false);
			}
			TrainingComponent.FlowEnd(FlowEndType.Complete);
			switchTabs(index);
			indexNumber = 0;
			uitool.setShiwaiSliderValue(Convert.ToSingle(indexNumber + 1) / Convert.ToSingle(count), indexNumber + 1, count);
		}
		public void noBtnClick()
		{
			for (int i = 0; i < coverTabs.Length; i++)
			{
				tabStrip.tabs[i].Set(false);
			}
		}
		private void ShowSwitchMode()
		{
			nextProcess.switchMode.SetActive(true);
		}
		public void switchTabs(int index)
		{
			outExamBtn.SetActive(false);
			info.gameObject.SetActive(false);
			setActiveIcon(false);
			if (flowMovie.activeSelf)
			{
				flowMovie.SetActive(false);
				movieManager.OnStop();
			}
			flowOperateImage.SetActive(false);
			flowOperateMovie.SetActive(false);
			UIManager.getInstance().selectTopicUI.gameObject.SetActive(false);
			if (!(alertMSG == null || !alertMSG.gameObject.activeSelf))
				ResourceManager.getInstance().getUIPool.Despawn(alertMSG.transform);
			examCountDown.onStop();
			TrainingTwinkle.removeHighlight();
			errorCount = 0;
			AudioManager.getInstance().AudioStop(AudioManager.MusicNumType.realtimeMusic);
			toolMenu.canelHindTool();
			switch (index)
			{
				case 0:
					info.gameObject.SetActive(true);
					setActiveIcon(true);
					uitool.gameObject.SetActive(true);
					initTrainMode(TrainMode.Study);
					dynamiclistUI.initDynamicListUI(TrainMode.Study);
					initUI(currentEquipName, currentTrainFlow, TrainMode.Study);
					tabStrip.tabs[1].GetComponent<UIButton>().enabled = true;
					tabStrip.tabs[2].GetComponent<UIButton>().enabled = true;
					if (!trainStartUI.gameObject.activeSelf)
					{
						isShowTrainStart = false;
						isStartTrain = false;
						showTrainStart();
					}
					break;
				case 1:
					uitool.gameObject.SetActive(true);
					initTrainMode(TrainMode.Training);
					dynamiclistUI.initDynamicListUI(TrainMode.Training);
					initUI(currentEquipName, currentTrainFlow, TrainMode.Training);
					tabStrip.tabs[0].GetComponent<UIButton>().enabled = true;
					tabStrip.tabs[2].GetComponent<UIButton>().enabled = true;
					if (!trainStartUI.gameObject.activeSelf)
					{
						isShowTrainStart = false;
						isStartTrain = false;
						showTrainStart();
					}
					break;
				case 2:
					outExamBtn.SetActive(true);
					uitool.gameObject.SetActive(false);
					initTrainMode(TrainMode.Exam);
					dynamiclistUI.initDynamicListUI(TrainMode.Exam);
					initUI(currentEquipName, currentTrainFlow, TrainMode.Exam);
					tabStrip.tabs[0].GetComponent<UIButton>().enabled = true;
					tabStrip.tabs[1].GetComponent<UIButton>().enabled = true;
					if (!trainStartUI.gameObject.activeSelf)
					{
						isShowTrainStart = false;
						isStartTrain = false;
						showTrainStart();
					}
					break;
			}
		}
		public void addCameraAngle(float x, float y)
		{
			mouseFocus.addCameraAngle(x, y, 0);
		}
		public void setActiveNext(Action action)
		{
			errorCount = 0;
			if (currentTrainMode == TrainMode.Exam)
				action();
			else
			{
				nextProcess.setActiveForNextProcessPanel(true);
				nextProcess.setYesButtonClick(action);
				if (toolMenu.lastName != null)
				{
					toolMenu.lastName.SetActive(false);
				}
			}
		}
		public void setSpecialActive()
		{
			if (toolMenu.lastName != null)
			{
				toolMenu.lastName.SetActive(!toolMenu.lastName.activeSelf);
			}
		}
		public void CallTextInfo(string str)
		{
			uitool.setTextl(str);
			uitool.setPopText(str);
		}
		public void showIconImage(string spriteName)
		{
			showIcon.GetComponent<UISprite>().atlas = toolMenu.atlas;
			showIcon.GetComponent<UISprite>().spriteName = toolMenu.spriteName;
			showIcon.GetComponent<UISprite>().MakePixelPerfect();
		}
		public void setActiveIcon(bool isshow)
		{
			if (showIcon != null)
			{
				showIcon.transform.parent.gameObject.SetActive(isshow);
			}
		}
		public void onRecordAngle()
		{
			mouseFocus.backAngle();
		}

		public void showFlowOperateImage(bool isShow, string image)
		{
			flowOperateImage.SetActive(isShow);
			if (isShow)
			{
				flowOperateImage.GetComponent<UISprite>().spriteName = image;
				StartCoroutine(makeFlowOperateImage());
			}
		}

		IEnumerator makeFlowOperateImage()
		{
			yield return null;
			flowOperateImage.GetComponent<UISprite>().MakePixelPerfect();
		}

		public void showFlowOperateMovie(bool isShow, string movie)
		{
			flowOperateMovie.SetActive(isShow);
			if (isShow)
			{
				movieManager.playMovie(movie, false);
				movieManager.GetComponent<UITexture>().MakePixelPerfect();
			}
		}

		public void showFlowMovie(bool isShow, string movie)
		{
			flowMovie.SetActive(isShow);
			if (isShow)
			{
				uitool.controlPanel.SetActive(false);
				movieManager.playMovie(movie, showControlPanel, false);
				movieManager.SetLooping(false);
			}
		}
		private void showControlPanel(string str)
		{
			uitool.controlPanel.SetActive(true);
		}
		public void flowMovieAction(string movie)
		{
			flowMovie.SetActive(false);
			TrainingComponent.PlayNextComponent();
		}

		public void skipMovie()
		{
			if (TrainingManager.getTrainingOperate()!= null&&currentTrainMode==TrainMode.Exam)
			{
				TrainingManager.gradeDetails[TrainingManager.getTrainingOperate().name].Score = 0;
				TrainingManager.gradeDetails[TrainingManager.getTrainingOperate().name].ErrorMsg = "观看视频";
			}
			uitool.controlPanel.SetActive(true);
			movieManager.OnStop();
			flowMovie.SetActive(false);
			TrainingComponent.PlayNextComponent();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="index">步骤的序号</param>
		public void cause(int subject, int index)
		{
			examGrade.GetComponent<Camera>().depth = -50;
			CoherentUISystem.Instance.UIViews.Remove(examGrade);
			currentTrainMode = TrainMode.Study;
			coverTabs[(int)currentTrainMode].gameObject.SetActive(true);
			switchTabs(0);
			//showTrainStart();
			hideTrainStart();
			index = int.Parse(index.ToString().Substring(index.ToString().Length - 3));
			flowOperateMenuClick(index);
			//examCountDown.onRestart();
			//TrainingManager.resetGradeDetails();
		}
		public int ExamNumber
		{
			get { return examNumber; }
			set { examNumber = value; }
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
	}
}

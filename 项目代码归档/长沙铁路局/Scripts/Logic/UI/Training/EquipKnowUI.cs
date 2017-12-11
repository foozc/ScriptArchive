using Assets.Scripts.util;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using UnityEngine;
using Assets.Scripts.VO;
using Assets.Scripts.Controller;
using Assets.Scripts.Logic.UI.CommonUI;
using Assets.Scripts.Configs;
using Assets.LSE.XF;
using Assets.Scripts.Logic.Scenes;
using Assets.Scripts.Tools;
using UnityEngine.SceneManagement;
using Assets.Scripts.Logic.Movies;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:S700K设备认知界面
*Author:作者
*
*/
namespace Assets.Scripts.Logic.UI.Training
{
	public class EquipKnowUI : UIbase
	{
		public UILabel info;
		public CoherentUIView H5Camera;
		public GameObject circuitUIStart;
		public bool circuitUIIsStart = false;
		public NextProcess change;
		public MyTreeMenu treeMenu;
		public ToolMenu toolMenu;
		public UIWidget alertMSGParent;
		public UIWidget center;
		public UIButton backSuperNodeBtn;
		public UITool uiTool;
		public TabStrip tabStrip;
		public UILabel runTime;
		public MovieManager movieManager;
		private bool isShowTreeMenu = true;
		private List<EquipKnow> equipKnows;
		private List<string> backTreeMenu = new List<string>();
		public GameObject popWindows;
		public CoherentUIView examCamera;
		private BubbleToast alertMSG;
		private MyMouseFocus _mouseFocus = null;
		private EquipTip currentEquipTip = null;
		private string currentModel = null;
		private EquipKnow currentGroup;
		private bool isWalkCamea = true;
		private bool isMusicContorlActive = false;
		private List<Material> myMaterial = new List<Material>();
		private List<Transform> myTransform = new List<Transform>();
		private string kemu;  //为了下面科目1和科目2播放不同组的音乐设置的变量
		public MyUIPlayTween treeTitle;
		public MyUIPlayTween toolTitle;
		public UILabel nameLable;
		public void setName(string name)
		{
			nameLable.text = name;
		}
		public void showSpInfoUI(GameObject obj)
		{
			if (isShowTreeMenu)
			{
				isShowTreeMenu = false;
				SpringPosition sp = SpringPosition.Begin(obj, new Vector3(316f, 0f, 0f), 15f);
			}
			else
			{
				isShowTreeMenu = true;
				SpringPosition sp = SpringPosition.Begin(obj, new Vector3(-97f, 0f, 0f), 15f);
			}
		}
		void Start()
		{
            //uiTool.setHomeBtnClick(setHomelBtnClick);
            AnimationContorll(false);
			foreach (Transform item in treeMenu.treeGrid.transform)
			{
				if (item.GetComponent<MyTreeMenuItem>().getNodeLevel() == 1)
				{
					count++;
					backTreeMenu.Add(item.GetComponent<MyTreeMenuItem>().getItemVO().value);
				}
			}
			initToolMenu();
		}
		/// <summary>
		/// 返回菜单界面
		/// </summary>
		public void backMenu()
		{
			treeMenu.clearChildren(treeMenu.treeGrid.transform);
			if (coverTab[coverTab.Length - 1].activeSelf)
			{
				coverTab[coverTab.Length - 1].SetActive(false);
				Title[Title.Length - 1].SetActive(false);
			}
			Title[0].SetActive(true);
			coverTab[0].SetActive(true);
			currentModel = null;
            movieManager.OnStop();
			nowValue = null;
			nextNumber = 0;
			uiTool.setShiwaiSliderValue(0 / count, 0, count);
			uiTool.setTextl(null);
			uiTool.setPopText(null);
			tabStrip.tabs[0].value = true;
			setSomeUiActive(false);
			DestroyUI(null);
			clear();
			AudioManager.getInstance().SetGroundScale(1f);
			AudioManager.getInstance().SetAudioVolume(AudioManager.MusicNumType.groundMusic, 0.2f, false);// 修改背景音乐的初始音量大小
			AudioManager.getInstance().AudioPlay(GlobalConfig.backgroundMusic, AudioManager.MusicNumType.groundMusic);
			AudioManager.getInstance().AudioStop(AudioManager.MusicNumType.realtimeMusic);

			if (alertMSG != null)
				ResourceManager.getInstance().getUIPool.Despawn(alertMSG.transform);

			ScreenRaycaster raycaster = GameObject.Find("FingerGestures").GetComponent<ScreenRaycaster>();
			raycaster.Cameras = new Camera[1];
			raycaster.Cameras[0] = GameObject.FindObjectOfType<Main>().GetComponent<Camera>();
            SceneManager.LoadScene("MainUI");
			UIManager.getInstance().setActiveUI(UIType.MainMenu);
            examCamera.GetComponent<Camera>().depth= examCamera.GetComponent<Camera>().depth = -50;
			CoherentUISystem.Instance.UIViews.Remove(examCamera);
		}

		/// <summary>
		/// 返回上级设备组
		/// </summary>
		public void backSuperNode()
		{
			MyTreeMenuItem item = treeMenu.getItem(this.currentModel);
			if (item == null)
				return;

			if (item.getParent() != null)
			{
				//backSuperNodeBtn.gameObject.SetActive(true);
				leafNodesClick(item.getParent().getItemVO().value, true);
				treeMenu.Locate(item.getParent().getItemVO().value, true, false);
			}
			else
			{
				backSuperNodeBtn.gameObject.SetActive(false);
				mouseFocus.ChangeCamera(false);
				treeMenu.allFold();
				this.isWalkCamea = true;
				this.currentGroup = null;
				this.currentModel = null;
			}

		}

		public void updateRunTime(Clock c, Clock.Stage s, ulong counter, float clockTime)
		{
			runTime.text = c.UnitConversion();
		}

		/// <summary>
		/// 显示警示信息
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
				bubbleTip.transform.localPosition = new Vector3(0, 70, 0);
				BubbleToast tip = bubbleTip.GetComponent<BubbleToast>();
				tip.init(Vector2.zero);
				this.alertMSG = tip;
				tip.setContent(msg, false);
			}
			else
			{
				alertMSG.setContent(msg, false);
			}
		}

		public void tabStripClick(int index)
		{

		}

		/// <summary>
		/// 初始化树形菜单界面
		/// </summary>
		public void initTreeMenu(string knowType, TrainUI.TrainMode mode)
        {
            change.setYesSwitchClick(changeMode);
            uiTool.setReturnBtnClick(backMenu);
            uiTool.setNextBtnClick(setNextBtnClick);
            uiTool.setPreBtnClick(setBackBtnClick);
            uiTool.setRatateBtnClick(setRotateBtnClick);
            //popWindows.GetComponent<PopWindowsInit>().setItemClick(setPopItemClick);
            tabStrip.init(1, tabStripClick,0);
            //tabStrip.setTabActiveState(0);
            clear();
			kemu = knowType;
			treeMenu.init();
			this.equipKnows = InitConfigs.Instance.SubjectDic[knowType];
			List<MyTreeMenuItemVO> vos = EquipKnowSwitchTreeItem(equipKnows);
			treeMenu.setValue(vos, leafNodesClick, true);
            if (mode == TrainUI.TrainMode.Exam)
            {
                tempMode = mode;
                changeMode();
            }
			info.text = "学员" + H5Camera.GetComponent<UICommunicationsScript>().getUsetName() + "，欢迎来到S700K学习模式";
			info.gameObject.SetActive(true);
			circuitUIStart.SetActive(true);
			circuitUIIsStart = false;
			treeMenu.treeGrid.repositionNow = true;
			treeMenu.treeGrid.Reposition();
			//leafNodesClick("Circle001", true);
			//treeTitle.Play(true);
			//toolTitle.Play(true);
			//specificInfoUI.setValue(null, null);
		}
		private List<MyTreeMenuItemVO> EquipKnowSwitchTreeItem(List<EquipKnow> items)
		{
			List<MyTreeMenuItemVO> vos = new List<MyTreeMenuItemVO>();
			int i = 1;
			foreach (EquipKnow item in items)
			{
				MyTreeMenuItemVO vo = new MyTreeMenuItemVO();
				vo.ItemName = i.ToString();
				vo.name = item.Name;
				vo.value = item.Model;
				if (item.EquipKnows.Count > 0)
					vo.treeMenuItemVO = EquipKnowSwitchTreeItem(item.EquipKnows);
				vos.Add(vo);
				i++;
			}
			return vos;
		}
		GameObject FirstPersonController;
		GameObject mainCamera = null;
		/// <summary>
		/// 树形菜单节点被点击事件委托(包括父节点被点击)
		/// </summary>
		/// <param name="name"></param>
		public void leafNodesClick(string value, bool isMenu)
		{
			EquipKnow equipKnow = InitConfigs.Instance.EquipKnowDic[value];
			if (equipKnow.audio!=null)
			{
				AudioManager.getInstance().AudioStop(AudioManager.MusicNumType.realtimeMusic);
				AudioManager.getInstance().AudioPlay("sbrz/" + equipKnow.audio, AudioManager.MusicNumType.realtimeMusic);
			}
			DestroyUI(null);
			uiTool.setActiveControlPanel(true);
			setSomeUiActive(false);
			AnimationContorll(false);
			uiTool.setShiwaiSliderValue(Convert.ToSingle(getNumberByLevel(1, value)) / count, Convert.ToInt32(getNumberByLevel(1, value)), count);
			this.currentModel = value;
			print(getItemLevel(value));
			string str = equipKnow.Detail;
			if (getItemLevel(value) != 4)
			{
				BackOriginal();
			}
			CallTextInfo(str);
			JudgeName(equipKnow.Name);
			if (getItemLevel(value) == 1)
			{
				treeMenu.resetCurrentFlow();
				treeMenu.setCurrentFlow();
			}
			if (equipKnow.Name.Equals("漫游认知"))
			{
				return;
			}
			
			//AudioManager.getInstance().AudioStop(AudioManager.MusicNumType.realtimeMusic);//先停止 后面再播放

            
			setScene(equipKnow.Model, isMenu);
			showAlertMSG(equipKnow.Warm);
			
			if (getItemLevel(value) >= 2)
			{
				istext = true;
				showEquipTip(equipKnow.Name, equipKnow.ModelObject);
			}
			else
			{
				istext = true;
				CloseText();
			}
		}
		
		public void mouseoverSceneEvent(GameObject obj)
		{
			//if (!equipKnowDic.ContainsKey(obj.name))
			//    return;
			EquipKnow group = new EquipKnow();
			List<EquipKnow> equips = myGetGroud(obj, out group);
			if (equips.Count == 0)
				return;
			if (equips != null)
			{
				group.ModelObject = GameObject.Find(group.Model);
				showEquipTip(group.Name, group.ModelObject);
				HighLightObj(group, MyMouseFocus.LightMode.light);
			}
		}
		public void mouseoutSceneEvent(GameObject obj)
		{
			hideEquipTip();
		}
		public void setScene(string objName, bool isMenu = false)
		{
			if (!InitConfigs.Instance.EquipKnowDic.ContainsKey(objName))
				return;
			EquipKnow equip = InitConfigs.Instance.EquipKnowDic[objName];
			List<EquipKnow> equips = null;
			EquipKnow group = null;
			if (!isMenu)
			{
				//equips = getGroud(equip, out group, true);
				equip.ModelObject = GameObject.Find(objName);
				equips = myGetGroud(equip.ModelObject, out group, true);
			}
			else
			{
				group = equip;
				equips = getMenuGroud(equip);
			}
			if (equips != null)
			{
				if (this.isWalkCamea)
				{
					this.isWalkCamea = false;
					//backSuperNodeBtn.gameObject.SetActive(true);
					mouseFocus.ChangeCamera(true);
				}
				if (equips.Count == 1)
					if (equips[0].Offset != null)
					{
						if (equips[0].LimitYaw != Vector2.zero)
							mouseFocus.setLimit(equips[0].LimitYaw, equips[0].LimitPitch, equips[0].LimitDistance);
						mouseFocus.ChangeTarget(equips[0].Model, equips[0].Offset.x, equips[0].Offset.y, equips[0].Offset.z);
					}
					else
						mouseFocus.ChangeTarget(equips[0].Model);
				else
				{
					EquipKnow know = InitConfigs.Instance.EquipKnowDic[this.currentModel];
					if (know.Offset != null)
					{
						if (know.LimitYaw != Vector2.zero)
							mouseFocus.setLimit(know.LimitYaw, know.LimitPitch, know.LimitDistance);
						mouseFocus.ChangeTarget(this.currentModel, know.Offset.x, know.Offset.y, know.Offset.z);
					}
					else
						mouseFocus.ChangeTarget(this.currentModel);
				}
				HighLightObj(group, MyMouseFocus.LightMode.flash);
			}
		}
		public void clickSceneEvent(GameObject obj)
		{
			EquipKnow group = new EquipKnow();
			if (myGetGroud(obj, out group, true).Count == 0)
				return;

			treeMenu.Locate(group.Model, false);
			mouseoverSceneEvent(group.ModelObject);
		}
		/// <summary>
		/// 显示设备顶部名称，设置显示名称，和设备中心点与设备顶部的偏移量
		/// </summary>
		/// <param name="equipName"></param>
		/// <param name="offset"></param>
		private EquipTip showEquipTip(string equipName, GameObject equipGameObject)
		{

			if (currentEquipTip == null)
			{
				GameObject obj = ResourceManager.getInstance().loadUISpawnPool("Prefabs/UI/MyUI/EquipTip", null);
				obj.transform.localPosition = Vector3.zero;
				currentEquipTip = obj.GetComponent<EquipTip>();
			}
			Vector2 off = Vector2.zero;
			if (equipGameObject != null)
			{
				if (equipGameObject.GetComponent<Renderer>() != null)
					off = new Vector2(0, equipGameObject.GetComponent<Renderer>().bounds.max.y - equipGameObject.GetComponent<Renderer>().bounds.center.y);
				currentEquipTip.init(Camera.main, equipGameObject.transform, off);
			}
			currentEquipTip.setValue(equipName);
			return currentEquipTip;
		}
		private void hideEquipTip()
		{
			if (currentEquipTip != null)
			{
				ResourceManager.getInstance().getUIPool.Despawn(currentEquipTip.transform);
				currentEquipTip = null;
			}
		}
		/// <summary>
		/// 从树形菜单聚焦物体，获取该节点下所有子节点
		/// </summary>
		/// <param name="equip"></param>
		/// <returns></returns>
		private List<EquipKnow> getMenuGroud(EquipKnow equip)
		{
			List<EquipKnow> equips = new List<EquipKnow>();
			if (equip.EquipKnows == null || equip.EquipKnows.Count == 0)
			{
				if (equip.ParentEquipKnow != null)
					this.currentGroup = equip.ParentEquipKnow;
				getAllChild(equip, equips);
			}
			else
			{
				this.currentGroup = equip;
				getAllChild(equip, equips);
			}
			return equips;

		}
		/// <summary>
		/// 获取该组所有子物体, 返回null：点击的物体不属于当前组
		/// 返回的集合中只有一个物体，点击的不是组，是物体，直接聚焦该物体
		/// </summary>
		/// <param name="equip"></param>
		/// <param name="equipGroup"></param>
		/// <param name="isClick"></param>
		/// <returns></returns>
		private List<EquipKnow> myGetGroud(GameObject equip, out EquipKnow equipGroup, bool isClick = false)
		{
			List<EquipKnow> equips = new List<EquipKnow>();
			Transform groupObject = equip.transform;

			equipGroup = null;

			if (currentGroup == null)
			{
				bool flag = false;
				bool flag1 = false;
				if (InitConfigs.Instance.EquipKnowDic.ContainsKey(groupObject.name))
				{
					flag1 = true;
					equipGroup = InitConfigs.Instance.EquipKnowDic[groupObject.name];
				}
				while (true)
				{
					if (groupObject.parent == null)
					{
						if (flag1)
							flag = true;
						break;
					}
					else if (!InitConfigs.Instance.EquipKnowDic.ContainsKey(groupObject.parent.name))
					{
						if (flag1)
						{
							flag = true;
							equipGroup = InitConfigs.Instance.EquipKnowDic[groupObject.name];
							break;
						}
						else
							groupObject = groupObject.parent;
					}
					else
					{
						flag1 = true;
						groupObject = groupObject.parent;
					}
				}
				if (!flag)
					return equips;
				if (isClick)
				{
					this.currentGroup = equipGroup;
					this.currentModel = equipGroup.Model;
				}
			}
			else
			{
				bool flag = false;
				if (InitConfigs.Instance.EquipKnowDic.ContainsKey(groupObject.name))
				{
					flag = true;
					equipGroup = InitConfigs.Instance.EquipKnowDic[groupObject.name];
				}
				else
				{
					while (true)
					{
						if (groupObject.parent == null)
							break;
						else if (!InitConfigs.Instance.EquipKnowDic.ContainsKey(groupObject.parent.name))
							groupObject = groupObject.parent;
						else
						{
							flag = true;
							if (equipGroup == null)
								equipGroup = InitConfigs.Instance.EquipKnowDic[groupObject.parent.name];
							break;
						}
					}
				}
				if (!flag)
					return equips;

				if (!myFindEquipGroup(currentGroup, equipGroup, isClick))
					return equips;

				flag = false;
				while (true)
				{
					if (equipGroup.ParentEquipKnow == currentGroup)
						break;
					flag = true;
					equipGroup = equipGroup.ParentEquipKnow;
				}

				if (isClick)
				{
					if (flag)
					{
						this.currentGroup = equipGroup;
						this.currentModel = equipGroup.Model;
					}
					else this.currentModel = equipGroup.Model;
				}
			}
			getAllChild(equipGroup, equips);
			return equips;
		}
		/// <summary>
		/// 该节点是否在当前组中
		/// </summary>
		/// <param name="group"></param>
		/// <param name="equip"></param>
		/// <param name="isClick"></param>
		/// <returns></returns>
		public bool myFindEquipGroup(EquipKnow group, EquipKnow equip, bool isClick = false)
		{
			bool flag = false;
			foreach (EquipKnow eq in group.EquipKnows)
			{
				if (eq.EquipKnows.Count != 0)
				{
					if (eq.Model.Equals(equip.Model))
						flag = true;
					else
						flag = myFindEquipGroup(eq, equip);
				}
				else
				{
					if (eq.Model.Equals(equip.Model))
						flag = true;
				}
				if (flag)
					break;
			}
			return flag;
		}
		private void getAllChild(EquipKnow equip, List<EquipKnow> equips)
		{
			if (equip.EquipKnows != null && equip.EquipKnows.Count > 0)
				foreach (EquipKnow item in equip.EquipKnows)
				{
					getAllChild(item, equips);
				}
			else
				equips.Add(equip);
		}
		public void HighLightObj(EquipKnow equip, MyMouseFocus.LightMode light)
		{
			List<string> equips = new List<string>();
			HighLightObj(equip, equips);
			if (equips.Count != 0)
			{
				string names = equips[0];
				for (int i = 1; i < equips.Count; i++)
				{
					names += "," + equips[i];
				}
				mouseFocus.HighLightObj(names, light);
			}
			else
			{
				return;
			}
		}
		public void HighLightObj(EquipKnow equip, List<string> equips)
		{
			if (equip.ModelObject == null)
				equip.ModelObject = GameObject.Find(equip.Model);
			if (equip.ModelObject != null)
			{
				getAllChildTransform(equip.ModelObject.transform, equips);
			}
		}
		public void getAllChildTransform(Transform tr, List<string> trs)
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
		private EquipKnow getCurrentGroup(EquipKnow equip)
		{
			return null;
		}
		private void clear()
		{
			isShowTreeMenu = true;
			isWalkCamea = true;
			alertMSG = null;
			currentEquipTip = null;
			currentModel = null;
			currentGroup = null;
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
		//void OnGUI()
		//{
		//    if (GUILayout.Button("树形菜单初始化"))
		//    {
		//        initTreeMenu();s
		//    }
		//}
		public void oncheck()
		{
			mouseFocus.ChangeCamera(false);
			backSuperNodeBtn.gameObject.SetActive(false);
			this.isWalkCamea = true;
			this.currentGroup = null;
			this.currentModel = null;
		}
		public void CallTextInfo(string str)
		{
            uiTool.setTextl(str);
			uiTool.setPopText(str);
			//GameObject TP = GameObject.Find("MoveText");
			//TP.GetComponent<UILabel>().text = str;
			//TP.GetComponent<MyTweenPosition>().ResetToBeginning();
			//TP.GetComponent<MyTweenPosition>().setTextChange(true);
		}
		public void DestroyUI(GameObject obj)
		{
			if (transform.Find("Center/xinghaorenzhi(Clone)")!=null)
			{
				Destroy(transform.Find("Center/xinghaorenzhi(Clone)").gameObject);
				return;
			}
			else if (transform.Find("DZrz_pos/dongzuoyuanli(Clone)") !=null)
			{
				Destroy(transform.Find("DZrz_pos/dongzuoyuanli(Clone)").gameObject);
				return;
			}
			else if(transform.Find("Dcyl_pos/daocharenzhi(Clone)")!=null)
			{
				Destroy(transform.Find("Dcyl_pos/daocharenzhi(Clone)").gameObject);
				return;
			}
			else if (transform.Find("Sbrz_pos/suobirenzhi(Clone)") != null)
			{
				Destroy(transform.Find("Sbrz_pos/suobirenzhi(Clone)").gameObject);
				return;
			}
			else if (transform.Find("Center/zuozhuangyouzhuang(Clone)") != null)
			{
				Destroy(transform.Find("Center/zuozhuangyouzhuang(Clone)").gameObject);
				return;
			}
			else if (transform.Find("DZYL_pos/jiancherenzhi(Clone)") != null)
			{
				Destroy(transform.Find("DZYL_pos/jiancherenzhi(Clone)").gameObject);
				return;
			}
			else if (transform.Find("DZYL_pos/weihuyujiancha(Clone)") != null)
			{
				Destroy(transform.Find("DZYL_pos/weihuyujiancha(Clone)").gameObject);
				return;
			}
		}
		private void JudgeName(string name)
		{
			if (name.Equals("型号认知"))
			{
				uiTool.setActiveControlPanel(false);
				GameObject instance = Instantiate(Resources.Load<GameObject>("Prefabs/UI/MyUI/xinghaorenzhi"));
				instance.transform.parent = center.transform;
				instance.transform.localScale = new Vector3(1, 1, 1);
				instance.transform.position = center.transform.position;
			}
			else if (name.Equals("左装，右装区别认知"))
			{
				uiTool.setActiveControlPanel(false);
				GameObject instance = Instantiate(Resources.Load<GameObject>("Prefabs/UI/MyUI/zuozhuangyouzhuang"));
				instance.transform.parent = center.transform;
				instance.transform.localScale = new Vector3(1, 1, 1);
				instance.transform.position = center.transform.position;
			}
			///点击动作原理聚焦SBRZ-zhuanzheji-renzhi-1130，
			///半透明显示（S700k_waike），
			///重复播放模型动画（SBRZ-zhuanzheji-renzhi-1130），重复播放提示框动画。
			else if (name.Equals("动作原理认知"))
			{
				mouseFocus.ChangeTarget("SBRZ-tuidongqi", -179.6f, 49.1f, 3.8f);
				UIEventListener.Get(movieManager.PlayByPrefabs("neirongxianshikuang-dongzuoyuanlirenzhi-neirong.mp4", "dongzuoyuanli", "DZrz_pos").transform.Find("CloseButton").gameObject).onClick = DestroyUI;
				//setFunctionOfCloseButton("CloseButton");
				ChangeShader("touming", "S700k_waike");
				AnimationContorll(true);
			}
			///点击道岔转换过程认知
			/// 聚焦SBRZ_s700K，半透明显示（S700k_waike），
			///重复播放模型动画（SBRZ-zhuanzheji-renzhi-1130），重复播放提示框动画。
			else if (name.Equals("道岔转换过程认知"))
			{
				mouseFocus.ChangeTarget("SBRZ_s700K", -178.5f, 51.9f, 1.1f);
				UIEventListener.Get(movieManager.PlayByPrefabs("neirongxianshikuang-daochazhuanhuan-neirong.mp4", "daocharenzhi", "Dcyl_pos").transform.Find("CloseButtonDaocha").gameObject).onClick = DestroyUI;
				//setFunctionOfCloseButton("CloseButtonDaocha");
				ChangeShader("touming", "S700k_waike");
				AnimationContorll(true);
			}
			///点击锁闭时零部件关系认知聚焦s700K_suobikuaijisuoshe，
			///半透明显示（S700k_waike，s700K_baochiqi）,
			///重复播放模型动画（SBRZ-zhuanzheji-renzhi-1130），重复播放提示框动画。
			else if (name.Equals("锁闭时零部件关系认知"))
			{
				UIEventListener.Get(movieManager.PlayByPrefabs("neirongxianshikuang-linbujianguanxiBJ-neirong.mp4", "suobirenzhi", "Sbrz_pos").transform.Find("CloseButtonSuobi").gameObject).onClick = DestroyUI;
				mouseFocus.ChangeTarget("s700K_suobikuaijisuoshe", 180, 45, 0.3f);
				ChangeShader("touming", "S700k_waike");
				ChangeShader("touming", "s700K_baochiqi");
				AnimationContorll(true);
			}
			else if (name.Equals("检测过程认知"))
			{
				mouseFocus.ChangeTarget("s700K_shangjiancegan", -180.0f, 47.0f, 0.9f);
				//mouseFocus.setLimit(new Vector2(-300, 0), new Vector2(15, 130), new Vector2(0.5f, 1.4f));
				GameObject instance = Instantiate(Resources.Load<GameObject>("Prefabs/UI/MyUI/jiancherenzhi"));
				Transform tf = GameObject.Find("DZYL_pos").transform;
				instance.transform.parent = tf;
				instance.transform.localScale = new Vector3(1, 1, 1);
				instance.transform.position = tf.position;
				UIEventListener.Get(instance.transform.Find("CloseButtonJianche").gameObject).onClick = DestroyUI;
				UIEventListener.Get(instance.transform.Find("1").gameObject).onClick = ChildrenBtnClick;
				UIEventListener.Get(instance.transform.Find("2").gameObject).onClick = ChildrenBtnClick;
				UIEventListener.Get(instance.transform.Find("3").gameObject).onClick = ChildrenBtnClick;
			}
			else if (name.Equals("维护与检查"))
			{
				mouseFocus.ChangeTarget("s700K_yaobachilun", -180.0f, 47.0f, 0.9f);
				//mouseFocus.setLimit(new Vector2(59, 240), new Vector2(0, 90), new Vector2(0.4f, 1f));
				GameObject instance = Instantiate(Resources.Load<GameObject>("Prefabs/UI/MyUI/weihuyujiancha"));
				Transform tf = GameObject.Find("DZYL_pos").transform;
				instance.transform.parent = tf;
				instance.transform.localScale = new Vector3(1, 1, 1);
				instance.transform.position = tf.position;
				UIEventListener.Get(instance.transform.Find("CloseButtonWeihu").gameObject).onClick = DestroyUI;
				UIEventListener.Get(instance.transform.Find("4").gameObject).onClick = ChildrenBtnClick;
				UIEventListener.Get(instance.transform.Find("5").gameObject).onClick = ChildrenBtnClick;
				UIEventListener.Get(instance.transform.Find("6").gameObject).onClick = ChildrenBtnClick;
			}
			else if (name.Equals("漫游认知"))
			{
				oncheck();
				setSomeUiActive(true);
			}
			/// 动力传动机构（聚焦s700k_donglichuandongjigou，半透明显示S700k_waike）
			else if (name.Equals("动力传动机构"))
			{
				ChangeShader("touming", "S700k_waike");
			}

			///检测和锁闭机构（聚焦s700K_jiancejisuobijigou，半透明显示S700k_waike、s700k_donglichuandongjigou）
			/// 安全装置（聚焦s700K_anquanzhuangzhi，半透明显示S700k_waike、s700k_donglichuandongjigou）
			/// 配线接口（聚焦s700K_peixianjiekou，半透明显示S700k_waike、s700k_donglichuandongjigou）
			else if (name.Equals("配线接口") || name.Equals("安全装置") || name.Equals("检测和锁闭机构"))
			{
				ChangeShader("touming", "S700k_waike");
				ChangeShader("touming", "s700k_donglichuandongjigou");
			}
		}
		private void setSomeUiActive(bool isActive)
		{
			uiTool.setActiveAutoPlayBtn(isActive);
			uiTool.setActivePlayBtn(isActive);
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
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value">显示的文字</param>
		///<param name = "modelName" > 按钮对应的模型文件名</param>
		///
		private void ShowEquiPTipByButton(string value, string modelName)
		{
			GameObject model = GameObject.Find(modelName);
			if (currentEquipTip == null)
			{
				GameObject obj = ResourceManager.getInstance().loadUISpawnPool("Prefabs/UI/MyUI/EquipTip", null);
				obj.transform.localPosition = Vector3.zero;
				currentEquipTip = obj.GetComponent<EquipTip>();
			}
			Vector2 off = Vector2.zero;
			if (model != null)
			{
				if (model.GetComponent<Renderer>() != null)
					off = new Vector2(0, model.GetComponent<Renderer>().bounds.max.y - model.GetComponent<Renderer>().bounds.center.y);
				currentEquipTip.init(Camera.main, model.transform, off);
			}
			currentEquipTip.setValue(value);
		}
		private void HighLightByButton(string modelname)
		{
			Transform tr = GameObject.Find(modelname).transform;
			List<string> listname = new List<string>();
			getAllChildTransform(tr, listname);
			string name = listname[0];
			for (int i = 0; i < listname.Count; i++)
			{
				name += "," + listname[i];
			}
			mouseFocus.HighLightObj(name, 4, MyMouseFocus.LightMode.flash);
		}
		private void ChangeShader(string ShaderName,string modelname)
		{
				foreach (Transform c in GameObject.Find(modelname).GetComponentsInChildren<Transform>())
				{
					if (c.GetComponent<MeshRenderer>() != null)
					{
						myTransform.Add(c);
						myMaterial.Add(c.GetComponent<MeshRenderer>().material);

						Material ma = Resources.Load("Materials/" + ShaderName) as Material;
						ma = Instantiate(ma);
						ma.mainTexture = c.GetComponent<MeshRenderer>().material.mainTexture;
						c.GetComponent<MeshRenderer>().material = ma;
					}
				}
		}
		private void BackOriginal()
		{
            List<Transform> trs = new List<Transform>();   
			for (int i = 0; i < myTransform.Count; i++)
			{
                if (myTransform[i] == null)
                    trs.Add(myTransform[i]);
                else
				    myTransform[i].GetComponent<MeshRenderer>().material = myMaterial[i];
			}
            for(int i = 0; i < trs.Count; i++)
                myTransform.Remove(trs[i]);
			myTransform.Clear();
			myMaterial.Clear();
		}
		public void AnimationContorll(bool isplay)
		{
			if (GameObject.Find("SBRZ-zhuanzheji-renzhi-1130") != null && GameObject.Find("SBRZ-1129"))
			{
				Animation zhuanzheji = GameObject.Find("SBRZ-zhuanzheji-renzhi-1130").GetComponent<Animation>();
				Animation tiegui = GameObject.Find("SBRZ-1129").GetComponent<Animation>();
				if (zhuanzheji != null && !isplay)
				{
					zhuanzheji.Stop();
					tiegui.Stop();
				}
				else if (zhuanzheji != null && isplay)
				{
					zhuanzheji.Play();
					tiegui.Play();
				}
			}
		}
		private int count = 0;
		private string lastNumber;
		private string getNumberByLevel(int level, string value)
		{
			MyTreeMenuItem ob = treeMenu.getItem(value);
			if (ob.getNodeLevel() == level)
			{
				nowValue = value;
				string[] sArray = treeMenu.getItem(value).name.Split('_');
				lastNumber = sArray[sArray.Length - 1];
				return sArray[sArray.Length - 1];
			}
			else if (ob.getNodeLevel() >= level)
			{
				return lastNumber;
			}
			else
				return null;
		}
		private int getItemLevel(string value)
		{
			MyTreeMenuItem ob = treeMenu.getItem(value);
			return ob.getNodeLevel();
		}
		private void setFunctionOfCloseButton(string CloseButtonName)
		{
			UIEventListener.Get(this.transform.Find(CloseButtonName).gameObject).onClick = DestroyUI;
		}
		private bool istext = false;
		public void CloseText()
		{
			if (istext)
			{
				istext = false;
				if (currentEquipTip!=null)
				{
					currentEquipTip.transform.Find("name").GetComponent<UILabel>().text = null;
				}
			}
		}

		//private void setHomelBtnClick()
		//{
		//	popWindows.SetActive(isPopWindowsActive);
		//	isPopWindowsActive = !isPopWindowsActive;
		//}
		private int nextNumber = 0;
		private string nowValue = null;
		private void setBackBtnClick()
		{
			nextNumber = backTreeMenu.IndexOf(nowValue);
			nextNumber--;
			if (nextNumber < 0)
			{
				nextNumber = backTreeMenu.Count - 1;
			}

			treeMenu.Locate(backTreeMenu[nextNumber], true);
		}
		private void setNextBtnClick()
		{
			nextNumber = backTreeMenu.IndexOf(nowValue);
			nextNumber++;
			if (nextNumber > backTreeMenu.Count - 1)
			{
				nextNumber = 0;
			}
			treeMenu.Locate(backTreeMenu[nextNumber], true);
		}
		private void setRotateBtnClick()
		{
			if (nowValue!=null)
				treeMenu.Locate(nowValue, true);
		}
		private void setPopItemClick(string value)
		{
			treeMenu.Locate(value, true);
		}
		private void initToolMenu()
		{
			toolMenu.initToolMenu(null);
		}
		public TrainUI.TrainMode tempMode;
		public TrainUI.TrainMode currentTrainMode;
		public void onTabClick(GameObject obj)
		{
			string [] name = obj.name.Split('_');
			change.setActiveSwitchMode(true);
			if (name[0].Equals("study"))
				tempMode = TrainUI.TrainMode.Study;
			else if (name[0].Equals("exam"))
				tempMode = TrainUI.TrainMode.Exam;
		}
		public GameObject[] coverTab=null;
		public GameObject[] Title= null;
		public void changeMode()
		{
			info.gameObject.SetActive(false);
			switch (tempMode)
			{
				case TrainUI.TrainMode.Study:
					info.gameObject.SetActive(true);
					circuitUIStart.SetActive(true);
					circuitUIIsStart = false;
					currentTrainMode = TrainUI.TrainMode.Study;
					if (coverTab[coverTab.Length - 1].activeSelf)
					{
						coverTab[coverTab.Length - 1].SetActive(false);
						Title[Title.Length - 1].SetActive(false);
					}
					Title[0].SetActive(true);
					coverTab[0].SetActive(true);
					changeExamCameraDepth();
					break;
				case TrainUI.TrainMode.Exam:
					currentTrainMode = TrainUI.TrainMode.Exam;
					if (coverTab[0].activeSelf)
					{
						coverTab[0].SetActive(false);
						Title[0].SetActive(false);
					}
					Title[Title.Length - 1].SetActive(true);
					coverTab[coverTab.Length - 1].SetActive(true);
					changeExamCameraDepth();
					break;
				default:
					break;
			}
		}
		public void changeMode(TrainUI.TrainMode mode)
        {
            if (coverTab[coverTab.Length - 1].activeSelf)
            {
                coverTab[coverTab.Length - 1].SetActive(false);
                Title[Title.Length - 1].SetActive(false);
            }
            Title[0].SetActive(true);
            coverTab[0].SetActive(true);
            tempMode = mode;
            changeMode();
		}
		public void changeExamCameraDepth()
		{
			examCamera.GetComponent<Camera>().depth= examCamera.GetComponent<Camera>().depth == 50 ? -50 : 50;
			if (examCamera.GetComponent<Camera>().depth==-50)
			{
				CoherentUISystem.Instance.UIViews.Remove(examCamera);
			}
			else
			{
                AudioManager.getInstance().AudioStop(AudioManager.MusicNumType.realtimeMusic);
				examCamera.GetComponent<Camera>().depth = 50;
				CoherentUISystem.Instance.UIViews.Add(examCamera);
                examCamera.GetComponent<UICommunicationsScript>().UnityToWeb("SbrzStudyToTest", "");
			}
		}
		public void ChildrenBtnClick(GameObject obj)
		{
			EquipKnow ChildrenBtn = InitConfigs.Instance.ChildrenBtnDic[obj.name];
			if (ChildrenBtn.audio!=null)
			{
				AudioManager.getInstance().AudioStop(AudioManager.MusicNumType.realtimeMusic);
				AudioManager.getInstance().AudioPlay("sbrz/" + ChildrenBtn.audio, AudioManager.MusicNumType.realtimeMusic);
			}
			BackOriginal();
			mouseFocus.ChangeTarget(ChildrenBtn.Model,ChildrenBtn.Offset.x, ChildrenBtn.Offset.y, ChildrenBtn.Offset.z);
			mouseFocus.setLimit(ChildrenBtn.LimitYaw, ChildrenBtn.LimitPitch, ChildrenBtn.LimitDistance);
			HighLightByButton(ChildrenBtn.Model);
			if (ChildrenBtn.transparent[0]!="")
			{
				foreach (string item in ChildrenBtn.transparent)
				{
					ChangeShader("touming", item);
				}
			}
			ShowEquiPTipByButton(ChildrenBtn.showInfo, ChildrenBtn.Model);
			istext = true;
		}
		public void playMovieStart()
		{
			circuitUIStart.SetActive(false);
			circuitUIIsStart = true;
		}
		public void onRecordAngle()
		{
			mouseFocus.backAngle();
		}


        private bool isShowToolMenu = true;
        public UISprite flowOperateTitle;
        public void showFlowMenu()
        {
            if (isShowToolMenu)
            {
                isShowToolMenu = false;
                flowOperateTitle.spriteName = flowOperateTitle.spriteName.Substring(0, flowOperateTitle.spriteName.LastIndexOf('_'));
            }
            else
            {
                isShowToolMenu = true;
                flowOperateTitle.spriteName = flowOperateTitle.spriteName + "_p";
            }
            flowOperateTitle.GetComponent<UIButton>().normalSprite = flowOperateTitle.spriteName;
        }
    }
}

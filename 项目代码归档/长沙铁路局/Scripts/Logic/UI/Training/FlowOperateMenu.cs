using Assets.Scripts.Controller;
using System;
using System.Collections.Generic;
using Training;
using UnityEngine;
using Assets.Scripts.Logic.UI.Training;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:树形菜单列表元素控制类
*Author:作者
*
*/
namespace Assets.Scripts.Logic.UI.Training
{
	public class FlowOperateMenu : MonoBehaviour
	{
		public class FlowItem
		{
			public int index;
			public GameObject obj;
			public GameObject arrow;
			public UILabel name;
		}

		public UIScrollBar scrollBar;
		public UISprite topBtn;
		public UISprite bottomBtn;
		public UIScrollView scrollView;
		public UIGrid grid;
		public GameObject flowItemPrefab;
		public UISprite foldFlowMenuIcon;
		public UILabel foldFlowMenuName;
		public UISprite foldFlowMenuFold;
		public UISprite flowOperateTitle;
		public GameObject maskPanel;

		private Color currentFlowColor = new Color(112f / 255, 125f / 255, 134f / 255, 1);
		private FlowItem currentFlowItem;
		private bool isShowToolMenu = true;
		private bool isShowGrid = true;
		private Dictionary<int, FlowItem> flows = null;
		private Action<int> action;
		private TrainUI.TrainMode currentTrainMode;
		private string[] currentFlowImg = new string[] { "shuxincaidanbiaotilanBJ-xuexi", "shuxincaidanbiaotilanBJ-lainxi", "shuxincaidanbiaotilanBJ-kaoshi" };
		private Color[] foldFlowMenuColor = new Color[]
		{
			new Color(182f / 255, 254f / 255, 214f / 255, 1),
			new Color(255f / 255, 216f / 255, 51f / 255, 1),
			new Color(165f / 255, 213f / 255, 249f / 255, 1),
		};
		private string[] foldFlowmenuIcon = new string[] { "shiwaishebei-xuexi", "shiwaishebei-lianxi", "shiwaishebei-kaohe" };
		private bool currentFlowMenuFold = true;
		private bool isCircuit = false;
		//public UIButton 
		void Start()
		{
			EventDelegate.Add(scrollBar.onChange, scrollBarChange);
		}
		private int count = 0;
		public void initFlowMenu(Action<int> action, TrainUI.TrainMode currentTrainMode)
		{
			if(!isShowGrid)
				showGrid();
			isCircuit = false;
			isShowToolMenu = false;
			showFlowMenu();
			flows = new Dictionary<int, FlowItem>();
			rest();
			this.action = action;
			this.currentTrainMode = currentTrainMode;
			TrainingBigFlow bigFlow = TrainingManager.getTrainingBigFlow(UIManager.getInstance().trainUI.CurrentTrainFlow);
			for (int i = 0; i < bigFlow.getFlowIndexs().Count; i++)
			{
				TrainingFlow flow = bigFlow.getTrainingFlow(i);
				GameObject obj = ResourceManager.getInstance().loadUISpawnPool(flowItemPrefab, null);
				obj.transform.position = Vector3.zero;
				obj.name = i.ToString();
				grid.AddChild(obj.transform);
				UIEventListener.Get(obj).onClick = flowItemClick;
				FlowItem flowItem = new FlowItem();
				flowItem.index = i;
				flowItem.obj = obj;
				flowItem.arrow = obj.transform.Find("arrow").gameObject;
				flowItem.arrow.gameObject.SetActive(false);
				flowItem.name = obj.transform.Find("name").GetComponent<UILabel>();
				flowItem.name.text = flow.name;
				flows.Add(i, flowItem);
				setCurrentFlow(0, currentTrainMode);
			}
			flowOperateTitle.spriteName = getFlowTitleSprite(currentTrainMode, isShowToolMenu);
			flowOperateTitle.GetComponent<UIButton>().normalSprite = flowOperateTitle.spriteName;

			scrollView.gameObject.SetActive(false);
			scrollView.gameObject.SetActive(true);
			grid.repositionNow = true;
			grid.Reposition();
			scrollBar.value = 0;
			scrollView.GetComponent<UIPanel>().Invalidate(true);
		}

		public void initFlowMenu(List<string> flowModes, Action<int> action, TrainUI.TrainMode currentTrainMode)
		{
			isCircuit = true;
			if (!isShowGrid)
				showGrid();
			isShowToolMenu = false;
			showFlowMenu();
			flows = new Dictionary<int, FlowItem>();
			rest();
			this.action = action;
			this.currentTrainMode = currentTrainMode;
			for (int i = 0; i < flowModes.Count; i++)
			{
				GameObject obj = ResourceManager.getInstance().loadUISpawnPool(flowItemPrefab, null);
				obj.transform.position = Vector3.zero;
				obj.name = i.ToString();
				grid.AddChild(obj.transform);
				UIEventListener.Get(obj).onClick = flowItemClick;
				FlowItem flowItem = new FlowItem();
				flowItem.index = i;
				flowItem.obj = obj;
				flowItem.arrow = obj.transform.Find("arrow").gameObject;
				flowItem.arrow.gameObject.SetActive(false);
				flowItem.name = obj.transform.Find("name").GetComponent<UILabel>();
				flowItem.name.text = flowModes[i];
				flows.Add(i, flowItem);
			}
			setCurrentFlow(0, currentTrainMode);
			flowOperateTitle.spriteName = getFlowTitleSprite(currentTrainMode, isShowToolMenu);
			flowOperateTitle.GetComponent<UIButton>().normalSprite = flowOperateTitle.spriteName;
			scrollView.gameObject.SetActive(false);
			scrollView.gameObject.SetActive(true);
			grid.repositionNow = true;
			grid.Reposition();
			scrollBar.value = 0;
			scrollView.GetComponent<UIPanel>().Invalidate(true);
		}

		/// <summary>
		/// 小流程成切换
		/// </summary>
		/// <param name="obj"></param>
		public void flowItemClick(GameObject obj)
		{
			setCurrentFlow(int.Parse(obj.name), currentTrainMode);
			if (action != null)
				action(int.Parse(obj.name));
		}

		public void switchFlow(int index)
		{
			setCurrentFlow(index, currentTrainMode);
		}

		private void setCurrentFlow(int index, TrainUI.TrainMode mode)
		{
			if (currentFlowItem != null)
			{
				currentFlowItem.name.color = Color.white;
				currentFlowItem.arrow.SetActive(false);
				currentFlowItem.obj.GetComponent<UISprite>().spriteName = "gaoliangxuanzhe-liebiaofenge";
				currentFlowItem.obj.GetComponent<UIButton>().normalSprite = "gaoliangxuanzhe-liebiaofenge";
			}
			flows[index].name.color = currentFlowColor;
			flows[index].arrow.SetActive(true);
			flows[index].obj.GetComponent<UISprite>().spriteName = currentFlowImg[(int)mode];
			flows[index].obj.GetComponent<UIButton>().normalSprite = currentFlowImg[(int)mode];
			foldFlowMenuName.color = foldFlowMenuColor[(int)mode];
			foldFlowMenuIcon.spriteName = foldFlowmenuIcon[(int)mode];
			currentFlowItem = flows[index];
		}

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
		public void flowMenufold()
		{
			if (currentFlowMenuFold)
				flowMenufold(false);
			else flowMenufold(true);
		}

		public void scrollBarTop()
		{
			float size = scrollBar.barSize;
			scrollBar.value -= size / 10;
		}

		public void scrollBarBottom()
		{
			float size = scrollBar.barSize;
			scrollBar.value += size / 10;
		}
		public void scrollBarChange()
		{
			if (scrollView.panel.GetViewSize().y > scrollView.bounds.size.y)
			{
				scrollBar.value =0;
			}
			if (scrollBar.alpha == 1)
			{
				topBtn.alpha = 1;
				topBtn.GetComponent<BoxCollider>().enabled = true;
				bottomBtn.alpha = 1;
				bottomBtn.GetComponent<BoxCollider>().enabled = true;
			}
			else
			{
				topBtn.alpha = 0;
				topBtn.GetComponent<BoxCollider>().enabled = false;
				bottomBtn.alpha = 0;
				bottomBtn.GetComponent<BoxCollider>().enabled = false;
			}
		}

		public void rest()
		{
			int count = grid.GetChildList().Count;
			List<Transform> trs = grid.GetChildList();
			for (int i = 0; i < count; i++)
			{
				ResourceManager.getInstance().destroyUILoad(trs[i].gameObject);
				//Destroy(trs[i].gameObject);
			}
			foreach (Transform item in grid.transform)
			{
				Destroy(item.gameObject);
			}
		}

		private void flowMenufold(bool isFold)
		{
			currentFlowMenuFold = isFold;
			if (isFold)
			{
				foreach (FlowItem flow in flows.Values)
				{
					flow.obj.SetActive(false);
				}
				foldFlowMenuIcon.spriteName = "gaoliangxuanzhe-you";
			}
			else
			{
				foreach (FlowItem flow in flows.Values)
				{
					flow.obj.SetActive(true);
				}
				foldFlowMenuIcon.spriteName = "gaoliangxuanzhe-xiai";
			}
			grid.repositionNow = true;
			grid.Reposition();
		}
		private string getFlowTitleSprite(TrainUI.TrainMode trainMode, bool isOpen)
		{
			string str = getTrainTypeStr();
			Debug.Log(trainMode);
			switch (trainMode)
			{
				case TrainUI.TrainMode.Study:
					maskPanel.gameObject.SetActive(false);
					str += "_xuexi";
					break;
				case TrainUI.TrainMode.Training:
					maskPanel.gameObject.SetActive(false);
					str += "_lianxi";
					break;
				case TrainUI.TrainMode.Exam:
					maskPanel.gameObject.SetActive(true);
					str += "_kaoshi";
					break;
			}
			if (isOpen)
				str += "_p";
			return str;
		}

		private string getTrainTypeStr()
		{
			string str = "";
			if (!isCircuit)
			{
				TrainingBigFlow bigFlow = TrainingManager.getTrainingBigFlow(UIManager.getInstance().trainUI.CurrentTrainFlow);
				switch (bigFlow.name)
				{
					case "设备拆解":
						str += "shebeichaijie";
						break;
					case "设备组装":
						str += "shebeizuzhuang";
						break;
					case "设备巡视":
						str += "shebeixunshi";
						break;
					case "设备检修":
						str += "shebeijianxiu";
						break;
                    case "设备故障":
                        str += "shebeiguzhang";
                        break;
                }
			}
			else
				str += "dianluyuanli";
			return str;
		}
		public Dictionary<int, FlowItem> getflows()
		{
			return this.flows;
		}
		public void showGrid()
		{
			if (isShowGrid)
			{
				isShowGrid = false;
				grid.gameObject.SetActive(false);
				foldFlowMenuFold.spriteName = "gaoliangxuanzhe-you";
				foldFlowMenuFold.MakePixelPerfect();
			}
			else
			{
				isShowGrid = true;
				grid.gameObject.SetActive(true);
				foldFlowMenuFold.spriteName = "gaoliangxuanzhe-xiai";
				foldFlowMenuFold.MakePixelPerfect();
				foreach (Transform item in grid.transform)
				{
					item.localScale = Vector3.one;
				}
			}
		}
	}
}

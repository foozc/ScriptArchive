using UnityEngine;
using System.Collections;
using Assets.Scripts.Logic.UI.Training;
using Assets.Scripts.Controller;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:公共键盘事件监听
*Author:作者
*/
public class keyListen : MonoBehaviour
{
	//public enum UIType
	//{
	//	MainMenu,
	//	EquipKnow,
	//	TrainUI,
	//	CircuitUI,
	//	GuideUI
	//}
	public NextProcess exitExam;
	public NextProcess back;
	public Camera uiCamera;
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Time.timeScale = 0;
			UIType uitype=UIManager.getInstance().getCurrentUIType();
			Debug.Log(uitype);
			switch (uitype)
			{
				case UIType.MainMenu:
					break;
				case UIType.EquipKnow:
					if (UIManager.getInstance().equipKnowUI.currentTrainMode == TrainUI.TrainMode.Exam)
					{
						uiCamera.depth = 50;
						UIManager.getInstance().equipKnowUI.gameObject.SetActive(false);
						exitExam.setActiveSwitchMode(true);
						exitExam.setYesSwitchClick(OnEquipKnowUIClick);
						return;
					}
					break;
				case UIType.TrainUI:
					if (UIManager.getInstance().trainUI.currentTrainMode == TrainUI.TrainMode.Exam&& UIManager.getInstance().trainUI.isStartTrain==true)
					{
						Time.timeScale = 1;
						UIManager.getInstance().trainUI.tabStrip.tabs[0].GetComponent<UIButton>().SetState(UIButtonColor.State.Pressed, false);
						UIManager.getInstance().trainUI.tabStrip.tabs[0].GetComponent<UIToggle>().Set(true);
						return;
					}
					break;
				case UIType.CircuitUI:
					if (UIManager.getInstance().circuitUI.currentTrainMode == TrainUI.TrainMode.Exam&&UIManager.getInstance().circuitUI.circuitUIIsStart==true)
					{
						exitExam.setActiveSwitchMode(true);
						exitExam.setYesSwitchClick(OnCircuitExamClick);
						return;
					}
					break;
				case UIType.GuideUI:
					break;
			}
			back.setBackHighLevelActive(true);
		}
	}
	private void OnCircuitExamClick()
	{
		UIManager.getInstance().circuitUI.currentTrainMode = TrainUI.TrainMode.Study;
		UIManager.getInstance().circuitUI.tempMode = TrainUI.TrainMode.Study;
		UIManager.getInstance().circuitUI.changeMode();
		exitExam.setYesSwitchClick(UIManager.getInstance().circuitUI.changeMode);
	}
	private void OnEquipKnowUIClick()
	{
		uiCamera.depth = 5;
		UIManager.getInstance().equipKnowUI.gameObject.SetActive(true);
		UIManager.getInstance().equipKnowUI.currentTrainMode = TrainUI.TrainMode.Study;
		UIManager.getInstance().equipKnowUI.tempMode = TrainUI.TrainMode.Study;
		UIManager.getInstance().equipKnowUI.changeMode();
		exitExam.setYesSwitchClick(UIManager.getInstance().equipKnowUI.changeMode);
	}
}

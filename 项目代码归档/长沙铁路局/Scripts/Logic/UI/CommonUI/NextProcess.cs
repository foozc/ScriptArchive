using UnityEngine;
using System.Collections;
using System;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:公共UI控制类
*Author:作者
*
*/
public class NextProcess : MonoBehaviour
{
	public GameObject nextProcessNoButton;
	public GameObject nextProcessYesButton;
	public GameObject nextProcessPanel;

	public GameObject isExitnoButton;
	public GameObject isExityesButton;
	public GameObject isExitPanel;

	public GameObject switchMode;
	public UIButton switchModeYesbutton;
	public UIButton switchModeNobutton;

	public UILabel TitleText;
	public UILabel conText;
	public GameObject closeBtn;
	public GameObject ImportantPoint;

	public GameObject backHighLevel;
	public UIButton yesBack;
	public UIButton noBack;

	private Action yesButtonAction;
	private Action noButtonAction;
	private Action isExitYesButtonAction;
	private Action isExitNoButtonAction;
	private Action yesAction;
	private Action noAction;
	private Action yesBackh;
	private Action noBackh;

	private void Start()
	{
		UIEventListener.Get(switchModeNobutton.gameObject).onClick = switchModeNoClick;
		UIEventListener.Get(switchModeYesbutton.gameObject).onClick = switchModeYesClick;
		UIEventListener.Get(nextProcessNoButton).onClick = nextProcessNoClick;
		UIEventListener.Get(nextProcessYesButton).onClick = nextProcessYesClick;
		UIEventListener.Get(isExitnoButton).onClick = IsExitnoClick;
		UIEventListener.Get(isExityesButton).onClick = IsExityesClick;
		UIEventListener.Get(closeBtn).onClick = closeImportantPoint;
	}
	public void setActiveForNextProcessPanel (bool isActive)
	{
		nextProcessPanel.SetActive(isActive);
	}
	public void setYesButtonClick(Action yesButtonAction)
	{
		this.yesButtonAction = yesButtonAction;
	}
	public void setNoButtonClick(Action noButtonAction)
	{
		this.noButtonAction = noButtonAction;
	}
	public void setActiveSwitchMode(bool isActive)
	{
		switchMode.SetActive(isActive);
	}
	public void setActiveForIsExitPanel(bool isActive)
	{
		isExitPanel.SetActive(isActive);
	}
	public void setIsExitYesButtonClick(Action isExitYesButtonAction)
	{
		this.isExitYesButtonAction = isExitYesButtonAction;
	}
	public void setIsExitNoButtonClick(Action isExitNoButtonAction)
	{
		this.isExitNoButtonAction = isExitNoButtonAction;
	}
	public void setYesBackh(Action yesBackh)
	{
		this.yesBackh = yesBackh;
	}
	public void setNoBackh(Action noBackh)
	{
		this.noBackh = noBackh;
	}
	public void setYesSwitchClick(Action yesAction)
	{
		this.yesAction = yesAction;
	}
	public void setNoSwitchClick(Action noAction)
	{
		this.noAction = noAction;
	}
	private void switchModeYesClick(GameObject obj)
	{
		if (yesAction != null)
		{
			yesAction();
		}
		setActiveSwitchMode(false);
	}
	private void switchModeNoClick(GameObject obj)
	{
		if (noAction != null)
		{
			noAction();
		}
		setActiveSwitchMode(false);
	}
	private void nextProcessYesClick(GameObject obj)
	{
		if (yesButtonAction != null)
		{
			yesButtonAction();
		}
		setActiveForNextProcessPanel(false);
	}
	private void nextProcessNoClick(GameObject obj)
	{
		if (noButtonAction != null)
		{
			noButtonAction();
		}
		setActiveForNextProcessPanel(false);
	}

	private void IsExityesClick(GameObject obj)
	{
		if (isExitYesButtonAction != null)
		{
			isExitYesButtonAction();
		}
		setActiveForIsExitPanel(false);
	}
	private void IsExitnoClick(GameObject obj)
	{
		if (isExitNoButtonAction != null)
		{
			isExitNoButtonAction();
		}
		setActiveForIsExitPanel(false);
	}
	public void yesBackClick( )
	{
		if (yesBackh != null)
		{
			yesBackh();
		}
		setBackHighLevelActive(false);
	}

	public void noBackClik( )
	{
		if (noBackh != null)
		{
			noBackh();
		}
		Time.timeScale = 1;
		setBackHighLevelActive(false);
	}

	public void setImportantPointActive(bool isActive)
	{
		ImportantPoint.SetActive(isActive);
	}
	public void setTitleText(string text)
	{
		TitleText.text = text;
	}
	public void setText(string text)
	{
		conText.text = text;
	}
	private void closeImportantPoint(GameObject obj)
	{
		setImportantPointActive(false);
	}
	public void setBackHighLevelActive(bool isActive)
	{
		backHighLevel.SetActive(isActive);
	}
}

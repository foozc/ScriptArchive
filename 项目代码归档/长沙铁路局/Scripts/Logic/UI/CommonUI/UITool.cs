using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Assets.LSE.XF;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:底部UI的控制类
*Author:作者
*
*/
public class UITool : MonoBehaviour
{
	public UILabel moveText;
	public UILabel popText;
	public UISlider shiwaiSlider;
	public UILabel label;
	public UISlider aduio;
	public UISprite pauseBtn;
	public GameObject controlPanel;
	public GameObject autoPlay;
	public GameObject play;
	public GameObject popWindows;
	public GameObject aduioContorl;
	public GameObject skipBtn;
	public GameObject textBox;
	public GameObject buttomUI;
	public UIScrollBar popTextScollrBar;
	public UIScrollView popTextScrollView;
	private Action skipAction;
	private Action upBtnAction;
	private Action downBtnAction;
	private Action rightBtnAction;
	private Action leftbtnBction;
	private Action autoPlayBtnAction;
	private Action playBtnAction;
	private Action homeBtnAction;
	private Action rotateBtnAction;
	private Action musicBtnAction;
	private Action returnBtnbtnAction;
	private Action preBtnAction;
	private Action pauseBtnAction;
	private Action nextBtnAction;
	private Action TextBoxActiveFalse;
	private Action TextBoxActiveTrue;
	void Start()
	{
		GameObject.FindObjectOfType<MyMouseFocus>().setMouseDown(isTouchScreen);
		UIEventListener.Get(autoPlay).onClick = autoPlayBtnClick;
		UIEventListener.Get(play).onClick = playBtnClick;
	}
	public void upBtnClick()
	{
		if (upBtnAction != null)
			upBtnAction();
	}
	public void setUpBtnClick(Action upBtnAction)
	{
		this.upBtnAction = upBtnAction;
	}

	public void downBtnClick()
	{
		if (downBtnAction != null)
			downBtnAction();
	}
	public void setDownBtnClick(Action downBtnAction)
	{
		this.downBtnAction = downBtnAction;
	}

	public void leftBtnClick()
	{
		if (leftbtnBction != null)
			leftbtnBction();
	}
	public void setLeftBtnClick(Action leftbtnBction)
	{
		this.leftbtnBction = leftbtnBction;
	}

	public void rightBtnClick()
	{
		if (rightBtnAction != null)
			rightBtnAction();
	}
	public void setRightBtnClick(Action rightBtnAction)
	{
		this.rightBtnAction = rightBtnAction;
	}

	public void autoPlayBtnClick(GameObject obj)
	{
		if (autoPlayBtnAction != null)
			autoPlayBtnAction();
	}
	public void setAutoPlayBtnClick(Action autoPlayBtnAction)
	{
		this.autoPlayBtnAction = autoPlayBtnAction;
	}

	public void playBtnClick(GameObject obj)
	{
		if (playBtnAction != null)
			playBtnAction();
	}
	public void setPlayBtnClick(Action playBtnAction)
	{
		this.playBtnAction = playBtnAction;
	}

	public void homeBtnClick()
	{
		if (homeBtnAction != null)
			homeBtnAction();
	}
	public void setHomeBtnClick(Action homeBtnAction)
	{
		this.homeBtnAction = homeBtnAction;
	}

	public void rotateBtnClick()
	{
		if (rotateBtnAction != null)
			rotateBtnAction();
	}
	public void setRatateBtnClick(Action rotateBtnAction)
	{
		this.rotateBtnAction = rotateBtnAction;
	}

	public void musicBtnClick()
	{
		if (musicBtnAction != null)
			musicBtnAction();
	}
	public void setMusicBtnClick(Action musicBtnAction)
	{
		this.musicBtnAction = musicBtnAction;
	}

	public void returnBtnBtnClick()
	{
		if (returnBtnbtnAction != null)
			returnBtnbtnAction();
	}
	public void setReturnBtnClick(Action returnBtnbtnAction)
	{
		this.returnBtnbtnAction = returnBtnbtnAction;
	}

	public void preBtnClick()
	{
		if (preBtnAction != null)
			preBtnAction();
	}
	public void setPreBtnClick(Action preBtnAction)
	{
		this.preBtnAction = preBtnAction;
	}

	public void pauseBtnClick()
	{
		if (pauseBtnAction != null)
			pauseBtnAction();
	}
	public void setPauseBtnClick(Action pauseBtnAction)
	{
		this.pauseBtnAction = pauseBtnAction;
	}

	public void nextBtnClick()
	{
		if (nextBtnAction != null)
			nextBtnAction();
	}
	public void setNextBtnClick(Action nextBtnAction)
	{
		this.nextBtnAction = nextBtnAction;
	}

	public void setTextl(string text)
	{
		if (moveText != null)
		{
			moveText.text = text;
            //moveText.GetComponent<MyTweenPosition>().ResetToBeginning();
            //moveText.GetComponent<MyTweenPosition>().setTextChange(true);
        }
	}
	public void EmptyTextOfMoveText()
	{
		if (moveText != null)
		{
			moveText.text = null;
		}
	}
	public void setPopText(string text)
	{
		if (popText != null)
		{
			popText.text = text;
		}
	}
	public void EmptyTextOfPopText()
	{
		if (popText != null)
		{
			popText.text = null;
		}
	}
	/// <summary> 
	///设置Slider的value值，设置label显示的文本
	/// </summary>
	/// <param name="value">slider的value值</param>
	/// <param name="numerator">label显示文本的分子部分</param>
	/// <param name="denominator">label显示文本的分母部分</param>
	public void setShiwaiSliderValue(float value, int numerator, int denominator)
	{
		if (shiwaiSlider != null && label != null)
		{
			shiwaiSlider.value = value;
			label.text = numerator + "/" + denominator;
		}
	}
	public void setShiwaiSliderValue(float value)
	{
		if (shiwaiSlider != null)
		{
			shiwaiSlider.value = value;
		}
	}
	public void setActiveControlPanel(bool active)
	{
		if (controlPanel != null)
		{
			controlPanel.SetActive(active);
		}
	}
	public void setActiveAutoPlayBtn(bool active)
	{
		if (autoPlay != null)
		{
			autoPlay.SetActive(active);
		}
	}
	public void setActivePlayBtn(bool active)
	{
		if (play != null)
		{
			play.SetActive(active);
		}
	}
	public void isTouchScreen()
	{
		Ray ray = UICamera.mainCamera.ScreenPointToRay(Input.mousePosition);// uicamera
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 200))
		{
			if (!hit.collider.gameObject.tag.Equals("popWindows"))
			{
				popWindows.GetComponent<TweenScale>().PlayForward();
			}
			if (!hit.collider.gameObject.tag.Equals("ControlAduio"))
			{
				aduioContorl.GetComponent<TweenScale>().PlayForward();
			}
		}
		else
		{
			popWindows.GetComponent<TweenScale>().PlayForward();
			aduioContorl.GetComponent<TweenScale>().PlayForward();
		}
	}
	public void contorlAduio()
	{
		AudioManager.getInstance().SetAudioVolume(AudioManager.MusicNumType.non, aduio.value);
	}
	public void setSkipBtnClick(Action skipAction)
	{
		this.skipAction = skipAction;
	}
	public void skipBtnClick()
	{
		if (skipAction!=null)
		{
			skipAction();
		}
	}
	public void setTextBoxActiveFalse()
	{
		moveText.transform.parent.parent.gameObject.SetActive(true);
		textBox.SetActive(false);
		deleteAchor();
		buttomUI.transform.localPosition = new Vector3(-496, -134, 0);
	}
	public void setTextBoxActiveTrue()
	{
		moveText.transform.parent.parent.gameObject.SetActive(false);
		textBox.SetActive(true);
		deleteAchor();
		buttomUI.transform.localPosition = new Vector3(-496, -160, 0);
	}
	public void deleteAchor()
	{
		foreach (Transform item in buttomUI.GetComponentInChildren<Transform>())
		{
			if (item.GetComponent<UIWidget>() != null)
			{
				if (item.GetComponent<UIWidget>().isAnchored)
				{
					item.GetComponent<UIWidget>().SetAnchor(null, 0, 0, 0, 0);
				}
			}
			if (item.childCount != 0)
			{
				foreach (Transform itemChild in item)
				{
					if (itemChild.GetComponent<UIWidget>() != null)
					{
						if (itemChild.GetComponent<UIWidget>().isAnchored)
						{
							itemChild.GetComponent<UIWidget>().SetAnchor(null, 0, 0, 0, 0);
							print(itemChild.name);
						}
					}
				}
			}
		}
	}

	public void scrollBarChange()
	{
		if (popTextScrollView.panel.GetViewSize().y > popText.localSize.y)
		{
			//textBox.transform.Find("outline").GetComponent<BoxCollider>().enabled = false;
			popTextScollrBar.value = 0;
		}
		else
		{
			//textBox.transform.Find("outline").GetComponent<BoxCollider>().enabled = true;
		}
		if (popTextScollrBar.alpha == 1)
		{
			//topBtn.alpha = 1;
			//topBtn.GetComponent<BoxCollider>().enabled = true;
			popTextScollrBar.alpha = 1;
			popTextScollrBar.GetComponent<BoxCollider>().enabled = true;
		}
		else
		{
			//topBtn.alpha = 0;
			//topBtn.GetComponent<BoxCollider>().enabled = false;
			popTextScollrBar.alpha = 0;
			popTextScollrBar.GetComponent<BoxCollider>().enabled = false;
		}
	}

}

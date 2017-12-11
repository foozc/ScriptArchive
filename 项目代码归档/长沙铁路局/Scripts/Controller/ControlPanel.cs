using UnityEngine;
using System.Collections;
using Assets.Scripts.Logic.UI.Training;
using Assets.Scripts.Controller;
using Assets.LSE.XF;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明：方向盘UI控制类
*Author:作者
*
*/
public class ControlPanel : MonoBehaviour
{
	private EquipKnowUI eui;
	private TrainUI train;
	private int WhichButton;
	private bool isdown = false;
	private bool isramble = false;
	private GameObject c001;
	private GameObject fpc;
	private Vector3 fpcPos;
	// Use this for initialization
	void Start()
	{
		eui = UIManager.getInstance().equipKnowUI;
		train = UIManager.getInstance().trainUI;
	}

	// Update is called once per frame
	void Update()
	{
		if (isramble)
		{
			fpc.transform.position = new Vector3(c001.transform.position.x, fpcPos.y, c001.transform.position.z);
		}
		if (isdown)
		{
			dowmbutton();
		}
	}
	public void OwnChoice()
	{
		fpc = GameObject.Find("First Person Controller");
		isramble = false;
		eui.AnimationContorll(false);
		//fpc.transform.position = fpcPos;
	}
	private void dowmbutton()
	{
		if (WhichButton == 1)
		{
			if (fpc != null)
				fpc.transform.Translate(Vector3.forward * Time.deltaTime * 2);
			else
				train.addCameraAngle(0, 1);
		}
		else if (WhichButton == 2)
		{
			if (fpc != null)
				fpc.transform.Translate(Vector3.left * Time.deltaTime * 2);
			else
				train.addCameraAngle(1, 0);
		}
		else if (WhichButton == 3)
		{
			if (fpc != null)
				fpc.transform.Translate(Vector3.right * Time.deltaTime * 2);
			else
				train.addCameraAngle(-0.8f, 0);
		}
		else if (WhichButton == 4)
		{
			if (fpc != null)
				fpc.transform.Translate(Vector3.back * Time.deltaTime * 2);
			else
				train.addCameraAngle(0, -0.8f);
		}
	}
	public void ramble()
	{
		eui.AnimationContorll(true);
		fpc = GameObject.Find("First Person Controller");
		fpcPos = fpc.transform.position;
		c001 = GameObject.Find("Camera001.Target");
		isramble = true;
	}

	public void forwardBt_down()
	{
		fpc = GameObject.Find("First Person Controller");
		isdown = true;
		WhichButton = 1;
	}

	public void leftBt_down()
	{
		fpc = GameObject.Find("First Person Controller");
		isdown = true;
		WhichButton = 2;
	}
	public void rightBt_down()
	{
		fpc = GameObject.Find("First Person Controller");
		isdown = true;
		WhichButton = 3;
	}
	public void backBt_down()
	{
		fpc = GameObject.Find("First Person Controller");
		isdown = true;
		WhichButton = 4;
	}
	public void OnRelease()
	{
		isdown = false;
		WhichButton = 0;
	}
}

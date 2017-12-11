using Assets.Scripts.Controller;
using System;
using System.Collections.Generic;
using UnityEngine;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:万用表控制
*Author:作者
*
*/
namespace Assets.Scripts.Logic.UI.CommonUI
{
    public class MultimeterUI : MonoBehaviour
    {
        public GameObject dangwei;
        public UILabel showNumber;
		private bool isOhm=false;
		private bool OnOff;
		private int currentStall = 0;
        public void stallSwitch()
        {
            if (currentStall == 9)
                currentStall = 0;
            else currentStall++;
			if (currentStall == 4)
			{
				isOhm = true;
				showNumber.gameObject.SetActive(true);
			}
			else
			{
				isOhm = false;
				showNumber.gameObject.SetActive(false);
			}
			dangwei.transform.rotation = Quaternion.Euler(0, 0, -22.5f * currentStall);
        }

        public void setShowNumber(string value)
        {
				showNumber.text = value;
        }
		public bool getIsOhm()
		{
			return isOhm;
		}
		public void setCurrentStall(int currentStall)
		{
			this.currentStall = currentStall;
		}
	}
}

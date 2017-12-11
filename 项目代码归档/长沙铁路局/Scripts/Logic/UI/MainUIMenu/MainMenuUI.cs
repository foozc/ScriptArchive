using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Assets.Scripts.Configs;
using Assets.Scripts.Logic.UI.Login;
using Assets.Scripts.Controller;
using System.Collections;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:主界面菜单
*Author:作者
*
*/
namespace Assets.Scripts.Logic.UI.MainUIMenu
{
    public class MainMenuUI : UIbase
    {
        private RoleType currentUI;
        private bool isFirst = true;
        void Start()
        {
            UIManager.getInstance().loading.SetActive(true);
            StartCoroutine(loadTime());
            isFirst = false;
        }

        IEnumerator loadTime()
        {
            yield return new WaitForSeconds(2);
            UIManager.getInstance().loading.SetActive(false);
        }
        
        public void roleTypeInit()
        {
            if (!isFirst)
            {
                CoherentUISystem.Instance.UIViews.Add(UIManager.getInstance().H5View);
                UIManager.getInstance().H5View.GetComponent<Camera>().depth = 1;
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

    }
}

using Assets.Scripts.Controller;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:漫游场景类
*Author:作者
*
*/
namespace Assets.Scripts.Logic.UI.Training
{
    public class RoamSceneUI:UIbase
    {

        private GameObject firstCame;
        private GameObject manyou_G;
        private bool isAutoRoam = false;
        private Animation myanimation;
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
        /// 返回菜单界面
        /// </summary>
        public void backMenu()
        {
            SceneManager.LoadScene("MainUI");
            UIManager.getInstance().setActiveUI(UIType.MainMenu);
            endAutoRoam();
        }

        public void autoRoam()
        {
            if (!isAutoRoam)
            {
                if (firstCame == null)
                    firstCame = GameObject.Find("First Person Controller");
                firstCame.SetActive(false);
                if (manyou_G == null)
                {
                    manyou_G = GameObject.Find("jiaohu").transform.Find("manyou_G").gameObject;
                }
                manyou_G.SetActive(true);
                myanimation = manyou_G.GetComponent<Animation>();
                myanimation.Play();
                myanimation["Take 001"].speed = 0.5f;
                isAutoRoam = true;
            }
            else
                endAutoRoam();
            //gameobj
        }

        public void endAutoRoam()
        {
            myanimation.Stop();
            isAutoRoam = false;
            firstCame.SetActive(true);
            manyou_G.SetActive(false);
        }
        void Update()
        {
            if(GetComponent<Animation>() != null && isAutoRoam)
                if (!myanimation.isPlaying)
                {
                    endAutoRoam();
                }
        }
    }
}

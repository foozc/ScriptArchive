using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Controller;
using Assets.Scripts.Logic.UI.Training;
using Assets.Scripts.VO;
using Assets.Scripts.DBLogic;
using Assets.Scripts.Configs;
using PluginEvent;
using UnityEngine.SceneManagement;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:学生菜单UI
*Author:作者
*
*/
namespace Assets.Scripts.Logic.UI.MainUIMenu
{
    public class MenuNode
    {
        public GameObject currentMenu;
        public UIButton currentBtn;
        public GameObject nextMenu;
    }
    public class StudentUIMenu: UIbase
    {

        public GameObject FirstMenu;

        public UIButton BackUP;
        public TableListUI tableListUI;

        private GameObject currentMenu;
        
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

        void Start()
        {
            if (UIManager.getInstance().viewMenuQueue.Count == 0)
                nextMenu(FirstMenu);
            //this.menuStruct = new Dictionary<string, MenuNode>();
            //init();
        }

        private void init()
        {
            
        }

        private void initGrade()
        {
            tableListUI.init(null);
            ScoreDBHelper helper = new ScoreDBHelper();
            List<Score> scores = helper.getScore(MySession.Id);
            setUserList(scores);
        }

        private void setUserList(List<Score> scores)
        {
            List<ListItemBase<Score>> listItems = new List<ListItemBase<Score>>();
            foreach (Score score in scores)
            {
                ScoreListItem item = new ScoreListItem(score);
                listItems.Add(item);
            }
            tableListUI.loadAllItem<Score>(listItems);
        }

        public void myclick(GameObject currentMenu, GameObject currentBtn, GameObject childMenu)
        {
            if (childMenu == null)
            {

            }
            else 
                nextMenu(childMenu);
        }


        private void nextMenu(GameObject nextMenu)
        {
            if (currentMenu != null)
            {
                currentMenu.SetActive(false);
                UIManager.getInstance().viewMenuQueue.Add(currentMenu);
            }
            nextMenu.SetActive(true);
            if (nextMenu.name.Equals("GradeView"))
                initGrade();
                //StudentScoresUI.getInstance().setStudentScore();
            currentMenu = nextMenu;
        }

        public void backUP()
        {
            
            if (currentMenu != null)
            {
                if (currentMenu.name.Equals("FirstMenu"))
                {
                    gameObject.SetActive(false);
                    UIManager.getInstance().exitLogin();
                }
                if (UIManager.getInstance().viewMenuQueue.Count > 0)
                {
                    GameObject last = UIManager.getInstance().viewMenuQueue[UIManager.getInstance().viewMenuQueue.Count - 1];

                    UIManager.getInstance().viewMenuQueue.Remove(last);
                    //nextMenu(last);
                    currentMenu.SetActive(false);
                    last.SetActive(true);
                    currentMenu = last;
                }
            }
            else
                UIManager.getInstance().exitLogin();
        }

        #region 培训场景
        #region 科目一
        public GameObject loading;
        /// <summary>
        /// 进入培训科目一设备认知场景
        /// </summary>
        public void SubOneEquipKnow()
        {
            //Application.LoadLevel("DeviceKnow1");
            GlobalConfig.loadName = "DeviceKnow1";
            //先进入LoadingScene场景
            SceneManager.LoadScene("LoadingScene");
            loading.SetActive(true);
            
            EquipKnowUI equipUI = (EquipKnowUI)UIManager.getInstance().setActiveUI(UIType.EquipKnow);
            equipUI.initTreeMenu("DeviceKnow1", TrainUI.TrainMode.Study);
        }

        /// <summary>
        /// 进入培训科目一，设备操作，火灾报警控制器开机
        /// </summary>
        public void hzbjkzqkj()
        {
            //Application.LoadLevel("Train1_hzbjkzqkj");
            //TrainUI equipUI = (TrainUI)UIManager.getInstance().setActiveUI(UIType.TrainUI);
            //equipUI.initTrainUI("火灾报警控制器开机");

            UIManager.getInstance().changeScene("Train1_hzbjkzqkj", UIType.TrainUI, "火灾报警控制器开机");
        }
        #endregion
        #endregion


        #region 进入考核场景
        
        #endregion

    }
}

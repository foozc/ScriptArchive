using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Assets.LSE.XF;
using Assets.Scripts.Controller;
using PluginEvent;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:流程指引类
*Author:作者
*
*/
namespace Assets.Scripts.Logic.UI.Training
{
    public class GuideHelperUI : UIbase
    {

        public UIButton backSuperNodeBtn;
        public UISprite trainStartUI;
        public UISprite guide;
        public UISprite finishStart;


        private string trageName = "caozuoyanshi06";
        private bool isWalkCamea = true;
        private MyMouseFocus _mouseFocus;
        private List<string> guideSprite = new List<string>() { "To move forward", "Move back", "To the left", "To the right", "Rotation Angle of view"
            ,"Switch target view", "Click on the red objects"};
        private int guideNum = 0;
        private bool isTwinkle = false;
        private bool isMouseover = false;

        private string sceneName;
        private UIType uitype;
        private string flow;
        public void nextStudent()
        {
            if (guideNum == guideSprite.Count-1)
            {//指引流程结束
                isTwinkle = false;
                guide.gameObject.SetActive(false);
                finishStart.gameObject.SetActive(true);
            }
            else
            {
                guideNum++;
                guide.spriteName = guideSprite[guideNum];
                if (guideNum == 5)
                    isTwinkle = true;
            }
        }
        public void finishStartTo()
        {
            UIManager.getInstance().changeScene(sceneName, uitype, flow);
        }


        void Update()
        {//处理：前后左右，鼠标滑动事件
            if (Input.GetKeyDown(KeyCode.W))
                handleGuide(0);
            else if (Input.GetKeyDown(KeyCode.S))
                handleGuide(1);
            else if (Input.GetKeyDown(KeyCode.A))
                handleGuide(2);
            else if (Input.GetKeyDown(KeyCode.D))
                handleGuide(3);
            twinkle();
        }

        float curTime = 0;

        private void twinkle()
        {
            if (isTwinkle && !isMouseover)
            {
                curTime += Time.deltaTime;
                if (curTime > 1)
                {
                    curTime -= 1;
                    PluginEventManager.FireEvent("ModelEffect", trageName + ",0,0,0");
                }
                else if (curTime > 0.5)
                {
                    PluginEventManager.FireEvent("ModelEffect", trageName + ",1,0,0");
                }
            }
        }

        public void setNextScene(string sceneName, UIType type, string flow)
        {
            this.sceneName = sceneName;
            this.uitype = type;
            this.flow = flow;
        }

        public void backSuperNode()
        {
            backSuperNodeBtn.gameObject.SetActive(false);
            mouseFocus.ChangeCamera(false);
            this.isWalkCamea = true;
        }

        public void LocatObjects()
        {
            if (isTwinkle)
            {
                clickEvent(GameObject.Find(trageName), true);
                handleGuide(5);
            }
        }

        public void clickEvent(GameObject obj, bool isLocat = false)
        {
            if (!obj.name.Equals(trageName))
                return;
            if (this.isWalkCamea || isLocat)
            {
                this.isWalkCamea = false;
                backSuperNodeBtn.gameObject.SetActive(true);
                mouseFocus.ChangeCamera(true);
                mouseFocus.setLimit(new Vector2(100f, 260f), new Vector2(10f, 80f), new Vector2(1f, 3f));
                mouseFocus.ChangeTarget(trageName, 180f, 26f, 3f);
            }
            else
            {
                if(obj.name.Equals(trageName))
                    handleGuide(6);
            }

        }

        public void handleGuide(int guide)
        {
            if (guideNum == guide)
                nextStudent();
        }

        public void showTrainStart()
        {
            trainStartUI.gameObject.SetActive(true);
        }
        public void hideTrainStart()
        {
            trainStartUI.gameObject.SetActive(false);
        }

        public void mouseoverSceneEvent(GameObject obj)
        {
            if (obj.name.Equals(trageName))
            {
                isMouseover = true;
                HighLightObj(obj);
            }
        }
        public void mouseoutSceneEvent()
        {
            isMouseover = false;

        }
        public void HighLightObj(GameObject equip, MyMouseFocus.LightMode light = MyMouseFocus.LightMode.light)
        {
            List<string> equips = new List<string>();

            if (equip.transform.childCount == 0)
                equips.Add(equip.name);
            else
                getAllChildTransform(equip.transform, equips);

            string names = equips[0];
            for (int i = 1; i < equips.Count; i++)
            {
                names += "," + equips[i];
            }
            mouseFocus.HighLightObj(names, light);
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

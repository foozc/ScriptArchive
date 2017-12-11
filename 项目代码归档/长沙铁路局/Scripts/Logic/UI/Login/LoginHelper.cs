using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Assets.Scripts.VO;
using Assets.Scripts.DBLogic;
using Assets.Scripts.Controller;
using Assets.Scripts.Configs;
using Assets.Scripts.Logic.UI.CommonUI;
using System.Collections;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:登录控制
*Author:作者
*
*/
namespace Assets.Scripts.Logic.UI.Login
{
    public enum RoleType
    {
        /// <summary>
        /// 学生
        /// </summary>
        studentCheckbox = 0,
        /// <summary>
        /// 教官
        /// </summary>
        instructorCheckbox,
        /// <summary>
        /// 管理员
        /// </summary>
        adminCheckbox
    }
    public class LoginHelper : UIbase
    {
        public UIInput username;
        public UIInput pwd;
        public int group = 1;
        public UIButton submitBtn;
        public GameObject msgParent = null;

        private UserDBHelper userDB = null;
        private BubbleToast msg = null;
        private bool isShowMsg = false;

        public UIButton exitSystemButton;

        //以下是介绍消防的按钮  加载图片
        public UIButton introductonButton;
        public UIButton leftButton;
        public UIButton rightButton;
        public UIButton skipButton;
        public UISprite back;
        public UISprite picture;
        public UIButton scanEnd;

        public GameObject load;

        public UISprite loadtext;
       // public SoftKey ytsoftkey;//引用由域天工具随机生成的加密类模块
        void Awake()
        {
            userDB = new UserDBHelper();
            UIEventListener.Get(username.gameObject).onClick = hideMsg;
            UIEventListener.Get(pwd.gameObject).onClick = hideMsg;
        }

        void Start()
        {
            jiemiok();
            load.SetActive(false);

            //GET请求   
            //StartCoroutine(loginjiami(GlobalConfig.serverHost));

        }

        IEnumerator loginjiami(string url)
        {
            WWW www = new WWW(url);

            while (!www.isDone)
            {
                yield return null;
            }

            if (www.error != null)
            {
                //GET请求失败   
                jiami();
                
               // Debug.Log("error is :" + www.error);
                www.Dispose();

            }
            else if (www.text == "yes")
            {
              //  Debug.Log("error is :" + www.text);
                jiemiok();
            }
            else if (www.text == "no")
            {
                //GET请求成功   
               // Debug.Log("request ok : " + www.text);
                jiami();
                www.Dispose();
            }
            else
            {
                jiami();
                www.Dispose();
            }

            load.SetActive(false);
        }

        public void chongshi()
        {
            StartCoroutine(loginjiami( GlobalConfig.serverHost));
        }

        private void jiami()
        {
            //在窗体开发装载时，必须首先调用该函数
            //ytsoftkey = new SoftKey();


            ////@def_net MessageBox.Show ("要测试网络版，请先运行通用服务程序，请确定通用服务程序已运行后，单击‘确定’。");
            ////@def_net short ret = ytsoftkey.ConnectSvr("127.0.0.1", 3001);
            ////@def_net if(ret!=0)MessageBox.Show( "不能连接服务器, 错误码为："+ytsoftkey.GetErrInfo(ret));

            ////检查是否存在对应的加密锁
            //try
            //{
            //    if (!ytsoftkey.YCheckKey_1())
            //    {
            //        Debug.Log("不存在对应的加密锁。");
            //        back.GetComponent<UIWidget>().depth = 10;
            //        UIAtlas backU = Resources.Load("Atlas/XF_introduction2", typeof(UIAtlas)) as UIAtlas;
            //        back.atlas = backU;
            //        back.spriteName = "MovieExplain_vagueGround";
            //        AudioManager.getInstance().gameObject.SetActive(false);
            //        GameObject.Find("introduction").transform.Find("quitxiaofang").gameObject.SetActive(true);
            //        GameObject.Find("SHUTB").transform.FindChild("ShutButton").gameObject.SetActive(false);
            //        leftButton.gameObject.SetActive(false);
            //        rightButton.gameObject.SetActive(false);
            //        skipButton.gameObject.SetActive(false);
            //        picture.gameObject.SetActive(false);
            //        scanEnd.gameObject.SetActive(false);
            //        introductonButton.gameObject.SetActive(false);
            //        //UIManager.getInstance().exitSystem();
            //        //exitSystem();
            //    }
            //    else
            //    {
            //        Debug.Log("存在对应的加密锁。");
            //    }
            //}
            //catch
            //{
                Debug.Log("不存在对应的加密锁。");
                back.GetComponent<UIWidget>().depth = 10;
                UIAtlas backU = Resources.Load("Atlas/XF_introduction2", typeof(UIAtlas)) as UIAtlas;
                back.atlas = backU;
                back.spriteName = "MovieExplain_vagueGround";
                AudioManager.getInstance().gameObject.SetActive(false);
                loadtext.spriteName = "jiamigoujianchez";
                GameObject.Find("introduction").transform.Find("quitxiaofang").gameObject.SetActive(true);
                //GameObject.Find("SHUTB").transform.FindChild("ShutButton").gameObject.SetActive(false);
                leftButton.gameObject.SetActive(false);
                rightButton.gameObject.SetActive(false);
                skipButton.gameObject.SetActive(false);
                picture.gameObject.SetActive(false);
                scanEnd.gameObject.SetActive(false);
                introductonButton.gameObject.SetActive(false);

         //   }
            AudioManager.getInstance().AudioPlay("XF_yuyin/kemu1/" + "Page1", AudioManager.MusicNumType.realtimeMusic);
        }

        public void jiemiok()
        {
            back.GetComponent<UIWidget>().depth = 0;
            UIAtlas backU = Resources.Load("Atlas/XFMenu", typeof(UIAtlas)) as UIAtlas;
            back.atlas = backU;
            back.spriteName = "background_image";
            loadtext.spriteName = "exam_next_loding";
            AudioManager.getInstance().gameObject.SetActive(true);
            GameObject.Find("introduction").transform.Find("quitxiaofang").gameObject.SetActive(false);
            //GameObject.Find("SHUTB").transform.FindChild("ShutButton").gameObject.SetActive(false);
            leftButton.gameObject.SetActive(true);
            rightButton.gameObject.SetActive(true);
            skipButton.gameObject.SetActive(true);
            picture.gameObject.SetActive(true);
           // scanEnd.gameObject.SetActive(true);
            introductonButton.gameObject.SetActive(true);
        }
        IEnumerator Countdown()
        {
            //for (float timer = 3; timer >= 0; timer -= Time.deltaTime)
            //{
                StartCoroutine(loginjiami("http://localhost:8080/"));
              //  Debug.Log(timer);
                yield return 0;
        }
        public void quitSystem()
        {
            //UIManager.getInstance().exitSystem();
            //  Application.Quit();
            StartCoroutine(loginjiami(GlobalConfig.serverHost));
        }
        public void introductonButton_clik()
        {
            UIAtlas backU = Resources.Load("Atlas/XF_introduction1",typeof(UIAtlas)) as UIAtlas;
            back.atlas = backU;
            back.spriteName = "Introduction background";
            //leftButton.gameObject.SetActive(true);
            rightButton.gameObject.SetActive(true);
            skipButton.gameObject.SetActive(true);
            picture.gameObject.SetActive(true);
            scanEnd.gameObject.SetActive(false);
            UIAtlas pictureU = Resources.Load("Atlas/XF_introduction1", typeof(UIAtlas)) as UIAtlas;
            picture.atlas = pictureU;
            string pictureSprite = picture.spriteName;
            string sname = "Page" + 1;
            picture.spriteName = sname;
            AudioManager.getInstance().AudioPlay("XF_yuyin/kemu1/" + sname, AudioManager.MusicNumType.realtimeMusic);
        }
        public void skipButton_click()
        {
            UIAtlas backU = Resources.Load("Atlas/XFMenu", typeof(UIAtlas)) as UIAtlas;
            back.atlas = backU;
            back.spriteName = "background_image";
           
            leftButton.gameObject.SetActive(false);
            rightButton.gameObject.SetActive(false);
            skipButton.gameObject.SetActive(false);
            picture.gameObject.SetActive(false);
            scanEnd.gameObject.SetActive(false);
            AudioManager.getInstance().AudioStop(AudioManager.MusicNumType.realtimeMusic);//返回的时候停止实时音乐
        }
        public void leftButton_click()
        {
            string pictureSprite = picture.spriteName;
            int NumbPicture=int.Parse(pictureSprite.Split('e')[1]);
            if (NumbPicture == 2)
            {
                leftButton.gameObject.SetActive(false);
            }
            if (NumbPicture == 11)
            {
                scanEnd.gameObject.SetActive(false);
                skipButton.gameObject.SetActive(true);
                rightButton.gameObject.SetActive(true);
            }
            if (NumbPicture == 6)
            {
                UIAtlas pictureU = Resources.Load("Atlas/XF_introduction1", typeof(UIAtlas)) as UIAtlas;
                picture.atlas = pictureU;

            }
            if (1 < NumbPicture && NumbPicture <= 11)
            {
                string sname = "Page" + (NumbPicture - 1);
                picture.spriteName = sname;
                AudioManager.getInstance().AudioPlay("XF_yuyin/kemu1/" + sname, AudioManager.MusicNumType.realtimeMusic);
                // picture.spriteName = "Page" + (NumbPicture - 1);
            }
        }
        public void rightButton_click()
        {
            string pictureSprite = picture.spriteName;
            int NumbPicture = int.Parse(pictureSprite.Split('e')[1]);
            if (NumbPicture == 1)
            {
                leftButton.gameObject.SetActive(true);
            }
            if (NumbPicture == 10)
            {
                rightButton.gameObject.SetActive(false);
                scanEnd.gameObject.SetActive(true);
                skipButton.gameObject.SetActive(false);
            }
            if (NumbPicture == 5)
            {
                UIAtlas pictureU = Resources.Load("Atlas/XF_introduction2", typeof(UIAtlas)) as UIAtlas;
                picture.atlas = pictureU;

            }

            if (1 <= NumbPicture && NumbPicture < 11)
            {
                string sname = "Page" + (NumbPicture + 1);
                picture.spriteName = sname;
                AudioManager.getInstance().AudioPlay("XF_yuyin/kemu1/" + sname, AudioManager.MusicNumType.realtimeMusic);
               // picture.spriteName = "Page" + (NumbPicture +1);
            }
        }


        private User getAllValue()
        {
            User user = new User();
            UIToggle tog = UIToggle.GetActiveToggle(1);
            user.Role = (short)((RoleType) Enum.Parse(typeof(RoleType), tog.name));
            if(username.value != "")
                user.Id = Int32.Parse(username.value);
            user.Pwd = pwd.value;
            return user;
        }
        
        public void login()
        {
            UIManager.getInstance().setActiveUI(UIType.MainMenu);
            //User user = getAllValue();
            //try
            //{
            //    User dbUser = userDB.getUser(user.Id, user.Role);
            //    if (dbUser != null)
            //        if (user.Pwd.Equals(dbUser.Pwd))
            //        {
            //            successHandle(dbUser);
            //            return;
            //        }
            //    failureHandle("用户名密码错误！");
            //}
            //catch (Exception)
            //{
            //    failureHandle("服务器连接错误！");
            //}
        }

        private void successHandle(User user)
        {
            Log.Logger.debug(Log.Module.Login, "登陆成功");
            //Application.LoadLevel("MainUI");
            MySession.Id = user.Id;
            MySession.Role = (RoleType)user.Role;
            UIManager.getInstance().setActiveUI(UIType.MainMenu);
        }

        private void failureHandle(string error)
        {
            Log.Logger.debug(Log.Module.Login, error);
            if (msg == null)
            {
                GameObject bubbleTip = ResourceManager.getInstance().loadUISpawnPool("Prefabs/UI/MyUI/BubbleToast", msgParent);
                bubbleTip.transform.localPosition = Vector3.zero;
                BubbleToast tip = bubbleTip.GetComponent<BubbleToast>();
                msg = tip;
                msg.setContent(error, false);
                isShowMsg = true;
                return;
            }
            else if (!isShowMsg)
            {
                GameObject bubbleTip = ResourceManager.getInstance().loadUISpawnPool("Prefabs/UI/BubbleToast", msgParent);
                bubbleTip.transform.localPosition = Vector3.zero;
            }
            msg.setContent(error, false);
            isShowMsg = true;
        }

        public void hideMsg(GameObject obj)
        {
            if (isShowMsg && msg != null)
            {
                msg.hideBubbleToast();
                ResourceManager.getInstance().getUIPool.Despawn(msg.transform);
                isShowMsg = false;
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

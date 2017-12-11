using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Assets.Scripts.Logic.UI;

namespace Assets.Scripts.Logic.UI
{
    public class HotKeyManager : UIwnd
    {
        #region  单例类
        private static HotKeyManager instance;

        public static HotKeyManager Instance
        {
            get
            {
                if (!instance)
                {
                    instance = GameObject.FindObjectOfType<HotKeyManager>();
                    if (!instance)
                    {
                        //GameObject insGo = Instantiate(Resources.Load("UI/Prefabs/"+typeof(T).Name)) as GameObject;
                        GameObject wnd = UIDownLoad.Instance.getUI("HotKeyManager",true);  //这里要求资源预制物名必须与资源类名一致，不一致需要修改一致/
                        instance = wnd.GetComponent<HotKeyManager>();
                    }
                    //if (!Instance)
                    //{
                    //    Instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    //}
                }
                return instance;
            }
        }
        #endregion

        private Dictionary<HotKeyType, IHotKey> hotKeyDic = new Dictionary<HotKeyType, IHotKey>();

        /// <summary>
        /// 添加快捷键
        /// </summary>
        /// <param name="keyType"></param>
        /// <param name="hotKey"></param>
        public void addHotKey(HotKeyType keyType, IHotKey hotKey)
        {
            if (!hotKeyDic.ContainsKey(keyType))
            {
                hotKeyDic.Add(keyType, hotKey);
            }
        }

        /// <summary>
        /// 移除快捷键
        /// </summary>
        /// <param name="keyType"></param>
        public void removeHotKey(HotKeyType keyType)
        {
            if(hotKeyDic.ContainsKey(keyType))
            {
                hotKeyDic.Remove(keyType);
            }
        }

        /// <summary>
        /// 重置快捷键
        /// </summary>
        /// <param name="keyType"></param>
        /// <param name="newKeyCode"></param>
        public void resetHotKey(HotKeyType keyType,KeyCode newKeyCode)
        {
            if (hotKeyDic.ContainsKey(keyType))
            {
                hotKeyDic[keyType].resetHotKey(newKeyCode);
            }
        }

        /// <summary>
        /// 不联网测试用
        /// </summary>
        //void Update()
        //{
        //    foreach (KeyValuePair<HotKeyType, IHotKey> item in hotKeyDic)
        //    {
        //        if (Input.GetKeyDown(item.Value.getHotKey()))
        //        {
        //            item.Value.getHotKeyDownEvent();
        //        }
        //    }
        //}

        public void UpDate()
        {
            foreach (KeyValuePair<HotKeyType, IHotKey> item in hotKeyDic)
            {
                if (Input.GetKeyDown(item.Value.getHotKey()))//item.Value!=null && 
                {
                    item.Value.getHotKeyDownEvent();
                }
            }
        }

        void OnGUI()
        {
            //获取被按下的按键的键名，该方法将用于做快捷键管理器的界面中/
            if (Input.anyKeyDown)
            {
                UnityEngine.Event e = UnityEngine.Event.current;
                if (e.isKey)
                {
                    Debug.Log(e.keyCode.ToString());
                }
            }
        }


        public override void open()
        {
            Debug.Log("打开热键窗口");
        }

        public override void close()
        {
            Debug.Log("关闭热键窗口");
        }

        public override int Depth
        {
            get
            {
                Debug.Log("暂未实现");
                return 1;
            }
            set
            {
                Debug.Log("暂未实现" + ";value=" + value);
            }
        }
    }


    public enum HotKeyType
    {
        Skill0,  //对应1号技能位/
        Skill1,  //对应2号技能位/
        Skill2,  //对应3号技能位/
        Skill3,  //对应4号技能位/
        Skill4,  //对应5号技能位/
        Skill5,  //对应6号技能位/
        Skill6,  //对应7号技能位/
        Skill7,  //对应8号技能位/
        BigSkill0,  //对应1号大招技能位/
        BigSkill1,  //对应2号大招技能位/
        BigSkill2,  //对应3号大招技能位/
        SkillUI,    //技能面板/
        Equip,      //装备/
        Map,        //地图/
        Role,       //角色/
        Bag,        //背包/
        Pet,        //宠物/
        Mounts,     //坐骑/
        Guild,      //公会/
        Team,       //组队/
        Treasure,   //宝具/
        Mall        //商城/
    }




    public interface IHotKey
    {
        /// <summary>
        /// 获取快捷键键值的方法/
        /// </summary>
        /// <returns>返回一个快捷键键值</returns>
        KeyCode getHotKey();  

        /// <summary>
        /// 重置一个快捷键的方法/
        /// </summary>
        /// <param name="key"></param>
        void resetHotKey(KeyCode key);

        /// <summary>
        /// 指定快捷键被按下事件的方法
        /// </summary>
        void getHotKeyDownEvent();
    }
}
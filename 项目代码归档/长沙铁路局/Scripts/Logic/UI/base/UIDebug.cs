using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Logic.UI
{
    public class UIDebug : MonoBehaviour
    {

        private static UIDebug instance;

        public static UIDebug Instance
        {
            get
            {
                if (!instance)
                {
                    instance = GameObject.FindObjectOfType<UIDebug>();
                    if (!instance)
                    {
                        GameObject insGo = Instantiate(Resources.Load("Prefabs/UI/UIDebugPanel")) as GameObject;
                        insGo.transform.parent = GameObject.Find("UI Root").transform;
                        insGo.transform.localEulerAngles = Vector3.zero;
                        insGo.transform.localPosition = Vector3.zero;
                        insGo.transform.localScale = Vector3.one;
                        instance = insGo.GetComponent<UIDebug>();
                    }
                }
                return instance;
            }
        }


        private UITextList textList;
        Dictionary<Color136, ColorValue> color136 = new Dictionary<Color136, ColorValue>();


        //void Awake()
        //{
        //    init();
        //}

        //private void init()
        //{
        //    this.textList = this.transform.Find("UIDebug/TextArea").GetComponent<UITextList>();
        //}

        public void log(object obj,Color136 color = Color136.Black)
        {
            if (this.textList == null) this.textList = this.transform.Find("UIDebug/TextArea").GetComponent<UITextList>();
            string content = obj.ToString();
            if (this.color136.Count == 0) color136 = TextStyle.getColor136();
            content = "[" + color136[color].HEX + "]" + content + "[-]";   //设置颜色/
            this.textList.Add(content);
        }

    }
}

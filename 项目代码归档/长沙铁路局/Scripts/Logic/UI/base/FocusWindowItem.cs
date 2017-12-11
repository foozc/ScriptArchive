using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Logic.UI
{
    public class FocusWindowItem:MonoBehaviour
    {

        //最高层的窗口关闭掉之后，底层的窗口的焦点collider没有关掉，导致必须点击一下后才能被激活正常使用/
        //应该要监听最高层窗口的状态，如果成为false之后，下面一层的窗口自动激活/

        private Action<FocusWindowItem> enable = null;
        private Action<FocusWindowItem> disable = null;
        private Action destroy = null;


        public void init(Action<FocusWindowItem> enableAction ,Action<FocusWindowItem> disableAction,Action destroyAction)
        {
            enable = enableAction;
            disable = disableAction;
            destroy = destroyAction;
        }


        /// <summary>
        /// 供类似于MapUI这种并不真的隐藏的对象使用，模拟OnEnable、OnDisable和OnDestroy的方法
        /// </summary>
        /// <param name="functionIndex"></param>
        public void setValue(int functionIndex)
        {
            switch (functionIndex)
            {
                case 0:
                    OnEnable();
                    break;
                case 1:
                    OnDisable();
                    break;
                case 2:
                    OnDisable();
                    OnDestroy();
                    break;
                default:                    
                    break;
            }
        }


        void OnEnable()
        {
            if (enable != null) enable(this);
        }


        void OnDisable()
        {
            if (disable != null) disable(this);
        }


        void OnDestroy()
        {
            if (destroy != null) destroy();
        }



        public static FocusWindowItem getItem(GameObject go)
        {
            FocusWindowItem item = go.GetComponent<FocusWindowItem>();
            if(item==null)
                item = go.AddComponent<FocusWindowItem>();
            return item;
        }
    }
}

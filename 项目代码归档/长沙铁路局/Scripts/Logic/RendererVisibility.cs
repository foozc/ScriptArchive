using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Logic
{
    public class RendererVisibility:MonoBehaviour
    {
        /// <summary>
        /// 状态改变时候执行一次,传出代表当前状态的bool值，可见为true，不可见为false
        /// </summary>
        public Action<bool> OnVisibleState = null;
        /// <summary>
        /// 可见状态下每帧执行,不可见状态不执行
        /// </summary>
        public Action OnWillRenderObj = null;


        /// <summary>
        /// 如果物体可见，每个相机都会调用OnWillRenderObject。【如果scene界面打开，在里面能看到物体，此方法会在game和scene中同时执行，每帧执行】
        /// 这个函数在消隐过程中调用，在渲染所有被消隐的物体之前被调用。可以用这个来创建具有依赖性的渲染纹理，只有在被渲染的物体可见时才更新这个渲染纹理。
        /// </summary>
        void OnWillRenderObject()
        {
            if (OnWillRenderObj != null) OnWillRenderObj();
        }

        /// <summary>
        /// 当脚本宿主被任何摄像机显示时候发送此消息【只有在scene视图和game视图中都可见的时候此方法才会被执行一次】
        /// </summary>
        void OnBecameVisible()
        {
            if (OnVisibleState!=null) OnVisibleState(true);
        }

        /// <summary>
        /// 当脚本宿主不被任何摄像机显示时候发送此消息【只有在scene视图和game视图中都不可见的时候此方法才会被执行一次】
        /// </summary>
        void OnBecameInvisible()
        {
            if (OnVisibleState != null) OnVisibleState(false);
        }


        /// <summary>
        /// 获得一个添加到指定游戏对象上的RendererVisibility的引用
        /// </summary>
        /// <param name="goHasRenderer"></param>
        /// <returns></returns>
        static public RendererVisibility Get(GameObject goHasRenderer)
        {
            Renderer rd = goHasRenderer.GetComponentInChildren<Renderer>();
            if (rd == null)
            {
                Debug.Log("操作失败：您要操作的对象及其子物体身上都不含有renderer组件，请检查！");
                return null;
            }
            RendererVisibility listener = rd.GetComponent<RendererVisibility>();
            if (listener == null) listener = rd.gameObject.AddComponent<RendererVisibility>();
            return listener;
        }
    }
}

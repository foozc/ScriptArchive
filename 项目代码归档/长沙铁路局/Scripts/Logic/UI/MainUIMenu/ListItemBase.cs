using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:列表元素根类
*Author:作者
*
*/
namespace Assets.Scripts.Logic.UI.MainUIMenu
{
    public abstract class ListItemBase<T>
    {
        protected string prefabName;
        protected T t;
        protected GameObject itemObject;
        protected UISprite sprite;
        public ListItemBase(T t)
        {
            this.t = t;
        }
        public abstract void setValue(GameObject obj);
        public string getPrefab()
        {
            return prefabName;
        }


        public GameObject getGameObject()
        {
            return itemObject;
        }

        public T getValue()
        {
            return t;
        }

        public void updateItem(T t)
        {
            this.t = t;
            setValue(itemObject);
        }

        public UISprite getUISprite()
        {
            return sprite;
        }

        public void setUISprite(string spriteName)
        {
            sprite.spriteName = spriteName;
        }


    }
}

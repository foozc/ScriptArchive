using System;
using System.Collections.Generic;
using Assets.Scripts.Logic.UI.MainUIMenu;
using System.Text;
using UnityEngine;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:用户列表元素设置
*Author:作者
*/
namespace Assets.Scripts.VO
{
    public class UserListItem : ListItemBase<User>
    {
        public UserListItem(User user): base(user)
        {
            prefabName = "Prefabs/UI/MyUI/UserItem";
        }
        public override void setValue(UnityEngine.GameObject obj)
        {
            obj.transform.Find("name").GetComponent<UILabel>().text = t.Name;
            obj.transform.Find("pwd").GetComponent<UILabel>().text = t.Pwd;
            obj.transform.Find("sno").GetComponent<UILabel>().text = t.Id.ToString();
            this.sprite = obj.transform.Find("Sprite").GetComponent<UISprite>();
            itemObject = obj;
        }
        public static string getSpriteName(bool isSingular)
        {
            if (isSingular)
                return "white_bar";
            else
                return "volume_bar";
        }
        
    }
}

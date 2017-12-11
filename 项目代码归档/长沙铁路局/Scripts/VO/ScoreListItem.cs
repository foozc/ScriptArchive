using Assets.Scripts.Logic.UI.MainUIMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:分数列表元素设置
*Author:作者
*/
namespace Assets.Scripts.VO
{
    public class ScoreListItem : ListItemBase<Score>
    {
        public ScoreListItem(Score score)
            : base(score)
        {
            prefabName = "Prefabs/UI/MyUI/ScoreItem1";
        }
        public override void setValue(UnityEngine.GameObject obj)
        {
            obj.transform.Find("name").GetComponent<UILabel>().text = t.User.Name;
            obj.transform.Find("sno").GetComponent<UILabel>().text = t.User.Id.ToString();
            obj.transform.Find("SubOne").GetComponent<UILabel>().text = t.SubOne.ToString("0.0");
            obj.transform.Find("SubTwo").GetComponent<UILabel>().text = t.SubTwo.ToString("0.0");
            obj.transform.Find("Score").GetComponent<UILabel>().text = t.Score1.ToString("0.0");
            obj.transform.Find("Date").GetComponent<UILabel>().text = t.Date.ToString("yyyy-MM-dd HH:mm");
            this.sprite = obj.transform.Find("Sprite").GetComponent<UISprite>();
            itemObject = obj;
        }

        public static string getSpriteName(bool isSingular)
        {
            if (isSingular)
                return "down_Grey_bar_c";
            else
                return "down_white_bar_d";
        }

    }
}

using Assets.Scripts.Logic.UI.Login;
using Assets.Scripts.VO;
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
*Desciption:功能说明 ：存储特定用户的属性及配置信息
*Author:作者
*
*/
namespace Assets.Scripts.Configs
{
	public class MySession
    {
        public static int Id;

        public static RoleType Role;
        public static User user;

        public static void Clear()
        {
            Id = 0;
            Role = RoleType.studentCheckbox;
            user = null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:工具栏元素
*Author:作者
*/
namespace Assets.Scripts.VO
{
    public class ToolModel
    {
        private int index;
        private string id;
        private string name;
        private string spriteName;
        private GameObject toolMenuItem;
        /// <summary>
        /// 工具在工具栏中的序号
        /// </summary>
        public int Index
        {
            get
            {
                return index;
            }

            set
            {
                index = value;
            }
        }
        /// <summary>
        /// 工具在数据库中的id
        /// </summary>
        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }
        /// <summary>
        /// 工具名称
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }
       

        /// <summary>
        /// 工具图片名称
        /// </summary>
        public string SpriteName
        {
            get
            {
                return spriteName;
            }

            set
            {
                spriteName = value;
            }
        }
        /// <summary>
        /// 实例之后的工具栏菜单工具的gameobject
        /// </summary>
        public GameObject ToolMenuItem
        {
            get
            {
                return toolMenuItem;
            }

            set
            {
                toolMenuItem = value;
            }
        }
    }
}

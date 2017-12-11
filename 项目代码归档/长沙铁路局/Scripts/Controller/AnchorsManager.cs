using UnityEngine;
using System.Collections;
using Assets.Scripts.Logic;
/*
*�汾��1.0
*Copyright 2013-2016 ���人�ɸ�����Ƽ����޹�˾
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:����˵����ê�������
*Author:����
*
*/
namespace Assets.Scripts.Controller
{
	public enum AnchorType
    {
        topLeft = 0,
        topCenter,
        topRight,
        bottomLeft,
        bottomCenter,
        bottomRight,
        Left,
        Center,
        Right
    }
    public class AnchorsManager : SingletonUI<AnchorsManager>
    {
        public GameObject topLeft;
        public GameObject topCenter;
        public GameObject topRight;
        public GameObject bottomLeft;
        public GameObject bottomCenter;
        public GameObject bottomRight;
        public GameObject Left;
        public GameObject Center;
        public GameObject Right;

        public GameObject getAnchorTransform(AnchorType type)
        {
            GameObject flag = null;
            switch (type)
            {
                case AnchorType.bottomCenter:
                    flag = bottomCenter;
                    break;
                case AnchorType.bottomLeft:
                    flag = bottomLeft;
                    break;
                case AnchorType.bottomRight:
                    flag = bottomRight;
                    break;
                case AnchorType.topRight:
                    flag = topRight;
                    break;
                case AnchorType.topCenter:
                    flag = topCenter;
                    break;
                case AnchorType.topLeft:
                    flag = topLeft;
                    break;
                case AnchorType.Left:
                    flag = Left;
                    break;
                case AnchorType.Center:
                    flag = Center;
                    break;
                case AnchorType.Right:
                    flag = Right;   
                    break;
            }
            return flag;
        }

         
    }
}
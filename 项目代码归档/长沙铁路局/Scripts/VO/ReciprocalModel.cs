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
*Desciption:功能说明:模型关联数据
*Author:作者
*/
namespace Assets.Scripts.VO
{
    public class ReciprocalModel
    {
        private string model;

        public string Model
        {
            get { return model; }
            set { model = value; }
        }

        private GameObject modelObject;

        public GameObject ModelObject
        {
            get { return modelObject; }
            set { modelObject = value; }
        }

        private Dictionary<string, ReciprocalModel> reciprocal = new Dictionary<string,ReciprocalModel>();

        public Dictionary<string, ReciprocalModel> Reciprocal
        {
            get { return reciprocal; }
        }


        private Vector3 offset;

        public Vector3 Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        private Vector2 limitYaw = Vector2.zero;

        public Vector2 LimitYaw
        {
            get { return limitYaw; }
            set { limitYaw = value; }
        }
        private Vector2 limitPitch = Vector2.zero;

        public Vector2 LimitPitch
        {
            get { return limitPitch; }
            set { limitPitch = value; }
        }
        private Vector2 limitDistance = Vector2.zero;

        public Vector2 LimitDistance
        {
            get { return limitDistance; }
            set { limitDistance = value; }
        }
    }
}

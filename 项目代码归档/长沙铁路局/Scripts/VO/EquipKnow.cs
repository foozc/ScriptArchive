using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:设备认知属性
*Author:作者
*/
namespace Assets.Scripts.VO
{
    public class EquipKnow
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _model;

        public string Model
        {
            get { return _model; }
            set { _model = value; }
        }
        private string _detail;

        public string Detail
        {
            get { return _detail; }
            set { _detail = value; }
        }
        private string _warm;

        public string Warm
        {
            get { return _warm; }
            set { _warm = value; }
        }

        private Vector3 _offset;

        public Vector3 Offset
        {
            get { return _offset; }
            set { _offset = value; }
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

        private UnityEngine.GameObject _modelObject;

        public UnityEngine.GameObject ModelObject
        {
            get { return _modelObject; }
            set { _modelObject = value; }
        }

		private string _audio;
		public string audio
		{
			get { return _audio; }
			set { _audio = value; }
		}
		private string _showInfo;
		public string showInfo
		{
			get { return _showInfo; }
			set { _showInfo = value; }
		}
		private List<string> _transparent=new List<string>();
		public List<string> transparent
		{
			get { return _transparent; }
			set { _transparent = value; }
		}
		private List<EquipKnow> _equipKnows = new List<EquipKnow>();

        public List<EquipKnow> EquipKnows
        {
            get { return _equipKnows; }
        }

        private EquipKnow _parentEquipKnow;

        public EquipKnow ParentEquipKnow
        {
            get { return _parentEquipKnow; }
            set { _parentEquipKnow = value; }
        }
    }
}

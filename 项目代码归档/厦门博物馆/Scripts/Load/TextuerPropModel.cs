using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Assets.Scripts.Load
{
    enum MusicTEYP
    {
        FolkMusic,
        StringedMusic,
        WoodwindMusic,
    };
    
    class TextuerPropModel
    {


        private static TextuerPropModel _instance = null;
        public List<TextuerProp> listTextuerProp;
        public List<string> listContent;
        public string TEYP;
        TextuerPropModel()
        {
            listTextuerProp = new List<TextuerProp>(0);
            listContent = new List<string>(0);
        }
        public static TextuerPropModel GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TextuerPropModel();
            }
            return _instance;
        }


        public void readTask(XmlNode xml)
        {
            TextuerProp prop = new TextuerProp();
            prop.number = int.Parse(xml.Attributes["number"].Value);
            prop.MusicName = xml.Attributes["name"].Value;
            prop.TEYP = xml.Attributes["type"].Value;
            prop.textureSong = xml.Attributes["texture1"].Value;
            prop.textureroll = xml.Attributes["texture2"].Value;
            prop.texture1 = xml.Attributes["texture3"].Value;
            prop.num = int.Parse(xml.Attributes["num"].Value);
            listTextuerProp.Add(prop);
        }

        public void readContent(XmlNode xml)
        {
           
            string texture1 = xml.Attributes["content"].Value;

            listContent.Add(texture1);
        }
    }
}

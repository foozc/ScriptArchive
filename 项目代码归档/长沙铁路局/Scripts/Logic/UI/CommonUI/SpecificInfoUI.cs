using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace Assets.Scripts.Logic.UI.CommonUI
{
    public class SpecificInfoUI : UIbase
    {
        public UILabel content;
        public UILabel titleName;
       
        private bool isShow = true;
        public override int Depth
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void setValue(string text, string title)
        {
            if (!isShow)
            {
                isShow = true;
                SpringPosition sp = SpringPosition.Begin(gameObject, new Vector3(50f, 0f, 0f), 15f);
            }
            //TypewriterEffect type = content.GetComponent<TypewriterEffect>();
            //if (type != null)
            //    Destroy(type);

            TypewriterEffect[] types = content.GetComponents<TypewriterEffect>();
            for (int i = types.Length -1; i >= 0; i--)
            {
                Destroy(types[i]);
            }


            titleName.text = title;
            content.text = text;
            TypewriterEffect effect = content.gameObject.AddComponent<TypewriterEffect>();
            effect.fadeInTime = 1;
            effect.delayOnPeriod = 1;
        }

        public void showSpInfoUI()
        {
            if (isShow)
            {
                isShow = false;
                SpringPosition sp = SpringPosition.Begin(gameObject, new Vector3(-359f, 0f, 0f), 15f);
            }
            else
            {
                isShow = true;
                SpringPosition sp = SpringPosition.Begin(gameObject, new Vector3(44f, 0f, 0f), 15f);
            }
        }

    }
}

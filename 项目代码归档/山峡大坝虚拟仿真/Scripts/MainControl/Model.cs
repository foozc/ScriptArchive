using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.MainControl
{
    public class Model
    {
        private string name;
        private string uiContent;
        private string modeName;
        private bool isShow = false;

        public string UiContent
        {
            get
            {
                return uiContent;
            }

            set
            {
                uiContent = value;
            }
        }

        public string ModeName
        {
            get
            {
                return modeName;
            }

            set
            {
                modeName = value;
            }
        }

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

        public bool IsShow
        {
            get
            {
                return isShow;
            }

            set
            {
                isShow = value;
            }
        }
    }
}

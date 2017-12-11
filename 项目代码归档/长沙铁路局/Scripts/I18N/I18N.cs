using UnityEngine;
using System.Collections;

using Assets.Scripts.Logic;

namespace Assets.Scripts.I18N
{
    public class I18N : MonoBehaviour, IObserver
    {
        private UILabel label = null;
        private int fontSize = 12;

        // Use this for initialization
        void Start()
        {
            label = GetComponent<UILabel>();
            if (label == null)
                throw new System.Exception("Not found!");
            if (label.bitmapFont == null)
                throw new System.Exception("Not found font!");
            fontSize = label.bitmapFont.defaultSize;

            I18NManager.getInstance().addObserver(this);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnDestroy()
        {
            I18NManager.getInstance().deleteObserver(this);
        }

        public void update(Observable o, params object[] args)
        {
            I18NManager mgr = o as I18NManager;
            label.bitmapFont = mgr.getFont(fontSize);
            label.MakePixelPerfect();
        }
    }
}
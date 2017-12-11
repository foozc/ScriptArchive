using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Logic.UI;
using System.Collections;

namespace Assets.Scripts.Logic.UI
{
    public class HotKeyTest : MonoBehaviour, IHotKey
    {
        public HotKeyType keyType = HotKeyType.Skill0;
        public KeyCode keyCode = KeyCode.Space;

        void Start()
        {
            Camera.main.GetComponent<HotKeyManager>().addHotKey(keyType, this);
            UIEventListener.Get(this.gameObject).onHover = mouseOnHover;
        }

        private void mouseOnHover(GameObject go, bool state)
        {
            if (state)
            {
                StartCoroutine(getKeyDown());
            }
            else
            {
                StopAllCoroutines();
            }
        }


        private IEnumerator getKeyDown()
        {
            while(true)
            {
                if(Input.GetKeyDown(keyCode))
                {
                    this.GetComponent<UILabel>().text = keyCode.ToString();
                }
                yield return null;
            }
        }


        void OnGUI()
        {
            if(Input.anyKeyDown)
            {
                UnityEngine.Event e = UnityEngine.Event.current;
                if(e.isKey)
                {
                    this.GetComponent<UILabel>().text = e.keyCode.ToString();
                }
            }
        }


        void OnMouseEnter()
        {
            Debug.Log("*******************");
        }


        KeyCode IHotKey.getHotKey()
        {
            return keyCode;
        }

        void IHotKey.resetHotKey(KeyCode key)
        {
            this.keyCode = key;
        }

        void IHotKey.getHotKeyDownEvent()
        {
            Debug.Log(keyType.ToString()+"== is down");
        }
    }
}

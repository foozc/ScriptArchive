using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Assets.Scripts.Controller;

namespace Assets.Scripts.Logic.UI.Login
{
    public  class MyLogin: MonoBehaviour
    {
        void Start()
        {
            UIManager.getInstance().loading.GetComponent<Loading>().myStart();
        }
    }
}

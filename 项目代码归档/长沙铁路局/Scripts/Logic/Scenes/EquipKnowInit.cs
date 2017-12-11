using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Assets.Scripts.Controller;

namespace Assets.Scripts.Logic.Scenes
{
    public class EquipKnowInit : MonoBehaviour
    {
        void Start()
        {
            ScreenRaycaster raycaster = GameObject.Find("FingerGestures").GetComponent<ScreenRaycaster>();
            raycaster.Cameras = new Camera[2];
            raycaster.Cameras[0] = GameObject.Find("Main Camera").GetComponent<Camera>();
            raycaster.Cameras[1] = GameObject.FindObjectOfType<Main>().GetComponent<Camera>();

            raycaster.Cameras[0].tag = "MainCamera";
            raycaster.Cameras[0].GetComponent<Camera>().depth = -1;
            raycaster.Cameras[1].tag = "Untagged";
            raycaster.Cameras[1].GetComponent<Camera>().depth = -2;

        }
    }
}

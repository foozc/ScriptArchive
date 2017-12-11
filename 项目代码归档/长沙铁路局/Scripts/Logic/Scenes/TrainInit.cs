using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using PluginEvent;
using Assets.Scripts.Controller;
using Training;
using Assets.Scripts.Configs;

namespace Assets.Scripts.Logic.Scenes
{
    public class TrainInit : MonoBehaviour
    {
        void Start()
        {

            initCamer();
            if (UIManager.getInstance().getCurrentUIType() != UIType.GuideUI )
            {
                UIManager.getInstance().trainUI.showTrainStart();
            }
        }


        public void initCamer()
        {
            ScreenRaycaster raycaster = GameObject.Find("FingerGestures").GetComponent<ScreenRaycaster>();
            raycaster.Cameras[0].transform.position = new Vector3(14.5f, 1, -130);
            raycaster.Cameras[0].transform.rotation = Quaternion.Euler(20, 0, 0);
			if (UIManager.getInstance().modename=="设备故障")
			{
				raycaster.Cameras[0].transform.position = new Vector3(-1.5f, 1.1f, 6.74f);
				raycaster.Cameras[0].transform.rotation = Quaternion.Euler(10, -30,-2);
				GameObject.Find("Camera").transform.position= new Vector3(-1.5f, 1.1f, 6.74f);
			}

            //ScreenRaycaster raycaster = GameObject.Find("FingerGestures").GetComponent<ScreenRaycaster>();
            //raycaster.Cameras = new Camera[2];
            //raycaster.Cameras[0] = GameObject.Find("Main Camera").GetComponent<Camera>();
            //raycaster.Cameras[1] = GameObject.FindObjectOfType<Main>().GetComponent<Camera>();

            //raycaster.Cameras[0].tag = "MainCamera";
            //raycaster.Cameras[0].GetComponent<Camera>().depth = -1;
            //raycaster.Cameras[1].tag = "Untagged";
            //raycaster.Cameras[1].GetComponent<Camera>().depth = -2;
        }
    }
}

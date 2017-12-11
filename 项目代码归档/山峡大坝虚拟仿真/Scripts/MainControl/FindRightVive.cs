using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class FindRightVive : MonoBehaviour
{
    public GameObject newCamera;
    // Update is called once per frame  
    void Update()
    {
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Menu))
        {
            if (newCamera.GetComponent<CameraRay>().enabled)
            {
                newCamera.GetComponent<CameraRay>().enabled = false;
            }
            else
            {
                newCamera.GetComponent<CameraRay>().enabled = true;
            }
            Debug.Log("Right menu press down");
        }
        if (ViveInput.GetPressDown(HandRole.LeftHand, ControllerButton.Menu))
        {
            if (newCamera.GetComponent<CameraRay>().enabled)
            {
                newCamera.GetComponent<CameraRay>().enabled = false;
            }
            else
            {
                newCamera.GetComponent<CameraRay>().enabled = true;
            }
            Debug.Log("Left menu press down");
        }
    }
}
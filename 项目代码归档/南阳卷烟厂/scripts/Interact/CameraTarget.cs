/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	CameraTarget 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017��10��10��18:04:05# 
 *Description: 		   	������岶׽���ų�UI�ĳ�ͻ��������Ϸ����ʱ����������Ч������
 *History: 				�޸İ汾��¼
*/

using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
public class CameraTarget : MonoBehaviour
{
    // Which layers targeting ray must hit (-1 = everything)
    public LayerMask targetingLayerMask = -1;

    // Targeting ray length
    public float targetingRayLength = 3f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        //�������UI��ʱ�������ƶ���ʱ��ִ����ز������������ߣ�
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (Dialog._instance.ObjectNameUI) { Dialog._instance.ObjectNameUI.SetActive(false); }
            return;
        }
        var directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (directionVector != Vector3.zero) { return; }
        TargetingRaycast();
    }

    public void TargetingRaycast()
    {
        // Current target object transform component
        Transform targetTransform = null;
        // If camera component is available
        if (cam != null)
        {
            RaycastHit hitInfo;
            // Create a ray from mouse coords
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            // Targeting raycast
            if (Physics.Raycast(ray, out hitInfo, targetingRayLength, targetingLayerMask.value))
            {
                // Cache what we've hit
                if (hitInfo.collider.tag == "HighlightObject")
                {
                    targetTransform = hitInfo.collider.transform;

                    SceneObjects._instance.ShowObjectName(targetTransform);

                    if (Input.GetButtonDown("Fire1"))  //�������
                    {
                        SceneObjects._instance.ShowObjectInfo(targetTransform);
                    }
                    if (Input.GetButtonUp("Fire2"))   //�����Ҽ�
                    {
                        if (Dialog._instance.ObjectNameUI) { Dialog._instance.ObjectNameUI.SetActive(false); }
                    }
                }
                else { if (Dialog._instance.ObjectNameUI) { Dialog._instance.ObjectNameUI.SetActive(false); } }
            }
            else
            {
                if (Dialog._instance.ObjectNameUI) { Dialog._instance.ObjectNameUI.SetActive(false); }
            }
        }
    }

}

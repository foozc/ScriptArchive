﻿using HTC.UnityPlugin.ColliderEvent;
using HTC.UnityPlugin.Utility;
using HTC.UnityPlugin.Vive;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MyMaterialChanger : MonoBehaviour
    , IColliderEventHoverEnterHandler
    , IColliderEventHoverExitHandler
    , IColliderEventPressEnterHandler
    , IColliderEventPressExitHandler
{
    private readonly static List<Renderer> s_rederers = new List<Renderer>();

    [NonSerialized]
    private Material currentMat;

    public Material Normal;
    public Material Heightlight;
    public Material Pressed;
    public Material dragged;
    public GameObject User;
    public int DownOrUp;

    public ControllerButton heighlightButton = ControllerButton.Trigger;

    private HashSet<ColliderHoverEventData> hovers = new HashSet<ColliderHoverEventData>();
    private HashSet<ColliderButtonEventData> presses = new HashSet<ColliderButtonEventData>();
    private IndexedSet<ColliderButtonEventData> drags = new IndexedSet<ColliderButtonEventData>();

    private void Start()
    {
        UpdateMaterialState();
    }

    public void OnColliderEventHoverEnter(ColliderHoverEventData eventData)
    {
        hovers.Add(eventData);
        Debug.Log("move in");
        //if(int.Parse(this.name.Substring(10, this.name.Length - 10))%2 == 0)
        if (DownOrUp == 1)
        {
            User.transform.position = new Vector3(User.gameObject.transform.position.x, User.gameObject.transform.position.y + 1, User.gameObject.transform.position.z);
        }
        else
        {
            User.transform.position = new Vector3(User.gameObject.transform.position.x, User.gameObject.transform.position.y - 1, User.gameObject.transform.position.z);
        }
        UpdateMaterialState();
    }

    public void OnColliderEventHoverExit(ColliderHoverEventData eventData)
    {
        hovers.Remove(eventData);
        Debug.Log("move out");
        UpdateMaterialState();
    }

    public void OnColliderEventPressEnter(ColliderButtonEventData eventData)
    {
        Debug.Log("press on");
        if (DownOrUp == 0)
        {
            User.transform.position = new Vector3(User.gameObject.transform.position.x, User.gameObject.transform.position.y - 1, User.gameObject.transform.position.z);
        }
        if (!eventData.IsViveButton(heighlightButton)) { return; }

        presses.Add(eventData);

        // check if this evenData is dragging me(or ancestry of mine)
        for (int i = eventData.draggingHandlers.Count - 1; i >= 0; --i)
        {
            if (transform.IsChildOf(eventData.draggingHandlers[i].transform))
            {
                drags.AddUnique(eventData);
                break;
            }
        }

        UpdateMaterialState();
    }

    public void OnColliderEventPressExit(ColliderButtonEventData eventData)
    {
        Debug.Log("press up");
        presses.Remove(eventData);

        UpdateMaterialState();
    }

    private void LateUpdate()
    {
        UpdateMaterialState();
    }

    private void OnDisable()
    {
        hovers.Clear();
        presses.Clear();
        drags.Clear();
    }

    private void UpdateMaterialState()
    {
        var targetMat = default(Material);

        if (drags.Count > 0)
        {
            drags.RemoveAll(e => !e.isDragging);
        }

        if (drags.Count > 0)
        {
            targetMat = dragged;
        }
        else if (presses.Count > 0)
        {
            targetMat = Pressed;
        }
        else if (hovers.Count > 0)
        {
            targetMat = Heightlight;
        }
        else
        {
            targetMat = Normal;
        }

        if (ChangeProp.Set(ref currentMat, targetMat))
        {
            SetChildRendererMaterial(targetMat);
        }
    }

    private void SetChildRendererMaterial(Material targetMat)
    {
        GetComponentsInChildren(true, s_rederers);

        if (s_rederers.Count > 0)
        {
            for (int i = s_rederers.Count - 1; i >= 0; --i)
            {
                s_rederers[i].sharedMaterial = targetMat;
            }

            s_rederers.Clear();
        }
    }
}

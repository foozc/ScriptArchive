using UnityEngine;
using System.Collections;
using Assets.Scripts.Controller;

public class MenuInit : MonoBehaviour {

	// Use this for initialization
	void Start () {
        CoherentUISystem.Instance.UIViews.Remove(UIManager.getInstance().trainUI.examGrade);
        UIManager.getInstance().trainUI.examGrade.GetComponent<Camera>().depth = -50;
    }

    // Update is called once per frame
    void Update () {
	    
	}
}

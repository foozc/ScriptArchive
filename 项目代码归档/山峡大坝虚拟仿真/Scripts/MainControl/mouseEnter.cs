using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseEnter : MonoBehaviour {
    public GameObject FirstCube;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnMouseEnter()
    {
        FirstCube.SetActive(false);
    }
    private void OnMouseExit()
    {

    }
}

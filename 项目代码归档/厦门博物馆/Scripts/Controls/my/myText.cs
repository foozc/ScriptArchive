using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class myText : MonoBehaviour {
    public GameObject Label;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Label.SetActive(false);
        Label.SetActive(true);
    }
}

using UnityEngine;
using System.Collections;

public class LoopAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<Animation>().wrapMode = WrapMode.Loop;
	}
	
	// Update is called once per frame
	void Update () {
	}
}

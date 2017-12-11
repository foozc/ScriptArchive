using UnityEngine;
using System.Collections;

public class AnimationSpeed : MonoBehaviour {

    public float speed = 0.1f;
	// Use this for initialization
	void Start () {
        gameObject.GetComponents<AudioSource>();
        GetComponent<Animation>()[GetComponent<Animation>().clip.name].speed = speed;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

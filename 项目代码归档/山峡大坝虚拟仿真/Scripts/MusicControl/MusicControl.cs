using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour {

    // Use this for initialization
    public static MusicControl _instance;
    public AudioClip[] _clips;

    private void Awake()
    {
        _instance = this;
    }

    public void setMusic(int i)
    {
        GetComponent<AudioSource>().clip = _clips[i];
        
    }

    public void playMusic()
    {
        GetComponent<AudioSource>().Play();
    }

    public void stopMusic()
    {
        GetComponent<AudioSource>().Stop();
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

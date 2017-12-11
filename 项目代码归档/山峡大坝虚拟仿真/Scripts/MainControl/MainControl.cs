using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HTC.UnityPlugin.ColliderEvent;
using HTC.UnityPlugin.Utility;
using HTC.UnityPlugin.Vive;
using UnityEngine.SceneManagement;


public class MainControl : MonoBehaviour {
    //public GameObject moveButton;
    /// <summary>
    /// barque control
    /// </summary>
    public Button advanceButton;
    public Button backButton;
    public Button stopButton;
    public GameObject carry;

    ///

    /// <summary>
    /// scene change
    /// </summary>
    public GameObject Scene_MainUI;
    public GameObject Scene_move_001;
    public GameObject Scene_move_002;

    public GameObject move_001_cameraOut;
    public GameObject move_001_cameraIn;

    /// <summary>
    /// speed
    /// </summary>
    public float speed;
    private bool moveOn = false;

    public Button backMain;

	// Use this for initialization
	void Start () {
        //moveButtonListen();
        //string name = PlayerPrefs.GetString("name");
       
    }
	
	// Update is called once per frame
	void Update () {
		
	}

 

    public void moveButtonListen()
    {
        float step = speed * Time.deltaTime;

        advanceButton.onClick.AddListener(delegate
        {
            moveOn = true;
            StartCoroutine(MoveToPosition());
        });

        backButton.onClick.AddListener(delegate
        {
            moveOn = true;
            StartCoroutine(BackToPosition());
        });

        stopButton.onClick.AddListener(delegate
        {
            StartCoroutine(StopToPosition());
        });

        backMain.onClick.AddListener(delegate
        {
            backMainScene();
        });
    }

    public void backMainScene()
    {
        Scene_MainUI.SetActive(true);
        Scene_move_001.SetActive(false);
        Scene_move_002.SetActive(false);
        move_001_cameraOut.SetActive(false);
        move_001_cameraIn.SetActive(false);
        //SceneManager.LoadScene("MainUI");
    }

    IEnumerator MoveToPosition()
    {
        while (Math.Abs(carry.transform.position.x - 20) > 0.0001)
        {
            if (moveOn)
            {
                carry.transform.position = Vector3.MoveTowards(carry.transform.position, new Vector3(20, 0, 0), speed * Time.deltaTime);
            }
            yield return 0;
        }
        
    }
    IEnumerator BackToPosition()
    {
        while (Math.Abs(carry.transform.position.x - 0) > 0.0001)
        {
            if (moveOn)
            {
                carry.transform.position = Vector3.MoveTowards(carry.transform.position, new Vector3(0, 0, 0), speed * Time.deltaTime);                
            }
            yield return 0;
        }
    }
    IEnumerator StopToPosition()
    {
        Debug.Log("stop");
        moveOn = false;
        yield return 0;
    }
}

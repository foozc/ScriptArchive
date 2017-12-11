using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HTC.UnityPlugin.ColliderEvent;
using HTC.UnityPlugin.Utility;
using HTC.UnityPlugin.Vive;
using UnityEngine.SceneManagement;


public class Move_002Control : MonoBehaviour
{
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
    /// first cube
    /// </summary>
    public GameObject FirstCube;
    //public Text FirstCubeText;
    public GameObject FirstCubeText;

    /// <summary>
    /// speed
    /// </summary>
    public float speed;
    private bool moveOn = false;

    public Button backMain;

    // Use this for initialization
    void Start()
    {
        moveButtonListen();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FirstCubeSetActive()
    {
        FirstCubeText.SetActive(true);
    }

    public void FirstCubeOffActive()
    {
        FirstCubeText.SetActive(false);
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
        SceneManager.LoadScene("MainUI");
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

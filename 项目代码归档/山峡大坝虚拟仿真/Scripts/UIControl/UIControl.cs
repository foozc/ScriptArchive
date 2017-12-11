using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HTC.UnityPlugin.ColliderEvent;
using HTC.UnityPlugin.Utility;
using HTC.UnityPlugin.Vive;
using UnityEngine.SceneManagement;
using Assets.Scripts.MainControl;

public class UIControl : MonoBehaviour
{
    public Image shangHint;
    public Image shangQianHint;
    public Image shangShangHint;
    public Image xiaHint;
    public Image xiaQianHint;
    public Image xiaShangHint;
    public GameObject yuanchu;
    public GameObject jinchu;
    public Animation huanrao;
    public Animation shang;
    public Animation xia;

    public Button SceneButton_000;
    public Button SceneButton_001;
    public Button SceneButton_002;

    /// <summary>
    /// scene change
    /// </summary>
    public GameObject Scene_MainUI;
    public GameObject Scene_move_001;
    public GameObject Scene_move_002;

    public GameObject move_001_cameraOut;
    public GameObject move_001_cameraIn;

    /// <summary>
    /// barque 0 and 1
    /// </summary>
    public GameObject barque0;
    public GameObject barque1;
    public GameObject barque3;

    public int musicNum;
    public MusicControl musicControl;

    //public GameObject Nanimation;
    private Animation m_ani;

    // Use this for initialization
    void Start()
    {
        SceneButtonListen();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void SceneButtonListen()
    {

        SceneButton_000.onClick.AddListener(delegate
        {
            gotoScene_000();
        });

        SceneButton_001.onClick.AddListener(delegate
        {
            gotoScene_001();
        });

        SceneButton_002.onClick.AddListener(delegate
        {
            gotoScene_002();
        });
    }

    public void gotoScene_000()
    {
        yuanchu.SetActive(true);
        jinchu.SetActive(false);
        barque3.SetActive(true);
        barque0.SetActive(false);
        barque1.SetActive(false);
        musicControl.setMusic(0);
        musicControl.playMusic();
        huanrao.Stop("Take 001");
        shang.Stop("Take 001");
        xia.Stop("Take 001");
        //Nanimation.GetComponent<Animation>().Stop("Take 001");
        //Nanimation.GetComponent<Animation>().Stop("Take 002");
        //Nanimation.GetComponent<Animation>().Stop("Take 003");
        //m_ani = Nanimation.GetComponent<Animation>();
        //foreach (AnimationState state in m_ani)
        //{
        //    state.speed = 0.5f;
        //}
        //Nanimation.GetComponent<Animation>().Play("Take 003");
        huanrao.Play("Take 001");
        PlayerPrefs.SetString("name", "0");
        Scene_MainUI.SetActive(false);
        Scene_move_001.SetActive(false);
        Scene_move_002.SetActive(false);
        move_001_cameraOut.SetActive(true);
        move_001_cameraIn.SetActive(false);
        //SceneManager.LoadScene("move_001");
    }
    public void gotoScene_001()
    {
        foreach(Model mode in DataManager.instance.getModes().Values)
        {
            mode.IsShow = false;
        }
        yuanchu.SetActive(false);
        jinchu.SetActive(true);
        barque0.SetActive(true);
        barque1.SetActive(false);
        barque3.SetActive(false);

        musicControl.setMusic(2);
        musicControl.playMusic();
        DataManager.instance.startTime(shangHint, shangQianHint, shangShangHint, true);


        huanrao.Stop("Take 001");
        shang.Stop("Take 001");
        xia.Stop("Take 001");
        //Nanimation.GetComponent<Animation>().Stop("Take 001");
        //Nanimation.GetComponent<Animation>().Stop("Take 002");
        //Nanimation.GetComponent<Animation>().Stop("Take 003");
        //m_ani = Nanimation.GetComponent<Animation>();
        //foreach (AnimationState state in m_ani)
        //{
        //    state.speed = 0.5f;
        //}
        //Nanimation.GetComponent<Animation>().Play("Take 001");
        shang.Play("Take 001");
        PlayerPrefs.SetString("name", "1");
        Scene_MainUI.SetActive(false);
        Scene_move_001.SetActive(true);
        Scene_move_002.SetActive(false);
        move_001_cameraOut.SetActive(false);
        move_001_cameraIn.SetActive(true);
        //SceneManager.LoadScene("move_001");
    }
    public void gotoScene_002()
    {
        foreach (Model mode in DataManager.instance.getModes().Values)
        {
            mode.IsShow = false;
        }
        yuanchu.SetActive(false);
        jinchu.SetActive(true);
        barque0.SetActive(false);
        barque1.SetActive(true);
        barque3.SetActive(false);

        musicControl.setMusic(1);
        musicControl.playMusic();
        DataManager.instance.startTime(xiaHint, xiaQianHint, xiaShangHint, true);
        huanrao.Stop("Take 001");
        shang.Stop("Take 001");
        xia.Stop("Take 001");
        //Nanimation.GetComponent<Animation>().Stop("Take 001");
        //Nanimation.GetComponent<Animation>().Stop("Take 002");
        //Nanimation.GetComponent<Animation>().Stop("Take 003");
        //m_ani = Nanimation.GetComponent<Animation>();
        //foreach (AnimationState state in m_ani)
        //{
        //    state.speed = 0.5f;
        //}
        //Nanimation.GetComponent<Animation>().Play("Take 002");
        xia.Play("Take 001");
        PlayerPrefs.SetString("name", "2");
        Scene_MainUI.SetActive(false);
        Scene_move_001.SetActive(false);
        Scene_move_002.SetActive(true);
        move_001_cameraOut.SetActive(false);
        move_001_cameraIn.SetActive(false);
        //SceneManager.LoadScene("move_002");
    }
 
}

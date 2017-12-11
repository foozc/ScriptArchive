using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using System;

public class MainUIControl : MonoBehaviour {

    public GameObject MainUI;
    public GameObject MenuUI;
    public GameObject MainUIMask;
    public GameObject MenuUIMask;
    /// <summary>
    /// button animation
    /// </summary>
    private bool isShowButtonAnimation = false;
    private GameObject buttonName;
    private float buttonY = 200;

    public GameObject musicalPiano;
    public GameObject musicalViolin;
    public GameObject musicalPipa;
    public GameObject musicalErhu;
    public GameObject musicalSuona;
    public GameObject musicalGuzheng;
    public GameObject musicalDatiqin;
    public GameObject muscialGuanxuanyue;

    /// <summary>
    /// main to menu animation 
    /// </summary>
    private string[] _paths;
    public MovieManager _MovieManager;
    private Action<string> showMenuUI;
    private Action<string> showMainUI;

    /// <summary>
    /// avPro
    /// </summary>
    public GameObject avPlane;
    private float avPlaneAlpha = 0;


    private bool isAvPlane = false;


    ///
    private bool endAvPlane = false;

    /// <summary>
    /// bganimation
    /// </summary>
    public MovieManager BgMovieManager;
    public MovieManager Bg2MovieManager;

    public GameObject bgPlane;
    public GameObject bg2Plane;
    private float bgPlaneAlpha = 0;
    private float bg2PlaneAlpha = 0;

    /// <summary>
    /// backNum
    /// </summary>
    private int AnimationNum;
    private string ButtonName;
    private int Iwidth = 1920;
    private int Iheight = 1080;


    /// <summary>
    /// MenuButton
    /// </summary>
    public GameObject MenuLabel;
    private bool isMenuButton;
    private float MenubuttonY = 0;
    public GameObject Menu1;
    public GameObject Menu2;
    public GameObject Menu3;
    public GameObject MenuMask;
    private bool isAnimationMenu;

    public EnhanceScrollView EnhanceScrollView1;
    public EnhanceScrollView EnhanceScrollView2;
    public EnhanceScrollView EnhanceScrollView3;
    //
    // Use this for initialization
    void Start()
    {
        Screen.SetResolution(Iwidth, Iheight, true);
        Screen.fullScreen = true;
        QualitySettings.SetQualityLevel(QualitySettings.names.Length - 1, true);

        isAnimationMenu = true;
        _paths = new string[7];
        _paths[0] = "MainUI.mp4";
        _paths[1] = "MainUIToMenuUI.mp4";
        _paths[2] = "MenuUIToMainUI.mp4";
        _paths[3] = "BigBuckBunny_720p30.mp4";
        _paths[4] = "SampleCube.mp4";
        _paths[5] = "SampleSphere.mp4";
        _paths[6] = "shouye_BJ.mp4";
    }

    // Update is called once per frame
    void Update() {

        ///showButton
        if (buttonY == 0)
        {
            isShowButtonAnimation = false;
            buttonY = 200;
            buttonName.GetComponentInChildren<UIButton>().isEnabled = true;

        }
        if (isShowButtonAnimation)
        {
            showButtonAnimation(buttonName);
        }

        if (MenubuttonY == -720)
        {
            isMenuButton = false;
            MenubuttonY = 0;
        }
        if (isMenuButton)
        {
            MenuButtonAnimation();
        }


        ///show changeAnimation
        if (isAvPlane)
        {
            avPlaneAnimation();
        }
        ///end changeAnimation
        ///
        if (endAvPlane)
        {
            endavPlaneAnimation();
        }

    }

    /// <summary>
    /// initMainUIButton
    /// </summary>
    public void MainUIbuttonInit()
    {
        isShowButtonAnimation = false;
        buttonY = 200;
        musicalPiano.GetComponentInChildren<UIPanel>().SetRect(0, 200, 82, 200);
        musicalPiano.GetComponentInChildren<UISprite>().spriteName = "transparent";

        musicalViolin.GetComponentInChildren<UIPanel>().SetRect(0, 200, 82, 200);
        musicalViolin.GetComponentInChildren<UISprite>().spriteName = "transparent";

        musicalPipa.GetComponentInChildren<UIPanel>().SetRect(0, 200, 82, 200);
        musicalPipa.GetComponentInChildren<UISprite>().spriteName = "transparent";

        musicalErhu.GetComponentInChildren<UIPanel>().SetRect(0, 200, 82, 200);
        musicalErhu.GetComponentInChildren<UISprite>().spriteName = "transparent";

        musicalSuona.GetComponentInChildren<UIPanel>().SetRect(0, 200, 82, 200);
        musicalSuona.GetComponentInChildren<UISprite>().spriteName = "transparent";

        musicalGuzheng.GetComponentInChildren<UIPanel>().SetRect(0, 200, 82, 200);
        musicalGuzheng.GetComponentInChildren<UISprite>().spriteName = "transparent";

        musicalDatiqin.GetComponentInChildren<UIPanel>().SetRect(0, 200, 82, 200);
        musicalDatiqin.GetComponentInChildren<UISprite>().spriteName = "transparent";

        muscialGuanxuanyue.GetComponentInChildren<UIPanel>().SetRect(0, 200, 82, 200);
        muscialGuanxuanyue.GetComponentInChildren<UISprite>().spriteName = "transparent";

    }

    /// <summary>
    /// paly change animation
    /// </summary>
    public void playMainToMenuAnimation(string ButtonName)
    {
        PlayerPrefs.SetString("ButtonName", ButtonName);
        AnimationNum = 0;
        avPlaneAlpha = 0;
        isAvPlane = true;
        MainUIbuttonInit();

        ///show mask to hide mainUI
        MainUIMask.SetActive(true);
        //MenuMask.SetActive(false);

        showMenuUI = center => showMenu();
        _MovieManager.playMovie(_paths[1], showMenuUI);
        _MovieManager.SetLooping(false);
    }

    /// <summary>
    /// change bgAnimation and slow to show change animation
    /// </summary>
    public void avPlaneAnimation()
    {
        if (avPlaneAlpha >= 0.99999)
        {
            isAvPlane = false;
            ///change bgAnimation
            if (AnimationNum == 0)
            {
                //BgMovieManager.playMovie("MenuUI.mp4");
                bgPlane.GetComponent<UITexture>().alpha = 0;
                bg2Plane.GetComponent<UITexture>().alpha = 1;
            }
            else if (AnimationNum == 1)
            {
                //BgMovieManager.playMovie("MainUI.mp4");
                bgPlane.GetComponent<UITexture>().alpha = 1;
                bg2Plane.GetComponent<UITexture>().alpha = 0;
            }
        }
        avPlane.GetComponent<UITexture>().alpha = avPlaneAlpha;
        avPlaneAlpha += 1.0f / 66;

    }




    /// <summary>
    /// end animation to show menu
    /// </summary>
    public void showMenu()
    {
        MainUI.SetActive(false);
        MenuUI.SetActive(true);
        avPlaneAlpha = 1;
        endAvPlane = true;

        //switch (PlayerPrefs.GetString("ButtonName"))
        //{
        //    case "pianoButton":
        //        MenuLabel.GetComponent<UILabel>().text = "pianoButton";
        //        break;
        //    case "xiaotiqinButton":
        //        MenuLabel.GetComponent<UILabel>().text = "xiaotiqinButton";
        //        break;
        //    case "pipaButton":
        //        MenuLabel.GetComponent<UILabel>().text = "pipaButton";
        //        break;
        //    case "erhuButton":
        //        MenuLabel.GetComponent<UILabel>().text = "erhuButton";
        //        break;
        //    case "suonaButton":
        //        MenuLabel.GetComponent<UILabel>().text = "suonaButton";
        //        break;
        //    case "guzhengButton":
        //        MenuLabel.GetComponent<UILabel>().text = "guzhengButton";
        //        break;
        //    case "datiqinButton":
        //        MenuLabel.GetComponent<UILabel>().text = "datiqinButton";
        //        break;
        //    case "guanxuanyueButton":
        //        MenuLabel.GetComponent<UILabel>().text = "guanxuanyueButton";
        //        break;
        //}
    }

    public void endavPlaneAnimation()
    {

        if (avPlaneAlpha <= 0.00001)
        {
            endAvPlane = false;
            MainUI.SetActive(true);


            if (isAnimationMenu)
            {
                MainUI.SetActive(false);
                MenubuttonY = 0;
                isMenuButton = true;
                Menu1.SetActive(true);
                Menu2.SetActive(true);
                Menu3.SetActive(true);
                EnhanceScrollView1.initialize();
                EnhanceScrollView2.initialize();
                EnhanceScrollView3.initialize();
            }
            isAnimationMenu = true;
        }
        avPlane.GetComponent<UITexture>().alpha = avPlaneAlpha;
        avPlaneAlpha -= 1.0f / 66;


    }

    /// <summary>
    /// sayHello
    /// </summary>
    public void sayHello()
    {
        Debug.Log("hello");
    }



    /// <summary>
    /// left button show
    /// </summary>
    /// <param name="getMusical"></param>
    /// <param name="getButton"></param>
    public void MainUIButtonShow(GameObject getMusical, GameObject getButton)
    {
        Menu1.SetActive(false);
        Menu2.SetActive(false);
        Menu3.SetActive(false);

        if (ButtonName != getMusical.name)
        {
            ButtonName = getMusical.name;
            MainUIbuttonInit();
            getMusical.GetComponent<UISprite>().spriteName = getMusical.name;
            //getButton.GetComponentInChildren<UIButton>().isEnabled = true;
            isShowButtonAnimation = true;
            buttonName = getButton;
            buttonName.GetComponentInChildren<UIButton>().isEnabled = false;
        }

    }

    /// <summary>
    /// button right show
    /// </summary>
    /// <param name="buttonName"></param>
    public void showButtonAnimation(GameObject buttonName)
    {
        buttonY -= 5;
        buttonName.GetComponent<UIPanel>().SetRect(0, buttonY, 82, 200);

    }


    ///
    public void MenuButtonAnimation()
    {
        MenubuttonY -= 10;
        MenuMask.GetComponent<UIPanel>().SetRect(0, MenubuttonY, 2560, 720);
        //Menu1.GetComponent<UIPanel>().SetRect(0, MenubuttonY, 820, 700);
        //Menu2.GetComponent<UIPanel>().SetRect(0, MenubuttonY, 820, 700);
        //Menu3.GetComponent<UIPanel>().SetRect(0, MenubuttonY, 820, 700);

    }

    /// <summary>
    /// back to mainUI
    /// </summary>
    public void backToMain()
    {
        isAnimationMenu = false;
        ButtonName = "";
        MenuUI.SetActive(false);
        AnimationNum = 1;
        avPlaneAlpha = 0;
        isAvPlane = true;
        MenuUIButtonInit();

        MenuMask.GetComponent<UIPanel>().SetRect(0, 0, 2560, 720);
        ///show mask to hide mainUI
        MainUIMask.SetActive(false);
        //MenuMask.SetActive(true);

        showMainUI = center => showMain();
        _MovieManager.playMovie(_paths[2], showMainUI);
        _MovieManager.SetLooping(false);
    }

    public void showMain()
    {
        avPlaneAlpha = 1;
        endAvPlane = true;
    }



    public void MenuUIButtonInit()
    {

    }

    public void ExitApp()
    {
        Application.Quit();
    }
}

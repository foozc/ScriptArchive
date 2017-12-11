using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainUIControls : MonoBehaviour {


    /// <summary>
    /// mainButton
    /// </summary>
    private int animationNum = -1;
    private GameObject buttonAnimation ;
    private GameObject buttonClick;    
    public float speed = 2;
    private float translation;
    private string cubeName;

    /// <summary>
    /// menuButton
    /// </summary>
    public GameObject MenuButton_000;
    public GameObject MenuButton_001;
    public GameObject MenuButton_002;
    public GameObject MenuButton_003;
    public GameObject MenuButton_004;

    public GameObject MenuSpriteAnimatin;

    public GameObject MainUI;
    public GameObject MusicUI;

    public Camera pointCamera;

    ///ray part 
    public GameObject nguiCamera;

    public Assets.Scripts.MovieManager movieManager;
    //public List<RaycastHit> touchRaycastHit;
    //public List<ra> 
    /// Use this for initialization
    void Start () {
        //movieManager.playMovie();
    }

    // Update is called once per frame
    void Update() {
        if (animationNum == 1)
        {
            palyAnimation();            
        }

        for (var i = 0; i < Input.touchCount; ++i)
        {
            
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                RaycastHit hit;
                // 从目前的触摸坐标，构建射线
                Ray ray = pointCamera.ScreenPointToRay(Input.GetTouch(i).position);
                Debug.Log("ray=" + ray);

                if (Physics.Raycast(ray, out hit))
                {
                    //Debug.Log("I'm looking at " + hit.transform.name);
                    Debug.Log("I'm looking at " + hit.collider.gameObject.name);
                    //
                    touchToMove(hit.collider.gameObject.name);
                    hit.collider.gameObject.transform.position = GetWorldPos(Input.GetTouch(i).position, hit.collider.gameObject.transform);
                    
                }
                else
                {
                    Debug.Log("I'm looking at nothing!");
                }
            }

            if(Input.GetTouch(i).phase == TouchPhase.Stationary)
            {
                RaycastHit hit;
                Ray ray = pointCamera.ScreenPointToRay(Input.GetTouch(i).position);

                if (Physics.Raycast(ray, out hit))
                {
                    hit.collider.gameObject.transform.position = GetWorldPos(Input.GetTouch(i).position, hit.collider.gameObject.transform);
                }
            }
            if (Input.GetTouch(i).phase == TouchPhase.Moved)
            {
                RaycastHit hit;
                // 从目前的触摸坐标，构建射线
                Ray ray = pointCamera.ScreenPointToRay(Input.GetTouch(i).position);

                if (Physics.Raycast(ray, out hit))
                {
                    hit.collider.gameObject.transform.position = GetWorldPos(Input.GetTouch(i).position,hit.collider.gameObject.transform);
                }
            }
            if (Input.GetTouch(i).phase == TouchPhase.Ended)
            {

            }
        }
    }

    public Vector3 GetWorldPos(Vector2 screenPos,Transform mTransform)
    {
        // return pointCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, mTransform.position.z));
        //return pointCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, pointCamera.nearClipPlane + 10));
        return pointCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, pointCamera.nearClipPlane + 10));
    }

    public void LogGamename1(GameObject button)
    {
        //button.GetComponent<UISprite>().spriteName = "";
        Debug.Log("111GameObject " + button.name);

    }
    public void LogGamename2(GameObject button)
    {
        button.GetComponent<UISprite>().spriteName = "MainUI";
        Debug.Log("222GameObject " + button.name);

    }
    public void LogGamename3(GameObject button)
    {
        //button.GetComponent<UISprite>().spriteName = "";
        Debug.Log("333GameObject " + button.name);

    }
    public void LogGamename4(GameObject button)
    {
        //button.GetComponent<UISprite>().spriteName = "";
        Debug.Log("444GameObject " + button.name);

    }

    public void setRay()
    {
        Ray ray1 = nguiCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit1;
        if(Physics.Raycast(ray1,out hit1))
        {
            Debug.Log(hit1.collider.gameObject.name);
            //touchToMove(hit1.collider.gameObject.name);
        }
    }
    public void touchToMove(string name)
    {
        cubeName = name;
        //cubeName = "";
        switch (cubeName)
        {
            case "Button_000":
                palyButtonAnimation(cubeName);
                break;
            case "Button_001":
                palyButtonAnimation(cubeName);
                break;
            case "Button_002":
                palyButtonAnimation(cubeName);
                break;
            case "Button_003":
                palyButtonAnimation(cubeName);
                break;
            case "Button_004":
                palyButtonAnimation(cubeName);
                break;
            case "MenuButton_000":
                palyMenuAnimation(cubeName);
                break;
            case "MenuButton_001":
                palyMenuAnimation(cubeName);
                break;
            case "MenuButton_002":
                palyMenuAnimation(cubeName);
                break;
            case "MenuButton_003":
                palyMenuAnimation(cubeName);
                break;
            case "MenuButton_004":
                palyMenuAnimation(cubeName);
                break;
        }
    }

    public void palyMenuAnimation(string cubeName)
    {
        MainUIInit();
        MainUI.SetActive(false);
        //MenuSpriteAnimatin.SetActive(true);
        //MenuSpriteAnimatin.GetComponent<UISpriteAnimation>().Play();
        MusicUI.SetActive(true);
    }

    public void MainUIInit()
    {
        ///init buttonANimation
        GameObject.Find("ButtonAnimation_000").GetComponent<UISprite>().spriteName = "buttonAnimation_0";
        GameObject.Find("ButtonAnimation_001").GetComponent<UISprite>().spriteName = "buttonAnimation_0";
        GameObject.Find("ButtonAnimation_002").GetComponent<UISprite>().spriteName = "buttonAnimation_0";
        GameObject.Find("ButtonAnimation_003").GetComponent<UISprite>().spriteName = "buttonAnimation_0";
        GameObject.Find("ButtonAnimation_004").GetComponent<UISprite>().spriteName = "buttonAnimation_0";

        ///init button
        GameObject.Find("Button_000").GetComponent<UIButton>().normalSprite = "guanshangButton";
        GameObject.Find("Button_001").GetComponent<UIButton>().normalSprite = "guanshangButton";
        GameObject.Find("Button_002").GetComponent<UIButton>().normalSprite = "guanshangButton";
        GameObject.Find("Button_003").GetComponent<UIButton>().normalSprite = "guanshangButton";
        GameObject.Find("Button_004").GetComponent<UIButton>().normalSprite = "guanshangButton";

        ///init menubutton
        MenuButton_000.SetActive(false);
        MenuButton_001.SetActive(false);
        MenuButton_002.SetActive(false);
        MenuButton_003.SetActive(false);
        MenuButton_004.SetActive(false);
    }

    //
    public void palyButtonAnimation(string cubeName)
    {
        MainUIInit();

        buttonClick = GameObject.Find(cubeName);

        ///hide button
        buttonClick.GetComponent<UIButton>().normalSprite = "transparent";
        //GameObject.Find(name).GetComponent<UISprite>().spriteName = "transparent";

        string ButtonAnimationName = "ButtonAnimation" + cubeName.Substring(6, 4);
        animationNum = 1;
        translation = 0;
        
        buttonAnimation = GameObject.Find(ButtonAnimationName);
        
    }
	
	public void exitApplication(){
        Debug.Log("111111");
		Application.Quit();
	}

    public void palyAnimation()
    {
        translation = translation + Time.deltaTime * speed;
        buttonAnimation.GetComponent<UISprite>().spriteName = "buttonAnimation_" + System.Math.Floor(translation);
        if(System.Math.Floor(translation) > 46)
        {
            ///end animation  to show button pic
            //buttonClick.GetComponent<UIButton>().normalSprite = "guanshangButton";
            ///stop animation
            animationNum = 0;

            ///show menuButton
            switch (cubeName)
            {
                case "Button_000":
                    MenuButton_000.SetActive(true);
                    break;
                case "Button_001":
                    MenuButton_001.SetActive(true);
                    break;
                case "Button_002":
                    MenuButton_002.SetActive(true);
                    break;
                case "Button_003":
                    MenuButton_003.SetActive(true);
                    break;
                case "Button_004":
                    MenuButton_004.SetActive(true);
                    break;
            }
        }
    }

}

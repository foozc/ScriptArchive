using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRay : MonoBehaviour {

    public Camera eyeCamera;
    public Image aimImg;
    public Text nameText;
    public Text contentText;
    public GameObject pic1;
    public GameObject pic2;
    public GameObject pic3;
    public UIControl gotoScene;




    /// <summary>
    /// 
    /// </summary>
    private int aim = 0;
    private int otherAim = 0;
    private int anOther = 0;
    private int controlKey = 0;
    private int endOther = 0;
	// Use this for initialization
	void Start () {
		
	}
    //public Texture2D [] textures;

    void OnGUI()
    {
        // Rect rect = new Rect(Screen.width * 0.4f + (Screen.width * 0.2f - Screen.height * 0.2f)/2, Screen.height * 0.4f, Screen.height * 0.2f, Screen.height * 0.2f);
        // GUI.DrawTexture(rect, textures[aim]);
        if (aim == 0)
        {
            aimImg.sprite = Resources.Load("UI/mainUI/none", typeof(Sprite)) as Sprite;
        }
        else
        {
            if (aim < 10)
            {
                aimImg.sprite = Resources.Load("UI/mainUI/mz/mz_0000" + aim, typeof(Sprite)) as Sprite;
            }
            else
            {
                aimImg.sprite = Resources.Load("UI/mainUI/mz/mz_000" + aim, typeof(Sprite)) as Sprite;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        Ray ray = eyeCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));//从摄像机发出到点击坐标的射线
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo,180f))
        {
            GameObject gameObj = hitInfo.collider.gameObject;
            if (gameObj.GetComponent<Image>() != null)
            {
                gameObj.GetComponent<Image>().sprite = Resources.Load("UI/mainUI/" + gameObj.name + "_p", typeof(Sprite)) as Sprite;
                //nameText.text = DataManager.instance.getModeData(gameObj.name).Name;
                //contentText.text = DataManager.instance.getModeData(gameObj.name).UiContent;
            }
            Debug.DrawLine(ray.origin, hitInfo.point);//划出射线，只有在scene视图中才能看到
            anOther++;
            if (anOther > 38)
            {
                if (gameObj.GetComponent<Image>() != null)
                {
                    gameObj.GetComponent<Image>().sprite = Resources.Load("UI/mainUI/" + gameObj.name + "_h", typeof(Sprite)) as Sprite;
                    //nameText.text = DataManager.instance.getModeData(gameObj.name).Name;
                    //contentText.text = DataManager.instance.getModeData(gameObj.name).UiContent;
                }
                if (otherAim / 3 < 31)
                {
                    otherAim++;
                    aim = otherAim / 3;
                    
                    //Debug.Log("click object name is " + gameObj.name);
                    //if (gameObj.tag == "boot")///当射线碰撞目标为boot类型的物品 ，执行拾取操作}
                }
                else
                {
                    switch (gameObj.name)
                    {
                        case "gudianguancha":
                            gotoScene.gotoScene_000();
                            break;
                        case "shangsheng":
                            gotoScene.gotoScene_001();
                            break;
                        case "xiajiang":
                            gotoScene.gotoScene_002();
                            break;

                    }
                    ControlsayHello();
                }
            }
        }
        else
        {
            pic1.GetComponent<Image>().sprite = Resources.Load("UI/mainUI/gudianguancha", typeof(Sprite)) as Sprite;
            pic2.GetComponent<Image>().sprite = Resources.Load("UI/mainUI/shangsheng", typeof(Sprite)) as Sprite;
            pic3.GetComponent<Image>().sprite = Resources.Load("UI/mainUI/xiajiang", typeof(Sprite)) as Sprite;
            //time control
            anOther = 0;
            otherAim = 0;
            aim = 0;
            controlKey = 0;
            ControlsayHello();
        }

    }

    public void ControlsayHello()
    {
        if(controlKey == 0 )
        {
            if (anOther != 0)
            {
                sayHello(true);
                controlKey++;
                endOther = 1;
            }
            else if(endOther == 1)
            {
                sayHello(false);
                endOther = 0;

            }
        }
        
    }
    public void sayHello(bool key)
    {
        if (key)
        {
            Debug.Log("hello");
        }
        else
        {
            Debug.Log("byebye");
        }
    }
}

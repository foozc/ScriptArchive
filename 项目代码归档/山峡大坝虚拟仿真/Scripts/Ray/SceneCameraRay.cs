using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneCameraRay : MonoBehaviour
{

    public Camera eyeCamera;
    public Image aimImg;
    public GameObject back;

    /// <summary>
    /// high light 
    /// </summary>
    private GameObject nHighGameObject;
    public HighLightModel nHigh;
    public Image highImg;
    public Text nameText;
    public Text contentText;
    private string ImgName;
    private string mark;


    /// <summary>
    /// scene change
    /// </summary>
    public GameObject Scene_MainUI;
    public GameObject Scene_move_001;
    public GameObject Scene_move_002;

    public GameObject move_001_cameraOut;
    public GameObject move_001_cameraIn;

    public GameObject barque1;
    public GameObject barque2;
    public GameObject water;

    /// <summary>
    /// 
    /// </summary>
    private int aim = 0;
    private int otherAim = 0;
    private int anOther = 0;
    private int controlKey = 0;
    private int endOther = 0;

    public int musicNum;

    public MusicControl musicControl;
    // Use this for initialization
    void Start()
    {
    }
    //public Texture2D [] textures;

    void OnGUI()
    {
        // Rect rect = new Rect(Screen.width * 0.4f + (Screen.width * 0.2f - Screen.height * 0.2f)/2, Screen.height * 0.4f, Screen.height * 0.2f, Screen.height * 0.2f);
        // GUI.DrawTexture(rect, textures[aim]);
        if (aim == 0)
        {
            aimImg.enabled = true;
            highImg.gameObject.SetActive(false);
            aimImg.sprite = Resources.Load("UI/mainUI/none", typeof(Sprite)) as Sprite;
        }
        else
        {

            if (DataManager.instance.getModeData(ImgName) == null || !DataManager.instance.getModeData(ImgName).IsShow)
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
    }
    // Update is called once per frame
    void Update()
    {
        //if (barque2 != null)
        //{
        //    water.SetActive(true);

        //    if (barque2.transform.position.x <= -210)
        //    {
        //        water.SetActive(false);
        //    }
        //}

        //if (barque1 != null)
        //{
        //    water.SetActive(false);

        //    if (barque1.transform.position.x >= -0.00468)
        //    {
        //        water.SetActive(true);
        //    }
        //}

        Ray ray = eyeCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));//从摄像机发出到点击坐标的射线
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo,600f) )
        {
            GameObject gameObj = hitInfo.collider.gameObject;    
            nHighGameObject = gameObj;
            ImgName = gameObj.name;
            if (mark != null && mark != ImgName)
            {
                mark = null;
                back.GetComponent<Image>().sprite = Resources.Load("UI/mainUI/back", typeof(Sprite)) as Sprite;
                if (nHighGameObject != null)
                {
                    nHigh.SetHighLight(nHighGameObject, false);
                }
                //time control
                anOther = 0;
                otherAim = 0;
                aim = 0;
                controlKey = 0;
                ControlsayHello();
            }
            else
            {
                mark = gameObj.name;
                if (ImgName == "back")
                {
                    gameObj.GetComponent<Image>().sprite = Resources.Load("UI/mainUI/" + gameObj.name + "_p", typeof(Sprite)) as Sprite;
                    //nameText.text = DataManager.instance.getModeData(gameObj.name).Name;
                    //contentText.text = DataManager.instance.getModeData(gameObj.name).UiContent;
                }
                Debug.DrawLine(ray.origin, hitInfo.point);//划出射线，只有在scene视图中才能看到
                anOther++;
                if (anOther > 18)
                {
                    nHigh.SetHighLight(nHighGameObject, true);
                    if (ImgName == "back")
                    {
                        gameObj.GetComponent<Image>().sprite = Resources.Load("UI/mainUI/" + gameObj.name + "_h", typeof(Sprite)) as Sprite;
                        //nameText.text = DataManager.instance.getModeData(gameObj.name).Name;
                        //contentText.text = DataManager.instance.getModeData(gameObj.name).UiContent;
                    }

                    if (otherAim / 2 < 30)
                    {
                        otherAim++;
                        aim = otherAim / 2;

                        //Debug.Log("click object name is " + gameObj.name);
                        //if (gameObj.tag == "boot")///当射线碰撞目标为boot类型的物品 ，执行拾取操作}
                    }
                    else
                    {
                        if (ImgName == "back")
                        {
                            musicControl.stopMusic();
                            Scene_MainUI.SetActive(true);
                            Scene_move_001.SetActive(false);
                            Scene_move_002.SetActive(false);
                            move_001_cameraOut.SetActive(false);
                            move_001_cameraIn.SetActive(false);
                            DataManager.instance.stopTime();
                            //SceneManager.LoadScene("MainUI");
                        }
                        else
                        {
                            ControlsayHello();
                        }
                    }
                }
            }
        }
        else
        {
            mark = null;
            back.GetComponent<Image>().sprite = Resources.Load("UI/mainUI/back", typeof(Sprite)) as Sprite;
            if (nHighGameObject != null)
            {
                nHigh.SetHighLight(nHighGameObject, false);
            }
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
        if (controlKey == 0)
        {
            if (anOther != 0)
            {
                sayHello(true);
                controlKey++;
                endOther = 1;
            }
            else if (endOther == 1)
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
            if (DataManager.instance.getModeData(ImgName) == null)
            {
                Debug.Log("meiyou:" + ImgName);
                return;
            }
            if (!DataManager.instance.getModeData(ImgName).IsShow)
            {
                nameText.text = DataManager.instance.getModeData(ImgName).Name;
                contentText.text = DataManager.instance.getModeData(ImgName).UiContent;
                //highImg.sprite = Resources.Load("UI/mainUI/"+ ImgName, typeof(Sprite)) as Sprite;
                highImg.gameObject.SetActive(true);
                DataManager.instance.getModeData(ImgName).IsShow = true;
            }
            Debug.Log("hello");
        }
        else
        {
            highImg.gameObject.SetActive(false);
            Debug.Log("byebye");
        }
    }
}

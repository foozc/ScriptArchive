using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Xml;
using System.Collections.Generic;
using RenderHeads.Media.AVProVideo;

public class MainUIControl : MonoBehaviour {

    public GameObject left, leftDown, right, rightDown;
    public TabTween leftTab, leftDownTab, rightTab, rightDownTab;
    private List<resume> buttonResume = new List<resume>();
    private List<resume> Resumes = new List<resume>();

    public MediaPlayer Movie1, Movie2, Movie3, Movie4, Movie5, Movie6, Movie7, Movie8, Movie9;
    private float movietime;

    public string upC, downC;

    // Use this for initialization
    void Start () {
        //Screen.SetResolution(7680, 2160, true);
        Screen.SetResolution(5760, 2160, true);
        Resumes = LoadResumeXMl();
        movietime = 0;

    }
	
	// Update is called once per frame
	void Update () {
        movietime += Time.deltaTime;
        if (movietime > 11)
        {

            Movie1.Rewind(false);
            Movie2.Rewind(false);
            Movie3.Rewind(false);
            Movie4.Rewind(false);
            Movie5.Rewind(false);
            Movie6.Rewind(false);
            Movie7.Rewind(false);
            Movie8.Rewind(false);
            Movie9.Rewind(false);


            movietime = 0;
        }
	}
    
    private List<resume> LoadResumeXMl()
    {
        string XMLPath = Application.streamingAssetsPath + "/XML/Resume.xml";
        XmlDocument Doc = new XmlDocument();
        Doc.Load(XMLPath);
        XmlNode datas = Doc.SelectSingleNode("data");
        foreach(XmlNode item in datas)
        {
            resume Rresum = new resume();
            XmlElement _item = (XmlElement)item;
            Rresum.Name = _item.GetAttribute("name");
            Rresum.Title = _item.GetAttribute("title");
            Rresum.Address = _item.GetAttribute("address");
            Rresum.Year = _item.GetAttribute("year");
            Rresum.Phone = _item.GetAttribute("phone");
            Rresum.Area = _item.GetAttribute("area");
            buttonResume.Add(Rresum);
        }
        return buttonResume;
    }

    

    /// <summary>
    /// wall move to left
    /// </summary>
    /// <param name="wall"></param>
    public void leftButtonClick(GameObject wall)
    {
        float Width, nowX;
        Width = wall.GetComponent<RectTransform>().rect.size.x;
        nowX = wall.GetComponent<RectTransform>().localPosition.x;
        if(nowX == 0)
        {
            /// nothing todo
        }
        else
        {
            wall.GetComponent<RectTransform>().localPosition = new Vector3(nowX + 318f, wall.GetComponent<RectTransform>().localPosition.y, wall.GetComponent<RectTransform>().localPosition.z);
        }
        
        //wall.GetComponent<RectTransform>().sizeDelta = new Vector2();
    }

    /// <summary>
    /// wall move to right
    /// </summary>
    /// <param name="wall"></param>
    public void rightButtonClick(GameObject wall)
    {
        float Width, nowX;
        Width = wall.GetComponent<RectTransform>().rect.size.x;
        nowX = wall.GetComponent<RectTransform>().localPosition.x;
        if (Mathf.Abs(nowX) == Mathf.Abs(Width -318f))
        {
            /// nothing todo
        }
        else
        {
            wall.GetComponent<RectTransform>().localPosition = new Vector3(nowX - 318f, wall.GetComponent<RectTransform>().localPosition.y, wall.GetComponent<RectTransform>().localPosition.z);
        }

    }
    public void test(int i) { }
    /// <summary>
    /// about button name to show content
    /// </summary>
    /// <param name="buttonName"></param>
    public void contentButtonClick(string buttonName)
    {
        GameObject wall = GameObject.Find(buttonName.Substring(0,buttonName.Length - 1));
       // Debug.Log(buttonName.Substring(0, buttonName.Length - 1));
        InitContentButton(buttonName.Substring(0, buttonName.Length - 1));
        ///change button image
        GameObject contentButton = GameObject.Find(buttonName);
        string buttonName_P = contentButton.GetComponent<Image>().sprite.name + "_p";
        contentButton.GetComponent<Image>().sprite = Resources.Load("UIPNG/Image/bigImg/" + buttonName_P, typeof(Sprite)) as Sprite;
        contentButton.transform.localScale = new Vector3(1.3f, 1.3f, 1f);
        contentButton.GetComponentsInChildren<Image>()[1].sprite = Resources.Load("NEWUI/transparent", typeof(Sprite)) as Sprite;
        float nowX = wall.GetComponent<RectTransform>().localPosition.x;
        string index = buttonName.Substring(buttonName.Length-1);
        if (buttonName.Substring(0, 4) == "Left")
        {
            downC = buttonName_P.Substring(0, buttonName_P.Length - 4);
            Debug.Log("downC:" + downC);
        }
        else
        {
            upC = buttonName_P.Substring(0, buttonName_P.Length - 4); ;
            Debug.Log("upC:" + upC);
        }
        switch (index)
        {
            case "0":
                            
                wall.GetComponent<RectTransform>().localPosition = new Vector3(-636f * 0, wall.GetComponent<RectTransform>().localPosition.y, wall.GetComponent<RectTransform>().localPosition.z);
                break;
            case "1":
                wall.GetComponent<RectTransform>().localPosition = new Vector3(-636f * 1, wall.GetComponent<RectTransform>().localPosition.y, wall.GetComponent<RectTransform>().localPosition.z);
                break;
            case "2":
                wall.GetComponent<RectTransform>().localPosition = new Vector3(-636f * 2, wall.GetComponent<RectTransform>().localPosition.y, wall.GetComponent<RectTransform>().localPosition.z);
                break;
            case "3":
                wall.GetComponent<RectTransform>().localPosition = new Vector3(-636f * 3, wall.GetComponent<RectTransform>().localPosition.y, wall.GetComponent<RectTransform>().localPosition.z);
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// use temp button name to init four content button
    /// </summary>
    /// <param name="buttonName"></param>
    public void InitContentButton(string buttonName)
    {
        string buttonName_Init,TempName;
        
        for (int i = 0; i<4; i++)
        {
            GameObject Button = GameObject.Find(buttonName + i);
            Button.GetComponentsInChildren<Image>()[1].sprite = Resources.Load("NEWUI/heidi", typeof(Sprite)) as Sprite;
            Button.transform.localScale = Vector3.one;
           // Debug.Log(Button.name);
            buttonName_Init = Button.GetComponent<Image>().sprite.name;
            if(buttonName_Init.Substring(buttonName_Init.Length - 2, 2) == "_p")
            {
                TempName = buttonName_Init.Substring(0,buttonName_Init.Length - 2);
                Button.GetComponent<Image>().sprite = Resources.Load("UIPNG/Image/bigImg/" + TempName, typeof(Sprite)) as Sprite;
            }            
        }
    }

    public void showDetialPic(GameObject i)
    {
        if(i.name == "LeftImage")
        {
            i.GetComponentsInChildren<Image>()[1].sprite = Resources.Load("UIPNG/Image/bigImg/" + downC, typeof(Sprite)) as Sprite;
            i.SetActive(true);
        }
        else
        {
            i.GetComponentsInChildren<Image>()[1].sprite = Resources.Load("UIPNG/Image/bigImg/" + upC, typeof(Sprite)) as Sprite;
            i.SetActive(false);
        }
        i.SetActive(true);
        Debug.Log(upC);
    }

    public void cleanDetail(GameObject i)
    {
        i.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    public void movieButtonClick(GameObject button)
    {
        string buttonName = button.name;
        string buttonPosition = buttonName.Substring(buttonName.Length - 3, 1);//0:left 1:leftDown 2:right 3:rightDown
        string buttonNum = buttonName.Substring(buttonName.Length - 1, 1);//0 , 1, 2
        string ImgTitle = "", ImgAddress = "", ImgYear = "", ImgPhone = "", ImgArea = "";
        foreach (resume item in Resumes)
        {
            if (item.Name == buttonName)
            {
                ImgTitle = item.Title;
                ImgAddress = item.Address;
                ImgYear = item.Year;
                ImgPhone = item.Phone;
                ImgArea = item.Area;
            }
        }
        switch (buttonPosition)
        {
            case "0":
                //left.SetActive(true);                
                leftTab.startAnimation(buttonNum);
                //leftTab.GetComponentsInChildren<Text>()[0].text = ImgTitle;
                //leftTab.GetComponentsInChildren<Text>()[2].text = ImgAddress;
                //leftTab.GetComponentsInChildren<Text>()[4].text = ImgYear;
                //leftTab.GetComponentsInChildren<Text>()[6].text = ImgPhone;
                //leftTab.GetComponentsInChildren<Text>()[8].text = ImgArea;
                break;

            case "1":
                //leftDown.SetActive(true);
                leftDownTab.startAnimation(buttonNum);
                //leftDownTab.GetComponentsInChildren<Text>()[0].text = ImgTitle;
                //leftDownTab.GetComponentsInChildren<Text>()[2].text = ImgAddress;
                //leftDownTab.GetComponentsInChildren<Text>()[4].text = ImgYear;
                //leftDownTab.GetComponentsInChildren<Text>()[6].text = ImgPhone;
                //leftDownTab.GetComponentsInChildren<Text>()[8].text = ImgArea;
                //leftDown.GetComponent<Image>().sprite = Resources.Load("UIPNG.Image" + buttonName, typeof(Sprite)) as Sprite;
                break;

            case "2":
                //right.SetActive(true);
                rightTab.startAnimation(buttonNum);
                //rightTab.GetComponentsInChildren<Text>()[0].text = ImgTitle;
                //rightTab.GetComponentsInChildren<Text>()[2].text = ImgAddress;
                //rightTab.GetComponentsInChildren<Text>()[4].text = ImgYear;
                //rightTab.GetComponentsInChildren<Text>()[6].text = ImgPhone;
                //rightTab.GetComponentsInChildren<Text>()[8].text = ImgArea;
                //right.GetComponent<Image>().sprite = Resources.Load("UIPNG.Image" + buttonName, typeof(Sprite)) as Sprite;
                break;

            case "3":
                //rightDown.SetActive(true);
                rightDownTab.startAnimation(buttonNum);
                //rightDownTab.GetComponentsInChildren<Text>()[0].text = ImgTitle;
                //rightDownTab.GetComponentsInChildren<Text>()[2].text = ImgAddress;
                //rightDownTab.GetComponentsInChildren<Text>()[4].text = ImgYear;
                //rightDownTab.GetComponentsInChildren<Text>()[6].text = ImgPhone;
                //rightDownTab.GetComponentsInChildren<Text>()[8].text = ImgArea;
                //rightDown.GetComponent<Image>().sprite = Resources.Load("UIPNG.Image" + buttonName, typeof(Sprite)) as Sprite;
                break;

            default:
                Debug.Log("error");
                break;

        }
    }
}

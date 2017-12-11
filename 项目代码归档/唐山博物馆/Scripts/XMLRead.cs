using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public class XMLRead : MonoBehaviour {
	public string path;

    /// <summary>
    /// init buttong
    /// </summary>
	public string buttonInit;
    public MainUIControl mainUIControl;
    public bool isRotation;
    public bool autoMove;
	private List<Info> infos = new List<Info>();
	private List<Image> buttons = new List<Image>();
	private GridLayoutGroup grid;
    public GameObject URLImage;
	//public TabTween tab;
    public Image NewcontentImg;
    public Image NewcontentImg0;
    public Image NewcontentImg1;
    public Image NewcontentImg2;
    public Image NewcontentButtonImg;
    public Image NewcontentButtonImg0;
    public Image NewcontentButtonImg1;
    public Image NewcontentButtonImg2;
    
    // Use this for initialization
    void Start ()
	{
		grid = GetComponent<GridLayoutGroup>();
        CreateButton();

    }
	private List<Info> LoadXml()
	{
		string xmlPath = Application.streamingAssetsPath + "/XML/" + path+".xml";
		XmlDocument doc = new XmlDocument();
		doc.Load(xmlPath);
		XmlNode datas = doc.SelectSingleNode("data");
		foreach (XmlNode info in datas)
		{
			Info Iinfo = new Info();
			XmlElement _info = (XmlElement)info;
			Iinfo.Url = _info.GetAttribute("url");
            Iinfo.Title = _info.GetAttribute("title");
            Iinfo.ImageName = _info.GetAttribute("imageName");
			Iinfo.ButtonImage = _info.GetAttribute("buttonImage");
			//Iinfo.Text = _info.GetAttribute("text");
            Iinfo.ContentImg = _info.GetAttribute("contentImg");
            Iinfo.ContentImg0 = _info.GetAttribute("contentImg0");
            Iinfo.ContentImg1 = _info.GetAttribute("contentImg1");
            Iinfo.ContentImg2 = _info.GetAttribute("contentImg2");
            Iinfo.ContentButtonImg = _info.GetAttribute("contentButtonImg");
            Iinfo.ContentButtonImg0 = _info.GetAttribute("contentButtonImg0");
            Iinfo.ContentButtonImg1 = _info.GetAttribute("contentButtonImg1");
            Iinfo.ContentButtonImg2 = _info.GetAttribute("contentButtonImg2");

            infos.Add(Iinfo);
		}
		return infos;
	}
	private void CreateButton()
	{
		int i = 0;
		float itemHeight=0;
        
		foreach (Info item in LoadXml())
		{
            //1img 1title  4img
			GameObject button=Instantiate(Resources.Load("Prefabs/ButtonPrefab")) as GameObject;
			button.transform.localScale =Vector3.one;
			button.transform.parent = this.transform;
			button.transform.Find("Image").GetComponent<Image>().sprite=Resources.Load("UIPNG/Image/bigImg/" + item.ButtonImage,typeof(Sprite))as Sprite;
			button.transform.Find("Title").GetComponent<Text>().text=item.Title;
			//button.transform.Find("Text").GetComponent<Text>().text = item.Text;
			button.GetComponent<RectTransform>().localScale = Vector3.one;
			buttons.Add(button.GetComponent<Image>());
			button.name = i.ToString();
			itemHeight = button.GetComponent<RectTransform>().rect.width;
			button.GetComponent<Button>().onClick.AddListener(delegate ()
			{
				this.ClickEvent(button.name);
			}
			);
            i++;
        }
        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().rect.width, 150f);
        //if (isRotation)
        //{
        //    GetComponent<RectTransform>().rotation = new Quaternion(0f,180f,0f,1f);
        //}
        this.ClickEvent("0");
    }

    private void ClickEvent(string ButtonName)
	{
        ///init button and button position
        Debug.Log(ButtonName);
        mainUIControl.InitContentButton(buttonInit);
        GameObject newButtonInit = GameObject.Find(buttonInit);
        newButtonInit.GetComponent<RectTransform>().localPosition = new Vector3(0f, newButtonInit.GetComponent<RectTransform>().localPosition.y, newButtonInit.GetComponent<RectTransform>().localPosition.z);

        ///init content img
        NewcontentImg.sprite = Resources.Load("UIPNG/Image/bigImg/" + infos[Convert.ToInt32(ButtonName)].ContentImg, typeof(Sprite)) as Sprite;
        NewcontentImg0.sprite = Resources.Load("UIPNG/Image/bigImg/" + infos[Convert.ToInt32(ButtonName)].ContentImg0, typeof(Sprite)) as Sprite;
        NewcontentImg1.sprite = Resources.Load("UIPNG/Image/bigImg/" + infos[Convert.ToInt32(ButtonName)].ContentImg1, typeof(Sprite)) as Sprite;
        NewcontentImg2.sprite = Resources.Load("UIPNG/Image/bigImg/" + infos[Convert.ToInt32(ButtonName)].ContentImg2, typeof(Sprite)) as Sprite;

        ///init button img
        NewcontentButtonImg.sprite = Resources.Load("UIPNG/Image/bigImg/" + infos[Convert.ToInt32(ButtonName)].ContentButtonImg, typeof(Sprite)) as Sprite;
        NewcontentButtonImg0.sprite = Resources.Load("UIPNG/Image/bigImg/" + infos[Convert.ToInt32(ButtonName)].ContentButtonImg0, typeof(Sprite)) as Sprite;
        NewcontentButtonImg1.sprite = Resources.Load("UIPNG/Image/bigImg/" + infos[Convert.ToInt32(ButtonName)].ContentButtonImg1, typeof(Sprite)) as Sprite;
        NewcontentButtonImg2.sprite = Resources.Load("UIPNG/Image/bigImg/" + infos[Convert.ToInt32(ButtonName)].ContentButtonImg2, typeof(Sprite)) as Sprite;

        ///first button click
        
        mainUIControl.contentButtonClick(buttonInit+"0");
        
        if (autoMove)
        {
            if ((int.Parse(ButtonName) == 0 || int.Parse(ButtonName) == 1) && grid.GetComponent<RectTransform>().localPosition.x < 442f)
            {
                //grid.GetComponent<RectTransform>().localPosition = new Vector3(221, grid.GetComponent<RectTransform>().localPosition.y, grid.GetComponent<RectTransform>().localPosition.z);
                grid.GetComponent<RectTransform>().DOLocalMoveX(442f, 0.5f, false);
            }
            if ((int.Parse(ButtonName) == 2 || int.Parse(ButtonName) == 3) && grid.GetComponent<RectTransform>().localPosition.x < 90f)
            {
                grid.GetComponent<RectTransform>().DOLocalMoveX(90f, 0.5f, false);
            }
            if ((int.Parse(ButtonName) == 4 || int.Parse(ButtonName) == 5) && grid.GetComponent<RectTransform>().localPosition.x < -260f)
            {
                grid.GetComponent<RectTransform>().DOLocalMoveX(-260f, 0.5f, false);
            }
        }

        if ((int.Parse(ButtonName) == 8 || int.Parse(ButtonName) == 9) && grid.GetComponent<RectTransform>().localPosition.x > 260f)
        {
            grid.GetComponent<RectTransform>().DOLocalMoveX(260f, 0.5f, false);
        }

        if ((int.Parse(ButtonName) == 10 || int.Parse(ButtonName) == 11) && grid.GetComponent<RectTransform>().localPosition.x > -90f)
        {
            grid.GetComponent<RectTransform>().DOLocalMoveX(-90f, 0.5f, false);
        }

        if ((int.Parse(ButtonName) == 12 || int.Parse(ButtonName) == 13 )&& grid.GetComponent<RectTransform>().localPosition.x > -440f)
        {
            grid.GetComponent<RectTransform>().DOLocalMoveX(-440f, 0.5f, false);
        }

        Resources.UnloadUnusedAssets();
        
        URLImage.GetComponentsInChildren<Image>()[1].sprite = Resources.Load("URL/"+ infos[int.Parse(ButtonName)].Url, typeof(Sprite)) as Sprite;
        URLImage.GetComponentsInChildren<Image>()[1].SetNativeSize();

        float URLHeight = URLImage.GetComponentsInChildren<RectTransform>()[2].rect.height;
        URLImage.GetComponentsInChildren<RectTransform>()[1].sizeDelta = new Vector2(URLImage.GetComponentsInChildren<RectTransform>()[1].rect.width, URLHeight);
        URLImage.GetComponentsInChildren<RectTransform>()[1].localPosition = new Vector3(URLImage.GetComponentsInChildren<RectTransform>()[1].localPosition.x, 0, URLImage.GetComponentsInChildren<RectTransform>()[1].localPosition.z);

        foreach (Image item in buttons)
		{
			item.sprite= Resources.Load("UIPNG/anniu", typeof(Sprite)) as Sprite;
            item.GetComponentInParent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        }
		buttons[Convert.ToInt32(ButtonName)].sprite= Resources.Load("NEWUI/dianji", typeof(Sprite)) as Sprite;
        buttons[Convert.ToInt32(ButtonName)].GetComponent<RectTransform>().localScale = new Vector3(1.1f, 1.1f, 1f);
        buttons[Convert.ToInt32(ButtonName)].GetComponentsInChildren<RectTransform>()[1].localScale = new Vector3(0.88f, 0.88f, 0.88f);
        //tab.showTab(ButtonName);
        //tab.showAround(ButtonName);
    }
    


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TableControl : MonoBehaviour
{
    public enum National
    {
        ZHUANGZU,
        MIAOZU,
        YAOZU,
        TONGZU
    }

    public enum Dress
    {
        ONE,
        TWO,
        THREE,
        FOUR,

    }
	public GameObject Button_head;
	public GameObject Button_clothes;
	public GameObject Button_jewelry;
    /// <summary>
    /// table change animation
    /// </summary>
    public TweenPosition hideIndexTable;
	//public GameObject hideMainTable;
	private GameObject showDescriptionTable;
	private UISprite Button_description;
    //public TweenPosition showExhibitionTable;
    public TweenPosition showTryTable;
    public TweenPosition showShareTable;
	private GameObject showDetailTable;

    /// <summary>
    /// button change
    /// </summary>
    private GameObject zhuangzuButton;
	private GameObject miaozuButton;
	private GameObject yaozuButton;
	private GameObject tongzuButton;

    public GameObject mainClothImage;
    //public UISprite Detail_Sptite;

    /// <summary>
    /// audio
    /// </summary>
    private AudioSource audioSource;

    /// <summary>
    /// button animation
    /// </summary>
    /// 
    //public UISpriteAnimation buttonAnimation;

    public National national = National.ZHUANGZU;
    public Dress dress = Dress.ONE;
    //public GameObject nationalDescription;
	private GameObject changeBgButton;


    /// <summary>
    /// rotation the cube
    /// </summary>
    //public GameObject dressCube;
    private Vector3 stwpIn;
    private Vector3 stwpOut;
    private float xRotation;
    //public GameObject rotationObject;
    public int rotationIsPlay = 0;

    private float mouseStartPos, mouseEndPos;
    private bool mBDrawMouseRect = false;

    /// <summary>
    /// dress change
    /// </summary>
    //public GameObject dressCamera;
    //public GameObject partPic;
    public string dressName;
    private string dressNum;
    private string picName;


    private string modelName;
    private string partName;
    /// <summary>
    /// connect with kinect
    /// </summary>
    private UIEvent uievent;

    public UISprite tryBg;
    public UITexture kinectImage;

    public UITexture kinectDepthImage;
    public gameManager gameManager;
    private GameObject partModel;
    private Vector3 handPos;

	private UIButton Button_bg1;
    private UIButton Button_bg2;
	private UIButton Button_bg3;
	private UIButton Button_true;
	public UIButton Button_isBG;

    public GameObject Title;


	private GameObject BriefUI;
	private UISprite brief;
    private GameObject manButton;
	private GameObject womanButton;
    public Transform model;
    private string briefName;

	private GameObject Human;
	private bool isShowModel=false;
	Transform head;
	Transform Cloth;
	Transform Jewelry;
	public UISpriteAnimation leftHand;
	private UISprite clothName;
    private void hideButton()
    {
		if (manButton!=null&&womanButton!=null) 
		{
			manButton.SetActive(false);
			womanButton.SetActive(false);	
		}
    }
    private void showButton()
    {
		if (manButton!=null&&womanButton!=null) 
		{
			manButton.SetActive(true);
			womanButton.SetActive(true);	
		}
    }
    private void showBriefUI()
    {
        hideExhibition();
		BriefUI=Instantiate(Resources.Load ("Prefabs/NGUIPrefabs/BriefUI"))as GameObject;
		BriefUI.transform.parent = GameObject.Find ("UI Root (3D)").transform;
		BriefUI.transform.localScale = new Vector3(1,1,1);
		BriefUI.transform.position = Vector3.zero;
        if (briefName != null)
        {
			BriefUI.transform.Find ("Button_Brief").GetComponent<UISprite>().spriteName=briefName;
        }
    }
    private void hideBriefUI()
    {
		showExhibition_p();
		Destroy (BriefUI);
		BriefUI = null;
    }
    /// <summary>
    /// try function
    /// </summary>
    //public void showButtonAnimation()
    //{
    //    buttonAnimation.Play();
    //}

    public void hideIndex()
    {
        hideIndexTable.ResetToBeginning();
        hideIndexTable.PlayForward();
    }

    public void showIndex()
    {
        hideIndexTable.PlayReverse();
    }
    private void woman()
    {
        hideButtons();
        YZ1_Buttons.SetActive(true);
		if (mainClothImage!=null) 
		{
			mainClothImage.GetComponent<UISprite>().spriteName = "yaozunv";
			mainClothImage.GetComponent<UISprite>().MakePixelPerfect();
			mainClothImage.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);	
		}
        dressName = "dress_3_";
        modelName = "YZ1_zhengti";
    }
    private void man()
    {
        hideButtons();
        YZ2_Buttons.SetActive(true);
		if (mainClothImage!=null) 
		{
			mainClothImage.GetComponent<UISprite>().spriteName = "yaozunan";
			mainClothImage.GetComponent<UISprite>().MakePixelPerfect();
			mainClothImage.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		}
        dressName = "dress_5_";
        modelName = "YZ2_zhengti";
    }
    private void Start()
    {
		loadHuman ();
        uievent = UIEvent.Instance();
        //                    case "shiyi":
        //                LoadModel();
        //break;
        //uievent.registerUI("Button_index", indexButtonClick);

        uievent.registerUI("WomanButton", woman);
        uievent.registerUI("ManButton", man);
        uievent.registerUI("Button_BriefUIback", hideBriefUI);
        uievent.registerUI("Button_zhuangzu", ZZ_ButtonClick);
        uievent.registerUI("Button_miaozu", MZ_ButtonClick);
        uievent.registerUI("Button_yaozu", YZ_ButtonClick);
        uievent.registerUI("Button_dongzu", DZ_ButtonClick);
        uievent.registerUI("Button_minzujianjie", getInDescription);
        uievent.registerUI("Button_fushijianjie", descriptionButtonClick);

        uievent.registerUI("Button_descriptionback", backMainTable);

        uievent.registerUI("Button_exihibitionback", backMainTable);
        uievent.registerUI("Button_try", showKinectImage);
        uievent.registerUI("Button_newDetail", showBriefUI);
        //uievent.registerUI("button_dress4", dressPartChange);
        //uievent.registerUI("button_dress5", dressPartChange);

        uievent.registerUI("Button_tryback", onButtonTryBack);
        uievent.registerUI("Button_take_photo", photoButtonClick);
        uievent.registerUI("Button_changeBg", isBgButton);

        uievent.registerUI("Button_head", showHeadPart);
        uievent.registerUI("Button_clothes", showClothPart);
        uievent.registerUI("Button_jewelry", showJewelryPart);

        uievent.registerUI("Button_bg1", bgChangeYI);
        uievent.registerUI("Button_bg2", bgChangeRE);
        uievent.registerUI("Button_bg3", bgChangeSAN);
        uievent.registerUI("Button_true", bgChangeShijing);

        uievent.registerUI("Button_back", tryButtonClick);
        uievent.registerUI("Button_shareback", tryButtonClick);
        uievent.registerUI("Button_sharebackmain", hideKinectImage);

        uievent.registerUI("Button_showDetailback", newDetailBackClick);

        uievent.registerUI("YZ1_Button_part1", onButtonPart1);//帽子
        uievent.registerUI("YZ1_Button_part2", onButtonPart2);//上衣
        uievent.registerUI("YZ1_Button_part3", onButtonPart3);//裤子
        uievent.registerUI("YZ1_Button_part6", onButtonPart6);//披肩
        uievent.registerUI("YZ1_Button_part7", onButtonPart7);

        uievent.registerUI("YZ2_Button_part1", onButtonPart1);
        uievent.registerUI("YZ2_Button_part2", onButtonPart2);
        uievent.registerUI("YZ2_Button_part3", onButtonPart3);
        uievent.registerUI("YZ2_Button_part7", onButtonPart7);

        uievent.registerUI("MZ_Button_part2", onButtonPart2);
        uievent.registerUI("MZ_Button_part4", onButtonPart4);
        uievent.registerUI("MZ_Button_part5", onButtonPart5);

        uievent.registerUI("DZ_Button_part1", onButtonPart1);
        uievent.registerUI("DZ_Button_part2", onButtonPart2);
        uievent.registerUI("DZ_Button_part4", onButtonPart4);
        uievent.registerUI("DZ_Button_part6", onButtonPart6);
        uievent.registerUI("DZ_Button_part7", onButtonPart7);
        uievent.registerUI("DZ_Button_part8", onButtonPart8);
        uievent.registerUI("DZ_Button_part9", onButtonPart9);
        uievent.registerUI("DZ_Button_part10", onButtonPart10);

        uievent.registerUI("ZZ_Button_part2", onButtonPart2);
        uievent.registerUI("ZZ_Button_part4", onButtonPart4);

		uievent.registerUI("Button_tryAngin", resetModel);

    }

    /// <summary>
    /// main
    /// </summary>
    /// <summary>
    /// description
    /// </summary>
    public void hideDescription()
    {
		Destroy (showDescriptionTable);
		showDescriptionTable = null;
		Button_description = null;
		Resources.UnloadUnusedAssets ();
    }

    public void showDescription()
    {
		showDescriptionTable= Instantiate (Resources.Load ("Prefabs/NGUIPrefabs/description"))as GameObject;
		showDescriptionTable.transform.parent = GameObject.Find ("UI Root (3D)").transform;
		showDescriptionTable.transform.localScale = new Vector3(1,1,1);
		showDescriptionTable.transform.position = Vector3.zero;
		Button_description=showDescriptionTable.transform.Find ("Button_description").GetComponent<UISprite> ();
		switch (modelName) 
		{
		case"DZ_zhengti":
			Button_description.spriteName = "jianjie_dong";
			break;
		case"YZ1_zhengti":
			Button_description.spriteName = "jianjie_yao";
			break;

		case"MZ_zhengti":
			Button_description.spriteName = "jianjie_miao";
			break;

		case"ZZ_zhengti":
			Button_description.spriteName = "jianjie_zhuang";
			break;

		case"YZ2_zhengti":
			Button_description.spriteName = "jianjie_yao";
			break;
		default:
			break;
		}
    }

    /// <summary>
    /// show and hide bg button
    /// </summary>
    public void isBgButton()
    {

        string nButtonPic = Button_isBG.normalSprite;
        switch (nButtonPic)
        {
            case "changjing":
                Button_isBG.normalSprite = "cj_xz";
                break;
            case "cj_xz":
                Button_isBG.normalSprite = "changjing";
                break;
        }

		if (changeBgButton!=null)
        {
			Destroy(changeBgButton);
			changeBgButton = null;
			Button_bg1 = null;
			Button_bg2 = null;
			Button_bg3 = null;
			Button_true = null;
        }
        else
        {
			loadChangeBgButton ();
        }

    }
	private void loadChangeBgButton()
	{
		changeBgButton= Instantiate (Resources.Load ("Prefabs/NGUIPrefabs/bg_button"))as GameObject;
		changeBgButton.transform.parent =GameObject.Find ("UI Root (3D)").transform;
		changeBgButton.transform.localScale = new Vector3(1,1,1);
		changeBgButton.transform.localPosition = changeBgButton.transform.position;

		Button_bg1 = changeBgButton.transform.Find ("Button_bg1").GetComponent<UIButton> ();
		Button_bg2 = changeBgButton.transform.Find ("Button_bg2").GetComponent<UIButton> ();
		Button_bg3 = changeBgButton.transform.Find ("Button_bg3").GetComponent<UIButton> ();
		Button_true = changeBgButton.transform.Find ("Button_true").GetComponent<UIButton> ();
	}
    public void hideTry()
    {
		isShowModel = false;
        showTryTable.PlayReverse();
    }

    public void showTry()
    {
        bgChangeShijing();
		isShowModel = true;
		if (changeBgButton!=null)
		{
			Destroy(changeBgButton);
		}
        showTryTable.PlayForward();
    }

    /// <summary>
    /// share
    /// </summary>
    public void hideShare()
    {
        showShareTable.PlayReverse();
    }

    public void showShare()
    {
        showShareTable.PlayForward();
    }

    /// <summary>
    /// showDetialTable
    /// </summary>
    public void hideDetail()
	{		
		Destroy (showDetailTable);
		resetHandUI ();

    }
	public void DetailEndEvent()
	{		
		Resources.UnloadUnusedAssets ();

		Destroy (showDetailTable);
		showDetailTable= Instantiate (Resources.Load ("Prefabs/NGUIPrefabs/show_detail"))as GameObject;
		showDetailTable.transform.parent = GameObject.Find ("UI Root (3D)").transform;
		showDetailTable.transform.localScale = new Vector3(1,1,1);
		showDetailTable.transform.position = Vector3.zero;
		showDetailTable.transform.Find ("PartName").GetComponent<UILabel> ().text = partTexts;
		loadPartModel ();
		resetHandUI ();

	}
	private void loadPartModel()
	{
		partModel = Instantiate(Resources.Load("Prefabs/" + partName)) as GameObject;
		partModel.transform.position = new Vector3(0, 1, 4);
	}
	private GameObject mask3;
    public void showDetail()
    {
		hideExhibition ();
		Resources.UnloadUnusedAssets ();
		showDetailTable= Instantiate (Resources.Load ("Prefabs/NGUIPrefabs/show_detail"))as GameObject;
		showDetailTable.transform.parent = GameObject.Find ("UI Root (3D)").transform;
		showDetailTable.transform.localScale = new Vector3(1,1,1);
		showDetailTable.transform.position = Vector3.zero;
		showDetailTable.transform.Find ("PartName").GetComponent<UILabel> ().text = partTexts;

		if (gameManager.IsFristP3) {
			mask3 = Instantiate (Resources.Load ("Prefabs/NGUIPrefabs/Mask3"))as GameObject;
			mask3.transform.parent = showDetailTable.transform;
			mask3.transform.localScale = new Vector3 (0.6f, 0.6f, 0.6f);
			mask3.transform.position = Vector3.zero;
			MaskManager m = mask3.GetComponent<MaskManager> ();
			m.addBox (showDetailTable.transform.Find ("Button_showDetailback").GetComponent<BoxCollider> ());
			m.setEndEvent (DetailEndEvent);
			gameManager.IsFristP3 = false;
		}
    }

    /// <summary>
    /// button table
    /// </summary>
    /// 
    public void sayHello()
    {
        print("hello");
        hideIndex();
        hideMain();
        hideDescription();
        hideExhibition();
        hideTry();
        hideShare();
        hideDetail();
    }

    public void indexButtonClick()
    {
        //AudioSource[] audioTable =  audioSource.GetComponents<AudioSource>();
        //audioTable[1].enabled = true;
        StartCoroutine(indexClick());
    }
    IEnumerator indexClick()
    {
        Title.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        Title.SetActive(false);
        if (kinectImage != null)
        {
            kinectImage.depth = -100;
            if (gameManager != null)
            {
                gameManager.destroyModel();
            }
        }
        hideIndex();
        hideMain();
        hideDescription();
        hideExhibition();
        hideTry();
        hideShare();
        hideDetail();
        ZZ_ButtonClick();
    }
    public void showMainClick()
    {
        //hideIndex();
        showMain();
        hideDescription();
        hideExhibition();
        hideTry();
        hideShare();
        hideDetail();
        dressName = "dress_1_";
        modelName = "ZZ_zhengti";
        hideButtons();
    }
    private void hideButtons()
	{	
		if (Exhibition != null) {
		
			YZ1_Buttons.SetActive (false);
			YZ2_Buttons.SetActive (false);
			MZ_Buttons.SetActive (false);
			DZ_Buttons.SetActive (false);
			ZZ_Buttons.SetActive (false);
		}
    }
    public void ZZ_ButtonClick()
    {
        hideButton();
        briefName = "fsjs_zhuang";
		if (mainClothImage!=null) 
		{
			mainClothImage.GetComponent<UISprite>().spriteName = "zhuangzu";
			mainClothImage.GetComponent<UISprite>().MakePixelPerfect();
			mainClothImage.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		}
        national = National.ZHUANGZU;
        dressName = "dress_1_";
        modelName = "ZZ_zhengti";
        dress = Dress.ONE;
    }
    public void MZ_ButtonClick()
    {
        hideButton();

        briefName = "fsjs_miao";
		if (mainClothImage!=null) 
		{
			mainClothImage.GetComponent<UISprite>().spriteName = "yifu";
			mainClothImage.GetComponent<UISprite>().MakePixelPerfect();
			mainClothImage.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		}
        dressName = "dress_2_";
        modelName = "MZ_zhengti";
        national = National.MIAOZU;
        dress = Dress.ONE;
    }
    public void YZ_ButtonClick()
    {
        showButton();
        briefName = "fsjs_yao";
		if (mainClothImage!=null) 
		{
			mainClothImage.GetComponent<UISprite>().spriteName = "yaozunv";
			mainClothImage.GetComponent<UISprite>().MakePixelPerfect();
			mainClothImage.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		}
        dressName = "dress_3_";
        modelName = "YZ1_zhengti";
        national = National.YAOZU;
        dress = Dress.ONE;
    }
    public void DZ_ButtonClick()
    {
        hideButton();
        briefName = "fsjs_dong";
		if (mainClothImage!=null) 
		{
			mainClothImage.GetComponent<UISprite>().spriteName = "dongzu";
			mainClothImage.GetComponent<UISprite>().MakePixelPerfect();
			mainClothImage.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		}
        dressName = "dress_4_";
        modelName = "DZ_zhengti";
        national = National.TONGZU;
        dress = Dress.ONE;
    }
	private void showPartButton()
	{
		Button_head.SetActive(true);
		Button_clothes.SetActive(true);
		Button_jewelry.SetActive(true);
	}
	private GameObject selectUI;
	private GameObject mask1;
	private GameObject Button_fushijianjie;
	private GameObject Button_minzujianjie;
	public void showMain_P()
	{
		Resources.UnloadUnusedAssets ();	
		selectUI= Instantiate (Resources.Load ("Prefabs/NGUIPrefabs/select"))as GameObject;
		selectUI.transform.parent = GameObject.Find ("UI Root (3D)").transform;
		selectUI.transform.localScale = new Vector3(1,1,1);
		selectUI.transform.position = Vector3.zero;
		manButton = selectUI.transform.Find ("ManButton").gameObject;
		womanButton = selectUI.transform.Find ("WomanButton").gameObject;
		tongzuButton = selectUI.transform.Find ("Button_dongzu").gameObject;
		yaozuButton = selectUI.transform.Find ("Button_yaozu").gameObject;
		miaozuButton = selectUI.transform.Find ("Button_miaozu").gameObject;
		zhuangzuButton = selectUI.transform.Find ("Button_zhuangzu").gameObject;
		clothName=selectUI.transform.Find ("ClothName").GetComponent<UISprite>();
		Button_fushijianjie = selectUI.transform.Find ("Button_fushijianjie").gameObject;
		Button_minzujianjie = selectUI.transform.Find ("Button_minzujianjie").gameObject;
		mainClothImage=selectUI.transform.Find ("MainClothImage").gameObject;

		switch (modelName) 
		{
		case"DZ_zhengti":
			mainClothImage.GetComponent<UISprite>().spriteName = "dongzu";
			mainClothImage.GetComponent<UISprite>().MakePixelPerfect();
			mainClothImage.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			break;
		case"YZ1_zhengti":
			mainClothImage.GetComponent<UISprite>().spriteName = "yaozunv";
			mainClothImage.GetComponent<UISprite>().MakePixelPerfect();
			mainClothImage.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			break;

		case"MZ_zhengti":
			mainClothImage.GetComponent<UISprite>().spriteName = "yifu";
			mainClothImage.GetComponent<UISprite>().MakePixelPerfect();
			mainClothImage.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			break;

		case"ZZ_zhengti":
			mainClothImage.GetComponent<UISprite>().spriteName = "zhuangzu";
			mainClothImage.GetComponent<UISprite>().MakePixelPerfect();
			mainClothImage.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			break;

		case"YZ2_zhengti":
			mainClothImage.GetComponent<UISprite>().spriteName = "yaozunan";
			mainClothImage.GetComponent<UISprite>().MakePixelPerfect();
			mainClothImage.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			break;
		default:
			break;
		}
	}
	public void showMain()
	{
		Resources.UnloadUnusedAssets ();	
		selectUI= Instantiate (Resources.Load ("Prefabs/NGUIPrefabs/select"))as GameObject;
		selectUI.transform.parent = GameObject.Find ("UI Root (3D)").transform;
		selectUI.transform.localScale = new Vector3(1,1,1);
		selectUI.transform.position = Vector3.zero;
		manButton = selectUI.transform.Find ("ManButton").gameObject;
		womanButton = selectUI.transform.Find ("WomanButton").gameObject;
		tongzuButton = selectUI.transform.Find ("Button_dongzu").gameObject;
		yaozuButton = selectUI.transform.Find ("Button_yaozu").gameObject;
		miaozuButton = selectUI.transform.Find ("Button_miaozu").gameObject;
		zhuangzuButton = selectUI.transform.Find ("Button_zhuangzu").gameObject;
		clothName=selectUI.transform.Find ("ClothName").GetComponent<UISprite>();
		Button_fushijianjie = selectUI.transform.Find ("Button_fushijianjie").gameObject;
		Button_minzujianjie = selectUI.transform.Find ("Button_minzujianjie").gameObject;
		mainClothImage=selectUI.transform.Find ("MainClothImage").gameObject;

		if (gameManager.IsFristP1) 
		{
			showTishi1 ();
			MaskManager m = mask1.GetComponent<MaskManager> ();
			m.addBox (manButton.GetComponent<BoxCollider> ());
			m.addBox (womanButton.GetComponent<BoxCollider> ());
			m.addBox (tongzuButton.GetComponent<BoxCollider> ());
			m.addBox (yaozuButton.GetComponent<BoxCollider> ());
			m.addBox (miaozuButton.GetComponent<BoxCollider> ());
			m.addBox (zhuangzuButton.GetComponent<BoxCollider> ());
			m.addBox (Button_fushijianjie.GetComponent<BoxCollider> ());
			m.addBox (Button_minzujianjie.GetComponent<BoxCollider> ());
			m.setEndEvent (mainEndEvent);
			gameManager.IsFristP1 = false;
		}
	}
	private void mainEndEvent()
	{		
		Destroy (selectUI);
		resetHandUI();
		Resources.UnloadUnusedAssets ();
		selectUI= Instantiate (Resources.Load ("Prefabs/NGUIPrefabs/select"))as GameObject;
		selectUI.transform.parent = GameObject.Find ("UI Root (3D)").transform;
		selectUI.transform.localScale = new Vector3(1,1,1);
		selectUI.transform.position = Vector3.zero;
		manButton = selectUI.transform.Find ("ManButton").gameObject;
		womanButton = selectUI.transform.Find ("WomanButton").gameObject;
		tongzuButton = selectUI.transform.Find ("Button_dongzu").gameObject;
		yaozuButton = selectUI.transform.Find ("Button_yaozu").gameObject;
		miaozuButton = selectUI.transform.Find ("Button_miaozu").gameObject;
		zhuangzuButton = selectUI.transform.Find ("Button_zhuangzu").gameObject;
		clothName=selectUI.transform.Find ("ClothName").GetComponent<UISprite>();
		Button_fushijianjie = selectUI.transform.Find ("Button_fushijianjie").gameObject;
		Button_minzujianjie = selectUI.transform.Find ("Button_minzujianjie").gameObject;
		mainClothImage=selectUI.transform.Find ("MainClothImage").gameObject;
	}
	public void hideMain()
	{	
		Destroy (selectUI);
		resetHandUI ();
	}
	public void showTishi1()
	{Resources.UnloadUnusedAssets ();
		mask1= Instantiate (Resources.Load ("Prefabs/NGUIPrefabs/Mask"))as GameObject;
		mask1.transform.parent = selectUI.transform;
		mask1.transform.localScale = new Vector3(0.6f,0.6f,0.6f);
		mask1.transform.position = Vector3.zero;
	}
	private GameObject mask2;
	private GameObject Exhibition;
	private GameObject YZ1_Buttons;
	private GameObject YZ2_Buttons;
	private GameObject MZ_Buttons;
	private GameObject DZ_Buttons;
	private GameObject ZZ_Buttons;

	private Transform showingButtons;
	public void showTishi2()
	{Resources.UnloadUnusedAssets ();
		mask2= Instantiate (Resources.Load ("Prefabs/NGUIPrefabs/Mask2"))as GameObject;
		mask2.transform.parent = Exhibition.transform;
		mask2.transform.localScale = new Vector3(0.6f,0.6f,0.6f);
		mask2.transform.position = Vector3.zero;
	}
	private void switchButton()
	{
		if (Exhibition!=null) 
		{
			clothName2 = Exhibition.transform.Find ("ClothName").GetComponent<UISprite>();
			switch (modelName) 
			{
			case"DZ_zhengti":
				Destroy (YZ1_Buttons);
				Destroy (YZ2_Buttons);
				Destroy (MZ_Buttons);
				Destroy (ZZ_Buttons);

				clothName2.spriteName = "4";
				clothName2.MakePixelPerfect ();
				clothName2.transform.localScale = new Vector3 (0.5f, 0.5f, 1.0f);
				showingButtons = DZ_Buttons.transform;
				break;
			case"YZ1_zhengti":
				Destroy (DZ_Buttons);
				Destroy (YZ2_Buttons);
				Destroy (MZ_Buttons);
				Destroy (ZZ_Buttons);


				clothName2.spriteName = "5";
				clothName2.MakePixelPerfect ();
				clothName2.transform.localScale = new Vector3 (0.5f, 0.5f, 1.0f);
				showingButtons = YZ1_Buttons.transform;
				break;
			case"MZ_zhengti":
				Destroy (DZ_Buttons);
				Destroy (YZ2_Buttons);
				Destroy (YZ1_Buttons);
				Destroy (ZZ_Buttons);

				clothName2.spriteName = "2";
				clothName2.MakePixelPerfect ();
				clothName2.transform.localScale = new Vector3 (0.5f, 0.5f, 1.0f);
				showingButtons = MZ_Buttons.transform;
				break;
			case"ZZ_zhengti":
				Destroy (DZ_Buttons);
				Destroy (YZ2_Buttons);
				Destroy (YZ1_Buttons);
				Destroy (MZ_Buttons);

				clothName2.spriteName = "1";
				clothName2.MakePixelPerfect ();
				clothName2.transform.localScale = new Vector3 (0.5f, 0.5f, 1.0f);
				showingButtons = ZZ_Buttons.transform;
				break;
			case"YZ2_zhengti":
				Destroy (DZ_Buttons);
				Destroy (ZZ_Buttons);
				Destroy (YZ1_Buttons);
				Destroy (MZ_Buttons);

				clothName2.spriteName = "3";
				clothName2.MakePixelPerfect ();
				clothName2.transform.localScale = new Vector3 (0.5f, 0.5f, 1.0f);
				showingButtons = YZ2_Buttons.transform;
				break;
			}
		}
	}
	public void hideExhibition()
	{
		Destroy (Exhibition);
		resetHandUI ();
	}
	private void ExiEndEvent()
	{			
		Destroy (Exhibition);
		resetHandUI ();
		Resources.UnloadUnusedAssets ();
		Exhibition= Instantiate (Resources.Load ("Prefabs/NGUIPrefabs/exhibition"))as GameObject;
		Exhibition.transform.parent = GameObject.Find ("UI Root (3D)").transform;
		Exhibition.transform.localScale = new Vector3(1,1,1);
		Exhibition.transform.position = Vector3.zero;
		YZ1_Buttons = Exhibition.transform.Find ("YZ_1").gameObject;
		YZ2_Buttons = Exhibition.transform.Find ("YZ_2").gameObject;
		MZ_Buttons = Exhibition.transform.Find ("MZ").gameObject;
		DZ_Buttons = Exhibition.transform.Find ("DZ").gameObject;
		ZZ_Buttons = Exhibition.transform.Find ("ZZ").gameObject;
		switchButton ();
	}
	public void showExhibition_p()
	{Resources.UnloadUnusedAssets ();
		Exhibition= Instantiate (Resources.Load ("Prefabs/NGUIPrefabs/exhibition"))as GameObject;
		Exhibition.transform.parent = GameObject.Find ("UI Root (3D)").transform;
		Exhibition.transform.localScale = new Vector3(1,1,1);
		Exhibition.transform.position = Vector3.zero;
		YZ1_Buttons = Exhibition.transform.Find ("YZ_1").gameObject;
		YZ2_Buttons = Exhibition.transform.Find ("YZ_2").gameObject;
		MZ_Buttons = Exhibition.transform.Find ("MZ").gameObject;
		DZ_Buttons = Exhibition.transform.Find ("DZ").gameObject;
		ZZ_Buttons = Exhibition.transform.Find ("ZZ").gameObject;

		switchButton ();
	}
	public void showExhibition()
	{Resources.UnloadUnusedAssets ();
		Exhibition= Instantiate (Resources.Load ("Prefabs/NGUIPrefabs/exhibition"))as GameObject;
		Exhibition.transform.parent = GameObject.Find ("UI Root (3D)").transform;
		Exhibition.transform.localScale = new Vector3(1,1,1);
		Exhibition.transform.position = Vector3.zero;
		YZ1_Buttons = Exhibition.transform.Find ("YZ_1").gameObject;
		YZ2_Buttons = Exhibition.transform.Find ("YZ_2").gameObject;
		MZ_Buttons = Exhibition.transform.Find ("MZ").gameObject;
		DZ_Buttons = Exhibition.transform.Find ("DZ").gameObject;
		ZZ_Buttons = Exhibition.transform.Find ("ZZ").gameObject;
		switchButton ();
		if (gameManager.IsFristP2) 
		{
			showTishi2 ();
			MaskManager m = mask2.GetComponent<MaskManager> ();
			foreach (Transform item in showingButtons) {
				if (item.GetComponent<BoxCollider> () != null) {
					m.addBox (item.GetComponent<BoxCollider> ());
				}	
			}
			m.addBox (Exhibition.transform.Find ("Button_exihibitionback").GetComponent<BoxCollider> ());
			m.addBox (Exhibition.transform.Find ("Button_try").GetComponent<BoxCollider> ());
			m.addBox (Exhibition.transform.Find ("Button_newDetail").GetComponent<BoxCollider> ());
			m.setEndEvent (ExiEndEvent);
			gameManager.IsFristP2 = false;
		}
	}
	private UISprite clothName2;
	public void ButtonHighLight()
	{	
		if (selectUI != null) 
		{
			manButton.GetComponent<UIButton> ().normalSprite = manButton.GetComponent<UIButton> ().disabledSprite;
			womanButton.GetComponent<UIButton> ().normalSprite = womanButton.GetComponent<UIButton> ().disabledSprite;
			tongzuButton.GetComponent<UIButton> ().normalSprite = tongzuButton.GetComponent<UIButton> ().disabledSprite;
			yaozuButton.GetComponent<UIButton> ().normalSprite = yaozuButton.GetComponent<UIButton> ().disabledSprite;
			miaozuButton.GetComponent<UIButton> ().normalSprite = miaozuButton.GetComponent<UIButton> ().disabledSprite;
			zhuangzuButton.GetComponent<UIButton> ().normalSprite = zhuangzuButton.GetComponent<UIButton> ().disabledSprite;
			switch (modelName) {
			case"DZ_zhengti":
				clothName.spriteName = "4";
				clothName.MakePixelPerfect ();
				clothName.transform.localScale = new Vector3 (0.6f, 0.6f, 1.0f);


				tongzuButton.GetComponent<UIButton> ().normalSprite = tongzuButton.GetComponent<UIButton> ().pressedSprite;
				showPartButton ();
				break;
			case"YZ1_zhengti":
				clothName.spriteName = "3";
				clothName.MakePixelPerfect ();
				clothName.transform.localScale = new Vector3 (0.6f, 0.6f, 1.0f);

				yaozuButton.GetComponent<UIButton> ().normalSprite = yaozuButton.GetComponent<UIButton> ().pressedSprite;
				if (womanButton.gameObject != null) {
					womanButton.GetComponent<UIButton> ().normalSprite = womanButton.GetComponent<UIButton> ().pressedSprite;
				}
				showPartButton ();
				Button_jewelry.SetActive (false);
				break;

			case"MZ_zhengti":
				clothName.spriteName = "2";
				clothName.MakePixelPerfect ();
				clothName.transform.localScale = new Vector3 (0.6f, 0.6f, 1.0f);
				miaozuButton.GetComponent<UIButton> ().normalSprite = miaozuButton.GetComponent<UIButton> ().pressedSprite;
				showPartButton ();
				Button_head.SetActive (false);
				Button_jewelry.SetActive (false);
				break;

			case"ZZ_zhengti":
				clothName.spriteName = "1";
				clothName.MakePixelPerfect ();
				clothName.transform.localScale = new Vector3 (0.6f, 0.6f, 1.0f);

				zhuangzuButton.GetComponent<UIButton> ().normalSprite = zhuangzuButton.GetComponent<UIButton> ().pressedSprite;
				showPartButton ();
				Button_head.SetActive (false);
				Button_jewelry.SetActive (false);
				break;

			case"YZ2_zhengti":
				clothName.spriteName = "5";
				clothName.MakePixelPerfect ();
				clothName.transform.localScale = new Vector3 (0.6f, 0.6f, 1.0f);

				yaozuButton.GetComponent<UIButton> ().normalSprite = yaozuButton.GetComponent<UIButton> ().pressedSprite;
				if (manButton.gameObject != null) {
					manButton.GetComponent<UIButton> ().normalSprite = manButton.GetComponent<UIButton> ().pressedSprite;
				}
				showPartButton ();
				Button_jewelry.SetActive (false);
				break;
			default:
				break;
			}
		}
	}
    public void getInDescription()
    {
        //hideIndex();
        hideMain();
        showDescription();
        hideExhibition();
        hideTry();
        hideShare();
        hideDetail();
    }
    public void newDetailButtonClick()
    {
        if (partModel != null)
        {
            GameObject.Destroy(partModel);
        }
        if (partName == null)
        {
            partName = dressName + "1";
        }
        showDetail();
        //hideIndex();
        hideMain();
        hideDescription();
        hideExhibition();
        hideTry();
        hideShare();
		if (mask3==null) {
			loadPartModel ();
		}
    }

    public void newDetailBackClick()
    {
        if (partModel != null)
        {
            GameObject.Destroy(partModel);
        }
		rightHand.spriteName="shubiao_00000";
		rightHand.MakePixelPerfect ();
        //hideIndex();
        hideMain();
        hideDescription();
		showExhibition_p();
        hideTry();
        hideShare();
        hideDetail();
    }
    public void onButtonTryBack()
    {
		tryBg.gameObject.SetActive(false);
        if (kinectImage != null)
        {
            kinectImage.depth = -100;
            if (gameManager != null)
            {
                gameManager.destroyModel();
            }
        }
        HideDepthImage();
        //hideIndex();
        hideMain();
        hideDescription();
		showExhibition_p();
        hideTry();
        hideShare();
        hideDetail();


		if (changeBgButton!=null) 
		{
			Destroy (changeBgButton);	
		}
        //dressCube = GameObject.Find(dressName);
        //dressCamera.transform.position = new Vector3(dressCube.transform.position.x, 0, 0);
        //Detail_Sptite.spriteName = "yifu";


    }
    public void descriptionButtonClick()
    {
        //hideIndex();
        hideMain();
        hideDescription();
		showExhibition();
        hideTry();
        hideShare();
        hideDetail();



        //dressCube = GameObject.Find(dressName);
        //dressCamera.transform.position = new Vector3(dressCube.transform.position.x, 0, 0);
        //Detail_Sptite.spriteName = "yifu";


    }
    public void showKinectImage()
    {
        if (partModel != null)
        {
            GameObject.Destroy(partModel);
        }
        Debug.Log("this is show image function");
        if (kinectImage != null)
        {
            kinectImage.depth = -6;
            if (gameManager != null)
            {
                model = gameManager.LoadModel(modelName).transform;
                this.GetComponent<SavePicture>().Model = model.gameObject;
            }
        }
        //hideIndex();
        hideMain();
        hideDescription();
        hideExhibition();
        showTry();
        hideShare();
        hideDetail();
		if (model != null) 
		{
			head = model.Find ("Head");
			Cloth = model.Find ("Cloth");
			Jewelry = model.Find("Jewelry");
		}
    }
    public void tryButtonClick()
    {
        if (kinectImage != null)
        {
            if (gameManager != null)
            {
                model = gameManager.LoadModel(modelName).transform;
                this.GetComponent<SavePicture>().Model = model.gameObject;
            }
        }
        //hideIndex();
        hideMain();
        hideDescription();
        hideExhibition();
        showTry();
        hideShare();
        hideDetail();
		if (model != null) 
		{
			head = model.Find ("Head");
			Cloth = model.Find ("Cloth");
			Jewelry = model.Find("Jewelry");
		}
		resetModelButton ();
		if (changeBgButton!=null) 
		{
			Destroy (changeBgButton);	
		}
    }

    public void photoButtonClick()
    {
        //hideIndex();
        hideMain();
        hideDescription();
        hideExhibition();
        hideTry();
        showShare();
        hideDetail();
		resetHandUI ();
        StartCoroutine(GetComponent<SavePicture>().nebutton());
		if (changeBgButton!=null) 
		{
			Destroy (changeBgButton);	
		}
    }
	public void resetHandUI()
	{      
		UITouth.getInstance ().setIsDoIt (false);
		rightHand.GetComponent<HandUIColliderEvent>().EXIT();
		leftHand.GetComponent<HandUIColliderEvent>().EXIT();	
	}
    public void hideKinectImage()
    {
        if (kinectImage != null)
        {
            kinectImage.depth = -100;
            if (gameManager != null)
            {
                gameManager.destroyModel();
            }
        }
        HideDepthImage();
        //hideIndex();
		showMain_P();
        hideDescription();
        hideExhibition();
        hideTry();
        hideShare();
        hideDetail();
    }

    public void backMainTable()
    {
        //hideIndex();
		showMain_P();
        hideDescription();
        hideExhibition();
        hideTry();
        hideShare();
        hideDetail();
    }
	private void loadHuman()
	{
		Resources.UnloadUnusedAssets ();
		Human= Instantiate (Resources.Load ("Prefabs/NGUIPrefabs/HumanCheck"))as GameObject;
		Human.transform.parent = GameObject.Find ("UI Root (3D)").transform;
		Human.transform.localScale = new Vector3(1,1,1);
		Human.transform.position = Vector3.zero;
	}
    public void backTable()
    {
        if (partModel != null)
        {
            GameObject.Destroy(partModel);
        }
		if (Human!=null) {
			Destroy (Human);
		}
		loadHuman ();
        if (kinectImage != null)
        {
            kinectImage.depth = -6;
        }
		if (BriefUI!=null) {
			Destroy (BriefUI);
		}
		tryBg.gameObject.SetActive(false);
        hideIndexTable.transform.localPosition = Vector3.zero;
        //hideIndex();
        hideMain();
        hideDescription();
        hideExhibition();
        hideTry();
        hideShare();
        hideDetail();
    }

    public void animationEnd(GameObject dressCubeEnd, float xRotationEnd)
    {


        if (this.rotationIsPlay != 0)
        {
            //Debug.Log(this);
            //Debug.Log(Time.deltaTime);
            dressCubeEnd.transform.rotation *= Quaternion.Euler(0, xRotationEnd * Time.deltaTime, 0);
            this.rotationIsPlay++;
            if (this.rotationIsPlay > 66)
            {

                this.rotationIsPlay = 0;
            }
        }
    }

    public void animationlast(GameObject dressCubeLast, float xRotationLast)
    {
        dressCubeLast.transform.rotation *= Quaternion.Euler(0, xRotationLast * Time.deltaTime, 0);
    }

    public void animationStop(GameObject dressCubeLast)
    {
        //dressCubeLast.transform.rotation *= Quaternion.Euler(0, 0 , 0);
    }
	private string partTexts;
    public void onButtonPart1()//帽子
    {
        partName = dressName + "1";
		partTexts="帽\n子";
        newDetailButtonClick();
    }
    public void onButtonPart2()//上衣
    {
        partName = dressName + "2";
		partTexts= "上\n衣";
        newDetailButtonClick();
    }
    public void onButtonPart3()//裤子
    {
        partName = dressName + "3";
		partTexts= "裤\n子";
        newDetailButtonClick();
    }
    public void onButtonPart4()//裙子
    {
        partName = dressName + "4";
		partTexts = "裙\n子";
        newDetailButtonClick();
    }
    public void onButtonPart5()//肚兜
    {
        partName = dressName + "5";
		partTexts = "肚\n兜";
        newDetailButtonClick();
    }
    public void onButtonPart6()//披肩
    {
        partName = dressName + "6";
		partTexts = "披\n肩";
        newDetailButtonClick();
    }
    public void onButtonPart7()//腰带
    {
        partName = dressName + "7";
		partTexts = "腰\n带"; 
        newDetailButtonClick();
    }
    public void onButtonPart8()//项链
    {
        partName = dressName + "8".ToString();
		partTexts= "项\n链";
        newDetailButtonClick();
    }
    public void onButtonPart9()//项圈
    {
        partName = dressName + "9";
		partTexts = "项\n圈";
        newDetailButtonClick();
    }
    public void onButtonPart10()//围裙
    {
        partName = dressName + "10";
		partTexts= "围\n裙";
        newDetailButtonClick();
    }
	void partButtonHightLight()
	{
		if (Button_head!=null) {
			Button_head.GetComponent<UIButton> ().normalSprite = Button_head.GetComponent<UIButton> ().disabledSprite;
		}
		if (Button_clothes!=null) {
			Button_clothes.GetComponent<UIButton> ().normalSprite = Button_clothes.GetComponent<UIButton> ().disabledSprite;
		}
		if (Button_jewelry!=null) {
			Button_jewelry.GetComponent<UIButton> ().normalSprite = Button_jewelry.GetComponent<UIButton> ().disabledSprite;
		}
		if (head!=null) 
		{
			if (head.gameObject.activeSelf) 
			{
				Button_head.GetComponent<UIButton> ().normalSprite = Button_head.GetComponent<UIButton> ().pressedSprite;
			}
		}
		if (Cloth!=null) 
		{
			if (Cloth.gameObject.activeSelf) 
			{
				Button_clothes.GetComponent<UIButton> ().normalSprite = Button_clothes.GetComponent<UIButton> ().pressedSprite;
			}
		}
		if (Jewelry!=null) 
		{
			if (Jewelry.gameObject.activeSelf) 
			{
				Button_jewelry.GetComponent<UIButton> ().normalSprite = Button_jewelry.GetComponent<UIButton> ().pressedSprite;
			}
		}
	}
    public void showHeadPart()
    {
        if (model != null)
        {
            head = model.Find("Head");
            if (head != null)
            {
				head.gameObject.SetActive (!head.gameObject.activeSelf);
				gameManager.resetObiSkin ();
            }
        }
    }
    public void showClothPart()
    {
        if (model != null)
        {
             Cloth = model.Find("Cloth");
            if (Cloth != null)
            {
				Cloth.gameObject.SetActive (!Cloth.gameObject.activeSelf);
				gameManager.resetObiSkin ();
            }
        }
    }
    public void showJewelryPart()
    {
        if (model != null)
        {
             Jewelry = model.Find("Jewelry");
            if (Jewelry != null)
            {
				Jewelry.gameObject.SetActive (!Jewelry.gameObject.activeSelf);
				gameManager.resetObiSkin ();
            }
        }
    }
    public void cubeHide()
    {
        string nButtonPic = UIButton.current.GetComponent<UIButton>().normalSprite;
        switch (nButtonPic)
        {
            case "fuzhuang":
                UIButton.current.GetComponent<UIButton>().normalSprite = "fuzhuang_xz";
                UIButton.current.GetComponent<UIButton>().hoverSprite = "fuzhuang_xz_p";
                UIButton.current.GetComponent<UIButton>().pressedSprite = "fuzhuang_xz_p";
                break;
            case "fuzhuang_xz":
                UIButton.current.GetComponent<UIButton>().normalSprite = "fuzhuang";
                UIButton.current.GetComponent<UIButton>().hoverSprite = "fuzhuangpp";
                UIButton.current.GetComponent<UIButton>().pressedSprite = "fuzhuangpp";
                break;

            case "shoushi":
                UIButton.current.GetComponent<UIButton>().normalSprite = "soushi_xz";
                UIButton.current.GetComponent<UIButton>().hoverSprite = "soushi_xz_p";
                UIButton.current.GetComponent<UIButton>().pressedSprite = "soushi_xz_p";
                break;
            case "soushi_xz":
                UIButton.current.GetComponent<UIButton>().normalSprite = "shoushi";
                UIButton.current.GetComponent<UIButton>().hoverSprite = "shoushi_p";
                UIButton.current.GetComponent<UIButton>().pressedSprite = "shoushi_p";
                break;

            case "toushi":
                UIButton.current.GetComponent<UIButton>().normalSprite = "toushi_xz";
                UIButton.current.GetComponent<UIButton>().hoverSprite = "toushi_xz_p";
                UIButton.current.GetComponent<UIButton>().pressedSprite = "toushi_xz_p";
                break;
            case "toushi_xz":
                UIButton.current.GetComponent<UIButton>().normalSprite = "toushi";
                UIButton.current.GetComponent<UIButton>().hoverSprite = "toushipp";
                UIButton.current.GetComponent<UIButton>().pressedSprite = "toushipp";
                break;
        }


        string nDressName = UIButton.current.GetComponent<UIButton>().name;
        switch (nDressName)
        {
            case "Button_head":
                nDressName = dressName.Substring(0, dressName.Length - 1) + "2";
                break;
            case "Button_clothes":
                nDressName = dressName.Substring(0, dressName.Length - 1) + "3";
                break;
            case "Button_jewelry":
                nDressName = dressName.Substring(0, dressName.Length - 1) + "4";
                break;

        }
        GameObject mDress = GameObject.Find(nDressName);
        //mDress.GetComponent<Renderer>().enabled = !mDress.GetComponent<Renderer>().enabled;
    }
    public void ShowDepthImage()
    {
        if (kinectDepthImage != null)
        {
            kinectDepthImage.depth = -2;
        }
    }
    public void HideDepthImage()
    {
        if (kinectDepthImage != null)
        {
            kinectDepthImage.depth = -100;
        }
    }
    public void HideKinectImage()
    {
        if (kinectImage != null)
        {
            kinectImage.depth = -100;
        }
    }
    public void ShowKinectImage()
    {
        if (kinectImage != null)
        {
            kinectImage.depth = -6;
        }
    }
    private void bgChangeYI()
    {
        tryBg.gameObject.SetActive(true);
        ShowDepthImage();
        HideKinectImage();
        tryBg.GetComponent<UISprite>().spriteName = "BG1";
    }
    private void bgChangeRE()
    {
        tryBg.gameObject.SetActive(true);
        ShowDepthImage();
        HideKinectImage();
        tryBg.GetComponent<UISprite>().spriteName = "BG2";
    }
    private void bgChangeSAN()
    {
        tryBg.gameObject.SetActive(true);
        ShowDepthImage();
        HideKinectImage();

        tryBg.GetComponent<UISprite>().spriteName = "BG3";
    }
    private void bgChangeShijing()
    {
        tryBg.gameObject.SetActive(false);
        HideDepthImage();
        ShowKinectImage();
    }
    /// <summary>
    /// try function
    /// </summary>

    public void cameraEnlarge()
    {

    }

    public void cameraEnduce()
    {

    }

    public void cameraUp()
    {

    }

    public void cameraDown()
    {

    }
	public UISprite rightHand;
    private void partModelRotation()
    {
        if (gameManager != null)
        {
			if (mask3 == null) 
			{
				if (gameManager.CheckrightHandClose () != null)
				if (gameManager.CheckrightHandClose ().HandeState == HandCloseInfo.HandCloseState.close) {
					if (partModel != null) 
					{
						rightHand.spriteName = "woquan";
						rightHand.MakePixelPerfect ();
					}
					float nPosition = handPos.x - gameManager.CheckrightHandClose ().Position.x;
					if (nPosition > 0) {
						nPosition = 64;
					} else if (nPosition < 0) {
						nPosition = -64;
					} else {
						nPosition = 0;
					}
					if (partModel != null) {
						partModel.transform.rotation *= Quaternion.Euler (0, 0, nPosition / 6);
					}
					handPos = gameManager.CheckrightHandClose ().Position;
				} 
				else 
				{
					if (partModel != null&&rightHand.spriteName=="woquan") 
					{
						rightHand.spriteName = "shubiao_00000";
						rightHand.MakePixelPerfect ();
					}
				}
			}
        }
    }
	private void bgButtons()
	{
		if (changeBgButton) 
		{
			Button_isBG.normalSprite = Button_isBG.pressedSprite;

			Button_true.normalSprite = Button_true.disabledSprite;
			Button_bg1.normalSprite = Button_bg1.disabledSprite;
			Button_bg2.normalSprite = Button_bg2.disabledSprite;
			Button_bg3.normalSprite = Button_bg3.disabledSprite;
			if (tryBg.gameObject.activeSelf) 
			{
				switch (tryBg.GetComponent<UISprite>().spriteName) 
				{
				case"BG1":
					Button_bg1.normalSprite = Button_bg1.pressedSprite;
					break;
				case"BG2":
					Button_bg2.normalSprite = Button_bg2.pressedSprite;
					break;
				case"BG3":
					Button_bg3.normalSprite = Button_bg3.pressedSprite;
					break;
				default:
					break;
				}	
			}
			else 
			{
				Button_true.normalSprite = Button_true.pressedSprite;
			}
		} 
		else 
		{
			Button_isBG.normalSprite = Button_isBG.disabledSprite;
		}
	}
	public void resetModel()
	{
		if (kinectImage != null)
		{
			if (gameManager != null)
			{
				if (model!=null) 
				{
					model = null;
				}
				model = gameManager.LoadModel(modelName).transform;
				this.GetComponent<SavePicture>().Model = model.gameObject;
				head = model.Find ("Head");
				Cloth = model.Find ("Cloth");
				Jewelry = model.Find("Jewelry");
			}
		}
	}
	private void hidePartButton()
	{
		Button_head.SetActive(false);
		Button_clothes.SetActive(false);
		Button_jewelry.SetActive(false);
	}
	public void resetModelButton()
	{
		switch (modelName) 
		{
			case"DZ_zhengti":
				showPartButton ();
				break;
			case"YZ1_zhengti":
				showPartButton ();
				Button_jewelry.SetActive (false);
				break;

			case"MZ_zhengti":
				showPartButton ();
				Button_head.SetActive (false);
				Button_jewelry.SetActive (false);
				break;

			case"ZZ_zhengti":
				showPartButton ();
				Button_head.SetActive (false);
				Button_jewelry.SetActive (false);
				break;

			case"YZ2_zhengti":
				showPartButton ();
				Button_jewelry.SetActive (false);
				break;
			default:
				break;
		}
		if (gameManager.modelIsDestory()) 
		{
			hidePartButton ();	
		}
	}
    void Update()
	{
		resetModelButton ();
		bgButtons ();
		ButtonHighLight ();
		partButtonHightLight ();
        partModelRotation();
        //animationControl();
        //animationEnd(dressCube, xRotation);
    }
}

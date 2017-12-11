using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SavePicture : MonoBehaviour
{
    //string url = "http://192.168.2.138:8080/UpLoad/UnityUpload.php";
    //string picUrl = "http://192.168.2.138:8080/UpLoad/upload/";
    //string codeUrl = "http://192.168.2.138:8080/UpLoad/qrcode/";
    string url = "http://120.27.116.57:8080/UpLoad/UnityUpload.php";
    string picUrl = "http://120.27.116.57:8080/UpLoad/upload/";
    string codeUrl = "http://120.27.116.57:8080/UpLoad/qrcode/";
    string path;
    private int picNum = 0;
    public UITexture image;
    public UITexture codeimage;
    public GameObject[] hideButtons;
    private GameObject model;
    private Texture2D tex;
    //public Material image;
    void Start()
    {
        path = Application.dataPath + "/pretty.jpg";
    }
    void OnGUI()
    {
    }

	/// 获取时间戳
	/// </summary>
	/// <returns></returns>
	public static string GetTimeStamp(System.DateTime time)
	{
		long ts = ConvertDateTimeToInt(time);
		return ts.ToString();
	}
	/// <summary>  
	/// 将c# DateTime时间格式转换为Unix时间戳格式  
	/// </summary>  
	/// <param name="time">时间</param>  
	/// <returns>long</returns>  
	public static long ConvertDateTimeToInt(System.DateTime time)
	{
		System.DateTime startTime = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
		long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
		return t;
	}

    IEnumerator getTexture2d()
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start(); //  开始监视代码运行时间


        yield return new WaitForEndOfFrame();

        int width = Screen.width;
        int height = Screen.height;
        tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);
        
        tex.Apply();
        byte[] bytes = tex.EncodeToPNG();


        stopwatch.Stop(); //  停止监视

        //  获取当前实例测量得出的总时间
        System.TimeSpan timespan = stopwatch.Elapsed;
        //   double hours = timespan.TotalHours; // 总小时
        //    double minutes = timespan.TotalMinutes;  // 总分钟
        //    double seconds = timespan.TotalSeconds;  //  总秒数
        double milliseconds = timespan.TotalMilliseconds;  //  总毫秒数

        //打印代码执行时间
        Debug.Log(milliseconds);

        // 将字节保存成图片，这个路径只能在PC端对图片进行读写操作  

        for (int i = 0; i < hideButtons.Length; i++)
        {
            hideButtons[i].SetActive(true);
        }
		GetComponent<TableControl> ().resetHandUI ();
        if (model!=null)
        {
            Debug.Log(" this tableControl is not null");
            GameObject.Destroy(model);
            model = null;
        }

        WWWForm form = new WWWForm();
        //Debug.Log(this.picNum+"aa");
		form.AddField("Name", "pic" + GetTimeStamp(System.DateTime.Now)+ this.picNum++);
        //Debug.Log(this.picNum+"bb");
        form.AddBinaryData("post", bytes);
        WWW www = new WWW(url, form);
       // Debug.Log(www.text);
        StartCoroutine(PostData(www));
      //  System.IO.File.WriteAllBytes(path, bytes);
        //Destroy(tex);
    }
    IEnumerator PostData(WWW www)
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start(); //  开始监视代码运行时间

        yield return www;
        //  获取当前实例测量得出的总时间
        System.TimeSpan timespan = stopwatch.Elapsed;
        //   double hours = timespan.TotalHours; // 总小时
        //    double minutes = timespan.TotalMinutes;  // 总分钟
        //    double seconds = timespan.TotalSeconds;  //  总秒数
        double milliseconds = timespan.TotalMilliseconds;  //  总毫秒数

        //打印代码执行时间
        Debug.Log(milliseconds);
        // Debug.Log(www.text);
        StartCoroutine(Load());
    }


    IEnumerator Load()
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start(); //  开始监视代码运行时间


        //WWW www = new WWW(picUrl+"pic"+ (this.picNum-1)+".png");
        //yield return www;
        //    Texture2D texture = www.texture;
        //    image.mainTexture = texture;
        //Debug.Log(www.text);

		WWW qqq = new WWW(codeUrl + "pic" + GetTimeStamp(System.DateTime.Now)+ (this.picNum-1) + ".png");
        //WWW qqq = new WWW(codeUrl + "pic" + this.picNum + ".png");

        yield return qqq;
        Debug.Log(qqq.text);
        if (qqq != null && string.IsNullOrEmpty(qqq.error))
        {
            Texture2D texture1 = qqq.texture;
          //  Debug.Log(texture1);
            codeimage.mainTexture = texture1;

            //   startTime = (double)Time.time - startTime;
            //  Debug.Log("WWW加载用时:" + startTime);
        }
        image.mainTexture = tex;

        stopwatch.Stop(); //  停止监视

        //  获取当前实例测量得出的总时间
        System.TimeSpan timespan = stopwatch.Elapsed;
        //   double hours = timespan.TotalHours; // 总小时
        //    double minutes = timespan.TotalMinutes;  // 总分钟
        //    double seconds = timespan.TotalSeconds;  //  总秒数
        double milliseconds = timespan.TotalMilliseconds;  //  总毫秒数

        //打印代码执行时间
        Debug.Log(milliseconds);
    }
    public GameObject CountDown;

    public GameObject Model
    {
        get
        {
            return model;
        }

        set
        {
            model = value;
        } 
    }

	public IEnumerator nebutton()
    {
        image.mainTexture = null;
        codeimage.mainTexture = null;
        Resources.UnloadUnusedAssets();
        CountDown.SetActive(true);
        for (int i = 0; i < hideButtons.Length; i++)
        {
            hideButtons[i].SetActive(false);
        }
        CountDown.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("UIPNG/daoshu_yi") as Texture2D;
        yield return new WaitForSeconds(1.0f);

        CountDown.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("UIPNG/daoshu_er") as Texture2D;
        yield return new WaitForSeconds(1.0f);

        CountDown.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("UIPNG/daoshu_san") as Texture2D;
        yield return new WaitForSeconds(1.0f);

        CountDown.SetActive(false);
        StartCoroutine(getTexture2d());
    }
}
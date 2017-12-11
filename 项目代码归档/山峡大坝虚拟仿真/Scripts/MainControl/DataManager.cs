using Assets.Scripts.MainControl;
using Assets.Scripts.util;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public TextAsset dataXml;
    public static DataManager instance;
    private Dictionary<string, Model> modes;
    public AudioSource audio;
    public AudioClip shangAudio;
    public AudioClip inAudio;
    public AudioClip outAudio;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //XmlUtil xmlUtil = new XmlUtil(dataXml.text, false);
        XmlDocument xDoc = new XmlDocument();
        xDoc.LoadXml(dataXml.text);
        XmlNodeList list = xDoc.SelectSingleNode("root").ChildNodes;
        modes = new Dictionary<string, Model>();
        foreach(XmlNode node in list)
        {
            Model mode = new Model();
            mode.Name = node.Attributes["设备名称"].Value;
            mode.UiContent = node.Attributes["UI内容"].Value;
            mode.ModeName = node.Attributes["模型名称"].Value;
            modes.Add(mode.ModeName, mode);
        }
    }

    public Model getModeData(string modeName)
    {
        if (modes.ContainsKey(modeName))
            return modes[modeName];
        return null;
    }

    public Dictionary<string, Model> getModes()
    {
        return modes;
    }

    float time = -1;
    float tempTime = -1;
    bool Shang = false;
    Image image;
    Image shangHint;
    Image qian;
    Image tempImgae;
    bool flag = false;
    private void Update()
    {
        if (tempTime >= 0)
        {
            tempTime += Time.deltaTime;
            if (tempTime > 3)
            {
                if (tempTime > 7)
                {
                    flag = false;
                    tempImgae.gameObject.SetActive(false);
                    tempTime = -1;
                }
                else if (tempTime >= 5)
                {
                    tempImgae.gameObject.SetActive(true);
                    image.gameObject.SetActive(false);
                    image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
                }
                else if (tempTime >= 3 && tempTime < 5)
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, (5 - tempTime) / 2f);
                }
            }
        }
        if (time >= 0)
        {
            time += Time.deltaTime;
            if (Shang)
            {
                if(time > 412 && time < 413 && !flag)
                {
                    flag = true;
                    tempTime = 0;
                    time = -1;
                    tempImgae = qian;
                    image.gameObject.SetActive(true);
                    image.sprite = Resources.Load("UI/outImage", typeof(Sprite)) as Sprite;
                    //audio.Stop();
                    //audio.clip = outAudio;
                    //audio.Play();
                }else if(time > 152 && time < 153 && !flag)
                {
                    flag = true;
                    tempTime = 0;
                    tempImgae = shangHint;
                    image.gameObject.SetActive(true);
                    image.sprite = Resources.Load("UI/shangImage", typeof(Sprite)) as Sprite;
                    //audio.Stop();
                    //audio.clip = shangAudio;
                    //audio.Play();
                }
                else if (time > 64 && time < 65 && !flag)
                {
                    flag = true;
                    tempTime = 0;
                    tempImgae = qian;
                    image.gameObject.SetActive(true);
                    image.sprite = Resources.Load("UI/inImage", typeof(Sprite)) as Sprite;
                    //audio.Stop();
                    //audio.clip = inAudio;
                    //audio.Play();
                }
            }else
            {
                if (time > 360 && time < 361 && !flag)
                {
                    flag = true;
                    tempTime = 0;
                    time = -1;
                    tempImgae = qian;
                    image.gameObject.SetActive(true);
                    image.sprite = Resources.Load("UI/outImage", typeof(Sprite)) as Sprite;
                    //audio.Stop();
                    //audio.clip = outAudio;
                    //audio.Play();
                }
                else if (time > 128 && time < 129 && !flag)
                {
                    flag = true;
                    tempTime = 0;
                    tempImgae = shangHint;
                    image.gameObject.SetActive(true);
                    image.sprite = Resources.Load("UI/shangImage", typeof(Sprite)) as Sprite;
                    //audio.Stop();
                    //audio.clip = shangAudio;
                    //audio.Play();
                }
                else if (time > 72 && time < 73 && !flag)
                {
                    flag = true;
                    tempTime = 0;
                    tempImgae = qian;
                    image.gameObject.SetActive(true);
                    image.sprite = Resources.Load("UI/inImage", typeof(Sprite)) as Sprite;
                    //audio.Stop();
                    //audio.clip = inAudio;
                    //audio.Play();
                }
            }
        }
    }
    
    public void startTime(Image image, Image hint, Image shangHint, bool shang)
    {
        time = 0;
        if (image != null)
        {
            image.gameObject.SetActive(false);
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        }
        if (tempImgae)
        {
            tempImgae.gameObject.SetActive(false);
        }
        tempTime = -1;
        this.qian = hint;
        this.shangHint = shangHint;
        this.image = image;
        this.Shang = shang;
    }
    public void stopTime()
    {
        time = -1;
    }
}

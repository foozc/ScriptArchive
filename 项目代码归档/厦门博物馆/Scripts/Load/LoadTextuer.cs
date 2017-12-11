using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using Assets.Scripts.Load;
public class LoadTextuer : MonoBehaviour {
    // Use this for initializatio
    void Awake()
    {
        OnXmlLoad();
        OnXmlLoadContent();
    }
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnXmlLoad()
    {
        //申请一块xml文件内存
        XmlDocument xmlDoc = new XmlDocument();
        //加载xml文件中的文本
        xmlDoc.Load(Application.streamingAssetsPath + "/config/link.xml");
        //用一个节点数组存储xml文件中root节点下的所有子节点
        XmlNodeList list = xmlDoc.SelectSingleNode("linker").ChildNodes;
        for (int i = 0; i < list.Count; i++)
        {
            TextuerPropModel prop = TextuerPropModel.GetInstance();
            prop.readTask(list[i]);
        }
    }

    void OnXmlLoadContent()
    {
        //申请一块xml文件内存
        XmlDocument xmlDoc = new XmlDocument();
        //加载xml文件中的文本
        xmlDoc.Load(Application.streamingAssetsPath + "/config/songContent.xml");
        //用一个节点数组存储xml文件中root节点下的所有子节点
        XmlNodeList list = xmlDoc.SelectSingleNode("linker").ChildNodes;
        for (int i = 0; i < list.Count; i++)
        {
            TextuerPropModel prop = TextuerPropModel.GetInstance();
            prop.readContent(list[i]);
        }
    }
    //设置歌曲类型为民乐合奏
    public void setSongTeypFolkMusic()
    {
        TextuerPropModel prop = TextuerPropModel.GetInstance();

        prop.TEYP = "FolkMusic";
    }

    //设置歌曲类型为拉弦乐
    public void setSongTeypStringedMusic()
    {
        TextuerPropModel prop = TextuerPropModel.GetInstance();

        prop.TEYP = "StringedMusic";
    }

    //设置歌曲类型为吹管乐
    public void setSongTeypWoodwindMusic()
    {
        TextuerPropModel prop = TextuerPropModel.GetInstance();

        prop.TEYP = "WoodwindMusic";
    }

    //设置歌曲类型为弹拨乐
    public void setSongTeypPluckInstrume()
    {
        TextuerPropModel prop = TextuerPropModel.GetInstance();

        prop.TEYP = "PluckInstrume";
    }
    //设置歌曲类型为弦乐
    public void setSongTeypStringMusic()
    {
        TextuerPropModel prop = TextuerPropModel.GetInstance();

        prop.TEYP = "StringMusic";
    }

    //设置歌曲类型为管弦乐
    public void setSongTeypOrchestralMusic()
    {
        TextuerPropModel prop = TextuerPropModel.GetInstance();

        prop.TEYP = "OrchestralMusic";
    }

    //设置歌曲类型为钢琴
    public void setSongTeypPiano()
    {
        TextuerPropModel prop = TextuerPropModel.GetInstance();

        prop.TEYP = "Piano";
    }
}

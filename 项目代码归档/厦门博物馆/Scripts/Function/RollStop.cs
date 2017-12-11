using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class RollStop : MonoBehaviour
{


    //纹理个数
    //private const int m_TextureConut = 6;
  //  private List<string> m_ListTextureName;

 
    //  private EnhanceScrollView m_DEV;
    private int item;
    public int Item
    {
        get
        {
            return item;
        }

        set
        {
            item = value;
        }
    }

    // Use this for initialization

    void Start()
    {
        //m_DEV = EnhanceScrollView.GetInstance;
        //m_DEV.setAct(getCurrentTextuer);



    }

    // Update is called once per frame
    void Update()
    {

    }



    public void getCurrentTextuer(int _curr)
    {
        item = _curr;

    }

    //public void Load()
    //{
    //    //List纹理地址的名字
    //    m_ListTextureName = new List<string>();
    //    //List纹理地址的前缀
    //    string front = "roll";

    //    //List加载纹理的地址名
    //    for (int i = 1; i < m_TextureConut + 1; i++)
    //    {
    //        string tmp = front;
    //        tmp = "Texture/" + front + i.ToString();
    //        m_ListTextureName.Add(tmp);

    //    }
    //}

    //public List<string> GetListTextureName()
    //{
    //    return m_ListTextureName;
    //}
}
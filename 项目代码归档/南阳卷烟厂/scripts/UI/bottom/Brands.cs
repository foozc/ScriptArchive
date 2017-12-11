/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	Brands 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年10月16日18:26:05# 
 *Description: 		   	品牌标签表格型列表：分行分列排列   
 *History: 				修改版本记录
*/

using assest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class Brands : MonoBehaviour
{
    private List<Brand> brands;   //决定总数量、内容、背景大小、位置

    public GameObject itemPrefab;                   //TextToggle预制体：决定单个项大小
    private const int columnCount = 2;              //列数：2
    private const int spacing = 10;                 //内间距
    private RectTransform parentBg;                 //背景矩形

    private float itemWidth, itemHeight;            //单个项的宽高
    private ToggleGroup group;                      //ToggleGroup
    public Text btnText;
    public GameObject image;
    private bool isOpen = false;
    public Color normal, selected;

    public static Brands _instance;

    void Awake()
    {
        _instance = this;
    }

    // Use this for initialization
    void Start()
    {
        if (itemPrefab)
        {
            itemWidth = itemPrefab.GetComponent<RectTransform>().sizeDelta.x;
            itemHeight = itemPrefab.GetComponent<RectTransform>().sizeDelta.y;
        }
        group = gameObject.GetComponent<ToggleGroup>();
        parentBg = gameObject.GetComponent<RectTransform>();
        //设置锚点为左上角
        parentBg.pivot = new Vector2(0, 1);

        GetBrands();
    }

    private void GetBrands()
    {
        brands = new List<Brand>();
        string query = "SELECT * FROM brand";
        DataSet ds = SqlAccess.ExecuteQuery(query);
        DataTable table = ds.Tables[0];
        for (int i = 0; i < table.Rows.Count; i++)
        {
            DataRow dataRow = table.Rows[i];
            Brand brand = new Brand();
            brand.job_num = dataRow[1].ToString();
            brand.name = dataRow[2].ToString();
            brands.Add(brand);
        }
        InitTable();
    }

    private void InitTable()
    {
        int itemCount = brands.Count;                                       //总数量
        int rowCount = Mathf.CeilToInt((float)itemCount / columnCount);     //总行数

        //计算整个背景的宽高
        Vector3 vec = parentBg.sizeDelta;
        vec.x = itemWidth * 2 + spacing * 3;
        vec.y = rowCount * itemHeight + (rowCount + 1) * spacing;
        parentBg.sizeDelta = vec;

        for (int i = 0; i < itemCount; i++)
        {
            GameObject item = Instantiate(itemPrefab);
            item.transform.parent = this.transform;
            int rowIndex = Mathf.CeilToInt(i / columnCount);              //哪一行
            if (i % columnCount == 0)                                     //第一列
            {
                float leftX = itemWidth / 2 + spacing;
                float leftY = -((itemHeight / 2) + (rowIndex * itemHeight) + (spacing * (rowIndex + 1)));
                item.transform.localPosition = new Vector3(leftX, leftY, 0);
            }                                                             //第二列
            else
            {
                float rightX = itemWidth * 1.5f + spacing * 2;
                float rightY = -((itemHeight / 2) + (rowIndex * itemHeight) + (spacing * (rowIndex + 1)));
                item.transform.localPosition = new Vector3(rightX, rightY, 0);
            }
        }

        for (int i = 0; i < this.transform.childCount; i++)
        {
            Brand brand = brands[i];
            Transform item = this.transform.GetChild(i);
            Text label = item.Find("Label").GetComponent<Text>();
            label.text = brand.name;                                                    //设置文字

            Toggle toggle = item.GetComponent<Toggle>();
            toggle.group = group;
            //if (i == 0) { toggle.isOn = true; }                                         //默认第一个被选中
            toggle.onValueChanged.AddListener((bool isSelected) =>                      //点击事件
            {
                int index = ToggleUtil.CurrentToggleIndex(gameObject);
                ToggleUtil.CurrentToggleHighlight(gameObject, normal, selected);
                btnText.text = brands[index].name;                                      //获取品牌名，设置给按钮
                ListManager._instance.OnBrandChanged(brands[index].job_num);            //获取品牌号，传递到ListManager，更新界面          
            });
        }
        btnText.text = "选择品牌";                                                      //默认空
        ToggleUtil.CurrentToggleHighlight(gameObject, normal, selected);
        transform.localPosition += Vector3.up * vec.y;                                  //设置位置
        gameObject.SetActive(false);                                                    //默认隐藏
    }

    public void ShowOrHide()
    {
        if (btnText)
        {
            if (isOpen)
            {
                image.transform.localScale = new Vector3(1, 1, 1);
                gameObject.SetActive(false);
                isOpen = false;
            }
            else
            {
                image.transform.localScale = new Vector3(1, -1, 1);
                gameObject.SetActive(true);
                isOpen = true;
            }
        }
    }

    public string GetCurrentBrand()
    {
        return btnText.text;
    }

    public string DefaultBrandNum()
    {
        return brands[0].job_num;
    }

    public string GetBrandName(string jobNum)
    {
        string brand = "";
        for (int i = 0; i < brands.Count; i++)
        {
            Brand item = brands[i];
            if (item.job_num.Equals(jobNum))
            {
                return item.name;
            }
        }
        return brand;
    }
}

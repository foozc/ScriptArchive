/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	DeviceInfoForm 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年10月11日09:50:24# 
 *Description: 		   	设备信息表格：获取数据、更新表格   
 *History: 				修改版本记录
*/

using assest;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class DeviceInfoForm : MonoBehaviour
{
    public static DeviceInfoForm _instance;
    private GameObject dragpanel_h_form;
    public GameObject itemPrefab_h;         //垂直滑动列表项预制体:生产信息
    private GameObject contentPanelRoot;
    public Sprite lan1, lan;

    string tableName_realtime, tableName_limit;
    int itemCounts = 0;

    void Awake()
    {
        _instance = this;
        dragpanel_h_form = transform.Find("dragpanel_h_form").gameObject;
    }

    /// <summary>
    /// 初始化设备信息列表“获取数据、更新表格
    /// </summary>
    public void InitDeviceInfoList(int deviceIndex, bool isRefresh = false)
    {
        tableName_realtime = GlobalData.Tables[deviceIndex];
        tableName_limit = tableName_realtime.Replace("_realtime", "_limit");
        //查询标题
        string titleQuery = "SELECT * FROM " + tableName_limit + " WHERE options = 'cn'";
        DataSet title = SqlAccess.ExecuteQuery(titleQuery);
        //查询id
        string idQuery = "SELECT max(_id) FROM " + tableName_realtime;
        DataSet dsID = SqlAccess.ExecuteQuery(idQuery);
        if (dsID.Tables.Count > 0)
        {
            DataTable table = dsID.Tables[0];
            if (table.Rows.Count > 0)
            {
                string id = table.Rows[0][0].ToString();
                if (!string.IsNullOrEmpty(id))
                {
                    //查询实时值
                    string valueQuery = "SELECT * FROM " + tableName_realtime + " WHERE _id=" + id;
                    DataSet value = SqlAccess.ExecuteQuery(valueQuery);
                    if (isRefresh)
                    {
                        UpdateForm(value);
                    }
                    else
                    {
                        InitDeviceInfoForm(title, value);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 初始化设备信息列表表格
    /// </summary>
    private void InitDeviceInfoForm(DataSet title, DataSet value)
    {
        if (itemPrefab_h && title != null && value != null)
        {
            Transform tr = dragpanel_h_form.transform;
            if (tr)
            {
                tr.GetComponent<ScrollRect>().vertical = false;
                tr.GetComponent<ScrollRect>().horizontal = false;
                tr = tr.Find("content");
                contentPanelRoot = tr.gameObject;
                //初始化表格：清空其他表格
                if (contentPanelRoot.transform.childCount > 0)
                {
                    for (int i = 0; i < contentPanelRoot.transform.childCount; i++)
                    {
                        Destroy(contentPanelRoot.transform.GetChild(i).gameObject);
                    }
                }
                //填充数据
                if (title.Tables.Count > 0 && value.Tables.Count > 0)
                {
                    DataTable titleTable = title.Tables[0];
                    DataTable valueTable = value.Tables[0];
                    if (titleTable.Rows.Count > 0 && valueTable.Rows.Count > 0)
                    {
                        itemCounts = titleTable.Rows[0].Table.Columns.Count - 3;
                        string brand = valueTable.Rows[0][valueTable.Rows[0].Table.Columns.Count - 2].ToString();
                        string standardQuery = "SELECT * FROM " + tableName_limit + " WHERE brand=" + brand
                                             + " AND options = 'standard_value'";
                        DataSet standard = SqlAccess.ExecuteQuery(standardQuery);
                        DataTable standardTable = standard.Tables[0];

                        for (int i = 0; i < itemCounts; i++)
                        {
                            GameObject item = Instantiate(itemPrefab_h);
                            if (i == 0)
                                Utils.FindGameObject(item, "bg").GetComponent<Image>().sprite = lan1;
                            else
                                Utils.FindGameObject(item, "bg").GetComponent<Image>().sprite = lan;

                            FillDataIntoForm(titleTable, valueTable, standardTable, i, item);
                            item.transform.SetParent(contentPanelRoot.transform);
                            item.transform.localScale = Vector3.one;
                        }
                        HorizontalLayoutGroup layout = contentPanelRoot.GetComponent<HorizontalLayoutGroup>();
                        layout.spacing = 0;
                        Vector2 vec = contentPanelRoot.GetComponent<RectTransform>().sizeDelta;
                        float itemWidth = itemPrefab_h.GetComponent<RectTransform>().sizeDelta.x;
                        vec.x = (itemCounts + 1) * (itemWidth + layout.spacing);                      //要把表头的宽度计算在内
                        contentPanelRoot.GetComponent<RectTransform>().sizeDelta = vec;
                        dragpanel_h_form.GetComponent<ScrollRect>().horizontal = true;                //设置为横向滑动
                    }
                }
            }
        }
    }

    /// <summary>
    /// 更新表格：在当前界面不变的情况下
    /// </summary>
    /// <param name="ds">数据</param>
    public void UpdateForm(DataSet value)
    {
        if (itemCounts == 0) { return; }
        DataTable valueTable = value.Tables[0];
        for (int i = 0; i < itemCounts; i++)
        {
            GameObject item = contentPanelRoot.transform.GetChild(i).gameObject;
            UpdateValue(valueTable, i, item);
        }
    }

    private void FillDataIntoForm(DataTable titleTable, DataTable valueTable, DataTable standardTable, int index, GameObject item)
    {
        DataRow titleRow = null, valueRow = null, standardRow = null;
        string text_title = "", text_value = "", text_standard = "";
        //根据数据库表实际情况决定row位置
        if (titleTable.Rows.Count > 0)
        {
            titleRow = titleTable.Rows[0];
            text_title = titleRow[index + 1].ToString();
        }
        if (valueTable.Rows.Count > 0)
        {
            valueRow = valueTable.Rows[0];
            text_value = valueRow[index + 2].ToString();
        }
        if (standardTable.Rows.Count > 0)
        {
            standardRow = standardTable.Rows[0];
            text_standard = standardRow[index + 1].ToString();
        }
        Utils.FindGameObject(item, "text_title").SetText(text_title);
        if (text_value.IndexOf(".") > 0)
        {
            text_value = double.Parse(text_value).ToString("f4");
        }
        Utils.FindGameObject(item, "text_value").SetText(text_value);
        Utils.FindGameObject(item, "text_standard").SetText(text_standard);
    }

    private void UpdateValue(DataTable valueTable, int index, GameObject item)
    {
        if (valueTable.Rows.Count > 0)
        {
            DataRow valueRow = valueTable.Rows[0];
            Utils.FindGameObject(item, "text_value").SetText(valueRow[index + 2].ToString());
        }
    }
}

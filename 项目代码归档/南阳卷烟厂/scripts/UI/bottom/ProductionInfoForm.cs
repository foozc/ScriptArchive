/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	ProductionInfoForm 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年10月10日17:44:35# 
 *Description: 		   	生产信息表格：获取数据、更新表格   
 *History: 				修改版本记录
*/

using assest;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class ProductionInfoForm : MonoBehaviour
{
    public static ProductionInfoForm _instance;

    void Awake()
    {
        _instance = this;
        dragpanel_v_form = transform.Find("dragpanel_v_form").gameObject;
    }

    private GameObject dragpanel_v_form;
    public GameObject itemPrefab;              //垂直滑动列表项预制体:生产信息
    public Sprite tiao1;                        //第一行背景
    public Sprite tiao;                         //其他行背景
    private GameObject contentPanelRoot;

    /// <summary>
    /// 初始化生产信息列表：获取数据、更新表格
    /// </summary>
    public void InitProductionInfoList(int flow, string brand, bool isRefresh = false)
    {
        string query;
        if (string.IsNullOrEmpty(brand))
        {
            query = "SELECT * FROM product_info WHERE flow=" + flow;
        }
        else {

            query = "SELECT * FROM product_info WHERE flow=" + flow + " AND brand=" + brand;
        }
        //查询结果
        DataSet ds = SqlAccess.ExecuteQuery(query);
        //初始化或者刷新
        if (isRefresh) { UpdateForm(ds); }
        else { InitPdtInfoForm(ds); }
    }

    /// <summary>
    /// 初始化生产信息表格
    /// </summary>
    private void InitPdtInfoForm(DataSet ds)
    {
        if (ds != null)
        {
            DataTable table = ds.Tables[0];
            int itemCounts = table.Rows.Count;
            Transform tr = transform.Find("dragpanel_v_form");
            if (tr)
            {
                tr.GetComponent<ScrollRect>().vertical = false;
                tr.GetComponent<ScrollRect>().horizontal = false;
                tr = tr.Find("content");
                if (tr)
                {
                    contentPanelRoot = tr.gameObject;
                    for (int i = 0; i < contentPanelRoot.transform.childCount; i++)
                    {
                        Destroy(contentPanelRoot.transform.GetChild(i).gameObject);
                    }
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        GameObject item = Instantiate(itemPrefab);
                        if (i == 0)
                        {
                            Utils.FindGameObject(item, "img_tiao").GetComponent<Image>().sprite = tiao1;
                        }
                        else
                        {
                            Utils.FindGameObject(item, "img_tiao").GetComponent<Image>().sprite = tiao;
                        }
                        DataRow dataRow = table.Rows[i];
                        FillDataInfoForm(dataRow, item);
                        item.transform.SetParent(contentPanelRoot.transform);
                        item.transform.localScale = Vector3.one;
                    }
                    VerticalLayoutGroup layout = contentPanelRoot.GetComponent<VerticalLayoutGroup>();
                    layout.spacing = 0;
                    Vector2 vec = contentPanelRoot.GetComponent<RectTransform>().sizeDelta;
                    float itemHeight = itemPrefab.GetComponent<RectTransform>().sizeDelta.y;
                    vec.y = itemCounts * (itemHeight + layout.spacing);
                    contentPanelRoot.GetComponent<RectTransform>().sizeDelta = vec;
                    dragpanel_v_form.GetComponent<ScrollRect>().vertical = true;
                }
            }
        }
    }

    /// <summary>
    /// 更新表格：在当前界面不变的情况下
    /// </summary>
    /// <param name="ds">数据</param>
    public void UpdateForm(DataSet ds)
    {
        DataTable table = ds.Tables[0];
        for (int i = 0; i < table.Rows.Count; i++)
        {
            DataRow dataRow = table.Rows[i];
            GameObject item = contentPanelRoot.transform.GetChild(i).gameObject;
            FillDataInfoForm(dataRow, item);
        }
    }

    /// <summary>
    /// 往表格填充数据
    /// </summary>
    /// <param name="dataRow"></param>
    /// <param name="item"></param>
    private void FillDataInfoForm(DataRow dataRow, GameObject item)
    {
        Utils.FindGameObject(item, "text_sequence").SetText(dataRow[1].ToString());
        Utils.FindGameObject(item, "text_number").SetText(dataRow[2].ToString());
        Utils.FindGameObject(item, "text_batchId").SetText(dataRow[3].ToString());
        Utils.FindGameObject(item, "text_workOrder").SetText(dataRow[4].ToString());
        Utils.FindGameObject(item, "text_groups").SetText(dataRow[5].ToString());
        Utils.FindGameObject(item, "text_classes").SetText(dataRow[6].ToString());
        Utils.FindGameObject(item, "text_startTime").SetText(dataRow[7].ToString());
        Utils.FindGameObject(item, "text_endTime").SetText(dataRow[8].ToString());
        Utils.FindGameObject(item, "text_workTime").SetText(dataRow[9].ToString());
        Utils.FindGameObject(item, "text_wTUnit").SetText(dataRow[10].ToString());
        Utils.FindGameObject(item, "text_output").SetText(dataRow[11].ToString());
        Utils.FindGameObject(item, "text_outUnit").SetText("公斤"); 
        Utils.FindGameObject(item, "text_chestId").SetText(dataRow[13].ToString());
        Utils.FindGameObject(item, "text_chestNum").SetText(dataRow[14].ToString());
        Utils.FindGameObject(item, "text_cnUnit").SetText(dataRow[15].ToString());
        Utils.FindGameObject(item, "text_saveTime").SetText(dataRow[16].ToString());
        Utils.FindGameObject(item, "text_timeUnit").SetText(dataRow[17].ToString());
    }

}

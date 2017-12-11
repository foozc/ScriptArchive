/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	DeviceInfo 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年10月18日14:04:32# 
 *Description: 		   	设备信息详情：点击设备弹出信息弹窗   
 *History: 				修改版本记录
*/

using assest;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class DeviceInfo : MonoBehaviour
{
    private Text deviceName;
    public GameObject itemPrefab;
    private GameObject contentPanelRoot;
    public Sprite bg1, bg2;
    //private GameObject chart;

    private string tableName_realtime;
    private string tableName_limit = "";
    private string titleQuery, valueQuery;
    private string brand;                       //当前品牌
    int itemCounts = 0;

    void Awake()
    {
        deviceName = transform.Find("Head/DeviceName").GetComponent<Text>();
        //chart = transform.Find("Chart").gameObject;
    }

    /// <summary>
    /// 初始化设备信息：传入设备序号、品牌
    /// </summary>
    public void InitDeviceInfo(int index)
    {
        //chart.SetActive(false);
        Dialog._instance.dragpanel_shebei.GetComponent<ScrollRect>().verticalScrollbar.value = 1;
        //查询当前值
        tableName_realtime = GlobalData.Tables[index];
        tableName_limit = tableName_realtime.Replace("_realtime", "_limit");
        deviceName.text = GlobalData.Devices[index];

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
                    InitDeviceInfoForm(title, value);
                }
            }
        }
    }

    private void InitDeviceInfoForm(DataSet title, DataSet value)
    {
        if (itemPrefab && title != null && value != null)
        {
            Transform tr = Dialog._instance.dragpanel_shebei.transform;
            if (tr)
            {
                tr.GetComponent<ScrollRect>().vertical = false;
                tr.GetComponent<ScrollRect>().horizontal = false;
                tr = tr.Find("content");
                contentPanelRoot = tr.gameObject;
                //初始化表格：清空表格
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
                    if (titleTable.Rows.Count > 0)
                    {
                        itemCounts = titleTable.Rows[0].Table.Columns.Count - 3;
                        brand = valueTable.Rows[0][valueTable.Rows[0].Table.Columns.Count - 2].ToString();
                        string standardQuery = "SELECT * FROM " + tableName_limit + " WHERE brand=" + brand
                                             + " AND options = 'standard_value'";
                        DataSet standard = SqlAccess.ExecuteQuery(standardQuery);
                        DataTable standardTable = standard.Tables[0];

                        for (int i = 0; i < itemCounts; i++)
                        {
                            GameObject item = Instantiate(itemPrefab);
                            if (i % 2 == 0)
                                Utils.FindGameObject(item, "img_shebei_item_bg").GetComponent<Image>().sprite = bg1;
                            else
                                Utils.FindGameObject(item, "img_shebei_item_bg").GetComponent<Image>().sprite = bg2;
                            FillDataIntoForm(titleTable, valueTable, i, item);
                            item.transform.SetParent(contentPanelRoot.transform);
                            item.transform.localScale = Vector3.one;
                        }
                        VerticalLayoutGroup layout = contentPanelRoot.GetComponent<VerticalLayoutGroup>();
                        layout.spacing = 0;
                        Vector2 vec = contentPanelRoot.GetComponent<RectTransform>().sizeDelta;
                        float itemHeight = itemPrefab.GetComponent<RectTransform>().sizeDelta.y;
                        vec.y = itemCounts * (itemHeight + layout.spacing);
                        contentPanelRoot.GetComponent<RectTransform>().sizeDelta = vec;
                        Dialog._instance.dragpanel_shebei.GetComponent<ScrollRect>().vertical = true;
                    }
                }
            }
        }
    }

    private void FillDataIntoForm(DataTable titleTable, DataTable valueTable, int index, GameObject item)
    {
        Button detail = Utils.FindGameObject(item, "button_detail").GetComponent<Button>();
        DataRow titleRow = null, valueRow = null;
        string text_title = "", text_value = "";

        if (titleTable.Rows.Count > 0)
        {
            titleRow = titleTable.Rows[0];
            text_title = titleRow[index + 1].ToString();
        }
        if (valueTable.Rows.Count > 0)
        {
            valueRow = valueTable.Rows[0];
            text_value = valueRow[index + 2].ToString();
            detail.enabled = true;
            detail.onClick.RemoveAllListeners();
            detail.onClick.AddListener(() =>
            {
                //if (chart) chart.SetActive(true);
                //DeviceInfoChart dic = chart.GetComponent<DeviceInfoChart>();
                //dic.InitChart(deviceName.text, tableName_realTime, titleRow[index+1].ToString(), index);
                Dialog._instance.dialog_historyInfo.SetActive(true);
                HistoryDetail hd = Dialog._instance.dialog_historyInfo.GetComponent<HistoryDetail>();
                hd.Init(brand, deviceName.text, text_title, tableName_realtime, index);
            });
        }
        else
        {
            Utils.FindGameObject(item, "text_value").SetText("");
            detail.enabled = false;
        }
        Utils.FindGameObject(item, "text_title").SetText(text_title);
        if (text_value.IndexOf(".") > 0)
        {
            text_value = double.Parse(text_value).ToString("f4");
        }
        Utils.FindGameObject(item, "text_value").SetText(text_value);
    }
}

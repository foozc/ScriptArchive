/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	DeviceInfo 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年10月30日09:36:46# 
 *Description: 		   	生产工序段信息详情：点击工序段弹出信息弹窗   
 *History: 				修改版本记录
*/

using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class ProductionInfo : MonoBehaviour
{
    private Text flowName;
    public GameObject itemPrefab;
    private GameObject contentPanelRoot;
    public Sprite bg1, bg2;

    private string[] titles;

    void Awake()
    {
        flowName = transform.Find("Head/FlowName").GetComponent<Text>();
    }

    /// <summary>
    /// 初始化生产工序段信息
    /// </summary>
    public void InitFlowInfo(int flow, string brand)
    {
        Dialog._instance.dragpanel_pdt.GetComponent<ScrollRect>().verticalScrollbar.value = 1;
        flowName.text = GlobalData.Flows[flow];

        titles = GlobalData.FlowTitles;                 //标题
        string[] items = GlobalData.FlowQueryItems;     //查询项
        string[] col, values;                           //条件项、条件值
        string[] operation;
        if (string.IsNullOrEmpty(brand))
        {
            col = new string[1];
            col[0] = "flow";

            values = new string[1];
            values[0] = (flow + 1).ToString();

            operation = new string[1];
            operation[0] = "=";
        }
        else
        {
            col = new string[2];
            col[0] = "flow";
            col[1] = "brand";

            values = new string[2];
            values[0] = (flow + 1).ToString();
            values[1] = brand;

            operation = new string[2];
            operation[0] = "=";
            operation[1] = "=";
        }
        DataSet ds = Init._instance.GetSqlAccess().SelectWhere("product_info", items, col, operation, values);
        if (ds != null) { InitFlowInfoForm(ds); }
    }

    private void InitFlowInfoForm(DataSet show)
    {
        if (itemPrefab && show != null)
        {
            int itemCounts = titles.Length;
            Transform tr = Dialog._instance.dragpanel_pdt.transform;
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
                if (show.Tables.Count > 0)
                {
                    DataTable showTable = show.Tables[0];
                    for (int i = 0; i < itemCounts; i++)
                    {
                        GameObject item = Instantiate(itemPrefab);
                        if (i % 2 == 0)
                            Utils.FindGameObject(item, "img_item_bg").GetComponent<Image>().sprite = bg1;
                        else
                            Utils.FindGameObject(item, "img_item_bg").GetComponent<Image>().sprite = bg2;
                        FillDataIntoForm(showTable, i, item);
                        item.transform.SetParent(contentPanelRoot.transform);
                        item.transform.localScale = Vector3.one;
                    }
                    VerticalLayoutGroup layout = contentPanelRoot.GetComponent<VerticalLayoutGroup>();
                    layout.spacing = 0;
                    Vector2 vec = contentPanelRoot.GetComponent<RectTransform>().sizeDelta;
                    float itemHeight = itemPrefab.GetComponent<RectTransform>().sizeDelta.y;
                    vec.y = itemCounts * (itemHeight + layout.spacing);
                    contentPanelRoot.GetComponent<RectTransform>().sizeDelta = vec;
                    Dialog._instance.dragpanel_pdt.GetComponent<ScrollRect>().vertical = true;
                }
            }
        }
    }

    private void FillDataIntoForm(DataTable showTable, int index, GameObject item)
    {
        Utils.FindGameObject(item, "text_title").SetText(titles[index]);
        if (showTable.Rows.Count > 0)
        {
            DataRow showRow = showTable.Rows[0];
            Utils.FindGameObject(item, "text_value").SetText(showRow[index].ToString());
        }
        else
        {
            Utils.FindGameObject(item, "text_value").SetText("");
        }
    }

}

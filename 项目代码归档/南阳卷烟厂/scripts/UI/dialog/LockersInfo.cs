/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	LockersInfo 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年11月11日20:08:45# 
 *Description: 		   	储柜信息详情：点击储柜弹出信息弹窗   
 *History: 				修改版本记录
*/

using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class LockersInfo : MonoBehaviour
{
    private Text lockerName;
    public GameObject itemPrefab;
    private GameObject contentPanelRoot;
    public Sprite bg1, bg2;

    string query;
    private string[] titles;                //标题、
    private string[] operation = { "=", "=" };

    void Awake()
    {
        lockerName = transform.Find("Head/LockerName").GetComponent<Text>();
    }

    /// <summary>
    /// 初始化储柜信息
    /// </summary>
    /// <param name="idnex"></param>
    /// <param name="brand"></param>
    public void InitLockerInfo(int idnex, string brand)
    {
        Dialog._instance.dragpanel_lockers.GetComponent<ScrollRect>().verticalScrollbar.value = 1;
        lockerName.text = "储柜" + (idnex + 1);

        titles = GlobalData.LockersTitles;                  //标题
        string[] items = GlobalData.LockersQueryItems;      //查询项
        string[] col = { "locker_id", "brand" };            //条件项
        string[] values = new string[2];
        values[0] = (idnex + 1).ToString();
        values[1] = brand.ToString();

        DataSet ds = Init._instance.GetSqlAccess().SelectWhere("lockers_info", items, col, operation, values);
        if (ds != null) { InitLockerInfoForm(ds); }
    }

    private void InitLockerInfoForm(DataSet show)
    {
        if (itemPrefab && show != null)
        {
            int itemCounts = titles.Length;
            Transform tr = Dialog._instance.dragpanel_lockers.transform;
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
                    Dialog._instance.dragpanel_lockers.GetComponent<ScrollRect>().vertical = true;
                }
            }
        }
    }

    private void FillDataIntoForm(DataTable showTable, int index, GameObject item)
    {
        Utils.FindGameObject(item, "text_title").SetText(titles[index]);
        if (showTable.Rows.Count == 0) { return; }
        DataRow showRow = showTable.Rows[0];
        Utils.FindGameObject(item, "text_value").SetText(showRow[index].ToString());
    }

}

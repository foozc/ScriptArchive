/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	GlobalManager 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年10月20日18:19:43# 
 *Description: 		   	全局管理器：检测和刷新故障信息
 *History: 				修改版本记录
*/

using assest;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager _instance;
    private Dictionary<string, List<string>> allCheckColumns = new Dictionary<string, List<string>>();
    private string[] col = { "isconfirm", "about", "startTime", "status", "value", "deviceName", "isShow", "tableName",
                             "alarmColumn", "brand" };

    public static List<AlarmInfo> alarmInfoList = new List<AlarmInfo>();            //所有警报信息
    public static List<AlarmInfo> alarmToDeal;                                      //待处理警报信息
    public static List<AlarmInfo> alarmDealt;                                       //已处理警报信息

    public bool Check = false;

    void Awake()
    {
        _instance = this;
        //if (Check)
        //{
        //    GetAllCheckColumns();                           //获取所有检测项目
        //}
    }

    // Use this for initialization
    void Start()
    {
        //if (Check)
        //{
        //    InvokeRepeating("CheckAlarm", 3, 7);           //检查故障：频率7秒
        //}
        InvokeRepeating("InitAlarmInfoList", 0, 3);     //刷新故障报警列表：频率3秒
        InvokeRepeating("CheckAlarmStatus", 1, 3);      //检测故障状态，刷新结束时间：频率3秒
    }

    public void StartCheck()
    {
        InvokeRepeating("InitAlarmInfoList", 0, 3);     //刷新故障报警列表：频率120秒
        InvokeRepeating("CheckAlarmStatus", 1, 3);      //检测故障状态，刷新结束时间：频率120秒
    }

    public void StopCheck()
    {
        CancelInvoke("InitAlarmInfoList");
        CancelInvoke("CheckAlarmStatus");
    }

    /// <summary>
    /// 获取可检测的项目：第一次过滤
    /// </summary>
    private void GetAllCheckColumns()
    {
        Dictionary<string, List<string>> allDevice = new Dictionary<string, List<string>>();
        string query = "SELECT * FROM all_device";
        DataSet ds = SqlAccess.ExecuteQuery(query);
        if (ds != null && ds.Tables.Count > 0)
        {
            DataTable table = ds.Tables[0];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow dataRow = table.Rows[i];
                List<string> columns = new List<string>();
                int index = 2;
                while (!string.IsNullOrEmpty(dataRow[index].ToString()))
                {
                    columns.Add(dataRow[index].ToString());
                    index++;
                }
                allDevice.Add(dataRow[1].ToString(), columns);
            }
        }

        foreach (KeyValuePair<string, List<string>> pair in allDevice)
        {
            string tableName_realTime = pair.Key;
            string tableName_limit = pair.Key.Replace("realtime", "limit");
            List<string> columns = new List<string>();
            foreach (string column in pair.Value)
            {
                string sql = "SELECT " + column + " FROM " + tableName_limit + " WHERE options = 'alarm' AND " + column + "=1";
                DataSet set = SqlAccess.ExecuteQuery(sql);
                if (set != null && set.Tables.Count > 0)
                {
                    DataTable table = set.Tables[0];
                    if (table.Rows.Count > 0)
                    {
                        columns.Add(column);
                    }
                }
            }
            allCheckColumns.Add(tableName_realTime, columns);
        }
    }

    /// <summary>
    /// 检测异常故障
    /// </summary>
    private void CheckAlarm()
    {
        foreach (KeyValuePair<string, List<string>> pair in allCheckColumns)
        {
            string tableName_realTime = pair.Key;
            string tableName_limit = pair.Key.Replace("realtime", "limit");
            foreach (string column in pair.Value)
            {
                //查询id
                string idQuery = "SELECT max(_id) FROM " + tableName_realTime;
                DataSet dsID = SqlAccess.ExecuteQuery(idQuery);
                if (dsID.Tables.Count > 0)
                {
                    DataTable table = dsID.Tables[0];
                    if (table.Rows.Count > 0)
                    {
                        string id = table.Rows[0][0].ToString();
                        if (!string.IsNullOrEmpty(id))
                        {
                            //查询最新实时值
                            string real = "SELECT " + column + ",brand,name,realTime FROM " + tableName_realTime + " WHERE _id=" + id;
                            DataSet dsReal = SqlAccess.ExecuteQuery(real);
                            if (dsReal.Tables.Count > 0)
                            {
                                DataTable valueTable = dsReal.Tables[0];
                                if (valueTable.Rows.Count > 0)
                                {
                                    string brand = valueTable.Rows[0][1].ToString();
                                    string standardQuery = "SELECT " + column + " FROM " + tableName_limit + " WHERE options='standard_value' AND brand=" + brand;
                                    string rangeQuery = "SELECT " + column + " FROM " + tableName_limit + " WHERE options='error_range' AND brand=" + brand;
                                    string cn = "SELECT " + column + " FROM " + tableName_limit + " WHERE options='cn'";
                                    DataSet dsStandard = SqlAccess.ExecuteQuery(standardQuery);
                                    DataSet dsRange = SqlAccess.ExecuteQuery(rangeQuery);
                                    DataSet dsCN = SqlAccess.ExecuteQuery(cn);
                                    if (dsStandard.Tables[0].Rows.Count > 0 && dsRange.Tables[0].Rows.Count > 0 && dsCN.Tables[0].Rows.Count > 0)
                                    {
                                        string value = dsReal.Tables[0].Rows[0][0].ToString();
                                        string standard = dsStandard.Tables[0].Rows[0][0].ToString();
                                        string range = dsRange.Tables[0].Rows[0][0].ToString();
                                        string name = dsReal.Tables[0].Rows[0][2].ToString();
                                        string time = dsReal.Tables[0].Rows[0][3].ToString();
                                        string columnCN = dsCN.Tables[0].Rows[0][0].ToString();
                                        if (string.IsNullOrEmpty(value)) { continue; }
                                        double realValue = double.Parse(value);
                                        double standardValue = double.Parse(standard);
                                        double rangeValue = double.Parse(range);
                                        if (Mathf.Abs((float)realValue - (float)standardValue) > rangeValue)
                                        {
                                            AlarmInfo info = new AlarmInfo();
                                            info.IsConfirm = 0;
                                            info.Show = 0;
                                            info.About = name + "【" + columnCN + "】数据出现异常(标准值：" + standardValue + ";误差范围：" + rangeValue
                                                       + ";当前值：" + realValue + ")";
                                            info.DeviceName = name;
                                            info.Status = "";
                                            info.Value = value;
                                            info.StartTime = time;
                                            info.TableName = tableName_realTime;
                                            info.Column = column;
                                            info.Brand = brand;
                                            CheckAlarmInfoInDB(info);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 检查数据库是否有同样的故障信息
    /// </summary>
    /// <param name="info"></param>
    private void CheckAlarmInfoInDB(AlarmInfo info)
    {
        string query = "SELECT * FROM alarm_log";
        DataSet ds = SqlAccess.ExecuteQuery(query);
        if (ds != null && ds.Tables.Count > 0)
        {
            DataTable table = ds.Tables[0];
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow dataRow = table.Rows[i];
                    string EndTime = dataRow[4].ToString();
                    string DeviceName = dataRow[7].ToString();
                    string Column = dataRow[10].ToString();
                    string Brand = dataRow[11].ToString();
                    if (string.IsNullOrEmpty(EndTime) && DeviceName.Equals(info.DeviceName) && Column.Equals(info.Column)
                        && Brand.Equals(info.Brand)) { return; }
                }
                // 有数据且没有同一个故障报警
                InsertAlarmIntoDB(info);
                return;
            }
            else
            {
                //数据库没有数据
                InsertAlarmIntoDB(info);
            }
        }
    }

    /// <summary>
    /// 向数据库插入一条警报信息
    /// </summary>
    private void InsertAlarmIntoDB(AlarmInfo info)
    {
        string[] values = { info.IsConfirm.ToString(), info.About, info.StartTime, info.Status, info.Value,
                            info.DeviceName, info.Show.ToString(),info.TableName,info.Column,info.Brand };
        Init._instance.GetSqlAccess().InsertInto("alarm_log", col, values);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 初始化警报列表（故障报警界面）
    /// </summary>
    public void InitAlarmInfoList()
    {
        alarmInfoList.Clear();
        string query = "SELECT * FROM alarm_log ORDER BY startTime desc";       //按照发生时间顺序降序排列
        DataSet ds = SqlAccess.ExecuteQuery(query);
        if (ds != null)
        {
            DataTable table = ds.Tables[0];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow dataRow = table.Rows[i];
                AlarmInfo item = new AlarmInfo();
                item.Id = int.Parse(dataRow[0].ToString());
                item.IsConfirm = int.Parse(dataRow[1].ToString());
                item.About = dataRow[2].ToString();
                item.StartTime = dataRow[3].ToString();
                item.EndTime = dataRow[4].ToString();
                item.Status = dataRow[5].ToString();
                item.Value = dataRow[6].ToString();
                item.DeviceName = dataRow[7].ToString();
                item.Show = int.Parse(dataRow[8].ToString());
                alarmInfoList.Add(item);
            }
        }

        if (alarmInfoList.Count > 0)
        {
            alarmToDeal = new List<AlarmInfo>();
            alarmDealt = new List<AlarmInfo>();
            foreach (AlarmInfo info in alarmInfoList)
            {
                if (info.IsConfirm == 0) { alarmToDeal.Add(info); }
                else { alarmDealt.Add(info); }
            }
            ShowAlarmDialog();
            if (AlarmInfoForm._instance)
                AlarmInfoForm._instance.Init(alarmToDeal, alarmDealt);
        }
    }

    /// <summary>
    /// 弹出故障报警弹窗
    /// </summary>
    private void ShowAlarmDialog()
    {
        Dialog._instance.alarmDialog.SetActive(false);
        foreach (AlarmInfo info in alarmToDeal)
        {
            //已查看了不提示
            if (info.Show == 1)
            {
                Dialog._instance.alarmDialog.SetActive(false);
                continue;
            }
            //鸟瞰视角时不提示
            if (ViewManager._instance.currentView == 2)
            {
                Dialog._instance.alarmDialog.SetActive(false);
                continue;
            }
            Dialog._instance.alarmDialog.SetActive(true);
            Dialog._instance.alarmText.text = info.DeviceName + "出现故障";
            Dialog._instance.tiaozhuan.onClick.RemoveAllListeners();
            Dialog._instance.tiaozhuan.onClick.AddListener(() =>
            {
                SkipToObject(info.DeviceName);
                Dialog._instance.alarmDialog.SetActive(false);
                ShowAlarmDialog();
            });
            Dialog._instance.close.onClick.RemoveAllListeners();
            Dialog._instance.close.onClick.AddListener(() =>
            {
                Dialog._instance.alarmDialog.SetActive(false);
                ShowAlarmDialog();
            });
            info.Show = 1;
            info.Status = "已查看";                     //显示过了
            UpdateAlarmInfoInDB(info);
            break;
        }
    }

    /// <summary>
    /// 跳转至游戏物体位置
    /// </summary>
    private void SkipToObject(string name)
    {
        //切换到故障报警界面
        TopUIPanel._instance.SetCurrentTab(3);
        ListManager._instance.ShowInfoList(4);
        SceneObjects._instance.SkipToObject(name);
    }

    /// <summary>
    /// 更新警报信息到数据库
    /// </summary>
    /// <param name="info"></param>
    private void UpdateAlarmInfoInDB(AlarmInfo info)
    {
        string update = "UPDATE `alarm_log` SET `isconfirm`=" + info.IsConfirm + ",`isShow`=" + info.Show + ",`status`='" + info.Status
                      + "' WHERE `_id`=" + info.Id;
        SqlAccess.ExecuteQuery(update);
    }

    /// <summary>
    /// 检查故障状态
    /// </summary>
    private void CheckAlarmStatus()
    {
        string query = "SELECT _id,endTime,tableName,alarmColumn,brand FROM alarm_log";
        DataSet ds = SqlAccess.ExecuteQuery(query);
        DataTable table = ds.Tables[0];
        if (table.Rows.Count > 0)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow dataRow = table.Rows[i];
                string id = dataRow[0].ToString();
                string endTime = dataRow[1].ToString();
                string tableName = dataRow[2].ToString();
                string Column = dataRow[3].ToString();
                string Brand = dataRow[4].ToString();
                if (string.IsNullOrEmpty(endTime))
                {
                    CheckAlarmEnd(id, tableName, Column, Brand);
                }
            }
        }
    }

    /// <summary>
    /// 检查故障是否结束，更新结束时间
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="column"></param>
    /// <param name="brand"></param>
    private void CheckAlarmEnd(string id, string tableName, string column, string brand)
    {
        string tableName_realTime = tableName;
        string tableName_limit = tableName.Replace("realtime", "limit");
        string real = "SELECT " + column + ",realTime FROM " + tableName_realTime + " as tr WHERE brand = " + brand
                     + " AND _id=(SELECT max(_id) FROM " + tableName_realTime
                     + " as tr1 WHERE tr.brand =tr1.brand)";
        string standard = "SELECT " + column + " FROM " + tableName_limit + " WHERE options='standard_value' AND brand=" + brand;
        string range = "SELECT " + column + " FROM " + tableName_limit + " WHERE options='error_range' AND brand=" + brand;
        string cn = "SELECT " + column + " FROM " + tableName_limit + " WHERE options='cn'";
        DataSet dsReal = SqlAccess.ExecuteQuery(real);
        DataSet dsStandard = SqlAccess.ExecuteQuery(standard);
        DataSet dsRange = SqlAccess.ExecuteQuery(range);

        if (dsReal.Tables.Count > 0 && dsStandard.Tables.Count > 0 && dsRange.Tables.Count > 0)
        {
            if (dsReal.Tables[0].Rows.Count > 0 && dsStandard.Tables[0].Rows.Count > 0 && dsRange.Tables[0].Rows.Count > 0)
            {
                string value = dsReal.Tables[0].Rows[0][0].ToString();
                string time = dsReal.Tables[0].Rows[0][1].ToString();
                float realValue = float.Parse(value);
                float standardValue = float.Parse(dsStandard.Tables[0].Rows[0][0].ToString());
                float rangeValue = float.Parse(dsRange.Tables[0].Rows[0][0].ToString());
                if (Mathf.Abs(realValue - standardValue) <= rangeValue)
                {
                    string update = "UPDATE `alarm_log` SET `endTime`=" + time + ",`status`=" + "'已恢复正常'" + ",`value`=" + value
                          + " WHERE `_id`=" + id;
                    SqlAccess.ExecuteQuery(update);
                }
            }
        }
    }

}

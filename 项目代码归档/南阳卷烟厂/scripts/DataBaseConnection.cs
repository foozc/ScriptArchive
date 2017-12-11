using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

public class DataBaseConnection : MonoBehaviour
{

		public static string strConnect;
		public static SqlConnection dbConnection;
		public static SqlDataAdapter da;
		DataSet ds;
		float connectCount = 0.0f;
		string strCountValue = "";
		string strTest = "";
		List<PlanTable> hisPlanDataList = new List <PlanTable> ();
		List<PlanTable> planDataList = new List <PlanTable> ();
		public struct STRUCT_WORKTIME
		{
				public int workTime_0;
				public int workTime_1;
				public int workTime_2;
				public int workTime_3;
				public int workTime_4;
				public int workTime_5;
				public int workTime_6;
				public int workTime_7;
				public int workTime_8;
				public int workTime_9;
		};

		public struct STRUCT_STR_SHOWCOUNT
		{
				public string strShowCount_0;
				public string strShowCount_1;
				public string strShowCount_2;
				public string strShowCount_3;
				public string strShowCount_4;
				public string strShowCount_5;
				public string strShowCount_6;
				public string strShowCount_7;
				public string strShowCount_8;
				public string strShowCount_9;
		};

		public  STRUCT_WORKTIME struct_workTime;
		public  STRUCT_STR_SHOWCOUNT struct_str_ShowCount;

		// Use this for initialization
		void Awake()
	{

	}
		void Start ()
		{ 
				strConnect = "server=10.65.100.138;database=NYMESDBTEST;uid=zsd;pwd=1qaz@wsx;";
				dbConnection = new SqlConnection (strConnect);
		try{
			dbConnection.Open ();
		}catch (SqlException e)
		{
			Debug.Log("error:"+e.Message.ToString());
		}
				
				if (dbConnection.State == ConnectionState.Open) {
						print ("connection successful");
				} else {
						print ("connections failed");
				}
				//GetCurrentPlan ("叶片回潮段");
				// GetPlan ("叶片回潮段");
				//InitSQL();
		}
	
		// Update is called once per frame
		void Update ()
		{
//				connectCount += Time.deltaTime;
//				if (connectCount >= 1.0f) {
//						//ExcuteSql();
//						connectCount = 0.0f;
//				}
		}

		void OnDestroy ()
		{
				dbConnection.Close ();
		}

		public string GetCurrentPlan (string name)
		{
				string sql = "SELECT * FROM [NYMESDBTEST].[dbo].[View_Zsd_MES_CurrentPlan] WHERE [NYMESDBTEST].[dbo].[View_Zsd_MES_CurrentPlan].TechNM ='" + name + "'";
				string[] value = ExcuteSqls (sql);

				string rtValue = "";
				if (value [0] != "") {
						string[] val = value [0].Split (',');
						
			rtValue = val [0] + "," + val [6] + "," + val [5] + "," + val [7] + "," + val [1] + "," + val [2] + "," + GetCurrentRemainingTime (val [7]) + "," + "null";
				} else {
						rtValue = "--,--,--,--,--,--,--,--";
				}
				return rtValue;

		}

		string GetCurrentRemainingTime (string startTime)
		{
				System.DateTime now = System.DateTime.Now;
				string[] time = startTime.Split (' ');
				string[] times = time [1].Split (':');
				int remainH;
				if (now.Hour > 12) {
						 remainH = now.Hour - 12 - int.Parse (times [0]);
				} else {
						 remainH = now.Hour - int.Parse (times [0]);
				}
				int remainM = now.Minute - int.Parse (times [1]);

				return (40 -(60 * remainH + remainM)).ToString();
		}
		
		public string GetPlan (string name)
		{
				string sql = "SELECT * FROM [NYMESDBTEST].[dbo].[View_Zsd_MES_Plan] WHERE [dbo].[View_Zsd_MES_Plan].TechNM ='" + name + "'";
				string[] value = ExcuteSqls (sql);
				planDataList.Clear ();
				for (int i = 0; i<value.Length; i++) {
						if (value [i] != "") {
								string[] val = value [i].Split (',');
								PlanTable pt = new PlanTable ();
								pt.sequence = (i+1).ToString ();
								pt.number = val [0];
								pt.batchId = val [1];
								pt.workOrder = val [2];
								pt.group = val [6];
								pt.classes = val [5];
								pt.startTime = val [7];
								pt.endTime = val [8];
								pt.workTime = val [12];
								pt.wTUnit = "时间";
								pt.output = val [9];
								pt.optUnit = val [10];
								pt.chestId = "--";
								pt.chestNum = "--";
								pt.cnUnit = val [10];
								pt.saveTime = "--";
								pt.timeUnit = "时间";
								planDataList.Add (pt);
						}
				}
				
				return GetPlanDataToString (planDataList);
		
		}

		public string GetHisPlan (string name)
		{
				string sql = "SELECT * FROM [NYMESDBTEST].[dbo].[View_Zsd_MES_HisPlan] WHERE [dbo].[View_Zsd_MES_HisPlan].TechNM ='" + name + "'";
				string[] value = ExcuteSqls (sql);
				hisPlanDataList.Clear ();
				for (int i = 0; i<value.Length; i++) {
						if (value [i] != "") {
								string[] val = value [i].Split (',');
								PlanTable pt = new PlanTable ();
								pt.sequence = i.ToString ();
								pt.number = val [0];
								pt.batchId = val [1];
								pt.workOrder = val [2];
								pt.group = val [5];
								pt.classes = val [4];
								pt.startTime = val [8];
								pt.endTime = val [9];
								pt.workTime = val [10];
								pt.wTUnit = "时间";
								pt.output = val [6];
								pt.optUnit = val [7];
								pt.chestId = val [11];
								pt.chestNum = val [12];
								pt.cnUnit = val [7];
								pt.saveTime = val [19];
								pt.timeUnit = "时间";
								hisPlanDataList.Add (pt);
						}
				}
		
				return GetPlanDataToString (CorrespondingPlanData ());
		
		}

		List<PlanTable> CorrespondingPlanData ()
		{
		int index = 1;
				List<PlanTable> list = new List <PlanTable> ();
				if (planDataList.Count > 0 && hisPlanDataList.Count > 0) {
						foreach (PlanTable pt in planDataList) {
								PlanTable pts = new PlanTable ();
								foreach (PlanTable hpt in hisPlanDataList) {
										if (pt.workOrder == hpt.workOrder) {
												pts = hpt;
												break;
										}
								}
								pts.sequence = index.ToString();
								list.Add (pts);
								index++;
						}
				}
				return list;

		}

		string GetPlanDataToString (List<PlanTable> lt)
		{
				string val = "";
				foreach (PlanTable pt in lt) {
						
						val += pt.sequence + ",";
						val += pt.number + ",";
						val += pt.batchId + ",";
						val += pt.workOrder + ",";
						val += pt.group + ",";
						val += pt.classes + ",";
						val += pt.startTime + ",";
						val += pt.endTime + ",";
						val += pt.workTime + ",";
						val += pt.wTUnit + ",";
						val += pt.output + ",";
						val += pt.optUnit + ",";
						val += pt.chestId + ",";
						val += pt.chestNum + ",";
						val += pt.cnUnit + ",";
						val += pt.saveTime + ",";
						val += pt.timeUnit + "|";

				}
		//Debug.Log ("a:"+val);
				return val;
		}

		string[] ExcuteSqls (string strSql)
		{
				string strTmpSql = strSql;
				da = new SqlDataAdapter (strTmpSql, dbConnection);
				ds = new DataSet ();
				da.Fill (ds);
				return GetRowsData (ds);
		}

		void ExcuteSql ()
		{
				// calculate the different work time
				InitSQL ();
				/*
		//begin to reverse count
		string strSqlCountExcute = "";  
		string strSqlGetNumExcute = "";

		strSqlCountExcute = "SELECT count(*) FROM [NYMESDBTEST].[dbo].[View_Zsd_MES_CurrentPlan] WHERE [NYMESDBTEST].[dbo].[View_Zsd_MES_CurrentPlan].TechNM = '烘叶丝处理段(气流)'";
		strSqlGetNumExcute = "select datediff(second,act_starttime,getdate()) from [dbo].[View_Zsd_MES_CurrentPlan] where [dbo].[View_Zsd_MES_CurrentPlan].TechNM = '烘叶丝处理段(气流)'";
		ExcuteSqlCount(ref strSqlCountExcute,ref strSqlGetNumExcute,ref struct_str_ShowCount.strShowCount_0, struct_workTime.workTime_0);

		strSqlCountExcute = "SELECT count(*) FROM [NYMESDBTEST].[dbo].[View_Zsd_MES_CurrentPlan] WHERE [NYMESDBTEST].[dbo].[View_Zsd_MES_CurrentPlan].TechNM = '混丝掺配段'";
		strSqlGetNumExcute = "select datediff(second,act_starttime,getdate()) from [dbo].[View_Zsd_MES_CurrentPlan] where [dbo].[View_Zsd_MES_CurrentPlan].TechNM = '混丝掺配段'";
		ExcuteSqlCount(ref strSqlCountExcute,ref strSqlGetNumExcute,ref struct_str_ShowCount.strShowCount_1, struct_workTime.workTime_1);

		strSqlCountExcute = "SELECT count(*) FROM [NYMESDBTEST].[dbo].[View_Zsd_MES_CurrentPlan] WHERE [NYMESDBTEST].[dbo].[View_Zsd_MES_CurrentPlan].TechNM = '加香储丝段'";
		strSqlGetNumExcute = "select datediff(second,act_starttime,getdate()) from [dbo].[View_Zsd_MES_CurrentPlan] where [dbo].[View_Zsd_MES_CurrentPlan].TechNM = '加香储丝段'";
		ExcuteSqlCount(ref strSqlCountExcute,ref strSqlGetNumExcute,ref struct_str_ShowCount.strShowCount_2, struct_workTime.workTime_2);

		strSqlCountExcute = "SELECT count(*) FROM [NYMESDBTEST].[dbo].[View_Zsd_MES_CurrentPlan] WHERE [NYMESDBTEST].[dbo].[View_Zsd_MES_CurrentPlan].TechNM = '叶片回潮段'";
		strSqlGetNumExcute = "select datediff(second,act_starttime,getdate()) from [dbo].[View_Zsd_MES_CurrentPlan] where [dbo].[View_Zsd_MES_CurrentPlan].TechNM = '叶片回潮段'";
		ExcuteSqlCount(ref strSqlCountExcute,ref strSqlGetNumExcute,ref struct_str_ShowCount.strShowCount_3, struct_workTime.workTime_3);

		strSqlCountExcute = "SELECT count(*) FROM [NYMESDBTEST].[dbo].[View_Zsd_MES_CurrentPlan] WHERE [NYMESDBTEST].[dbo].[View_Zsd_MES_CurrentPlan].TechNM = '叶片加料段'";
		strSqlGetNumExcute = "select datediff(second,act_starttime,getdate()) from [dbo].[View_Zsd_MES_CurrentPlan] where [dbo].[View_Zsd_MES_CurrentPlan].TechNM = '叶片加料段'";
		ExcuteSqlCount(ref strSqlCountExcute,ref strSqlGetNumExcute,ref struct_str_ShowCount.strShowCount_4, struct_workTime.workTime_4);

		strSqlCountExcute = "SELECT count(*) FROM [NYMESDBTEST].[dbo].[View_Zsd_MES_CurrentPlan] WHERE [NYMESDBTEST].[dbo].[View_Zsd_MES_CurrentPlan].TechNM = '叶片切丝段'";
		strSqlGetNumExcute = "select datediff(second,act_starttime,getdate()) from [dbo].[View_Zsd_MES_CurrentPlan] where [dbo].[View_Zsd_MES_CurrentPlan].TechNM = '叶片切丝段'";
		ExcuteSqlCount(ref strSqlCountExcute,ref strSqlGetNumExcute,ref struct_str_ShowCount.strShowCount_5, struct_workTime.workTime_5);

		strSqlCountExcute = "SELECT count(*) FROM [NYMESDBTEST].[dbo].[View_Zsd_MES_CurrentPlan] WHERE [NYMESDBTEST].[dbo].[View_Zsd_MES_CurrentPlan].TechNM = '梗预处理段'";
		strSqlGetNumExcute = "select datediff(second,act_starttime,getdate()) from [dbo].[View_Zsd_MES_CurrentPlan] where [dbo].[View_Zsd_MES_CurrentPlan].TechNM = '梗预处理段'";
		ExcuteSqlCount(ref strSqlCountExcute,ref strSqlGetNumExcute,ref struct_str_ShowCount.strShowCount_6, struct_workTime.workTime_6);
		
		strSqlCountExcute = "SELECT count(*) FROM [NYMESDBTEST].[dbo].[View_Zsd_MES_CurrentPlan] WHERE [NYMESDBTEST].[dbo].[View_Zsd_MES_CurrentPlan].TechNM = '梗处理段'";
		strSqlGetNumExcute = "select datediff(second,act_starttime,getdate()) from [dbo].[View_Zsd_MES_CurrentPlan] where [dbo].[View_Zsd_MES_CurrentPlan].TechNM = '梗处理段'";
		ExcuteSqlCount(ref strSqlCountExcute,ref strSqlGetNumExcute,ref struct_str_ShowCount.strShowCount_7, struct_workTime.workTime_7);
		
		strSqlCountExcute = "SELECT count(*) FROM [NYMESDBTEST].[dbo].[View_Zsd_MES_CurrentPlan] WHERE [NYMESDBTEST].[dbo].[View_Zsd_MES_CurrentPlan].TechNM = '切梗丝加料段'";
		strSqlGetNumExcute = "select datediff(second,act_starttime,getdate()) from [dbo].[View_Zsd_MES_CurrentPlan] where [dbo].[View_Zsd_MES_CurrentPlan].TechNM = '切梗丝加料段'";
		ExcuteSqlCount(ref strSqlCountExcute,ref strSqlGetNumExcute,ref struct_str_ShowCount.strShowCount_8, struct_workTime.workTime_8);
		
		strSqlCountExcute = "SELECT count(*) FROM [NYMESDBTEST].[dbo].[View_Zsd_MES_CurrentPlan] WHERE [NYMESDBTEST].[dbo].[View_Zsd_MES_CurrentPlan].TechNM = '烘梗丝加香段'";
		strSqlGetNumExcute = "select datediff(second,act_starttime,getdate()) from [dbo].[View_Zsd_MES_CurrentPlan] where [dbo].[View_Zsd_MES_CurrentPlan].TechNM = '烘梗丝加香段'";
		ExcuteSqlCount(ref strSqlCountExcute,ref strSqlGetNumExcute,ref struct_str_ShowCount.strShowCount_9, struct_workTime.workTime_9);
		*/
				/*
        //add the following code to trigger the different color
		string strSqlTimeDiff = "";
		strSqlTimeDiff = "select count(*), datediff(second,[dbo].[View_Zsd_MES_HisPlan].ACT_STARTTIME,[dbo].[View_Zsd_MES_HisPlan].ACT_ENDTIME)from [dbo].[View_Zsd_MES_HisPlan] JOIN [dbo].[View_Zsd_MES_Plan] ON [dbo].[View_Zsd_MES_HisPlan].WO = [dbo].[View_Zsd_MES_Plan].WO where [NYMESDBTEST].[dbo].[View_Zsd_MES_HisPlan].TechNM = '烘叶丝处理段(气流)'";
		ExcuteSqlDelay(ref strSqlTimeDiff,ref struct_workTime.workTime_0);
		strSqlTimeDiff = "select count(*), datediff(second,[dbo].[View_Zsd_MES_HisPlan].ACT_STARTTIME,[dbo].[View_Zsd_MES_HisPlan].ACT_ENDTIME)from [NYMESDBTEST].[dbo].[View_Zsd_MES_HisPlan] JOIN [dbo].[View_Zsd_MES_Plan] ON [dbo].[View_Zsd_MES_HisPlan].WO = [dbo].[View_Zsd_MES_Plan].WO where [NYMESDBTEST].[dbo].[View_Zsd_MES_HisPlan].TechNM = '混丝掺配段'";
		ExcuteSqlDelay(ref strSqlTimeDiff,ref struct_workTime.workTime_1);
		strSqlTimeDiff = "select count(*), datediff(second,[dbo].[View_Zsd_MES_HisPlan].ACT_STARTTIME,[dbo].[View_Zsd_MES_HisPlan].ACT_ENDTIME)from [NYMESDBTEST].[dbo].[View_Zsd_MES_HisPlan] JOIN [dbo].[View_Zsd_MES_Plan] ON [dbo].[View_Zsd_MES_HisPlan].WO = [dbo].[View_Zsd_MES_Plan].WO where [NYMESDBTEST].[dbo].[View_Zsd_MES_HisPlan].TechNM = '加香储丝段'";
		ExcuteSqlDelay(ref strSqlTimeDiff,ref struct_workTime.workTime_2);
		strSqlTimeDiff = "select count(*), datediff(second,[dbo].[View_Zsd_MES_HisPlan].ACT_STARTTIME,[dbo].[View_Zsd_MES_HisPlan].ACT_ENDTIME)from [NYMESDBTEST].[dbo].[View_Zsd_MES_HisPlan] JOIN [dbo].[View_Zsd_MES_Plan] ON [dbo].[View_Zsd_MES_HisPlan].WO = [dbo].[View_Zsd_MES_Plan].WO where [NYMESDBTEST].[dbo].[View_Zsd_MES_HisPlan].TechNM = '叶片回潮段'";
		ExcuteSqlDelay(ref strSqlTimeDiff,ref struct_workTime.workTime_3);
		strSqlTimeDiff = "select count(*), datediff(second,[dbo].[View_Zsd_MES_HisPlan].ACT_STARTTIME,[dbo].[View_Zsd_MES_HisPlan].ACT_ENDTIME)from [NYMESDBTEST].[dbo].[View_Zsd_MES_HisPlan] JOIN [dbo].[View_Zsd_MES_Plan] ON [dbo].[View_Zsd_MES_HisPlan].WO = [dbo].[View_Zsd_MES_Plan].WO where [NYMESDBTEST].[dbo].[View_Zsd_MES_HisPlan].TechNM = '叶片加料段'";
		ExcuteSqlDelay(ref strSqlTimeDiff,ref struct_workTime.workTime_4);
		strSqlTimeDiff = "select count(*), datediff(second,[dbo].[View_Zsd_MES_HisPlan].ACT_STARTTIME,[dbo].[View_Zsd_MES_HisPlan].ACT_ENDTIME)from [NYMESDBTEST].[dbo].[View_Zsd_MES_HisPlan] JOIN [dbo].[View_Zsd_MES_Plan] ON [dbo].[View_Zsd_MES_HisPlan].WO = [dbo].[View_Zsd_MES_Plan].WO where [NYMESDBTEST].[dbo].[View_Zsd_MES_HisPlan].TechNM = '叶片切丝段'";
		ExcuteSqlDelay(ref strSqlTimeDiff,ref struct_workTime.workTime_5);

        strSqlTimeDiff = "select count(*), datediff(second,[dbo].[View_Zsd_MES_HisPlan].ACT_STARTTIME,[dbo].[View_Zsd_MES_HisPlan].ACT_ENDTIME)from [NYMESDBTEST].[dbo].[View_Zsd_MES_HisPlan] JOIN [dbo].[View_Zsd_MES_Plan] ON [dbo].[View_Zsd_MES_HisPlan].WO = [dbo].[View_Zsd_MES_Plan].WO where [NYMESDBTEST].[dbo].[View_Zsd_MES_HisPlan].TechNM = '梗预处理段'";
		ExcuteSqlDelay(ref strSqlTimeDiff,ref struct_workTime.workTime_6);
		strSqlTimeDiff = "select count(*), datediff(second,[dbo].[View_Zsd_MES_HisPlan].ACT_STARTTIME,[dbo].[View_Zsd_MES_HisPlan].ACT_ENDTIME)from [NYMESDBTEST].[dbo].[View_Zsd_MES_HisPlan] JOIN [dbo].[View_Zsd_MES_Plan] ON [dbo].[View_Zsd_MES_HisPlan].WO = [dbo].[View_Zsd_MES_Plan].WO where [NYMESDBTEST].[dbo].[View_Zsd_MES_HisPlan].TechNM = '梗处理段'";
		ExcuteSqlDelay(ref strSqlTimeDiff,ref struct_workTime.workTime_7);
		strSqlTimeDiff = "select count(*), datediff(second,[dbo].[View_Zsd_MES_HisPlan].ACT_STARTTIME,[dbo].[View_Zsd_MES_HisPlan].ACT_ENDTIME)from [NYMESDBTEST].[dbo].[View_Zsd_MES_HisPlan] JOIN [dbo].[View_Zsd_MES_Plan] ON [dbo].[View_Zsd_MES_HisPlan].WO = [dbo].[View_Zsd_MES_Plan].WO where [NYMESDBTEST].[dbo].[View_Zsd_MES_HisPlan].TechNM = '切梗丝加料段'";
		ExcuteSqlDelay(ref strSqlTimeDiff,ref struct_workTime.workTime_8);
		strSqlTimeDiff = "select count(*), datediff(second,[dbo].[View_Zsd_MES_HisPlan].ACT_STARTTIME,[dbo].[View_Zsd_MES_HisPlan].ACT_ENDTIME)from [NYMESDBTEST].[dbo].[View_Zsd_MES_HisPlan] JOIN [dbo].[View_Zsd_MES_Plan] ON [dbo].[View_Zsd_MES_HisPlan].WO = [dbo].[View_Zsd_MES_Plan].WO where [NYMESDBTEST].[dbo].[View_Zsd_MES_HisPlan].TechNM = '烘梗丝加香段'";
		ExcuteSqlDelay(ref strSqlTimeDiff,ref struct_workTime.workTime_9);
		 */

		}

		void InitSQL ()
		{
				string strSqlWorkTime = "";
				strSqlWorkTime = "SELECT * FROM [dbo].[View_Zsd_MES_Plan]";
				//strSqlWorkTime = "SELECT  [dbo].[View_Zsd_MES_Plan].PROCESS_TIME FROM [dbo].[View_Zsd_MES_Plan] JOIN [dbo].[View_Zsd_MES_CurrentPlan] ON [dbo].[View_Zsd_MES_CurrentPlan].WO = [dbo].[View_Zsd_MES_Plan].WO WHERE [dbo].[View_Zsd_MES_CurrentPlan].TechNM = '烘叶丝处理段(气流)'" ;
				ExcuteInitSqlWorkTime (ref strSqlWorkTime, ref struct_workTime.workTime_0, ref struct_str_ShowCount.strShowCount_0);
//		strSqlWorkTime = "SELECT  [dbo].[View_Zsd_MES_Plan].PROCESS_TIME FROM [dbo].[View_Zsd_MES_Plan] JOIN [dbo].[View_Zsd_MES_CurrentPlan] ON [dbo].[View_Zsd_MES_CurrentPlan].WO = [dbo].[View_Zsd_MES_Plan].WO WHERE [dbo].[View_Zsd_MES_CurrentPlan].TechNM = '混丝掺配段'";
//		ExcuteInitSqlWorkTime (ref strSqlWorkTime,ref struct_workTime.workTime_1,ref struct_str_ShowCount.strShowCount_1);
//		strSqlWorkTime = "SELECT  [dbo].[View_Zsd_MES_Plan].PROCESS_TIME FROM [dbo].[View_Zsd_MES_Plan] JOIN [dbo].[View_Zsd_MES_CurrentPlan] ON [dbo].[View_Zsd_MES_CurrentPlan].WO = [dbo].[View_Zsd_MES_Plan].WO WHERE [dbo].[View_Zsd_MES_CurrentPlan].TechNM = '加香储丝段'";
//		ExcuteInitSqlWorkTime (ref strSqlWorkTime,ref struct_workTime.workTime_2,ref struct_str_ShowCount.strShowCount_2);
//		strSqlWorkTime = "SELECT  [dbo].[View_Zsd_MES_Plan].PROCESS_TIME FROM [dbo].[View_Zsd_MES_Plan] JOIN [dbo].[View_Zsd_MES_CurrentPlan] ON [dbo].[View_Zsd_MES_CurrentPlan].WO = [dbo].[View_Zsd_MES_Plan].WO WHERE [dbo].[View_Zsd_MES_CurrentPlan].TechNM = '叶片回潮段'";
//		ExcuteInitSqlWorkTime (ref strSqlWorkTime,ref struct_workTime.workTime_3,ref struct_str_ShowCount.strShowCount_3);
//		strSqlWorkTime = "SELECT  [dbo].[View_Zsd_MES_Plan].PROCESS_TIME FROM [dbo].[View_Zsd_MES_Plan] JOIN [dbo].[View_Zsd_MES_CurrentPlan] ON [dbo].[View_Zsd_MES_CurrentPlan].WO = [dbo].[View_Zsd_MES_Plan].WO WHERE [dbo].[View_Zsd_MES_CurrentPlan].TechNM = '叶片加料段'";
//		ExcuteInitSqlWorkTime (ref strSqlWorkTime,ref struct_workTime.workTime_4,ref struct_str_ShowCount.strShowCount_4);
//		strSqlWorkTime = "SELECT  [dbo].[View_Zsd_MES_Plan].PROCESS_TIME FROM [dbo].[View_Zsd_MES_Plan] JOIN [dbo].[View_Zsd_MES_CurrentPlan] ON [dbo].[View_Zsd_MES_CurrentPlan].WO = [dbo].[View_Zsd_MES_Plan].WO WHERE [dbo].[View_Zsd_MES_CurrentPlan].TechNM = '叶片切丝段'";
//		ExcuteInitSqlWorkTime (ref strSqlWorkTime,ref struct_workTime.workTime_5,ref struct_str_ShowCount.strShowCount_5);
//
//		strSqlWorkTime = "SELECT  [dbo].[View_Zsd_MES_Plan].PROCESS_TIME FROM [dbo].[View_Zsd_MES_Plan] JOIN [dbo].[View_Zsd_MES_CurrentPlan] ON [dbo].[View_Zsd_MES_CurrentPlan].WO = [dbo].[View_Zsd_MES_Plan].WO WHERE [dbo].[View_Zsd_MES_CurrentPlan].TechNM = '梗预处理段'";
//		ExcuteInitSqlWorkTime (ref strSqlWorkTime,ref struct_workTime.workTime_6,ref struct_str_ShowCount.strShowCount_6);
//		strSqlWorkTime = "SELECT  [dbo].[View_Zsd_MES_Plan].PROCESS_TIME FROM [dbo].[View_Zsd_MES_Plan] JOIN [dbo].[View_Zsd_MES_CurrentPlan] ON [dbo].[View_Zsd_MES_CurrentPlan].WO = [dbo].[View_Zsd_MES_Plan].WO WHERE [dbo].[View_Zsd_MES_CurrentPlan].TechNM = '梗处理段'";
//		ExcuteInitSqlWorkTime (ref strSqlWorkTime,ref struct_workTime.workTime_7,ref struct_str_ShowCount.strShowCount_7);
//		strSqlWorkTime = "SELECT [dbo].[View_Zsd_MES_Plan].PROCESS_TIME FROM [dbo].[View_Zsd_MES_Plan] JOIN [dbo].[View_Zsd_MES_CurrentPlan] ON [dbo].[View_Zsd_MES_CurrentPlan].WO = [dbo].[View_Zsd_MES_Plan].WO WHERE [dbo].[View_Zsd_MES_CurrentPlan].TechNM = '切梗丝加料段'";
//		ExcuteInitSqlWorkTime (ref strSqlWorkTime,ref struct_workTime.workTime_8,ref struct_str_ShowCount.strShowCount_8);
//		strSqlWorkTime = "SELECT [dbo].[View_Zsd_MES_Plan].PROCESS_TIME FROM [dbo].[View_Zsd_MES_Plan] JOIN [dbo].[View_Zsd_MES_CurrentPlan] ON [dbo].[View_Zsd_MES_CurrentPlan].WO = [dbo].[View_Zsd_MES_Plan].WO WHERE [dbo].[View_Zsd_MES_CurrentPlan].TechNM = '烘梗丝加香段'";
//		ExcuteInitSqlWorkTime (ref strSqlWorkTime,ref struct_workTime.workTime_9,ref struct_str_ShowCount.strShowCount_9);
		}

		void ExcuteInitSqlWorkTime (ref string strSql, ref int workTime, ref string strShowCount)
		{
				string strTmpSql = strSql;
				da = new SqlDataAdapter (strTmpSql, dbConnection);
				ds = new DataSet ();
				da.Fill (ds);
				string strTmpCount = "";
				string strTmpWorkTime = "";
				int IntTmpWorkTime;
				PrintTable (ds);
				//strTmpCount = ds.Tables [0].Columns[0].ToString();

				//Debug.Log (strTmpCount);
//		if (strTmpCount == "") 
//		{
//			IntTmpWorkTime = 0;
//		}
//		else
//		{
//			strTmpWorkTime = ds.Tables[0].Rows[0][1].ToString();
//			IntTmpWorkTime = System.Int32.Parse (strTmpWorkTime);
//		}
//		workTime = IntTmpWorkTime * 60;
//		strShowCount = workTime.ToString();
		}

		void PrintTable (DataSet ds)
		{
				string txt = "";
				foreach (DataRow mDr in ds.Tables[0].Rows) {
						foreach (DataColumn mDc in ds.Tables[0].Columns) {
								txt += mDr [mDc].ToString () + ",";

						}
						Debug.Log (txt);
						txt = "";
				}
		}

		string[] GetRowsData (DataSet ds)
		{

				string txt = "";
				foreach (DataRow mDr in ds.Tables[0].Rows) {
						foreach (DataColumn mDc in ds.Tables[0].Columns) {
								txt += mDr [mDc].ToString () + ",";
				
						}
						if (txt.Length > 0) {
								txt.Substring (0, txt.Length - 1);
						}
						txt += "|";
				}
				if (txt.Length > 0) {
						txt.Substring (0, txt.Length - 1);
				}
				string[] value = txt.Split ('|');
				return value;

		}

//		void ExcuteSqlCount (ref string strSql, ref string strSqlNum, ref string strShowCount, int workTime)
//		{
//				string strSqlCountExcute = "";
//				strSqlCountExcute = strSql;
//				da = new SqlDataAdapter (strSqlCountExcute, dbConnection);
//				ds = new DataSet ();
//				da.Fill (ds);
//				string strTmpCountValue = "";
//				strTmpCountValue = ds.Tables [0].Rows [0] [0].ToString ();
//				int tmpCountValue = System.Int32.Parse (strTmpCountValue);
//				if (tmpCountValue == 0) {
//						//do nothing but to show the zero count time number
//				} else {
//						ExcuteSqlGetNum (ref strSqlNum, ref strShowCount, workTime);
//				}
//		}

//		void ExcuteSqlGetNum (ref string strSqlNum, ref string strShowCount, int workTime)
//		{
//				string strSqlGetNumExcute = "";
//				strSqlGetNumExcute = strSqlNum;
//				da = new SqlDataAdapter (strSqlGetNumExcute, dbConnection);
//				ds = new DataSet ();
//				da.Fill (ds);
//				string strTmpCountValue = "";
//				strTmpCountValue = ds.Tables [0].Rows [0] [0].ToString ();
//				int IntTmpCountValue = System.Int32.Parse (strTmpCountValue);
//				IntTmpCountValue = workTime - IntTmpCountValue;
//				if (IntTmpCountValue <= 0) {
//						IntTmpCountValue = 0;
//						//begin to calc the time diff
//				} else {
//						//has finished the work in time
//				}
//				strCountValue = IntTmpCountValue.ToString ();
//				Debug.Log (strCountValue);
//				strShowCount = strCountValue;
//		}
//
//		void ExcuteSqlDelay (ref string strSqlDelay, ref int workTime)
//		{
//				string strLocalSqlDelay = "";
//				strLocalSqlDelay = strSqlDelay;
//				da = new SqlDataAdapter (strLocalSqlDelay, dbConnection);
//				ds = new DataSet ();
//				da.Fill (ds);
//
//				string strCount = "";
//				string strDelay = "";
//				int IntDelay = 0;
//				strCount = ds.Tables [0].Rows [0] [0].ToString ();
//				if (strCount == "0")
//						strDelay = "0";
//				else {
//						strDelay = ds.Tables [0].Rows [0] [1].ToString ();
//						IntDelay = System.Int32.Parse (strDelay);
//						if (IntDelay >= workTime) {
//								//make different color for different time
//						}
//				}
//		 
//		}
}

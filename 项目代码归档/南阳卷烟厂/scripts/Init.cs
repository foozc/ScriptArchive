/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	Init 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年10月23日17:07:01# 
 *Description: 		   	系统初始化：初始化分辨率；初始化数据库链接   
 *History: 				修改版本记录
*/

using assest;
using UnityEngine;

public class Init : MonoBehaviour {

    public static Init _instance;
    private SqlAccess sql;

    void Awake()
    {
        _instance = this;
        Screen.SetResolution(1920, 1080, true);
        sql = new SqlAccess();              //初始化数据库连接 
    }

    public SqlAccess GetSqlAccess()
    {
        return sql;
    }

    void OnDestroy() { if (sql != null) { sql.Close(); } }
}

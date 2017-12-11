/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	GlobalData 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年10月24日11:33:57# 
 *Description: 		   	全局公用数据、常量   
 *History: 				修改版本记录
*/

public class GlobalData
{
    public static string[] Flows = { "真空回潮段", "叶片处理段", "叶丝处理段", "掺配处理", "加香储丝段", "梗处理段", "梗丝处理段" };

    public static string[] Devices = { "叶片松散回潮机", "叶片加料机", "加香机", "薄板烘丝机", "气流烘丝机", "叶丝超级回潮机",
                                       "滚筒式梗丝加料回潮机", "梗丝加料机", "闪蒸式梗丝膨化装置", "振动流化干燥机" };

    public static string[] Tables = { "yepian_huichao_realtime", "yepian_jialiao_realtime","hunshi_jiaxiang_realtime",
                                      "yeshi_baoban_realtime","yeshi_qiliu_realtime","yeshi_qiliucaoqihuicao_realtime",
                                      "yangeng_huicao_realtime","gengsi_jialiao_realtime","gengsi_zhengyageng_realtime","honggengsi_realtime"};

    //工序段数据库查询项
    public static string[] FlowQueryItems = { "sequence", "number", "batchId", "workOrder", "groups", "classes", "startTime", "endTime",
                                           "workTime",  "output","chestId","chestNum","saveTime"};
    //储柜数据库查询项
    public static string[] LockersQueryItems = { "In_Num", "In_Amount", "Save_Time" };

    //工序段查询项对应中文标题
    public static string[] FlowTitles = { "序号", "牌号", "批次号", "工单号", "班组", "班次", "开始时间", "结束时间", "工作时间",
                                            "产量" ,"进柜号", "进柜量", "储柜时间"};
    public static string[] LockersTitles = { "进柜号", "进柜量", "储柜时间" };

}

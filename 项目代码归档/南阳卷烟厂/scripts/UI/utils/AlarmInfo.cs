/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	AlarmInfo 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年10月20日14:03:27# 
 *Description: 		   	故障警报信息实体类
 *History: 				修改版本记录
*/

public class AlarmInfo
{

    public int Id { set; get; }
    public int IsConfirm { set; get; }
    public string About { set; get; }
    public string StartTime { set; get; }
    public string EndTime { set; get; }
    public string Status { set; get; }
    public string Value { set; get; }
    public string DeviceName { set; get; }
    public int Show { set; get; }
    public string TableName { set; get; }
    public string Column { set; get; }
    public string Brand { set; get; }

    public override string ToString()
    {
        return "IsConfirm:" + IsConfirm + ",About:" + About + ",StartTime:" + StartTime + ",EndTime:" + EndTime
               + ",Status:" + Status + ",Value:" + Value + ",DeviceName:" + DeviceName + ",Show:" + Show + ",TableName:" + TableName
               + ",Column:" + Column + ",Brand:" + Brand;
    }
}

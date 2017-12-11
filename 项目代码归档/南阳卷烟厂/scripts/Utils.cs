/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	Utils 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年11月6日11:51:38# 
 *Description: 		    工具类：提供某些通用的方法
 *History: 				修改版本记录
*/

using UnityEngine;
using UnityEngine.UI;

public static class Utils
{
    /// <summary>
    /// 查找物体
    /// </summary>
    /// <param name="parentGameObject"></param>
    /// <param name="names"></param>
    /// <returns></returns>
    public static GameObject FindGameObject(GameObject parentGameObject, params string[] names)
    {
        GameObject go = parentGameObject;
        string findPath = "";
        foreach (string name in names)
        {
            findPath += name;
            if (go == null)
            {
                go = GameObject.Find(name);
            }
            else
            {
                Transform tr = go.transform.Find(name);
                if (tr == null)
                {
                    Debug.LogError("canot find gameobject : " + findPath);
                }
                else
                {
                    go = tr.gameObject;
                }
            }
            if (go == null)
            {
                Debug.LogError("canot find gameobject : " + findPath);
                break;
            }
            findPath += "->";
        }
        return go;
    }

    /// <summary>
    /// 扩展方法:设置文本
    /// </summary>
    /// <param name="target"></param>
    /// <param name="text"></param>
    public static void SetText(this GameObject target, string text)
    {
        if (target)
        {
            Text textComponent = target.GetComponent<Text>();
            if (textComponent)
            {
                textComponent.text = text;
            }
        }
    }
}

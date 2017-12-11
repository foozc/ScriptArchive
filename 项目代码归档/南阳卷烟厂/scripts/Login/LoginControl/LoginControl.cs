/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	LoginControl 
 *Author:       	   	#FUZHICHAO# 
 *Date:         	   	#DATE# 
 *Description: 		   	登录功能   
 *History: 				修改版本记录
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using assest;
using System.Text;
using System;
using UnityEngine.UI;
using System.Security.Cryptography;

public class LoginControl : MonoBehaviour {

    //管理员界面与用户界面登录切换
    public GameObject login_panel;
    public GameObject admin_panel;    

    //用户界面账号密码
    public UIInput login_username;
    public UIInput login_password;

    //管理员界面账号密码
    public UIInput admin_username;
    public UIInput admin_password;

    //弹出框以及弹出框信息
    public GameObject Alert;
    public GameObject Mark;
    public GameObject AdminMark;
    public UILabel AlertLabel;

    //弹出框显示时间
    private float alertTime = 0;

    //弹出框倒计时
    private bool loginBool = false; 
    private float loginTime = 0;
    private bool adminBool = false;
    private float adminTime = 0;

    //登录界面与管理界面
    public GameObject Login_UI;
    public GameObject Admin_UI;

    //用户信息操作界面
    //public GameObject Update_info_panel;
    //public GameObject Add_info_panel;
    //public GameObject Delete_info_panel;
    public GameObject ScrowControl;
    private string change_id;

    public GameObject ScrowViews;
    public GameObject Update_panel;

    /// <summary>
    /// after admin login find all data
    /// </summary>
    private List<List<string>> userData = new List<List<string>>();

    // Use this for initialization
    void Start () {
        
    }

    //登录界面初始化
    public void LoginMainInit()
    {
        alertTime = 0;
        loginBool = false;
        loginTime = 0;

        adminBool = false;
        adminTime = 0;
    }
	
	// Update is called once per frame
	void Update () {
		if(Alert.activeSelf == true)
        {
            //show message
            alertTime += Time.deltaTime;
            if(alertTime > 1.2)
            {
                Mark.SetActive(false);
                AdminMark.SetActive(false);
                Alert.SetActive(false);
                alertTime = 0;
            }
        }

        if(loginBool == true)
        {
            //loading scene
            loginTime += Time.deltaTime;
            if(loginTime > 0.2)
            {
                //  Debug.Log("show the NY scene");
                Globe.nextSceneName = "NY";
                UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
                loginTime = 0;
                loginBool = false;
            }
        }

        if(adminBool == true)
        {
            //loading admin panel
            adminTime += Time.deltaTime;
            if(adminTime > 0.5)
            {
                //
                Admin_UI.SetActive(true);
                Admin_UI_Init();
                Login_UI.SetActive(false);
                adminTime = 0;
                adminBool = false;
            }
        }
	}


    //系统管理模块登录
    public void goto_admin_panel()
    {
        login_panel.SetActive(false);

        admin_panel_init();
        admin_panel.SetActive(true);
    }

    //三维漫游模块登录
    public void goto_login_panel()
    {
        admin_panel.SetActive(false);

        login_panel_init();
        login_panel.SetActive(true);
    }

    //系统管理模块登录初始化
    public void admin_panel_init()
    {
        admin_username.value = "";
        admin_password.value = "";
    }

    //三维漫游模块登录初始化
    public void login_panel_init()
    {
        login_username.value = "";
        login_password.value = "";
    }


    //三维漫游模块登录验证
    public void Login_Confirm()
    {
        SqlAccess sql = new SqlAccess();

        string[] items = new string[2];
        items[0] = "password";
        items[1] = "gradation";

        string[] cols = new string[1];
        cols[0] = "username";

        string[] operations = new string[1];
        operations[0] = "=";

        string[] values = new string[1];
        values[0] = login_username.value;

        string[] results = new string[2];

        DataSet ds = sql.SelectWhere("user_info", items, cols, operations, values);
        
        if (ds != null)
        {
            DataTable table = ds.Tables[0];
            foreach (DataRow dataRow in table.Rows)
            {
                int i = 0;
                foreach (DataColumn dataColumn in table.Columns)
                {
                    results[i++] = dataRow[dataColumn].ToString();
                }
                i = 0;
            }
        }
        Debug.Log(results.Length);
        
        //results[0]:password results[1]:gradaation
        if (results[0] == md5(login_password.value))
        {            
            //Alert.SetActive(true);
            loginBool = true;
            //AlertLabel.text = "登录成功";
            if(results[1] == "1")
            {
                sql.Close();
                PlayerPrefs.SetString("gradation", "1");
                //UnityEngine.SceneManagement.SceneManager.LoadScene("NY");
            }
            else
            {
                sql.Close();
                PlayerPrefs.SetString("gradation", "0");
                //UnityEngine.SceneManagement.SceneManager.LoadScene("NY");
            }
        }
        else
        {
            Alert.SetActive(true);
            Mark.SetActive(true);
            AlertLabel.text = "登录失败";
        }


        //PlayerPrefs.SetString("gradation", "0");
        //UnityEngine.SceneManagement.SceneManager.LoadScene("NY");
    }


    //系统管理模块登录验证
    public void Admin_Confirm()
    {
        SqlAccess sql = new SqlAccess();

        string[] items = new string[3];
        items[0] = "password";
        items[1] = "gradation";
        items[2] = "name";

        string[] cols = new string[1];
        cols[0] = "username";

        string[] operations = new string[1];
        operations[0] = "=";

        string[] values = new string[1];
        values[0] = admin_username.value;

        string[] results = new string[3];

        DataSet ds = sql.SelectWhere("user_info", items, cols, operations, values);

        if (ds != null)
        {
            DataTable table = ds.Tables[0];
            foreach (DataRow dataRow in table.Rows)
            {
                int i = 0;
                foreach (DataColumn dataColumn in table.Columns)
                {
                    results[i++] = dataRow[dataColumn].ToString();
                }
                i = 0;
            }
        }
        Debug.Log(results.Length);

        //results[0]:password results[1]:gradaation
        if (results[0] == md5(admin_password.value))
        {
            
            if (results[1] == "1")
            {
                //Alert.SetActive(true);
                sql.Close();
                PlayerPrefs.SetString("admin_name", results[2]);
                //AlertLabel.text = "登录成功";

                //goto admin panel
                adminBool = true;
            }
            else
            {
                Debug.Log(results[1]);
                Alert.SetActive(true);
                Mark.SetActive(true);
                sql.Close();
                AlertLabel.text = "无权限";
            }
        }
        else
        {
            Alert.SetActive(true);
            Mark.SetActive(true);
            sql.Close();
            AlertLabel.text = "登录失败";
        }


        PlayerPrefs.SetString("gradation", "0");
    }

    /// <summary>
    /// md5 crypt
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static String md5(String s)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);
        bytes = md5.ComputeHash(bytes);
        md5.Clear();


        string ret = "";
        for (int i = 0; i < bytes.Length; i++)
        {
            ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
        }
        //BitConverter.ToString(output).Replace("-", "");
        //return ret.PadLeft(32, '0');
        return BitConverter.ToString(bytes).Replace("-", "");

    }

    public void Back_Login()
    {
        Admin_UI.SetActive(false);
        Login_UI.SetActive(true);
        admin_panel_init();
        login_panel_init();
    }

    /// <summary>
    /// admin成功登录后信息刷新
    /// </summary>
    public void Admin_UI_Init()
    {
        SqlAccess sql = new SqlAccess();
    
        string[] items = new string[5];
        items[0] = "_id";
        items[1] = "name";
        items[2] = "username";
        items[3] = "password";
        items[4] = "gradation";

        string[] cols = new string[1];
        cols[0] = "gradation";

        string[] operations = new string[1];
        operations[0] = "!=";

        string[] values = new string[1];
        values[0] = "3";
        

        DataSet ds = sql.SelectWhere("user_info", items, cols, operations, values);

        userData.Clear();
        if (ds != null)
        {
            DataTable table = ds.Tables[0];


            foreach (DataRow dataRow in table.Rows)
            {
                List<string> userLittleData = new List<string>();
                foreach (DataColumn dataColumn in table.Columns)
                {                   
                    userLittleData.Add(dataRow[dataColumn].ToString());
                }
                userData.Add(userLittleData);
                //userLittleData.Clear();
            }
            CreatButton();    
        }
    }

    private void list_init()
    {
        for (int i = 0; i < ScrowViews.transform.childCount; i++)
        {
            Destroy(ScrowViews.transform.GetChild(i).gameObject);
        }
    }

    

    /// <summary>
    /// 自动生产信息列表
    /// </summary>
    private void CreatButton()
    {
        list_init();
        int i = 0;
        if(userData.Count > 10)
        {
            ScrowControl.SetActive(true);
        }
        else
        {
            ScrowControl.SetActive(false);
        }
        foreach (List<string> items in userData)
        {
            int q = 0;
            GameObject num = Instantiate(Resources.Load("Prefabs/Num")) as GameObject;
            if (i%2 == 0)
            {
                num.GetComponents<UISprite>()[0].spriteName = "Gly_zhuyemian_di02";
            }
            else
            {
                num.GetComponents<UISprite>()[0].spriteName = "Gly_zhuyemian_di01";
            }

            num.transform.parent = ScrowViews.transform;
            num.transform.localScale = Vector3.one;
            num.transform.localPosition = new Vector3(597f, 151f - i++ * 50f, 0);
            
            ////初始化数据，q=3时密码显示**** q=4时根据值显示用户权限
            foreach (string item in items)
            {
                if(q == 0)
                {
                    num.name = item;
                    num.GetComponentsInChildren<UIButton>()[0].name = item;
                }
                if (q != 3 && q != 4)
                {
                    num.GetComponentsInChildren<UILabel>()[q++].text = item;
                }
                else if (q == 3)
                {
                    num.GetComponentsInChildren<UILabel>()[q++].text = "******";
                }
                else if (q == 4)
                {
                    if(item == "0")
                    {
                        num.GetComponentsInChildren<UILabel>()[q++].text = "用户";
                    }
                    else
                    {
                        num.GetComponentsInChildren<UILabel>()[q++].text = "管理员";
                    }
                }

            }
            num.GetComponentsInChildren<UILabel>()[q].text = i.ToString();
            UIEventListener.Get(num.GetComponentsInChildren<UIButton>()[0].transform.gameObject).onClick = this.UserUpdate;
            UIEventListener.Get(num.GetComponentsInChildren<UIButton>()[1].transform.gameObject).onClick = this.UserDelete;

        }
    }

    

    /// <summary>
    /// 通过id获取用户信息编号
    /// </summary>
    /// <param name="gameobjectName"></param>
    /// <returns></returns>
    private int get_info_by_id(string gameobjectName)
    {
        int i = 0;
        int k = 0;
        foreach (List<string> items in userData)
        {
            int q = 0;
            foreach (string item in items)
            {
                if (q == 0)
                {
                    if (item == gameobjectName)
                    {
                        k = i;
                    }
                }
                q++;
            }
            i++;
        }
        return k;
    }



    /// <summary>
    /// 用户信息修改
    /// </summary>
    private void UserUpdate(GameObject gameobject)
    {
        AdminMark.SetActive(true);
        info_panel_init();
        GameObject Update_info_panel = Instantiate(Resources.Load("Prefabs/Update_info_panel")) as GameObject;
        Update_info_panel.transform.parent = Update_panel.transform;
        Update_info_panel.transform.localPosition = Vector3.zero;
        Update_info_panel.transform.localScale = Vector3.one;
        Update_info_panel.SetActive(true);

        int k = 0;
        //find strings
        change_id = gameobject.name;
        k = get_info_by_id(change_id);
        //Debug.Log("userdata length:" + userData[k].Count);
        Update_info_panel.GetComponentsInChildren<UIInput>()[0].value = userData[k][0];
        Update_info_panel.GetComponentsInChildren<UIInput>()[1].value = userData[k][1];
        Update_info_panel.GetComponentsInChildren<UIInput>()[2].value = userData[k][2];
        Update_info_panel.GetComponentsInChildren<UIInput>()[3].value = "";
        if(userData[k][4] == "1")
        {
            Update_info_panel.GetComponentsInChildren<UIToggle>()[0].value = true;
            Update_info_panel.GetComponentsInChildren<UIToggle>()[1].value = false;
        }
        else
        {
            Update_info_panel.GetComponentsInChildren<UIToggle>()[0].value = false;
            Update_info_panel.GetComponentsInChildren<UIToggle>()[1].value = true;
        }
        //Update_info_panel.GetComponentsInChildren<UIInput>()[4].value = userData[k][4];
        UIEventListener.Get(Update_info_panel.GetComponentsInChildren<UIButton>()[0].transform.gameObject).onClick = this.UserUpdateAdmin;
        UIEventListener.Get(Update_info_panel.GetComponentsInChildren<UIButton>()[1].transform.gameObject).onClick = this.UserUpdateUser;
        UIEventListener.Get(Update_info_panel.GetComponentsInChildren<UIButton>()[2].transform.gameObject).onClick = this.UserUpdateSubmit;
        UIEventListener.Get(Update_info_panel.GetComponentsInChildren<UIButton>()[3].transform.gameObject).onClick = this.UserUpdateCancel;
    }

    private void UserUpdateAdmin(GameObject gameobject)
    {
        gameobject.transform.parent.GetComponentsInChildren<UIToggle>()[0].value = true;
        gameobject.transform.parent.GetComponentsInChildren<UIToggle>()[1].value = false;
    }

    private void UserUpdateUser(GameObject gameobject)
    {
        gameobject.transform.parent.GetComponentsInChildren<UIToggle>()[0].value = false;
        gameobject.transform.parent.GetComponentsInChildren<UIToggle>()[1].value = true;
    }

    /// <summary>
    /// 
    ///  public DataSet UpdateInto(string tableName, string[] cols, string[] colsvalues, string selectkey, string selectvalue)
    /// </summary>
    /// <param name="gameobject"></param>
    private void UserUpdateSubmit(GameObject gameobject)
    {
        if (gameobject.transform.parent.GetComponentsInChildren<UIInput>()[3].value.IndexOf(" ")> -1)
        {
            Alert.SetActive(true);
            Mark.SetActive(true);
            AlertLabel.text = "密码不允许输入空格";
        }
        else
        {
            SqlAccess sql = new SqlAccess();
            string tableName = "user_info";
            string[] cols = new string[4];
            string[] colsvalues = new string[4];
            string selectkey = "_id";
            string selectvalue = change_id;
            colsvalues[0] = gameobject.transform.parent.GetComponentsInChildren<UIInput>()[1].value;
            colsvalues[1] = gameobject.transform.parent.GetComponentsInChildren<UIInput>()[2].value;
            colsvalues[2] = md5(gameobject.transform.parent.GetComponentsInChildren<UIInput>()[3].value);
            if (gameobject.transform.parent.GetComponentsInChildren<UIToggle>()[0].value == true)
            {
                colsvalues[3] = "1";
            }
            else
            {
                colsvalues[3] = "0";
            }
            cols[0] = "name";
            cols[1] = "username";
            if(gameobject.transform.parent.GetComponentsInChildren<UIInput>()[3].value == "")
            {
                cols[2] = "username";
                colsvalues[2] = gameobject.transform.parent.GetComponentsInChildren<UIInput>()[2].value;
            }
            else
            {
                cols[2] = "password";
            }
            
            cols[3] = "gradation";
            sql.UpdateInto(tableName, cols, colsvalues, selectkey, selectvalue);

            Destroy(gameobject.transform.parent.gameObject);
            Alert.SetActive(true);
            Mark.SetActive(true);
            AlertLabel.text = "修改成功";
            Admin_UI_Init();
        }
    }


    private void UserUpdateCancel(GameObject gameobject)
    {
        AdminMark.SetActive(false);
        Destroy(gameobject.transform.parent.gameObject);
    }



    /// <summary>
    /// 用户删除
    /// </summary>
    private void UserDelete(GameObject gameobject)
    {
        Debug.Log("delete_id:" );
        AdminMark.SetActive(true);
        info_panel_init();
        GameObject Delete_info_panel = Instantiate(Resources.Load("Prefabs/Delete_info_panel")) as GameObject;
        Delete_info_panel.transform.parent = Update_panel.transform;
        Delete_info_panel.transform.localPosition = Vector3.zero;
        Delete_info_panel.transform.localScale = Vector3.one;
        Delete_info_panel.SetActive(true);

        int k = 0;
        //find strings
        change_id = gameobject.transform.parent.name;
        k = get_info_by_id(change_id);
        //Debug.Log("userdata length:" + userData[k].Count);
        
        UIEventListener.Get(Delete_info_panel.GetComponentsInChildren<UIButton>()[0].transform.gameObject).onClick = this.UserDeleteSubmit;
        UIEventListener.Get(Delete_info_panel.GetComponentsInChildren<UIButton>()[1].transform.gameObject).onClick = this.UserDeleteCancel;
    }

    private void UserDeleteSubmit(GameObject gameobject)
    {
        DataSet da = SqlAccess.ExecuteQuery("DELETE  FROM user_info WHERE _id = '" + change_id + "'");
        Debug.Log("delete：" + da.Tables[0].Rows.Count);
        Destroy(gameobject.transform.parent.gameObject);
        Alert.SetActive(true);
        Mark.SetActive(true);
        AlertLabel.text = "删除成功";
        Admin_UI_Init();
    }

    private void UserDeleteCancel(GameObject gameobject)
    {
        AdminMark.SetActive(false);
        Destroy(gameobject.transform.parent.gameObject);
    }



    /// <summary>
    /// 用户添加
    /// </summary>
    public void UserAdd()
    {
        info_panel_init();

        AdminMark.SetActive(true);
        GameObject Add_info_panel = Instantiate(Resources.Load("Prefabs/Add_info_panel")) as GameObject;
        Add_info_panel.transform.parent = Update_panel.transform;
        Add_info_panel.transform.localPosition = Vector3.zero;
        Add_info_panel.transform.localScale = Vector3.one;
        Add_info_panel.SetActive(true);
        
        //Debug.Log("userdata length:" + userData[k].Count);
        Add_info_panel.GetComponentsInChildren<UIInput>()[0].value = "";
        Add_info_panel.GetComponentsInChildren<UIInput>()[1].value = "";
        Add_info_panel.GetComponentsInChildren<UIInput>()[2].value = "";
        Add_info_panel.GetComponentsInChildren<UIInput>()[3].value = "";

        Add_info_panel.GetComponentsInChildren<UIToggle>()[0].value = false;
        Add_info_panel.GetComponentsInChildren<UIToggle>()[1].value = true;
        
        //Update_info_panel.GetComponentsInChildren<UIInput>()[4].value = userData[k][4];
        UIEventListener.Get(Add_info_panel.GetComponentsInChildren<UIButton>()[0].transform.gameObject).onClick = this.UserAddAdmin;
        UIEventListener.Get(Add_info_panel.GetComponentsInChildren<UIButton>()[1].transform.gameObject).onClick = this.UserAddUser;
        UIEventListener.Get(Add_info_panel.GetComponentsInChildren<UIButton>()[2].transform.gameObject).onClick = this.UserAddSubmit;
        UIEventListener.Get(Add_info_panel.GetComponentsInChildren<UIButton>()[3].transform.gameObject).onClick = this.UserAddCancel;

    }

    private void UserAddAdmin(GameObject gameobject)
    {
        gameobject.transform.parent.GetComponentsInChildren<UIToggle>()[0].value = true;
        gameobject.transform.parent.GetComponentsInChildren<UIToggle>()[1].value = false;
    }

    private void UserAddUser(GameObject gameobject)
    {
        gameobject.transform.parent.GetComponentsInChildren<UIToggle>()[0].value = false;
        gameobject.transform.parent.GetComponentsInChildren<UIToggle>()[1].value = true;
    }

    private void UserAddSubmit(GameObject gameobject)
    {
        if (gameobject.transform.parent.GetComponentsInChildren<UIInput>()[3].value.IndexOf(" ") > -1||
            gameobject.transform.parent.GetComponentsInChildren<UIInput>()[0].value == ""|| 
            gameobject.transform.parent.GetComponentsInChildren<UIInput>()[1].value == ""|| 
            gameobject.transform.parent.GetComponentsInChildren<UIInput>()[2].value == "")
        {
            Alert.SetActive(true);
            Mark.SetActive(true);
            AlertLabel.text = "请完善信息";
        }
        else
        {
            SqlAccess sql = new SqlAccess();
            DataSet da = SqlAccess.ExecuteQuery("SELECT * FROM user_info WHERE _id = '" + gameobject.transform.parent.GetComponentsInChildren<UIInput>()[0].value + "'");
            DataTable ta = da.Tables[0];
            DataSet ga = SqlAccess.ExecuteQuery("SELECT * FROM user_info WHERE username = '" + gameobject.transform.parent.GetComponentsInChildren<UIInput>()[2].value + "'");
            //Debug.Log("dalong:" + da.Tables.Count);
            //Debug.Log("long:" + ta.Rows.Count);
            if (ta.Rows.Count > 0 ||ga.Tables[0].Rows.Count>0)
            {
                if (ta.Rows.Count > 0)
                {
                    //Destroy(gameobject.transform.parent.gameObject);
                    Alert.SetActive(true);
                    Mark.SetActive(true);
                    AlertLabel.text = "该工号异常";
                    Admin_UI_Init();
                }
                else if(ga.Tables[0].Rows.Count > 0)
                {
                    //Destroy(gameobject.transform.parent.gameObject);
                    Alert.SetActive(true);
                    Mark.SetActive(true);
                    AlertLabel.text = "该账号已存在";
                    Admin_UI_Init();
                }
            }
            else
            {
                string tableName = "user_info";
                string[] cols = new string[5];
                string[] colsvalues = new string[5];
                colsvalues[0] = gameobject.transform.parent.GetComponentsInChildren<UIInput>()[1].value;
                colsvalues[1] = gameobject.transform.parent.GetComponentsInChildren<UIInput>()[2].value;
                colsvalues[2] = md5(gameobject.transform.parent.GetComponentsInChildren<UIInput>()[3].value);
                colsvalues[4] = gameobject.transform.parent.GetComponentsInChildren<UIInput>()[0].value; 
                if (gameobject.transform.parent.GetComponentsInChildren<UIToggle>()[0].value == true)
                {
                    colsvalues[3] = "1";
                }
                else
                {
                    colsvalues[3] = "0";
                }
                cols[0] = "name";
                cols[1] = "username";
                cols[2] = "password";
                cols[3] = "gradation";
                cols[4] = "_id";
                sql.InsertInto(tableName, cols, colsvalues);

                Destroy(gameobject.transform.parent.gameObject);
                Alert.SetActive(true);
                Mark.SetActive(true);
                AlertLabel.text = "添加成功";
                Admin_UI_Init();
            }
        }
    }

    private void UserAddCancel(GameObject gameobject)
    {
        AdminMark.SetActive(false);
        Destroy(gameobject.transform.parent.gameObject);
    }

    public void info_panel_init()
    {
        //Update_info_panel.SetActive(false);
        //Add_info_panel.SetActive(false);
        //Delete_info_panel.SetActive(false);
    }

}

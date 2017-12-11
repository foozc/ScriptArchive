using Assets.Scripts.VO;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;
using Mysql.Util;
using System.Data;
using Assets.Scripts.Logic.UI.Login;
using System.IO;
using Excel;
using UnityEngine;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明：从数据库获得用户的信息
*Author:作者
*
*/
namespace Assets.Scripts.DBLogic
{
	public class UserDBHelper
	{
		public UserDBHelper()
		{

		}

		public User getUser(string id, string pwd, string role)
		{
			string sql = "select * from railway_user where ID = " + id + " and pwd = " + pwd + " and role = " + role;
			MysqlHelper db = new MysqlHelper();
			DataTable tb = MysqlHelper.ExecuteDataTable(sql);
			if (tb.Rows.Count > 0)
			{
				DataRow dr = tb.Rows[0];
				User user = new User();
				user.Id = Int32.Parse(dr["ID"].ToString());
				user.Name = dr["Name"].ToString();
				user.Pwd = dr["Pwd"].ToString();
				string aa = dr["Role"].ToString();
				user.Role = short.Parse(dr["Role"].ToString());
				user.Sex = dr["sex"].ToString();
				MysqlHelper.CloseConn();
				return user;
			}
			else
			{
				MysqlHelper.CloseConn();
				return null;
			}
		}
		public User getUser(string id)
		{
			string sql = "select * from railway_user where ID = " + id;
			MysqlHelper db = new MysqlHelper();
			DataTable tb = MysqlHelper.ExecuteDataTable(sql);
			if (tb.Rows.Count > 0)
			{
				DataRow dr = tb.Rows[0];
				User user = new User();
				user.Id = Int32.Parse(dr["ID"].ToString());
				user.Name = dr["Name"].ToString();
				user.Pwd = dr["Pwd"].ToString();
				string aa = dr["Role"].ToString();
				user.Role = short.Parse(dr["Role"].ToString());
				user.Term = dr["term"].ToString();
				user.Workshop = dr["workshop"].ToString();
				user.Group = dr["groups"].ToString();
				MysqlHelper.CloseConn();
				return user;
			}
			else
			{
				MysqlHelper.CloseConn();
				return null;
			}
		}



		public List<User> getUser(string Condition, RoleType type)
		{
			string sql;
			MySqlParameter[] pars = null;
			if (Condition == null || Condition.Equals(""))
			{
				sql = "SELECT * FROM USER where Role = @Param1";
				MySqlParameter p1 = new MySqlParameter("@Param1", MySqlDbType.Int16);
				p1.Value = (Int16)type;
				pars = new MySqlParameter[] { p1 };
			}
			else
			{
				sql = "SELECT * FROM USER WHERE ID = @Param1 OR NAME = @Param2 and role = @Param3";
				MySqlParameter p1 = new MySqlParameter("@Param1", MySqlDbType.String);
				p1.Value = Condition;
				MySqlParameter p2 = new MySqlParameter("@Param2", MySqlDbType.String);
				p2.Value = Condition;
				MySqlParameter p3 = new MySqlParameter("@Param3", MySqlDbType.Int16);
				p3.Value = (Int16)type;
				pars = new MySqlParameter[] { p1, p2, p3 };

			}
			DataTable tb = MysqlHelper.ExecuteDataTable(sql, pars);
			List<User> users = new List<User>();
			if (tb.Rows.Count > 0)
				foreach (DataRow dr in tb.Rows)
				{
					User user = new User();
					user.Id = Int32.Parse(dr["ID"].ToString());
					user.Name = dr["Name"].ToString();
					user.Pwd = dr["Pwd"].ToString();
					string aa = dr["Role"].ToString();
					user.Role = short.Parse(dr["Role"].ToString());
					users.Add(user);
				}
			return users;
		}

		public bool updateUser(User user)
		{
		
			string sql = "update railway_user set name='" + user.Name + "',sex='" + user.Sex + "',phone=" + user.Phone + ",IDcard='" + user.IdCard + "',term=" + user.Term + ",workshop=" + user.Workshop + ",groups=" + user.Group + ",pwd=" + user.Pwd + ",role=" + '0' + " where ID='" + user.AccountID + "';";
			UnityEngine.Debug.Log(sql);
			int AffectNum = MysqlHelper.ExecuteNonQuery(sql);
			if (AffectNum > 0)
				return true;
			else return false;
		}

		public bool deleteUser(int id)
		{
			string sql = "delete from railway_user where ID = " + id;
			int AffectNum = MysqlHelper.ExecuteNonQuery(sql);
			if (AffectNum > 0)
				return true;
			else return false;
		}

		public bool importUser(string path)
		{
			bool isOk = false;
            try
            {
                FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
				//if (excelReader.FieldCount==-1)
				//{
				//	Debug.Log("Excel格式不符合规范");
				//	return false;
				//}
                DataSet result = excelReader.AsDataSet();
				//int columns = result.Tables[0].Columns.Count;
				//if (columns!=8)//暂时先直接用8，这里判断Excel的列数是否符合规范
				//{
				//	Debug.Log("Excel数据列数不符合规范");
				//	return false;
				//}
				int rows = result.Tables[0].Rows.Count;
                for (int i = 0; i < rows; i++)
                {
                    if (i > 1)
                    {
                        string[] excelValue = new string[8];
                        for (int j = 0; j < 8; j++)
                        {
                            string nvalue = result.Tables[0].Rows[i][j].ToString();
                            if (j >= excelValue.Length || nvalue == "")
                            {
                                break;
                            }
                            excelValue[j] = nvalue;
                        }
						if (excelValue[excelValue.Length - 1] != null)
							isOk= initUser(excelValue);
						else
						{
							Debug.Log("第" + i + "行数据不符合规范");
							return false;
						}
					}
                }
                return isOk;
            }
            catch (Exception)
            {
                return false;
            }
		}
		/// <summary>
		/// 
		/// 姓名 性别 电话 身份证 期数 车间 班组 密码
		/// </summary>
		/// <param name="value"></param>
		public bool initUser(string[] value)
		{
			int comId = 0;
			if (!int.TryParse(value[2], out comId) || !int.TryParse(value[3], out comId) || !int.TryParse(value[4], out comId) || !int.TryParse(value[5], out comId) || !int.TryParse(value[6], out comId))
			{
				Debug.Log("数据类型错误");
				return false;
			}
			string sql = "SELECT MAX(Id) FROM railway_user";
			MysqlHelper db = new MysqlHelper();
			DataTable tb = MysqlHelper.ExecuteDataTable(sql);
			int maxId = int.Parse(tb.Rows[0][0].ToString()) + 1;
			User user = new User();
			user.AccountID = maxId.ToString();//依次叠加
			user.Role = 0;//默认为0
			user.Name = value[0];//名字
			if (value[1].Equals("男") || value[1].Equals("女"))
				user.Sex = value[1];//性别
			else
				user.Sex = "男";
			user.Phone = value[2];//电话
			user.IdCard = value[3];//身份证
			user.Term = value[4];//期数
			user.Workshop = value[5];//车间
			user.Group = value[6];//班组
			user.Pwd = value[7];//密码
			addUser(user);
			return true;
		}

		public bool addUser(User user)
		{
			string sqls = "select * from railway_user";
			MysqlHelper db = new MysqlHelper();
			DataTable tb = MysqlHelper.ExecuteDataTable(sqls);
			if (tb.Rows.Count > 0)
			{
				foreach (DataRow dr in tb.Rows)
				{
					User inuser = new User();
					inuser.Id = Int32.Parse(dr["ID"].ToString());
					if (inuser.Id==int.Parse(user.AccountID))
					{
						return false;
					}
				}
			}
				//[{"name":Name,"sex":registerSex,"phone":registerNumber,"ide":registerCopy,"term":registerTerm,"workshop":registerShop,"team":registerTeams,"id":registerID,"pwd":registerPsw}]
				string sql = "insert into railway_user (name,sex,phone,IDcard,term,workshop,groups,ID,pwd,role) values('" + user.Name + "', '" + user.Sex +
				"', '" + user.Phone + "', '" + user.IdCard + "', '" + user.Term + "', '" + user.Workshop + "', '" + user.Group + "', '" + user.AccountID + "', '" + user.Pwd + "', '0');";

			// UnityEngine.Debug.Log(sql);
			int AffectNum = MysqlHelper.ExecuteNonQuery(sql);
			if (AffectNum > 0)
				return true;
			else return false;
		}
		//[{"name":name,"sex":sex,"phone":phone,"ide":identify,"id":id,"pwd":password}]
		public List<User> getXueyuanUserByPage(int pageNum, int num)
		{
			string sql = "select * from railway_user where role = '0' order by id desc limit " + (pageNum - 1) * num + "," + pageNum * num;
			DataTable tb = MysqlHelper.ExecuteDataTable(sql);
			List<User> users = new List<User>();
			if (tb.Rows.Count > 0)
			{
				foreach (DataRow dr in tb.Rows)
				{
					User user = new User();
					user.AccountID = dr["ID"].ToString();
					user.IdCard = dr["IDcard"].ToString();
					user.Name = dr["name"].ToString();
					user.Sex = dr["sex"].ToString();
					user.Phone = dr["phone"].ToString();
					user.Pwd = dr["pwd"].ToString();
					user.Term = dr["term"].ToString();
					user.Group = dr["groups"].ToString();
					user.Workshop = dr["workshop"].ToString();
					string aa = dr["role"].ToString();
					user.Role = short.Parse(dr["Role"].ToString());
					users.Add(user);
				}
			}
			else
			{
				MysqlHelper.CloseConn();
				return null;
			}
			return users;
		}

		public int getTotalXueyuanUserNum()
		{
			string sql = "select * from railway_user where role = '0'";
			MysqlHelper db = new MysqlHelper();
			DataTable tb = MysqlHelper.ExecuteDataTable(sql);
			MysqlHelper.CloseConn();
			return tb.Rows.Count;
		}
	}
}

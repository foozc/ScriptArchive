using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class Mes_Device_SqlAccess
    {
        public static SqlConnection dbConnection = new SqlConnection();
        static string server = "10.65.100.134";
        static string uid = "nymes";
        static string pwd = "nymes";
        static string database = "Runtime";



        public Mes_Device_SqlAccess()
        {
            OpenSql();
        }


        public static void OpenSql()
        {
            try
            {
                string conString = string.Format("server = {0}; user id = {1}; password = {2};database={3};", server, uid, pwd, database);
                dbConnection.ConnectionString = conString;
                dbConnection.Open();
            }
            catch (Exception e)
            {
                throw new Exception("服务器连接失败，请重新检查是否打开Sql服务。" + e.Message.ToString());

            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void close_sql()
        {
            if (dbConnection != null)
            {
                dbConnection.Close();
                dbConnection.Dispose();
                dbConnection = null;
            }
        }

       

        public static String ExecuteQuery(string sqlString)
        {
            string result = "";
            if (dbConnection.State == ConnectionState.Open)
            {
                SqlCommand com = new SqlCommand();
                try
                {
                    com.Connection = dbConnection;
                    com.CommandType = CommandType.Text;
                    //"SELECT * FROM spc.PPA_TAG_PARA ptp WHERE  ptp.Para_NAME LIKE '%梗丝%'"
                    //SELECT * FROM [nymes].[VIEW_CUT_PLANS] WHERE PLAN_DATE='2017-11-12'
                    //
                    com.CommandText = sqlString;
                    SqlDataReader dr = com.ExecuteReader();

                    while (dr.Read())
                    {
                        //Console.WriteLine(String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}",
                        //    dr[0], dr[1], dr[2], dr[3], dr[4], dr[5], dr[6], dr[7], dr[8], dr[9],
                        //    dr[10], dr[11], dr[12], dr[13], dr[14], dr[15]));
                        result = dr[2].ToString();
                        ///sqlserver 2005
                    }
                    

                    dr.Close();
                }
                catch (Exception ee)
                {
                    throw new Exception("SQL:" + sqlString + "/n" + ee.Message.ToString());
                }
                finally
                {
                }
                if (result.IndexOf(".") >= 0)
                {
                    if (result.Length > (result.IndexOf(".") + 3))
                    {
                        if (result.Length > (result.IndexOf(".") + 5))
                        {
                            result = result.Substring(0, result.IndexOf(".") + 5);
                        }
                        else
                        {
                            result = result.Substring(0, result.IndexOf(".") + 3);
                        }
                    }
                    
                }
                return result;
            }
            return null;
        }

    }
}

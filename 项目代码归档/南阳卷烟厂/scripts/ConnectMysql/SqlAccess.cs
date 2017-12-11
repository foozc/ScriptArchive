using UnityEngine;  
using System;  
using System.Data;  
using System.Collections;   
using MySql.Data.MySqlClient;
using MySql.Data;
using System.IO;
using System.Xml;

namespace assest
{
    public class SqlAccess
    {


        public static MySqlConnection dbConnection;
        //如果只是在本地的话，写localhost就可以。
        // static string host = "localhost";  
        //如果是局域网，那么写上本机的局域网IP
        static string host = "127.0.0.1";
        static string port = "";
        static string id = "root";
        static string pwd = "";
        static string database = "ny_";
        static string charset = "gbk";



        public SqlAccess()
        {
            OpenSql();
        }


        public static void OpenSql()
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(Application.streamingAssetsPath + "/Config/ConnectMysql.XML");
            XmlNode xn = XmlDoc.SelectSingleNode("/config");
            XmlNodeList xns = xn.ChildNodes;
            host = xns.Item(0).InnerText;
            port = xns.Item(1).InnerText;
            id = xns.Item(2).InnerText;
            pwd = xns.Item(3).InnerText;
            database = "ny_";

            try
            {
                string connectionString = string.Format("Host = {0};Port = {1}; User ID = {2}; Password = {3};Database={4};Charset={5}", host, port, id, pwd, database, charset);
                dbConnection = new MySqlConnection(connectionString);
                dbConnection.Open();
            }
            catch (Exception e)
            {
                throw new Exception("服务器连接失败，请重新检查是否打开MySql服务。" + e.Message.ToString());

            }

        }

        public DataSet CreateTable(string name, string[] col, string[] colType)
        {
            if (col.Length != colType.Length)
            {

                throw new Exception("columns.Length != colType.Length");

            }

            string query = "CREATE TABLE " + name + " (" + col[0] + " " + colType[0];

            for (int i = 1; i < col.Length; ++i)
            {

                query += ", " + col[i] + " " + colType[i];

            }

            query += ")";

            return ExecuteQuery(query);
        }

        public DataSet CreateTableAutoID(string name, string[] col, string[] colType)
        {
            if (col.Length != colType.Length)
            {

                throw new Exception("columns.Length != colType.Length");

            }

            string query = "CREATE TABLE " + name + " (" + col[0] + " " + colType[0] + " NOT NULL AUTO_INCREMENT";

            for (int i = 1; i < col.Length; ++i)
            {

                query += ", " + col[i] + " " + colType[i];

            }

            query += ", PRIMARY KEY (" + col[0] + ")" + ")";

            Debug.Log(query);

            return ExecuteQuery(query);
        }

        //插入一条数据，包括所有，不适用自动累加ID。
        public DataSet InsertInto(string tableName, string[] values)
        {

            string query = "INSERT INTO " + tableName + " VALUES (" + "'" + values[0] + "'";

            for (int i = 1; i < values.Length; ++i)
            {

                query += ", " + "'" + values[i] + "'";

            }

            query += ")";

            Debug.Log(query);
            return ExecuteQuery(query);

        }


        //插入部分ID
        public DataSet InsertInto(string tableName, string[] col, string[] values)
        {

            if (col.Length != values.Length)
            {

                throw new Exception("columns.Length != colType.Length");

            }

            string query = "INSERT INTO " + tableName + " (" + col[0];
            for (int i = 1; i < col.Length; ++i)
            {

                query += ", " + col[i];

            }

            query += ") VALUES (" + "'" + values[0] + "'";
            for (int i = 1; i < values.Length; ++i)
            {

                query += ", " + "'" + values[i] + "'";

            }

            query += ")";

            return ExecuteQuery(query);

        }


        public DataSet SelectWhere(string tableName, string[] items, string[] col, string[] operation, string[] values)
        {

            if (col.Length != operation.Length || operation.Length != values.Length)
            {

                throw new Exception("col.Length != operation.Length != values.Length");

            }

            string query = "SELECT " + items[0];

            for (int i = 1; i < items.Length; ++i)
            {

                query += ", " + items[i];

            }

            query += " FROM " + tableName + " WHERE " + col[0] + operation[0] + "'" + values[0] + "' ";

            for (int i = 1; i < col.Length; ++i)
            {

                query += " AND " + col[i] + operation[i] + "'" + values[i] + "' ";

            }

            return ExecuteQuery(query);
        }

        public DataSet SelectAll(string tableName, string[] col, string[] operation, string[] values)
        {

            if (col.Length != operation.Length || operation.Length != values.Length)
            {

                throw new Exception("col.Length != operation.Length != values.Length");

            }

            string query = "SELECT *";

            query += " FROM " + tableName + " WHERE " + col[0] + operation[0] + "'" + values[0] + "' ";

            for (int i = 1; i < col.Length; ++i)
            {

                query += " AND " + col[i] + operation[i] + "'" + values[i] + "' ";

            }

            return ExecuteQuery(query);

        }

        public DataSet UpdateInto(string tableName, string[] cols, string[] colsvalues, string selectkey, string selectvalue)
        {

            string query = "UPDATE " + tableName + " SET " + cols[0] + " = " + "'"+ colsvalues[0] + "'";

            for (int i = 1; i < colsvalues.Length; ++i)
            {

                query += ", " + cols[i] + " =" + "'"+ colsvalues[i] + "'";
            }

            query += " WHERE " + selectkey + " = " + "'"+selectvalue + "'" + " ";

            return ExecuteQuery(query);
        }


        public DataSet Delete(string tableName, string[] cols, string[] colsvalues)
        {
            string query = "DELETE FROM " + tableName + " WHERE " + cols[0] + " = " + colsvalues[0];

            for (int i = 1; i < colsvalues.Length; ++i)
            {

                query += " or " + cols[i] + " = " + colsvalues[i];
            }
            Debug.Log(query);
            return ExecuteQuery(query);
        }

        public void Close()
        {

            if (dbConnection != null)
            {
                dbConnection.Close();
                dbConnection.Dispose();
                dbConnection = null;
            }

        }

        public static DataSet ExecuteQuery(string sqlString)
        {
            if (dbConnection.State == ConnectionState.Open)
            {
                DataSet ds = new DataSet();
                try
                {

                    MySqlDataAdapter da = new MySqlDataAdapter(sqlString, dbConnection);
                    da.Fill(ds);

                }
                catch (Exception ee)
                {
                    throw new Exception("SQL:" + sqlString + "/n" + ee.Message.ToString());
                }
                finally
                {
                }
                return ds;
            }
            return null;
        }


    }
}
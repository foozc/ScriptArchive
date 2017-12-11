using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Timers;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// alarm data
        /// </summary>
        private Dictionary<string, List<string>> allCheckColumns = new Dictionary<string, List<string>>();
        private string[] col = { "isconfirm", "about", "startTime", "status", "value", "deviceName", "isShow", "tableName",
                             "alarmColumn", "brand" };


        /// <summary>
        /// 各个数据表
        /// </summary>
        string[] product_info = new string[20];
        string[] gengsi_jialiao_realtime = new string[5];

        string[] gengsi_zhengyageng_realtime = new string[10];
        string[] honggengsi_realtime = new string[15];
        string[] hunshi_jiaxiang_realtime = new string[12];

        string[] yangeng_huicao_realtime = new string[7];
        string[] yepian_huichao_realtime = new string[16];
        string[] yepian_jialiao_realtime = new string[24];

        string[] yeshi_baoban_realtime = new string[17];
        string[] yeshi_qiliu_realtime = new string[14];
        string[] yeshi_qiliucaoqihuicao_realtime = new string[11];


        /// <summary>
        /// 标准值数据表
        /// </summary>
        string[] gengsi_jialiao_limit = new string[5];

        string[] gengsi_zhengyageng_limit = new string[10];
        string[] honggengsi_limit = new string[15];
        string[] hunshi_jiaxiang_limit = new string[12];

        string[] yangeng_huicao_limit = new string[7];
        string[] yepian_huichao_limit = new string[16];
        string[] yepian_jialiao_limit = new string[24];

        string[] yeshi_baoban_limit = new string[17];
        string[] yeshi_qiliu_limit = new string[14];
        string[] yeshi_qiliucaoqihuicao_limit = new string[11];



        string[] device_key = { "gengsi_jialiao_realtime", 
                                  "gengsi_zhengyageng_realtime", "honggengsi_realtime", "hunshi_jiaxiang_realtime",
                                  "yangeng_huicao_realtime", "yepian_huichao_realtime", "yepian_jialiao_realtime", 
                                  "yeshi_baoban_realtime", "yeshi_qiliu_realtime","yeshi_qiliucaoqihuicao_realtime" };

        string[] gengsi_jialiao_realtime_datakey = { "GSJX_CKSF", "GSJXJ_JXBLSET", "GSJX_JXJDJPL", "GSJX_JXLJJD"};



        string[] gengsi_zhengyageng_realtime_datakey = { "GS_SJLL1", "GS_SZZQLL", "GSGZCKSF", "GS_GZQSF", "GS_GZCKWD", 
                                                       "GSPZ_LLSET1", "GSPZ_GZCKSFSET", "GSGZ_SZZQLLSET", "GS_SJLL"};
        string[] honggengsi_realtime_datakey ={ "GSFXPL_DJPLSET", "GSFX_CCFJPLSET", "GSJX_CKSF", "GY_SW", "XGJ_SWSET", 
                                                       "GYCL_CL", "GC_RFPL", "GC_RFWD", "GC_PCFMKD", "YGHC_JSL", 
                                                   "YGHC_WDSET", "GCL_YGHCJCKSF", "GHC_SSLL", "GHC_LJLL"};
        string[] hunshi_jiaxiang_realtime_datakey ={ "JX_CKSF", "JXJ_JXJD", "JX_SJLL", "JX_BLSET", "JX_GTDJPLSET", 
                                                       "JXJ_SSJD", "JX1_SSLL", "JX2_SSLL", "JX1_LJLL", "JX2_LJLL", 
                                                   "JX_BLSET"};



        string[] yangeng_huicao_realtime_datakey = { "QGSJL_JLLJJD", "JLJ3038BLSET", "QGSJL_RKWLHSL", "QGSJL_WLLL", "GJL_LJLL", 
                                                       "QGSJL_CKWLHSL"};
        string[] yepian_huichao_realtime_datakey = { "YY_LLSJ", "YY_GTWD", "YY_GTDJPL", "SSHC_RFFJPL", "YY_CKSF", 
                                                       "SSHC_RKSF", "SSHC_CKSFSET", "SSHC_GTDJSET", "SSHC_RFFJPL_BD", "YY_GTWDSET", 
                                                   "SSHC_SLLSET", "YY_SJLL", "YPHC_CL", "YPHC_CKWD", "GSJL_CKSF"};
        string[] yepian_jialiao_realtime_datakey = { "YC_SJLL", "YC_XHFJPL", "YC_GTDJPL", "YC_RFWD", "YL_LYWDONUSE", 
                                                       "YC_CKSF", "JL_JLJD", "YPJL_RKSF", "YPJL_CKSFSET", "YPJL_XHFJSET", 
                                                   "YPJL_GTDJPLSET", "YPJL_JLBLSET_BD", "YPJL_TWSET", "YPJL_SHLLSET", "YC_LYWD",
                                                   "YC_SJLL_BD", "YPJL_CKWD", "YPJL_JLBLSET", "JLT1_SSLL", "JLT2_SSLL",
                                                   "JLT1_LJLL", "JLT2_LJLL", "JL_SSJD"};



        string[] yeshi_baoban_realtime_datakey = { "BB_GTPL", "BB_RFWD", "BB_RFDJPL", "BB_FCFMKDH", "BB_FCFMKD", 
                                                       "BB_CKSF", "BB_CKSF_BD", "BBHS_CKSFSET", "BBHS_GTDJPLSET", "BBHS_RFFJPLSET", 
                                                   "BB_PCFMKDSET", "BB_RKSF", "BBHS_GTDJPLSET_BD", "BB_GTWD", "BBHSJ_CKWD"
                                                 ,"HYSBB_TBWD"};
        string[] yeshi_qiliu_realtime_datakey = { "SH93_HCQSSLL", "SH93_CKSF", "SH93_JSZQLL", "SH93_SSZQLL", "SH93_XHFJPL", 
                                                       "SH93_HHWD", "SH93_CKWD", "SH93_JSZQLLSET", "SH93_SSZQLLSET", "SH93_XHFJPLSET", 
                                                   "SH93_CKSFSET", "SH93_PCMKDSET", "QLHS_RKSF"};
        string[] yeshi_qiliucaoqihuicao_realtime_datakey = { "SH93_HCLSSLL", "RCC_HFWD", "RCC_DJPL", "RCC_CKSF", "RCC_HFWD_BD", 
                                                       "RCC_DJPL_BD", "QL_SJLL", "YSZWXS_SHLLSET", "QL_SJLL_BD", "QLHS_RKWLHSL"};

        ///        string[] device_key = { "gengsi_jialiao_realtime", 
        //"gengsi_zhengyageng_realtime", "honggengsi_realtime", "hunshi_jiaxiang_realtime",
        //"yangeng_huicao_realtime", "yepian_huichao_realtime", "yepian_jialiao_realtime", 
        //"yeshi_baoban_realtime", "yeshi_qiliu_realtime","yeshi_qiliucaoqihuicao_realtime" };


        string interval_brand;
        string gengsi_jialiao_realtime_string;

        string gengsi_zhengyageng_realtime_string;
        string honggengsi_realtime_string;
        string hunshi_jiaxiang_realtime_string;

        string yangeng_huicao_realtime_string;
        string yepian_huichao_realtime_string;
        string yepian_jialiao_realtime_string;

        string yeshi_baoban_realtime_string;
        string yeshi_qiliu_realtime_string;
        string yeshi_qiliucaoqihuicao_realtime_string;

        /// <summary>
        /// start
        /// </summary>
        public Form1()
        {
            SqlAccess sql = new SqlAccess();
            Mes_Device_SqlAccess MDsql = new Mes_Device_SqlAccess();

            Mes_Product_SqlAccess MPsql = new Mes_Product_SqlAccess();
            InitializeComponent();
            init_stringArray_name();



            //获取所有可检测项
            GetAllCheckColumns();
            System.Timers.Timer tmr = new System.Timers.Timer(5000);
            tmr.Elapsed += new ElapsedEventHandler(OtherTheard);
            tmr.AutoReset = true;
            tmr.Enabled = true;
            GC.KeepAlive(tmr);///定时执行


            System.Timers.Timer Insert_Alarm_Log = new System.Timers.Timer(5000);
            Insert_Alarm_Log.Elapsed += new ElapsedEventHandler(Insert_Alarm_Log_Data);
            Insert_Alarm_Log.AutoReset = true;
            Insert_Alarm_Log.Enabled = true;
            GC.KeepAlive(Insert_Alarm_Log);///定时执行
            




            ////MessageBox.Show("软件开启成功");
            ////Mes_Device_SqlAccess MDsql = new Mes_Device_SqlAccess();

            ////gengsi_jialiao_realtime[1] = MDsql.ExecuteQuery("SELECT * FROM DBO.Live WHERE TagName='" + gengsi_jialiao_realtime_datakey[0] + "'");

            ////MDsql.close_sql();

            ////Mes_Product_SqlAccess MPsql = new Mes_Product_SqlAccess();

            ////Mes_Device_SqlAccess MDsql = new Mes_Device_SqlAccess();

            ////SELECT * FROM DBO.Live WHERE TagName='JX_JXJD'
            ////SELECT * FROM spc.PPA_TAG_PARA ptp WHERE  ptp.Para_NAME LIKE '%梗丝%'
            ////String[] MPds = MPsql.ExecuteQuery("SELECT * FROM [nymes].[VIEW_CUT_PLANS]");
            ////DataTable MPdt = MPds.Tables[0];
            ////DataRow MPdr = MPdt.Rows[0];

            //// String result = MDsql.ExecuteQuery("SELECT * FROM DBO.Live WHERE TagName='GS_GZQSF'");


            ////UpdateOldTime("201711091849");
            ////UpdateOldTime("1");
            label1.Text = "正在存储数据...";

              //// label1.Text = result;
            
        }



        //insert 

        public void Insert_Alarm_Log_Data(object sender, ElapsedEventArgs e)
        {
            CheckAlarm();
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
                                            if (Math.Abs((float)realValue - (float)standardValue) > rangeValue)    
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
            SqlAccess InserAlarmInto = new SqlAccess();
            InserAlarmInto.InsertInto("alarm_log", col, values);
        }


        /// <summary>
        /// 另起线程，每隔五秒执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OtherTheard(object sender, ElapsedEventArgs e)
        {
            keepOnApp();
            //SqlAccess.ExecuteQuery("INSERT INTO test(`test`) VALUES('1')");
        }


        /// <summary>
        /// init array
        /// </summary>
        private void init_stringArray_name()
        {
            ///生产数据初始化
            gengsi_jialiao_realtime[0] = "gengsi_jialiao_realtime";

            gengsi_zhengyageng_realtime[0] = "gengsi_zhengyageng_realtime";
            honggengsi_realtime[0] = "honggengsi_realtime";
            hunshi_jiaxiang_realtime[0] = "hunshi_jiaxiang_realtime";

            yangeng_huicao_realtime[0] = "yangeng_huicao_realtime";
            yepian_huichao_realtime[0] = "yepian_huichao_realtime";
            yepian_jialiao_realtime[0] = "yepian_jialiao_realtime";

            yeshi_baoban_realtime[0] = "yeshi_baoban_realtime";
            yeshi_qiliu_realtime[0] = "yeshi_qiliu_realtime";
            yeshi_qiliucaoqihuicao_realtime[0] = "yeshi_qiliucaoqihuicao_realtime";

            ///标准值 表名
            gengsi_jialiao_limit[0] = "gengsi_jialiao_limit";

            gengsi_zhengyageng_limit[0] = "gengsi_zhengyageng_limit";
            honggengsi_limit[0] = "honggengsi_limit";
            hunshi_jiaxiang_limit[0] = "hunshi_jiaxiang_limit";

            yangeng_huicao_limit[0] = "yangeng_huicao_limit";
            yepian_huichao_limit[0] = "yepian_huichao_limit";
            yepian_jialiao_limit[0] = "yepian_jialiao_limit";

            yeshi_baoban_limit[0] = "yeshi_baoban_limit";
            yeshi_qiliu_limit[0] = "yeshi_qiliu_limit";
            yeshi_qiliucaoqihuicao_limit[0] = "yeshi_qiliucaoqihuicao_limit";
        }

       

        /// <summary>
        /// 实时数据库数据存入sql、即主机设备
        /// </summary>
        /// <param name="stringArrays"></param>
        public void setMysqlData(string[] stringArrays)
        {
            ///获取表名
            string mysqlDataName = stringArrays[0];

            ///获取字段名
            string[] mysqlKeyNames = new string[stringArrays.Count()-1];

            for (int i = 0; i < mysqlKeyNames.Count(); i++)
            {
                DataSet ds = SqlAccess.ExecuteQuery("SELECT `" + i + "` FROM all_device WHERE `name`='" + mysqlDataName + "'");
                DataTable dt = ds.Tables[0];
                DataRow dr = dt.Rows[0];
                mysqlKeyNames[i] = dr[0].ToString();
                //DataColumn一列            
            }

            ///stringArray里的第1个数，对应字段表中第0个字段，需要注意，因为sringArray里第0个数存储的表名、转存数组数据
            string[] sqlDataTemp = new string[stringArrays.Count()-1];
            for (int i = 0; i < sqlDataTemp.Count(); i++)
            {
                sqlDataTemp[i] = stringArrays[i + 1];
            }
            
            ///存储数据
            ///

            
            ///        string[] device_key = { "gengsi_jialiao_realtime", 
                                  //"gengsi_zhengyageng_realtime", "honggengsi_realtime", "hunshi_jiaxiang_realtime",
                                  //"yangeng_huicao_realtime", "yepian_huichao_realtime", "yepian_jialiao_realtime", 
                                  //"yeshi_baoban_realtime", "yeshi_qiliu_realtime","yeshi_qiliucaoqihuicao_realtime" };
            ////根据表名修改机器名称


            string DeviceName = "";

            string brandName = "";
            switch (mysqlDataName){
                case "gengsi_jialiao_realtime":
                    brandName = gengsi_jialiao_realtime_string;
                    DeviceName = "梗丝加料机";
                    break;

                case "gengsi_zhengyageng_realtime":
                    brandName = gengsi_zhengyageng_realtime_string;
                    DeviceName = "闪蒸式梗丝膨化装置";///蒸压梗机
                    break;
                case "honggengsi_realtime":
                    brandName = honggengsi_realtime_string;
                    DeviceName = "振动流化干燥机";///烘梗丝机
                    break;
                case "hunshi_jiaxiang_realtime":
                    brandName = hunshi_jiaxiang_realtime_string;
                    DeviceName = "加香机";
                    break;

                case "yangeng_huicao_realtime":
                    brandName = yangeng_huicao_realtime_string;
                    DeviceName = "滚筒式梗丝加料回潮";
                    break;
                case "yepian_huichao_realtime":
                    brandName = yepian_huichao_realtime_string;
                    DeviceName = "叶片松散回潮机";
                    break;
                case "yepian_jialiao_realtime":
                    brandName = yepian_jialiao_realtime_string;
                    DeviceName = "叶片加料机";
                    break;

                case "yeshi_baoban_realtime":
                    brandName = yeshi_baoban_realtime_string;
                    DeviceName = "薄板烘丝机";
                    break;
                case "yeshi_qiliu_realtime":
                    brandName = yeshi_qiliu_realtime_string;
                    DeviceName = "气流烘丝机";
                    break;
                case "yeshi_qiliucaoqihuicao_realtime":
                    brandName = yeshi_qiliucaoqihuicao_realtime_string;
                    DeviceName = "叶丝超级回潮机";
                    break;
            }



            
            ///存储语句 字段编写
            string myQuery = "INSERT INTO " + mysqlDataName + "( " + "`_id`, `name`,`brand`,`realtime`," + "`" + mysqlKeyNames[0] + "`";

            ///因为是从1开始 ，所以结尾为++i少一个
            for (int i = 1; i < mysqlKeyNames.Count(); ++i)
            {
                myQuery += ", " + "`" + mysqlKeyNames[i] + "`";
            }

            myQuery += ")";

            ////get real time 
            string VrealTime;
            System.DateTime realTime = new System.DateTime();
            realTime = System.DateTime.Now;
            VrealTime = realTime.ToString("yyyyMMddHHmm");//201711091010

            string idTime;
            idTime = realTime.ToString("yyyyMMddHHmmss");


            ////存储语句 字段值编写
            myQuery = myQuery + " VALUES ('" + idTime + "', '" + DeviceName + "', " + brandName + ", '" + VrealTime + "','" + sqlDataTemp[0] + "'";

            for (int i = 1; i < sqlDataTemp.Count(); ++i)
            {

                myQuery += ", " + "'" + sqlDataTemp[i] + "'";

            }

            myQuery += ")";
            //Console.WriteLine(myQuery);
            SqlAccess.ExecuteQuery(myQuery);             
        }

        /// <summary>
        /// 存入生产数据
        /// </summary>
        public void setProductData()
        {
            ///获取表名
            string productDataName = "product_info";

            string[] mysqlKeyNames = {"id_","sequence","number","batchId","workOrder","groups","classes","startTime","endTime","workTime","wTUnit","output","outChestId","chestId","chestNum",
                                     "cnUnit","saveTime","timeUnit","flow","brand"};

            

            ///存储数据
            string myQuery = "INSERT INTO " + productDataName + "( `" + mysqlKeyNames[0] + "`";

            ///因为是从1开始 ，所以结尾为++i少一个
            for (int i = 1; i < mysqlKeyNames.Count(); ++i)
            {
                myQuery += ", " + "`" + mysqlKeyNames[i] + "`";
            }

            myQuery += ")";

            myQuery = myQuery + " VALUES ( '" + product_info[0] + "'";

            for (int i = 1; i < product_info.Count(); ++i)
            {

                myQuery += ", " + "'" + product_info[i] + "'";

            }

            myQuery += ")";

            SqlAccess.ExecuteQuery(myQuery);      

            

        }

        private void findTimeNode()
        {
        }

        /// <summary>
        /// 每隔7天或新的月份，清空数据
        /// </summary>
        private void cleanMysql()
        {
            
            string newTime, newDay, oldDay;
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;
            newTime = currentTime.ToString("yyyyMMddHHmm");//201711091010

            string myQuery = "SELECT timeNote FROM TimeNode WHERE _id = 0";
            DataSet ds2 = SqlAccess.ExecuteQuery(myQuery);
            DataTable dt2 = ds2.Tables[0];
            DataRow dr2 = dt2.Rows[0];
            string oldTime = "";
            oldTime = dr2[0].ToString();

            newDay = newTime.Substring(6,2);
            oldDay = oldTime.Substring(6,2);


            //月份改变数据归零 并更新时间
            if ((int.Parse(newDay) - int.Parse(oldDay)) >= 0)
            {
                //天数超过7天，数据归零 并更新时间
                if ((int.Parse(newDay) - int.Parse(oldDay)) > 6)
                {
                    DeleteData();
                    UpdateOldTime(newTime);
                }
            }
            else
            {
                DeleteData();
                UpdateOldTime(newTime);
            }
            //MessageBox.Show("软件清理成功");
        }

        /// <summary>
        /// 清空数据 
        /// </summary>
        private void DeleteData()
        {
            

            SqlAccess.ExecuteQuery("delete from gengsi_jialiao_realtime");

            SqlAccess.ExecuteQuery("delete from gengsi_zhengyageng_realtime");
            SqlAccess.ExecuteQuery("delete from honggengsi_realtime");
            SqlAccess.ExecuteQuery("delete from hunshi_jiaxiang_realtime");

            SqlAccess.ExecuteQuery("delete from yangeng_huicao_realtime");
            SqlAccess.ExecuteQuery("delete from yepian_huichao_realtime");
            SqlAccess.ExecuteQuery("delete from yepian_jialiao_realtime");

            SqlAccess.ExecuteQuery("delete from yeshi_baoban_realtime");
            SqlAccess.ExecuteQuery("delete from yeshi_qiliu_realtime");
            SqlAccess.ExecuteQuery("delete from yeshi_qiliucaoqihuicao_realtime");
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        private void UpdateOldTime(string newTime)
        {
            string myQuery = "UPDATE TimeNode SET timeNote = '" + newTime + "' WHERE _id = 0";
            SqlAccess.ExecuteQuery(myQuery);
        }


        /// <summary>
        /// 执行步骤1234
        /// </summary>
        private void keepOnApp()
        {
            ///清理
            cleanMysql();

            ///查询数据
            find_product_data();

            find_device_data(gengsi_jialiao_realtime, gengsi_jialiao_realtime_datakey);

            find_device_data(gengsi_zhengyageng_realtime, gengsi_zhengyageng_realtime_datakey);
            find_device_data(honggengsi_realtime, honggengsi_realtime_datakey);
            find_device_data(hunshi_jiaxiang_realtime, hunshi_jiaxiang_realtime_datakey);

            find_device_data(yangeng_huicao_realtime, yangeng_huicao_realtime_datakey);
            find_device_data(yepian_huichao_realtime, yepian_huichao_realtime_datakey);
            find_device_data(yepian_jialiao_realtime, yepian_jialiao_realtime_datakey);

            find_device_data(yeshi_baoban_realtime, yeshi_baoban_realtime_datakey);
            find_device_data(yeshi_qiliu_realtime, yeshi_qiliu_realtime_datakey);
            find_device_data(yeshi_qiliucaoqihuicao_realtime, yeshi_qiliucaoqihuicao_realtime_datakey);

            ///存储主机设备数据
            setMysqlData(gengsi_jialiao_realtime);

            setMysqlData(gengsi_zhengyageng_realtime);
            setMysqlData(honggengsi_realtime);
            setMysqlData(hunshi_jiaxiang_realtime);

            setMysqlData(yangeng_huicao_realtime);
            setMysqlData(yepian_huichao_realtime);
            setMysqlData(yepian_jialiao_realtime);

            setMysqlData(yeshi_baoban_realtime);
            setMysqlData(yeshi_qiliu_realtime);
            setMysqlData(yeshi_qiliucaoqihuicao_realtime);


            Standard_Value_Step();

            /////存储生产表单数据
            //setProductData();

        }


        public void Standard_Value_Step()
        {
            ///查询标准值
            findStandardValue(gengsi_jialiao_limit, gengsi_jialiao_realtime_datakey);

            findStandardValue(gengsi_zhengyageng_limit, gengsi_zhengyageng_realtime_datakey);
            findStandardValue(honggengsi_limit, honggengsi_realtime_datakey);
            findStandardValue(hunshi_jiaxiang_limit, hunshi_jiaxiang_realtime_datakey);

            findStandardValue(yangeng_huicao_limit, yangeng_huicao_realtime_datakey);
            findStandardValue(yepian_huichao_limit, yepian_huichao_realtime_datakey);
            findStandardValue(yepian_jialiao_limit, yepian_jialiao_realtime_datakey);

            findStandardValue(yeshi_baoban_limit, yeshi_baoban_realtime_datakey);
            findStandardValue(yeshi_qiliu_limit, yeshi_qiliu_realtime_datakey);
            findStandardValue(yeshi_qiliucaoqihuicao_limit, yeshi_qiliucaoqihuicao_realtime_datakey);

            ///存储标准值入数据库
            InsertStandardValue(gengsi_jialiao_limit);

            InsertStandardValue(gengsi_zhengyageng_limit);
            InsertStandardValue(honggengsi_limit);
            InsertStandardValue(hunshi_jiaxiang_limit);

            InsertStandardValue(yangeng_huicao_limit);
            InsertStandardValue(yepian_huichao_limit);
            InsertStandardValue(yepian_jialiao_limit);

            InsertStandardValue(yeshi_baoban_limit);
            InsertStandardValue(yeshi_qiliu_limit);
            InsertStandardValue(yeshi_qiliucaoqihuicao_limit);

        }



        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns></returns>
        public int GetCurrentTimeUnix()
        {
            TimeSpan cha = (DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)));
            int t = (int)cha.TotalSeconds;
            return t;
        }


        /// <summary>
        /// 查找工单数据
        /// </summary>
        public void find_product_data()
        {
            //SELECT * FROM [nymes].[VIEW_CUT_PLANS] WHERE TECH_NM='叶片回潮段' AND PLAN_DATE='" + pro_time_str + "'
            //SELECT * FROM [nymes].[VIEW_CUT_PLANS] WHERE TECH_NM='叶片切丝段' AND PLAN_DATE='" + pro_time_str + "'
            //SELECT * FROM [nymes].[VIEW_CUT_PLANS] WHERE TECH_NM='烘叶丝处理段(薄板)' AND PLAN_DATE='" + pro_time_str + "'
            //SELECT * FROM [nymes].[VIEW_CUT_PLANS] WHERE TECH_NM='加香储丝段' AND PLAN_DATE='" + pro_time_str + "'
            //SELECT * FROM [nymes].[VIEW_CUT_PLANS] WHERE (TECH_NM='梗处理段' or TECH_NM='梗预处理段') AND PLAN_DATE='" + pro_time_str + "'
            //SELECT * FROM [nymes].[VIEW_CUT_PLANS] WHERE (TECH_NM='切梗丝加料段' or TECH_NM='烘梗丝加香段') AND PLAN_DATE='" + pro_time_str + "'
            //

            SqlAccess.ExecuteQuery("delete from product_info");

            ///get now time 
            DateTime pro_time = new DateTime();
            pro_time = DateTime.Now;

            string pro_time_str = pro_time.ToString("yyyy-MM-dd");


            ///get data
            string[] product_result = new string[25];
            List<string[]> product_result_array = new List<string[]>();

            product_result = Mes_Product_SqlAccess.ExecuteQuery("SELECT TOP 1  * FROM [nymes].[VIEW_CUT_PLANS] WHERE TECH_NM='叶片回潮段' AND PLAN_DATE='" + pro_time_str + "' order by ACT_ENDTIME desc");///nInvalid object name 'nymes.VIEW_CUT_PLANS'

            product_result_array.Add(product_result);

            product_result = Mes_Product_SqlAccess.ExecuteQuery("SELECT TOP 1  * FROM [nymes].[VIEW_CUT_PLANS] WHERE TECH_NM='叶片切丝段' AND PLAN_DATE='" + pro_time_str + "' order by ACT_ENDTIME desc");///nInvalid object name 'nymes.VIEW_CUT_PLANS'

            product_result_array.Add(product_result);

            product_result = Mes_Product_SqlAccess.ExecuteQuery("SELECT TOP 1  * FROM [nymes].[VIEW_CUT_PLANS] WHERE TECH_NM='烘叶丝处理段(薄板)' AND PLAN_DATE='" + pro_time_str + "' order by ACT_ENDTIME desc");///nInvalid object name 'nymes.VIEW_CUT_PLANS'

            product_result_array.Add(product_result);

            product_result = Mes_Product_SqlAccess.ExecuteQuery("SELECT TOP 1  * FROM [nymes].[VIEW_CUT_PLANS] WHERE TECH_NM='加香储丝段' AND PLAN_DATE='" + pro_time_str + "' order by ACT_ENDTIME desc");///nInvalid object name 'nymes.VIEW_CUT_PLANS'

            product_result_array.Add(product_result);

            ///product_result = Mes_Product_SqlAccess.ExecuteQuery("SELECT * FROM [nymes].[VIEW_CUT_PLANS] WHERE (TECH_NM='梗处理段' or TECH_NM='梗预处理段') AND PLAN_DATE='" + pro_time_str + "'");///nInvalid object name 'nymes.VIEW_CUT_PLANS'
            product_result = Mes_Product_SqlAccess.ExecuteQuery("SELECT TOP 1  * FROM [nymes].[VIEW_CUT_PLANS] WHERE TECH_NM='梗处理段' AND PLAN_DATE='" + pro_time_str + "' order by ACT_ENDTIME desc");///nInvalid object name 'nymes.VIEW_CUT_PLANS'

            product_result_array.Add(product_result);

            product_result = Mes_Product_SqlAccess.ExecuteQuery("SELECT TOP 1  * FROM [nymes].[VIEW_CUT_PLANS] WHERE TECH_NM='梗预处理段' AND PLAN_DATE='" + pro_time_str + "' order by ACT_ENDTIME desc");///nInvalid object name 'nymes.VIEW_CUT_PLANS'

            product_result_array.Add(product_result);


            //product_result = Mes_Product_SqlAccess.ExecuteQuery("SELECT * FROM [nymes].[VIEW_CUT_PLANS] WHERE (TECH_NM='切梗丝加料段' or TECH_NM='烘梗丝加香段') AND PLAN_DATE='" + pro_time_str + "'");///nInvalid object name 'nymes.VIEW_CUT_PLANS'
            product_result = Mes_Product_SqlAccess.ExecuteQuery("SELECT TOP 1  * FROM [nymes].[VIEW_CUT_PLANS] WHERE TECH_NM='切梗丝加料段' AND PLAN_DATE='" + pro_time_str + "' order by ACT_ENDTIME desc");///nInvalid object name 'nymes.VIEW_CUT_PLANS'

            product_result_array.Add(product_result);

            product_result = Mes_Product_SqlAccess.ExecuteQuery("SELECT TOP 1  * FROM [nymes].[VIEW_CUT_PLANS] WHERE TECH_NM='烘梗丝加香段' AND PLAN_DATE='" + pro_time_str + "' order by ACT_ENDTIME desc");///nInvalid object name 'nymes.VIEW_CUT_PLANS'

            product_result_array.Add(product_result);


            string startTimeNode1 = "";
            string endTimeNode1 = "";
                                                                        
            ////set data
            int product_info_id = 0;

            foreach (String[] items in product_result_array)
            {
                

                for (int k = 0; k < 20; k++)
                {
                    product_info[k] = " ";
                }

                product_info[0] = product_info_id.ToString();
                product_info[1] = product_info_id++.ToString();
                //Console.WriteLine(product_result.Count());
                //Console.WriteLine(items.Length);//25
               // Console.WriteLine("");

                for (int i = 0; i < 25; i++)
                {
                   // Console.Write(items[i]);
                    switch (i)
                    {
                        case 0://WO
                            product_info[4] = items[i];
                            break;
                        case 1://LOT
                            product_info[3] = items[i];
                            break;
                        case 2://PLAN_DATE
                            break;
                        case 3://SHIFT_ID
                            product_info[6] = items[i];
                            break;
                        case 4://TEAM_ID
                            product_info[5] = items[i];
                            break;
                        case 5://MAT_ID
                            interval_brand = items[i];
                            product_info[19] = items[i];
                            
                            break;
                        case 6://MAT_NM
                            InsertBrand(items[i]);
                            product_info[2] = items[i];
                            break;
                        case 7://ROUTING_ID
                            break;
                        case 8://ROUTING_NM
                            break;
                        case 9://TECH_ID
                            break;
                        case 10://TECH_NM'叶片回潮段  叶片切丝段   烘叶丝处理段(薄板)   加香储丝段   梗处理段   梗预处理段   切梗丝加料段   烘梗丝加香段
                            switch (items[i])
                            {
                                case "真空回潮段":
                                    product_info[18] = "1";
                                    break;
                                case "叶片回潮段":
                                    product_info[18] = "2";///松散回潮机  jialiao 
                                    yepian_huichao_realtime_string = interval_brand;
                                    yepian_jialiao_realtime_string = interval_brand;                 
                                    break;
                                case "叶片切丝段":
                                    product_info[18] = "3";/// jiaxiang 
                                    break;
                                case "烘叶丝处理段(薄板)":///baoban  qiliu  chaoji 
                                    product_info[18] = "4";
                                    yeshi_baoban_realtime_string = interval_brand;
                                    yeshi_qiliu_realtime_string = interval_brand;
                                    yeshi_qiliucaoqihuicao_realtime_string = interval_brand;
                                    break;
                                case "加香储丝段":///
                                    product_info[18] = "5";
                                    hunshi_jiaxiang_realtime_string = interval_brand;
                                    break;
                                case "梗处理段":////  geng hui chao 
                                    product_info[18] = "6";//66
                                    yangeng_huicao_realtime_string = interval_brand;
                                    break;
                                case "梗预处理段"://// geng hui chao 
                                    product_info[18] = "6";
                                    yangeng_huicao_realtime_string = interval_brand;
                                    break;
                                case "切梗丝加料段":///gengsijialiaoji
                                    gengsi_jialiao_realtime_string = interval_brand;
                                    product_info[18] = "7";
                                    
                                    break;
                                case "烘梗丝加香段":///hong gengsiji    zheng ya geng 
                                    product_info[18] = "7";//77
                                    honggengsi_realtime_string = interval_brand;
                                    gengsi_zhengyageng_realtime_string = interval_brand;
                                    break;
                            }
                            
                            break;
                        case 11://QTY
                            break;
                        case 12://SEQ
                            break;
                        case 13://TEC_SEQ
                            break;
                        case 14://STATE_ID
                            break;
                        case 15://STATE_TIME

                            break;
                        case 16://END_TIME
                            break;
                        case 17://ACT_STARTIME
                            product_info[7] = items[i];
                            if (items[i].IndexOf(":") == 12)
                            {
                                startTimeNode1 = items[i].Substring(items[i].IndexOf(" ")+1, 1);
                            }
                            else
                            {
                                startTimeNode1 = items[i].Substring(items[i].IndexOf(" ") + 1, 2);
                            }
                            break;
                        case 18://ACT_ENDTIME
                            product_info[8] = items[i];
                            //Console.WriteLine(items[i]);
                            //Console.WriteLine(items[i].IndexOf(":"));
                            if (items[i].IndexOf(":") == 12)
                            {
                                endTimeNode1 = items[i].Substring(items[i].IndexOf(" ") + 1, 1);
                            }
                            else
                            {
                                endTimeNode1 = items[i].Substring(items[i].IndexOf(" ") + 1, 2);
                            }
                            break;
                        case 19://IN_SILOS
                            break;
                        case 20://INSLOS_NM
                            product_info[13] = items[i];
                            break;
                        case 21://OUT_SILOS
                            break;
                        case 22://OUT_SILOS_NM
                            product_info[12] = items[i];
                            break;
                        case 23://IN_QTY
                            product_info[14] = items[i];
                            break;
                        case 24://OUT_QTY
                            product_info[11] = items[i];
                            break;

                    }
                    
                }

                product_info[10] = "小时";
                product_info[15] = "公斤";
                product_info[17] = "小时";
                //Console.WriteLine(startTimeNode1);
                //Console.WriteLine(endTimeNode1);
                product_info[9] = (int.Parse(endTimeNode1) - int.Parse(startTimeNode1)).ToString();
                product_info[16] = product_info[9];


                ///因为局部变量的原因，提前写入数据库
                setProductData();
                
            }


        }

        /// <summary>
        /// 查找主机设备数据
        /// </summary>
        public void find_device_data(string[] dataString,string[] keyString)
        {
            for (int i = 0; i < keyString.Length; i++)
            {
                dataString[i + 1] = "";
                dataString[i + 1] = Mes_Device_SqlAccess.ExecuteQuery("SELECT * FROM DBO.Live WHERE TagName='" + keyString[i] + "'");
                if (dataString[i + 1].Length >= 20)
                {
                    dataString[i + 1] = dataString[i + 1].Substring(0, 18);
                }
            }
            
        }


        /// <summary>
        /// 插入品牌
        /// </summary>
        /// <param name="brandName"></param>
        public void InsertBrand(string brandName)
        {
            ///interval_brand  branName
            
            DataSet dsIB =  SqlAccess.ExecuteQuery("SELECT * FROM brand WHERE job_num = '"+ interval_brand + "'");
            DataTable dtIB = dsIB.Tables[0];
            if (dtIB.Rows.Count > 0)
            {

            }
            else
            {
                SqlAccess.ExecuteQuery("INSERT INTO brand (_id, job_num, name) VALUES (" + GetCurrentTimeUnix().ToString() + ",'" + interval_brand + "' , '" + brandName + "') ");
            }


        }


        /// <summary>
        /// 查询标准值
        /// </summary>
        public void findStandardValue(string[] dataString, string[] keyString)
        {
            for (int i = 0; i < keyString.Length; i++)
            {
                dataString[i + 1] = "";
                dataString[i + 1] = Mes_Device_SqlAccess.ExecuteQuery("SELECT * FROM DBO.Live WHERE TagName='" + keyString[i] + "_SET'");
                if (dataString[i + 1].Length >= 20)
                {
                    dataString[i + 1] = dataString[i + 1].Substring(0, 18);
                }
            }
        }

        /// <summary>
        /// 插入标准值
        /// </summary>
        public void InsertStandardValue(string[] stringArrays)
        {
            ///获取表名
            string mysqlDataName = stringArrays[0];

            ///获取字段名
            string[] mysqlKeyNames = new string[stringArrays.Count() - 1];

            for (int i = 0; i < mysqlKeyNames.Count(); i++)
            {
                DataSet ds = SqlAccess.ExecuteQuery("SELECT `" + i + "` FROM all_device WHERE `name`='" + mysqlDataName + "'");
                DataTable dt = ds.Tables[0];
                DataRow dr = dt.Rows[0];
                mysqlKeyNames[i] = dr[0].ToString();
                //DataColumn一列            
            }

            ///stringArray里的第1个数，对应字段表中第0个字段，需要注意，因为sringArray里第0个数存储的表名、转存数组数据
            string[] sqlDataTemp = new string[stringArrays.Count() - 1];
            for (int i = 0; i < sqlDataTemp.Count(); i++)
            {
                sqlDataTemp[i] = stringArrays[i + 1];
            }

            ///存储数据
            ///


            ///        string[] device_key = { "gengsi_jialiao_realtime", 
            //"gengsi_zhengyageng_realtime", "honggengsi_realtime", "hunshi_jiaxiang_realtime",
            //"yangeng_huicao_realtime", "yepian_huichao_realtime", "yepian_jialiao_realtime", 
            //"yeshi_baoban_realtime", "yeshi_qiliu_realtime","yeshi_qiliucaoqihuicao_realtime" };
            ////根据表名修改机器名称



            string brandName = "";
            switch (mysqlDataName)
            {
                case "gengsi_jialiao_limit":
                    brandName = gengsi_jialiao_realtime_string;
                    break;

                case "gengsi_zhengyageng_limit":
                    brandName = gengsi_zhengyageng_realtime_string;
                    break;
                case "honggengsi_limit":
                    brandName = honggengsi_realtime_string;
                    break;
                case "hunshi_jiaxiang_limit":
                    brandName = hunshi_jiaxiang_realtime_string;
                    break;

                case "yangeng_huicao_limit":
                    brandName = yangeng_huicao_realtime_string;
                    break;
                case "yepian_huichao_limit":
                    brandName = yepian_huichao_realtime_string;
                    break;
                case "yepian_jialiao_limit":
                    brandName = yepian_jialiao_realtime_string;
                    break;

                case "yeshi_baoban_limit":
                    brandName = yeshi_baoban_realtime_string;
                    break;
                case "yeshi_qiliu_limit":
                    brandName = yeshi_qiliu_realtime_string;
                    break;
                case "yeshi_qiliucaoqihuicao_limit":
                    brandName = yeshi_qiliucaoqihuicao_realtime_string;
                    break;
            }

            
            ///查询品牌标准值是否存在
            string findIsExistQuery = "SELECT * FROM " + mysqlDataName + " WHERE brand = " + brandName + "";

            DataSet DSFE = SqlAccess.ExecuteQuery(findIsExistQuery);
            DataTable DTFE = DSFE.Tables[0];
            if (DTFE.Rows.Count == 0)
            {
            





            ///存储语句 字段编写
            string myQuery = "INSERT INTO " + mysqlDataName + "( " + "`_id`,`brand`,`options`," + "`" + mysqlKeyNames[0] + "`";

            ///因为是从1开始 ，所以结尾为++i少一个
            for (int i = 1; i < mysqlKeyNames.Count(); ++i)
            {
                myQuery += ", " + "`" + mysqlKeyNames[i] + "`";
            }

            myQuery += ")";

            ////get real time 
            string VrealTime;
            System.DateTime realTime = new System.DateTime();
            realTime = System.DateTime.Now;
            VrealTime = realTime.ToString("yyyyMMddHHmm");//201711091010

            string idTime;
            idTime = realTime.ToString("yyyyMMddHHmmss");


            ////存储语句 字段值编写
            myQuery = myQuery + " VALUES ('" + idTime + "', " + brandName + ", '" + "standard_value" + "','" + sqlDataTemp[0] + "'";

            for (int i = 1; i < sqlDataTemp.Count(); ++i)
            {

                myQuery += ", " + "'" + sqlDataTemp[i] + "'";

            }

            myQuery += ")";
            //Console.WriteLine(myQuery);
            SqlAccess.ExecuteQuery(myQuery);
            }
            else
            {
                Console.WriteLine("you");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        ///流程 -》 1.读取数据库时间戳，若大于7天，清楚数据库-》2.获取工单数据，-》3.获取主机设备数据 -》4.存入数据库 -》间隔5s -》循环1234
    }

    

}

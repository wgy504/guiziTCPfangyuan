using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;//导入用MySql的包
using System.Data;//引用DataTable

namespace guiziTCPfangyuan
{
    public class DBHelper
    {
        public string database = "hdm0050213_db";
        public string local = "hdm-005.hichina.com";
        public string user = "hdm0050213";
        public string password = "";

        /// <summary>
        /// 得到连接对象
        /// </summary>
        /// <returns></returns>
        public MySqlConnection GetConn()
        {
            MySqlConnection mysqlconn = new MySqlConnection("Database='" + database + "';Data Source='" + local + "';User Id='" + user + "';Password='" + password + "'");
            return mysqlconn;
        }
    }


    public class SQLHelper : DBHelper
    {
        public SQLHelper(string dbName,string dbUrl,string dbUser,string dbPwd)
        {
            database = dbName;
            local = dbUrl;
            user = dbUser;
            password = dbPwd;
        }

        /// <summary>
        /// 查询操作
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable Selectinfo(string sql)
        {
            MySqlConnection mysqlconn = null;
            MySqlDataAdapter sda = null;
            DataTable dt = null;
            try
            {
                mysqlconn = base.GetConn();

                sda = new MySqlDataAdapter(sql, mysqlconn);
                dt = new DataTable();
                sda.Fill(dt);
                mysqlconn.Close();
                return dt;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 增删改操作
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>执行后的条数</returns>
        public int AddDelUpdate(string sql)
        {

            MySqlConnection conn = null;
            MySqlCommand cmd = null;

            try
            {
                conn = base.GetConn();
                conn.Open();
                cmd = new MySqlCommand(sql, conn);
                int i = cmd.ExecuteNonQuery();
                conn.Close();
                return i;                
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}




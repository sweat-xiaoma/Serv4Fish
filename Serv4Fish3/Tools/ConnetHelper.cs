using System;
using MySql.Data.MySqlClient;

namespace Serv4Fish3.Tools
{
    public class ConnetHelper
    {
        //public ConnetHelper()
        //{
        //}

        //public const string ConnectionString = "Database=testFish;Data Source=127.0.0.1;port=3306;User Id=root;Password=;";
        public const string CONNECTION_STRING = "Database=testFish;Data Source=127.0.0.1;port=3306;user=root;pwd=;";

        public static MySqlConnection Connect()
        {
            MySqlConnection conn = new MySqlConnection(CONNECTION_STRING);
            try
            {
                conn.Open();
                return conn;
            }
            catch (Exception ex)
            {
                Console.WriteLine("连接数据库异常： " + ex.Message);
            }
            return null;
        }

        public static void CloseConnection(MySqlConnection conn)
        {
            try
            {
                if (conn != null)
                {
                    conn.Close();
                }
                else
                {
                    Console.WriteLine("mysqlconnection 不能为空");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("数据库关闭异常" + ex.Message);
            }

        }
    }
}

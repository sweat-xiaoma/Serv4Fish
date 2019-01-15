using System;
using MySql.Data.MySqlClient;
using Serv4Fish3.Model;

namespace Serv4Fish3.DAO
{
    public class WalletDAO
    {
        public Wallet GetWalletByUsername(MySqlConnection conn, string username)
        {
            MySqlDataReader reader = null;

            try
            {
                string sqlStr = "select money,diamond,eth from wallet where username = @username";
                MySqlCommand command = new MySqlCommand(sqlStr, conn);
                command.Parameters.AddWithValue("username", username);
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    int money = reader.GetInt32("money");
                    int diamond = reader.GetInt32("diamond");
                    float eth = reader.GetFloat("eth");
                    Wallet wallet = new Wallet(username, money, diamond, eth);
                    return wallet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] " + "[VerifyUser] 出现异常" + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return null;
        }


        public void UpdateWalletMoney(MySqlConnection conn, string username, int money)
        {
            string sqlstr = string.Format("UPDATE `wallet` set `money`= {0} WHERE username = {1};", money, username);

            MySqlCommand command = new MySqlCommand(sqlstr, conn);

            int re = command.ExecuteNonQuery(); // 返回受影响的行数，为int值。可以根据返回的值进行判断是否成功。

            if (re > 0)
            {
                // 操作成功
            }
            else
            {
                // 操作失败
            }

            conn.Close();//关闭连接
        }
    }
}

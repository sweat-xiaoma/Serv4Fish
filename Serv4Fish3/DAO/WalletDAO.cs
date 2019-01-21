using System;
using MySql.Data.MySqlClient;
using Serv4Fish3.Model;
using Serv4Fish3.Tools;

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
                Console.WriteLine("[" + DateTime.Now + "] " + "[GetWalletByUsername] 出现异常" + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return null;
        }


        public void UpdateWalletMoney(MySqlConnection conn, string username, int oldmoney, int newmoney)
        {
            //Wallet walletnew = this.GetWalletByUsername(conn, username);
            //if (walletnew == null)
            //{
            //    Console.WriteLine("[" + DateTime.Now + "] " + "[UpdateWalletMoney] 获取钱包出现异常 username: " + username);
            //    return;
            //}
            Console.WriteLine("54!");
            Console.WriteLine(conn);
            string sqlstr = string.Format("UPDATE `wallet` set `money`= {0} WHERE username = {1};", newmoney, username);
            Console.WriteLine("56!");

            MySqlCommand command = new MySqlCommand(sqlstr, conn);
            Console.WriteLine("58!");
            Console.WriteLine(command);
            int re = command.ExecuteNonQuery(); // 返回受影响的行数，为int值。可以根据返回的值进行判断是否成功。
            Console.WriteLine("61!");
            Console.WriteLine(re);
            //if (re > 0) // 操作成功
            //else // 操作失败

            // 插入一条日志
            int changeNumber = newmoney - oldmoney;
            // 字段说明                                                用户名      操作时间    变化值     新余额     备注说明    
            string sqlstr72 = string.Format("INSERT INTO money_log (`username`, `w_time`, `number`, `remain`, `remark`, `type`) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')",
                username, Util.TimeInterval1970(), changeNumber, newmoney, "捕鱼游戏场景~", 2333);

            MySqlCommand command85 = new MySqlCommand(sqlstr72, conn);
            Console.WriteLine("73!");
            Console.WriteLine(command85);
            command85.ExecuteNonQuery();
        }
    }
}

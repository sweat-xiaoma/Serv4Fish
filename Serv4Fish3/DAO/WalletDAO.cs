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


        public void UpdateWalletMoney(MySqlConnection conn, string username,
                int oldMoney, int newMoney,
                int oldDiamond, int newDiamond)
        {

            try
            {
                string sqlstr = string.Format("UPDATE `wallet` set `money`= {0}, `diamond`={1} WHERE username = {2};", newMoney, newDiamond, username);

                MySqlCommand command = new MySqlCommand(sqlstr, conn);
                int re = command.ExecuteNonQuery(); // 返回受影响的行数，为int值。可以根据返回的值进行判断是否成功。
                if (re > 0) // 操作成功 
                {
                    // 插入一条日志(money)
                    int changeNumber = newMoney - oldMoney;
                    if (changeNumber != 0)
                    {
                        // 字段说明                                                用户名      操作时间    变化值     新余额     备注说明
                        string sqlstr64 = string.Format("INSERT INTO money_log (`username`, `w_time`, `number`, `remain`, `remark`, `type`) " +
                                                                                "VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')",
                                                        username, Util.TimeInterval1970(), changeNumber, newMoney, "捕鱼游戏场景~金币变动", 2333);

                        MySqlCommand command68 = new MySqlCommand(sqlstr64, conn);
                        command68.ExecuteNonQuery();
                    }

                    // 插入一条日志(money)
                    int changeNumberDiamond = newDiamond - oldDiamond;
                    if (changeNumberDiamond != 0)
                    {
                        // 字段说明                                                用户名      操作时间    变化值     新余额     备注说明
                        string sqlstr77 = string.Format("INSERT INTO diamond_log (`username`, `w_time`, `number`, `remain`, `remark`, `type`) " +
                                                                                "VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')",
                                                        username, Util.TimeInterval1970(), changeNumberDiamond, newDiamond, "捕鱼游戏场景~钻石变动~", 2333);

                        MySqlCommand command81 = new MySqlCommand(sqlstr77, conn);
                        command81.ExecuteNonQuery();
                    }
                }
                //else // 操作失败

            }
            catch (Exception ex)
            {
                //Console.WriteLine("玩家退出时保存钱包数据异常: " + ex.Message);
                Console.WriteLine("[" + DateTime.Now + "] WalletDAO UpdateWalletMoney 异常: " + ex.Message);
            }
        }



    }
}

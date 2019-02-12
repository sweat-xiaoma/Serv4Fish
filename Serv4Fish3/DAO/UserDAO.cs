using System;
using MySql.Data.MySqlClient;
using Serv4Fish3.Model;

namespace Serv4Fish3.DAO
{
    public class UserDAO
    {
        public User VerifyUser(MySqlConnection conn, string guid)
        {
            MySqlDataReader reader = null;

            try
            {
                MySqlCommand command = new MySqlCommand("select * from user where guid= @guid", conn);
                command.Parameters.AddWithValue("guid", guid);
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    string nickname = reader.GetString("nickname");
                    string username = reader.GetString("username");
                    int cannon_lv = reader.GetInt32("cannon_lv");
                    User user = new User(id, username, nickname, cannon_lv);
                    return user;
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

        public void UpdateUser(MySqlConnection conn, string username, int cannonlvopen)
        {
            try
            {
                //string sqlstr = string.Format("UPDATE `wallet` set `money`= {0}, `diamond`={1} WHERE username = {2};", newMoney, newDiamond, username);
                string sqlstr = string.Format("UPDATE `user` set `cannon_lv`= {0} WHERE username = {1};", cannonlvopen, username);

                MySqlCommand command = new MySqlCommand(sqlstr, conn);
                int re = command.ExecuteNonQuery(); // 返回受影响的行数，为int值。可以根据返回的值进行判断是否成功。
                if (re > 0) // 操作成功 
                {
                }
                //else // 操作失败

            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] UserDAO UpdateUser 异常: " + ex.Message);
            }

        }
    }
}

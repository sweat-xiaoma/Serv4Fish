using System;
using MySql.Data.MySqlClient;
using Serv4Fish3.Model;

namespace Serv4Fish3.DAO
{
    public class UserDAO
    {
        public User VerifyUser(MySqlConnection conn, string username)
        {
            MySqlDataReader reader = null;

            try
            {
                MySqlCommand command = new MySqlCommand("select * from user where username= @username", conn);
                command.Parameters.AddWithValue("username", username);
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    string nickname = reader.GetString("nickname");
                    User user = new User(id, username, nickname);
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[VerifyUser] 出现异常" + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return null;
        }
    }
}

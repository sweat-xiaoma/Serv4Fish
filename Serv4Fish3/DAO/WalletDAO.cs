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
                MySqlCommand command = new MySqlCommand("select money,diamond,eth from wallet where username = @username", conn);
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

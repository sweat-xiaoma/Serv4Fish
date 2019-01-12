using System;
using Serv4Fish3.Model;
using MySql.Data.MySqlClient;

namespace Serv4Fish3.DAO
{
    public class ResultDAO
    {
        //public ResultDAO()
        //{
        //}

        public Result GetResultByUserid(MySqlConnection connection, int userid)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand command = new MySqlCommand("select * from result where userid=@userid", connection);
                command.Parameters.AddWithValue("userid", userid);
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    int idRead = reader.GetInt32("id");
                    int useridRead = reader.GetInt32("userid");
                    int totalresultRead = reader.GetInt32("totalresult");
                    int winresultRead = reader.GetInt32("winresult");

                    Result result = new Result(idRead, useridRead, totalresultRead, winresultRead);
                    return result;
                }
                else
                {
                    Result result = new Result(-1, userid, 0, 0);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] " + "[GetResultByUserid] 出现异常" + ex.Message);
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

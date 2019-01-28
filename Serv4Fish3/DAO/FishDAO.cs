using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Serv4Fish3.Model;

namespace Serv4Fish3.DAO
{
    public class FishDAO
    {
        public List<Fish> VerifyFishStaticData(MySqlConnection conn)
        {
            List<Fish> fishList = new List<Fish>();
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand command = new MySqlCommand("select * from game_fishes", conn);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Fish fish = new Fish(
                        reader.GetInt32("id"),
                        reader.GetInt32("life"),
                        reader.GetInt32("speed"),
                        reader.GetInt32("kill_bonus"),
                        reader.GetInt32("count_max")
                    );

                    fishList.Add(fish);
                }

                return fishList;
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
    }
}

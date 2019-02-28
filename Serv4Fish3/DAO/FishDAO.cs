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
                Console.WriteLine(reader.FieldCount);
                Console.WriteLine("加载鱼的数据...");
                while (reader.Read())
                {
                    Fish fish = new Fish(
                        reader.GetInt32("id"),
                        reader.GetInt32("life"),
                        reader.GetInt32("speed"),
                        reader.GetInt32("kill_bonus"),
                        reader.GetInt32("count_max"),
                        reader.GetInt32("kill_bonus_d")
                    );

                    fishList.Add(fish);

                    Console.WriteLine(
                            "[fish] name: " + reader.GetString("name")
                            + "\tid: " + reader.GetInt32("id")
                            + "\tlife: " + reader.GetInt32("life")
                            + "\tkillBonus: " + reader.GetInt32("kill_bonus")
                            + "\tkillBonusDiamond: " + reader.GetInt32("kill_bonus_d")
                            );
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

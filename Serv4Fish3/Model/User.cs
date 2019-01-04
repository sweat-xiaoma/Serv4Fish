using System;
using MySql.Data.MySqlClient;



namespace Serv4Fish3.Model
{
    public class User
    {
        public readonly int Id;
        public readonly string Username;
        public int Corner; // 玩家的座位 -1没座位，0左下，1右下，2右上，3左上

        public User(int id, string username)
        {
            this.Id = id;
            this.Username = username;
        }


    }
}

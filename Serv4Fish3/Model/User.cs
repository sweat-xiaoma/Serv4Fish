using System;
using MySql.Data.MySqlClient;



namespace Serv4Fish3.Model
{
    public class User
    {
        public readonly int Id;
        public readonly string Username;
        public readonly string Nickname;
        public int Corner; // 玩家的座位 -1没座位，0左下，1右下，2右上，3左上

        public int CannonLvCurr = 1;
        // 1,10,20,30...100,
        // 150,200,250...1000,
        // 1100,1200,1300...10000
        public int CannonLvOpen = 1; // 已解锁的炮倍数

        public User(int id, string username, string nickname, int cannonlv)
        {
            this.Id = id;
            this.Username = username;
            this.Nickname = nickname;
            this.CannonLvOpen = cannonlv;
            this.CannonLvCurr = cannonlv;
        }

    }
}

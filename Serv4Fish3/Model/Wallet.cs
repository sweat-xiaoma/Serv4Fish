using System;
namespace Serv4Fish3.Model
{
    public class Wallet
    {
        public readonly string Username;
        public int Money; // 金币
        public int Diamond; // 钻石
        public float Eth; // eth

        public Wallet(string username, int money, int diamond, float eth)
        {
            this.Username = username;
            this.Money = money;
            this.Diamond = diamond;
            this.Eth = eth;
        }

    }
}

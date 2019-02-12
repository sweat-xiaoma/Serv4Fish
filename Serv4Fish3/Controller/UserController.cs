using System;
using FishCommon3;
using Serv4Fish3.ServerSide;
using Serv4Fish3.DAO;
using Serv4Fish3.Model;
using Serv4Fish3.Tools;

namespace Serv4Fish3.Controller
{
    public class UserController : BaseController
    {
        public UserController()
        {
            requestCode = RequestCode.User;
        }

        public string Login(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            string guid = strs[0];

            if (!server.CheckUserRepeat(guid)) // 检测用户重复登陆
            {
                return ((int)ReturnCode.Fail).ToString();
            }

            UserDAO userDAO = new UserDAO();
            User user = userDAO.VerifyUser(client.MySQlConn, guid);
            if (user == null)
            {
                return ((int)ReturnCode.Notdfound).ToString();
            }
            else
            {
                WalletDAO walletDAO = new WalletDAO();
                Wallet wallet = walletDAO.GetWalletByUsername(client.MySQlConn, user.Username);
                client.SetUserData(user, wallet); // 设置玩家信息
                return ((int)ReturnCode.Success).ToString() + ","
                                                + wallet.Money + ","
                                                + wallet.Diamond;


            }
        }

        public string ChangeCost(string data, Client client, Server server)
        {
            int currLv = client.GetUser().CannonLvCurr;
            int lvMax = client.GetUser().CannonLvOpen;

            if (data == "a") // 加+
            {
                int nextLv = this.NextCannonLv(currLv);
                if (nextLv > lvMax) // 尚未解锁
                {
                    //client.GetUser().CannonLvCurr = nextLv;
                    client.GetUser().CannonLvCurr = 1;
                }
                else
                {
                    client.GetUser().CannonLvCurr = nextLv;
                }
            }
            else // 减-
            {
                int preLv = this.PreCannonLv(currLv, lvMax);
                client.GetUser().CannonLvCurr = preLv;
            }

            string re = client.GetUser().Corner + "|" + client.GetUser().CannonLvCurr;

            Room room = client.Room;
            if (room != null)
                room.BroadcastMessage(null, ActionCode.ChangeCost, re);

            return "";
        }

        //// 客户端接收到服务器的心跳之后 调用这个
        //public string HeartBeatServ(string data, Client client, Server server)
        //{
        //    if (data == "b")
        //    {
        //        // 更新一下最后一次 心跳时间
        //        client.LastTickTime = Util.GetTimeStamp();
        //    }
        //    return "";
        //}

        // 接收到客户端 响应的心跳
        public string PongFromClient(string data, Client client, Server server)
        {
            if (data == "c")
            {
                client.LastTickTime = Util.GetTimeStamp();
            }
            System.Console.WriteLine("收到了{0} 客户端发来的pong {1} ",
                        client.ipaddress, DateTime.Now);
            return "";
        }

        public string UpgradeCannon(string data, Client client, Server server)
        {
            if (data == "gg")
            {
                int currLv = client.GetUser().CannonLvCurr;
                int nextlv = this.NextCannonLv(currLv);
                int cost = nextlv / 5; // 等级除以5 作为升级消耗钻石数目
                int userDiamond = client.GetWallet().Diamond;

                if (cost > userDiamond)
                {
                    // 钻石不足
                    string data114 = ((int)ReturnCode.Fail).ToString();
                    server.SendResponse(client, ActionCode.UpgradeCannon, data114);
                }
                else
                {
                    Room room = client.Room;
                    // 升级成功
                    // 1. 扣除 wallet 中钻石, 并广播钻石数目改变
                    client.GetWallet().Diamond -= cost;

                    // 2. 增加用户的 cannon_lv 并广播炮改变
                    client.GetUser().CannonLvCurr =
                        client.GetUser().CannonLvOpen = nextlv;

                    string data128 = client.GetUser().Corner + "|"
                        + client.GetWallet().Diamond;
                    room.BroadcastMessage(null, ActionCode.UpdateDiamond, data128);

                    string data132 = client.GetUser().Corner + "|" +
                        nextlv;
                    room.BroadcastMessage(null, ActionCode.ChangeCost, data132);

                    string data136 = ((int)ReturnCode.Success).ToString();
                    server.SendResponse(client, ActionCode.UpgradeCannon, data136);
                }
            }
            return "";
        }

        int NextCannonLv(int currLv)
        {
            int nextLv = 0;
            if (currLv <= 1)
            {
                nextLv = 10;
            }
            else if (currLv < 100)
            {
                nextLv = currLv + 10;
            }
            else if (currLv < 1000)
            {
                nextLv = currLv + 50;
            }
            else if (currLv < 10000)
            {
                nextLv = currLv + 100;
            }

            return nextLv;
        }

        int PreCannonLv(int currLv, int maxLv)
        {
            int preLv = 0;
            if (currLv == 1)
            {
                preLv = maxLv;
            }
            else if (currLv <= 10)
            {
                preLv = 1;
            }
            else if (currLv <= 100)
            {
                preLv = currLv - 10;
            }
            else if (currLv <= 1000)
            {
                preLv = currLv - 50;
            }
            else if (currLv <= 10000)
            {
                preLv = currLv - 100;
            }

            return preLv;
        }
    }
}

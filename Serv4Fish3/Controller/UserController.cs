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
            string username = strs[0];

            if (!server.CheckUserRepeat(username)) // 检测用户重复登陆
            {
                return ((int)ReturnCode.Fail).ToString();
            }

            UserDAO userDAO = new UserDAO();
            User user = userDAO.VerifyUser(client.MySQlConn, username);
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
            if (data == "a")
            {
                client.GetUser().Cost += 5; // 加倍
            }
            else
            {
                client.GetUser().Cost -= 5; // 减倍数
            }

            if (client.GetUser().Cost < 10)
            {
                client.GetUser().Cost = 10;
            }

            string re = client.GetUser().Corner + "|" + client.GetUser().Cost;

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
    }
}

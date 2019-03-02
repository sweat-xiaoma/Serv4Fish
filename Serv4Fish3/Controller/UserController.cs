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
        const int Scene_Open1 = 1;
        const int Scene_Open2 = 30;
        const int Scene_Open3 = 60;

        public UserController()
        {
            requestCode = RequestCode.User;
        }

        public string Login(string data, Client client, Server server)
        {
            Console.WriteLine("Login hashcode: " + client.GetHashCode());
            string[] strs = data.Split('|');
            string guid = strs[0];
            SceneIndex gameindex = (SceneIndex)int.Parse(strs[1]);

            UserDAO userDAO = new UserDAO();
            User user = userDAO.VerifyUser(client.MySQlConn, guid);
            if (user == null)
            {
                return ((int)ReturnCode.Notdfound).ToString();
            }
            else
            {
                if (!server.CheckUserRepeat(user.Username)) // 检测用户重复登陆
                {
                    return ((int)ReturnCode.Fail).ToString();
                }

                bool enterFail = true;
                SceneIndex enterScene = gameindex; // 最后进入的场景
                switch (gameindex)
                {
                    case SceneIndex.AUTO: // 快速游戏 优先进入高级场景
                        enterFail = false;
                        if (user.CannonLvOpen >= Scene_Open3)
                            enterScene = SceneIndex.SCENE_3;
                        else if (user.CannonLvOpen >= Scene_Open2)
                            enterScene = SceneIndex.SCENE_2;
                        else
                            enterScene = SceneIndex.SCENE_1;

                        break;
                    case SceneIndex.SCENE_1: // 场景1
                        enterFail = false;
                        break;
                    case SceneIndex.SCENE_2: // 场景2
                        enterFail = user.CannonLvOpen < Scene_Open2;
                        break;
                    case SceneIndex.SCENE_3: // 场景3
                        enterFail = user.CannonLvOpen < Scene_Open3;
                        break;
                }
                if (enterFail)
                    return ((int)ReturnCode.ERROR_A).ToString();

                user.EnterScene = enterScene;

                WalletDAO walletDAO = new WalletDAO();
                Wallet wallet = walletDAO.GetWalletByUsername(client.MySQlConn, user.Username);
                client.SetUserData(user, wallet); // 设置玩家信息
                return ((int)ReturnCode.Success).ToString() + ","
                                                + wallet.Money + ","
                                                + wallet.Diamond + ","
                                                + user.CannonLvOpen;
            }
        }

        public string ChangeCost(string data, Client client, Server server)
        {
            int currLv = client.GetUser().CannonLvCurr;
            int lvMax = client.GetUser().CannonLvOpen;

            if (data == "a") // 加+
            {
                if (currLv >= lvMax) // 尚未解锁
                {
                    //client.GetUser().CannonLvCurr = nextLv;
                    client.GetUser().CannonLvCurr = 1;
                }
                else
                {
                    //int nextLv = ;
                    client.GetUser().CannonLvCurr = this.NextCannonLv(currLv);
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

        //// 接收到客户端 响应的心跳
        //public string PongFromClient(string data, Client client, Server server)
        //{
        //    if (data == "c")
        //    {
        //        client.LastTickTime = Util.GetTimeStamp();
        //    }
        //    System.Console.WriteLine("收到了{0} 客户端发来的pong {1} ",
        //                client.ipaddress, DateTime.Now);
        //    return "";
        //}

        public string UpgradeCannon(string data, Client client, Server server)
        {
            if (data == "gg")
            {
                int lvOpen = client.GetUser().CannonLvOpen;

                if (lvOpen >= Defines.CANNON_LV_MAX)
                {
                    if (client.GetUser().CannonLvCurr < lvOpen)
                    {
                        client.GetUser().CannonLvCurr = lvOpen;
                        string data148 = client.GetUser().Corner + "|" + lvOpen;
                        client.Send(ActionCode.ChangeCost, data148);
                    }

                    string data141 = ((int)ReturnCode.ERROR_A).ToString();
                    //server.SendResponse2Client(client, ActionCode.UpgradeCannon, data141);
                    client.Send(ActionCode.UpgradeCannon, data141);
                    return "";
                }

                int nextlv = this.NextCannonLv(lvOpen);
                int cost = nextlv / 5; // 等级除以5 作为升级消耗钻石数目
                int userDiamond = client.GetWallet().Diamond;

                if (cost > userDiamond)
                {
                    // 钻石不足
                    string data114 = ((int)ReturnCode.Fail).ToString();
                    //server.SendResponse2Client(client, ActionCode.UpgradeCannon, data114);
                    client.Send(ActionCode.UpgradeCannon, data114);
                    return "";
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

                    string data177 = (int)ReturnCode.Success + "," + nextlv;
                    //server.SendResponse2Client(client, ActionCode.UpgradeCannon, data177);
                    client.Send(ActionCode.UpgradeCannon, data177);
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
            else if (currLv < Defines.CANNON_LV_MAX)
            {
                nextLv = currLv + 10;
            }
            //else if (currLv < 1000)
            //{
            //    nextLv = currLv + 50;
            //}
            //else if (currLv < 10000)
            //{
            //    nextLv = currLv + 100;
            //}
            else if (currLv >= Defines.CANNON_LV_MAX)
            {
                nextLv = Defines.CANNON_LV_MAX;
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
            //else if (currLv <= 1000)
            //{
            //    preLv = currLv - 50;
            //}
            //else if (currLv <= 10000)
            //{
            //    preLv = currLv - 100;
            //}

            return preLv;
        }
    }
}

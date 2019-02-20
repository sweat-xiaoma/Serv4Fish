using System;
using FishCommon3;
using Serv4Fish3.ServerSide;

namespace Serv4Fish3.Controller
{
    public class GameController : BaseController
    {
        public GameController()
        {
            requestCode = RequestCode.Game;
        }

        //public string StartGame(string data, Client client, Server server)
        //{
        //    if (client.IsRoomOwner()) // 房主才能开始
        //    {
        //        Room room = client.Room;
        //        room.BroadcastMessage(client, ActionCode.StartGame, ((int)ReturnCode.Success).ToString()); // 给其他客户端广播 开始游戏

        //        room.StartTimer(); // 开始倒计时

        //        return ((int)ReturnCode.Success).ToString(); // 给当前客户端发送 开始游戏
        //    }
        //    else
        //    {
        //        return ((int)ReturnCode.Fail).ToString();
        //    }
        //}

        public string Rotate(string data, Client client, Server server)
        {
            Room room = client.Room;
            if (room != null)
                room.BroadcastMessage(client, ActionCode.Rotate, data); // 直接转发
            return "";
        }

        public string Shoot(string data, Client client, Server server)
        {
            Room room = client.Room;
            if (room != null)
            {
                // 玩家 - 扣钱 (广播 同步金币)
                client.GetWallet().Money -= client.GetUser().CannonLvCurr;
                string data48 = client.GetUser().Corner + "|" + client.GetWallet().Money;
                room.BroadcastMessage(null, ActionCode.UpdateMoney, data48);

                // 直接转发
                room.BroadcastMessage(client, ActionCode.Shoot, data);
            }
            return "";
        }

        //public string FishSync(string data, Client client, Server server)
        //{
        //    return "";
        //}

        //public string FishGenerate(string data, Client client, Server server)
        //{
        //    string[] strs = data.Split('|');

        //    FishData fishData = new FishData();
        //    fishData.hp = int.Parse(strs[1]);
        //    fishData.coin = int.Parse(strs[2]);

        //    Room room = client.Room;
        //    if (room != null)
        //    {
        //        room.AddFish(strs[0], fishData);
        //        room.BroadcastMessage(client, ActionCode.FishGenerate, data); // 直接转发
        //    }

        //    return "";
        //}

        public string FishHit(string data, Client client, Server server)
        {
            Room room = client.Room;

            string[] strs = data.Split('|');
            //string fishguid = strs[0];
            //int fishguid = int.Parse(strs[0]);
            //int damage = int.Parse(strs[1]);
            room.HitFish(client, strs[0]); // 后续操作交给 Room 处理。
            return "";
        }

        public string FishOutScreen(string data, Client client, Server server)
        {
            Room room = client.Room;
            //int fishguid = int.Parse(data);
            room.FishOutByClient(client, data);
            return "";
        }
    }
}

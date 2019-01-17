//#define DEBUG_VIEW

using System;
using System.Collections;
using System.Collections.Generic;
using Serv4Fish3.Model;
using System.Text;
using FishCommon3;

namespace Serv4Fish3.ServerSide
{
    enum RoomState
    {
        WaitingJoin, // 等待人齐
        WaitingBattle, // 等待开战
        Battle, // 
        End
    }

    public class FishData
    {
        public int hp; // 血量
        public int coin; // 金币
    }


    public class Room
    {
        //List<FishData> fishList = new List<FishData>();

        Dictionary<string, FishData> fishDic = new Dictionary<string, FishData>();

        public const int MaxPeople = 4;
        //public const int MaxPeople = 1;

        Client[] clientArray = new Client[MaxPeople];
        RoomState state = RoomState.WaitingJoin;

        Server server;

        public Room(Server server)
        {
            this.server = server;
        }

        // 房间等待加入
        public bool IsWaitingJoin()
        {
            return state == RoomState.WaitingJoin;
        }

        //// 第一个就是房主
        //public bool IsRoomOwner(Client client)
        //{
        //    return client == clientArray[0];
        //}

        //public void AddClient(Client client)
        public void AddClient(Client client, int index)
        {
            // 判断是否有房主 没有房主的话 自己当房主
            // 正常情况除非房内没人，不然是有房主的
            bool hasMaster = false;
            foreach (Client item in clientArray)
            {
                if (item != null && item.isMaster == 1) // 房主
                {
                    hasMaster = true;
                    break;
                }
            }

            if (hasMaster == false)
            {
                client.isMaster = 1;
            }


            //clientList.Add(client);
            if (clientArray[index] == null)
            {
                clientArray[index] = client;
                client.Room = this;
                client.GetUser().Corner = index;
            }
            else
            {
                Console.WriteLine("[" + DateTime.Now + "] " + "[Room AddClient] 用户进房间座位有人了");
            }

            //if (clientList.Count >= Room.MaxPeople) // 最大人数
            //{
            //    state = RoomState.WaitingBattle; // 满员了 等待开始战斗
            //}
        }

        //public void RemoveClient(Client client)
        public void RemoveClient(Client client, int index)
        {
            client.Room = null;
            //clientList.Remove(client);
            clientArray[index] = null;

            //if (clientList.Count >= 2) // 最大人数
            //{
            //    state = RoomState.WaitingBattle; // 满员了 等待开始战斗
            //}
            //else
            //{
            //    state = RoomState.WaitingJoin;
            //}
        }

        //// 获取房主信息  第一个进房间的人（创建者）
        //public string GetHouseOwnerData()
        //{
        //    return clientList[0].GetUserData();
        //}

        public void QuitRoom(Client client)
        {
            //Console.WriteLine("127 QuitRoom");

            // 退出的人 是否是房主
            bool quitIsMaster = client.isMaster == 1;

            // 从座位上移除
            this.RemoveClient(client, client.GetUser().Corner);

            // 判断房间里没人了就销毁房间
            bool isEmpty = true;
            foreach (var item in clientArray)
            {
                if (item != null)
                {
                    isEmpty = false;
                    break;
                }
            }
            if (isEmpty)
            {
                //server.RemoveRoom(this);
                DestroyRoom();
            }
            else
            {
                if (quitIsMaster) // 退出的是房主， 且房间里还有人 -》 转移房主
                {
                    foreach (Client item in clientArray)
                    {
                        if (item != null) // 剩余人的第一个当房主
                        {
                            item.isMaster = 1;
                            break;
                        }
                    }
                }
            }

            // 广播给房间内其他人 我走了～再见。
            BroadcastMessage(client, ActionCode.UpdateRoom, GetRoomData());
        }

        // 销毁房间
        //public void Close()
        void DestroyRoom()
        {
            //foreach (Client client in clientArray)
            //{
            //    client.Room = null;
            //}
            server.RemoveRoom(this);
        }

        //public int GetId()
        //{
        //    if (clientList.Count > 0)
        //    {
        //        return clientList[0].GetUserId();
        //    }
        //    return -1;
        //}

        /// <summary>
        /// 获取一个空座位
        /// </summary>
        /// <returns>获得座位号，-1表示没有空位.</returns>
        public int EmptySeat()
        {
            for (int i = 0; i < clientArray.Length; i++)
            {
                if (clientArray[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public string GetRoomData() // 房间里的用户信息
        {
            StringBuilder sb = new StringBuilder();
            foreach (Client client in clientArray)
            {
                if (client == null)
                    sb.Append("|");
                else
                    sb.Append(client.GetUserData() + "|");
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 房内广播
        /// </summary>
        /// <param name="excludeClient">不接受广播的用户 一般是当前客户端不用再接收</param>
        public void BroadcastMessage(Client excludeClient, ActionCode actionCode, string data)
        {
#if DEBUG_VIEW
            StringBuilder sb = new StringBuilder();
#endif
            foreach (Client client in clientArray)
            {
                if (client != null && client != excludeClient)
                {
#if DEBUG_VIEW
                    sb.Append(client.GetUser().Username + ",");
#endif 
                    server.SendResponse(client, actionCode, data);
                }
            }

#if DEBUG_VIEW
            Console.WriteLine("[" + DateTime.Now + "] " + "房间[{0}]内广播消息[{1}]给[{2}]", this.GetHashCode(), actionCode, sb);
#endif

        }

        //public void StartTimer()
        //{
        //    new Thread(RunTimer).Start();
        //}

        //void RunTimer()
        //{
        //    Thread.Sleep(1000); // 三秒倒计时前先等一秒
        //    for (int i = 3; i > 0; i--)
        //    {
        //        BroadcastMessage(null, ActionCode.ShowTimer, i.ToString());
        //        Thread.Sleep(1000);
        //    }
        //    BroadcastMessage(null, ActionCode.StartPlay, "ee");
        //}


        //// 更新房主，每次有人退出
        //void UpdateMaster()
        //{
        //    foreach (Client client in clientArray)
        //    {
        //        if (client != null)
        //        {

        //        }
        //    }
        //}

        public void AddFish(string fishguid, FishData fishData)
        {
            if (fishDic.ContainsKey(fishguid))
            {
                fishDic.Remove(fishguid);
            }

            fishDic.Add(fishguid, fishData);
#if DEBUG_VIEW
            Console.WriteLine("鱼+1，鱼数量: " + fishDic.Count);
#endif
        }

        public void HitFish(Client client, string fishguid)
        {
            if (fishDic.ContainsKey(fishguid))
            {
                FishData findFish = fishDic[fishguid];

                findFish.hp -= client.GetUser().Cost;

                if (findFish.hp <= 0)
                {
                    int killCorner = client.GetUser().Corner;
                    // 广播 鱼死了,发钱
                    string data2 = killCorner + "|" + fishguid + "|" + findFish.coin;
                    this.BroadcastMessage(null, ActionCode.FishDead, data2);

                    // 玩家 - 加钱 (广播 同步金币)
                    client.GetWallet().Money += findFish.coin;
                    string data316 = client.GetUser().Corner + "|" + client.GetWallet().Money;
                    this.BroadcastMessage(null, ActionCode.UpdateMoney, data316);

                    fishDic.Remove(fishguid); // 移除掉

#if DEBUG_VIEW
                    Console.WriteLine("鱼死 鱼减少， 鱼数量: " + fishDic.Count);
#endif
                }
#if DEBUG_VIEW
                else
                {
                    //Console.WriteLine("鱼没死 " + "  : " + fishguid);
                    Console.WriteLine("鱼没死 fishguid:{0} hp:{1} damage:{2}",
                    fishguid, findFish.hp, damage);
                }
#endif
            }
#if DEBUG_VIEW
            else
            {
                Console.WriteLine("没找到鱼: " + fishguid);
            }
#endif
        }

        public void FishOutByClient(Client client, string fishguid)
        {
            if (fishDic.ContainsKey(fishguid))
            {
                FishData findData = fishDic[fishguid];
                fishDic.Remove(fishguid); // 出屏了。 移除掉。 
#if DEBUG_VIEW
                Console.WriteLine("出屏 鱼减少， 鱼数量: " + fishDic.Count);
#endif
            }
        }

    }

}

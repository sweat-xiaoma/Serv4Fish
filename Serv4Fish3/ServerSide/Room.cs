using System;
using System.Collections;
using System.Collections.Generic;
using Serv4Fish3.Model;
using System.Text;
using FishCommon3;
using System.Threading;

namespace Serv4Fish3.ServerSide
{
    enum RoomState
    {
        WaitingJoin, // 等待人齐
        WaitingBattle, // 等待开战
        Battle, // 
        End
    }

    public class Room
    {
        public const int MaxPeople = 4;

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

        // 第一个就是房主
        public bool IsRoomOwner(Client client)
        {
            return client == clientArray[0];
        }

        //public void AddClient(Client client)
        public void AddClient(Client client, int index)
        {
            //clientList.Add(client);
            if (clientArray[index] == null)
            {
                clientArray[index] = client;
                client.Room = this;
                client.GetUser().Corner = index;
            }
            else
            {
                Console.WriteLine("[Room AddClient] 用户进房间座位有人了");
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
            // 从座位上移除
            this.RemoveClient(client, client.GetUser().Corner);

            // 广播给房间内其他人 我走了～再见。
            BroadcastMessage(client, ActionCode.UpdateRoom, GetRoomData());

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
        }

        // 销毁房间
        //public void Close()
        void DestroyRoom()
        {
            foreach (Client client in clientArray)
            {
                client.Room = null;
            }
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
            foreach (Client client in clientArray)
            {
                if (client != null && client != excludeClient)
                {
                    server.SendResponse(client, actionCode, data);
                }
            }
        }

        public void StartTimer()
        {
            new Thread(RunTimer).Start();
        }

        void RunTimer()
        {
            Thread.Sleep(1000); // 三秒倒计时前先等一秒
            for (int i = 3; i > 0; i--)
            {
                BroadcastMessage(null, ActionCode.ShowTimer, i.ToString());
                Thread.Sleep(1000);
            }
            BroadcastMessage(null, ActionCode.StartPlay, "ee");


        }
    }

}

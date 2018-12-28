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
        List<Client> clientList = new List<Client>();
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
            return client == clientList[0];
        }

        public void AddClient(Client client)
        {
            clientList.Add(client);
            client.Room = this;

            if (clientList.Count >= 2) // 最大人数
            {
                state = RoomState.WaitingBattle; // 满员了 等待开始战斗
            }
        }

        public void RemoveClient(Client client)
        {
            client.Room = null;
            clientList.Remove(client);

            if (clientList.Count >= 2) // 最大人数
            {
                state = RoomState.WaitingBattle; // 满员了 等待开始战斗
            }
            else
            {
                state = RoomState.WaitingJoin;
            }
        }

        // 获取房主信息  第一个进房间的人（创建者）
        public string GetHouseOwnerData()
        {
            return clientList[0].GetUserData();
        }

        public void Close(Client client)
        {
            if (client == clientList[0]) // 只有房主退出才关闭房间
            {
                server.RemoveRoom(this);
            }
            else
            {
                clientList.Remove(client);
            }
        }

        public int GetId()
        {
            if (clientList.Count > 0)
            {
                return clientList[0].GetUserId();
            }
            return -1;
        }

        public string GetRoomData() // 房间信息 和 房间里的用户信息
        {
            //string 
            StringBuilder sb = new StringBuilder();
            foreach (Client client in clientList)
            {
                sb.Append(client.GetUserData() + "|");
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Broadcasts the message.
        /// </summary>
        /// <param name="excludeClient">不接受广播的用户 一般是当前客户端不用再接收</param>
        public void BroadcastMessage(Client excludeClient, ActionCode actionCode, string data)
        {
            foreach (Client client in clientList)
            {
                if (client != excludeClient)
                {
                    server.SendResponse(client, actionCode, data);
                }
            }
        }

        // 销毁房间
        public void Close()
        {
            foreach (Client client in clientList)
            {
                client.Room = null;
            }
            server.RemoveRoom(this);
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

﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using FishCommon3;
using Serv4Fish3.Controller;
using System.Timers;
using Serv4Fish3.Tools;

namespace Serv4Fish3.ServerSide
{
    public class Server
    {
        IPEndPoint ipEndPoint;
        Socket serverSocket;
        List<Client> clientList = new List<Client>();
        List<Room> roomList = new List<Room>();

        ControllerManager controllerManager;

        public Server(string ipStr, int port)
        {
            controllerManager = new ControllerManager(this);
            SetIpAndPort(ipStr, port);
        }
        public void SetIpAndPort(string ipStr, int port)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
        }

        // 心跳计时器
        Timer heartbeatTimer = new Timer(1000);

        public void Start()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Udp);
            serverSocket.Bind(ipEndPoint);
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallback, null);
            Console.WriteLine("[" + DateTime.Now + "] " + "服务启动成功～");


            this.heartbeatTimer.Elapsed += this.HeartbeatTimer_Elapsed;
            this.heartbeatTimer.AutoReset = false;
            this.heartbeatTimer.Enabled = true;
            Console.WriteLine("[" + DateTime.Now + "] " + "开启心跳检查～");



        }

        void HeartbeatTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // 处理心跳
            this.HeartBeat();
            heartbeatTimer.Start();
        }

        void HeartBeat()
        {
            //// 发给客户端, 客户端收到 返回一个在 User 中, 如果 上次收到的时间到这次 差距过大, 表示掉线了
            //// todo 先只给房主发
            foreach (Client client in this.clientList)
            {
                if (client.isMaster == 1)
                {
                    //// 返回时间超过的客户端 踢掉
                    //long timeNow = Util.GetTimeStamp();
                    //if (client.LastTickTime < timeNow - 20) // 20 秒超时
                    //{
                    //    //lock
                    //}
                    //client.Send(ActionCode.HeartBeatServ, "a");
                    client.HeartBeat();
                }

            }


        }

        void AcceptCallback(IAsyncResult ar)
        {
            Socket clientSocket = serverSocket.EndAccept(ar);
            Client client = new Client(clientSocket, this);
            client.Start();
            clientList.Add(client);

            // 尾巴循环
            serverSocket.BeginAccept(AcceptCallback, null); // 循环接收新客户端
        }

        public void RemoveClient(Client client)
        {
            lock (clientList)
                clientList.Remove(client);
        }

        public void SendResponse(Client client, ActionCode actionCode, string data)
        {
            client.Send(actionCode, data);
        }

        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            controllerManager.HandleRequest(requestCode, actionCode, data, client);
        }

        //public void CreateRoom(Client client)
        Room CreateRoom(Client client)
        {
            Room room = new Room(this);
            room.AddClient(client, 0); // 创建的空房间 玩家坐在 0 号座位上
            roomList.Add(room);
            return room;
        }


        public void RemoveRoom(Room room)
        {
            if (roomList != null && room != null)
            {
                Console.WriteLine("[" + DateTime.Now + "] " + "销毁空房间 [{0}] ", room.GetHashCode());
                roomList.Remove(room);
            }
        }

        public List<Room> ListRoom()
        {
            return roomList;
        }

        public List<Client> ListClient()
        {
            return clientList;
        }

        // 快速游戏 （进入最近的一个有空座位的房间）
        public Room JoinRoomFast(Client client)
        {
            foreach (Room item in roomList)
            {
                int seat = item.EmptySeat();
                if (seat != -1) // 找到空位了坐下
                {
                    item.AddClient(client, seat);
                    return item;
                }
            }

            // 没有空房间，创建一个
            Room room = this.CreateRoom(client);
            return room;
        }

        // 用户重复连接 // todo 用guid检测登陆过期
        public bool CheckUserRepeat(string username)
        {
            foreach (Client client in clientList)
            {
                if (client.GetUser() != null)
                    if (client.GetUser().Username == username)
                    {
                        return false;
                    }
            }
            return true;
        }
    }
}

using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
//using Serv4Fish.Common_framework_2_0;
using FishCommon3;
using Serv4Fish3.Controller;


namespace Serv4Fish3.ServerSide
{
    public class Server
    {
        private IPEndPoint ipEndPoint;
        private Socket serverSocket;
        private List<Client> clientList = new List<Client>();
        private List<Room> roomList = new List<Room>();

        ControllerManager controllerManager;

        //public Server() { }
        public Server(string ipStr, int port)
        {
            //ipEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
            controllerManager = new ControllerManager(this);
            SetIpAndPort(ipStr, port);
        }
        public void SetIpAndPort(string ipStr, int port)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
        }


        public void Start()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipEndPoint);
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallback, null);
            Console.WriteLine("服务启动成功～");
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

        /// <summary>
        /// 第一个client是创建者
        /// </summary>
        public void CreateRoom(Client client)
        {
            Room room = new Room(this);
            room.AddClient(client);
            roomList.Add(room);
        }

        public void RemoveRoom(Room room)
        {
            if (roomList != null && room != null)
            {
                roomList.Remove(room);
            }
        }

        public List<Room> ListRoom()
        {
            return roomList;
        }

        public Room GetRoomById(int id)
        {
            foreach (Room room in roomList)
            {
                if (room.GetId() == id) // 找到了要查询的房间房主id
                    return room;
            }
            return null;
        }
    }
}

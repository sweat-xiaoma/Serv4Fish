using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using FishCommon3;
using Serv4Fish3.Controller;
using System.Timers;
using Serv4Fish3.Tools;
using Serv4Fish3.Model;
using Serv4Fish3.DAO;
using MySql.Data.MySqlClient;
using System.Threading;

namespace Serv4Fish3.ServerSide
{
    public class Server
    {
        IPEndPoint ipEndPoint;
        Socket serverSocket;
        List<Fish> fishList;
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

        //// 心跳计时器
        //System.Timers.Timer heartbeatTimer = new System.Timers.Timer(1000);

        public void Start()
        {
            initServSocket();
            //initHeartBeat();
            initFishStaticData();
            //initGenerateFish();
            initFishThread();
        }

        void initServSocket()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipEndPoint);
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallback, null);
            Console.WriteLine("[" + DateTime.Now + "] " + "服务启动成功～");
        }

        //void initHeartBeat()
        //{
        //    this.heartbeatTimer.Elapsed += this.HeartbeatTimer_Elapsed;
        //    this.heartbeatTimer.AutoReset = false;
        //    this.heartbeatTimer.Enabled = true;
        //    Console.WriteLine("[" + DateTime.Now + "] " + "开启心跳检查～");
        //}

        void initFishStaticData()
        {
            MySqlConnection mysqlConn = ConnectHelper.Connect();
            FishDAO fishDAO = new FishDAO();
            this.fishList = fishDAO.VerifyFishStaticData(mysqlConn);
            ConnectHelper.CloseConnection(mysqlConn);

            //Console.WriteLine("fishcount: " + this.fishList.Count);
            Console.WriteLine("[" + DateTime.Now + "] " + "加载鱼群静态数据成功~");
        }

        void initFishThread()
        {
            Thread thread = new Thread(generateFishWithDelay);
            thread.Start();
            Console.WriteLine("[" + DateTime.Now + "] " + "管理鱼群线程开始～");
        }

        void generateFishWithDelay()
        {
            Random random = new Random();
            while (true)
            {
                Thread.Sleep(TimeSpan.FromSeconds(Defines.GENERATE_RATE));
                //Thread.Sleep(TimeSpan.FromSeconds(Defines.GENERATE_RATE * 10));
                //Thread.Sleep(TimeSpan.FromSeconds(0.1f));
                if (this.fishList.Count == 0)
                    continue;
                int randomIndex = random.Next(0, this.fishList.Count);
                //randomIndex = 17;
                //randomIndex = random.Next(16, 18);
                Fish fishvo = this.fishList[randomIndex];
                try
                {
                    foreach (Room item in this.roomList)
                    {
                        item.GenerateFishs(fishvo);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("");
                    Console.WriteLine("[" + DateTime.Now + "] generateFishWithDelay 异常 Exception: " + ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine("");
                }

                //Thread.Sleep(TimeSpan.FromSeconds(30f));
            }
        }


        //void HeartbeatTimer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    // 处理心跳
        //    this.HeartBeat();
        //    heartbeatTimer.Start();
        //}

        //void HeartBeat()
        //{
        //    //// 发给客户端, 客户端收到 返回一个在 User 中, 如果 上次收到的时间到这次 差距过大, 表示掉线了
        //    //// todo 先只给房主发
        //    foreach (Client client in this.clientList)
        //    {
        //        if (client.isMaster == 1)
        //        {
        //            //// 返回时间超过的客户端 踢掉
        //            //long timeNow = Util.GetTimeStamp();
        //            //if (client.LastTickTime < timeNow - 20) // 20 秒超时
        //            //{
        //            //    //lock
        //            //}
        //            //client.Send(ActionCode.HeartBeatServ, "a");
        //            client.HeartBeat();
        //        }

        //    }
        //}

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

        //public void SendResponse2Client(Client client, ActionCode actionCode, string data)
        //{
        //    //Console.WriteLine(client.GetHashCode());
        //    client.Send(actionCode, data);
        //}

        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            controllerManager.HandleRequest(requestCode, actionCode, data, client);
        }

        //public void CreateRoom(Client client)
        Room CreateRoom(Client client, SceneIndex sceneIndex)
        {
            Room room = new Room(this, sceneIndex);
            room.AddClient(client, 0); // 创建的空房间 玩家坐在 0 号座位上
            roomList.Add(room);
            return room;
        }

        public void RemoveRoom(Room room)
        {
            lock (roomList)
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
        public Room JoinRoomFast(Client client, SceneIndex sceneIndex)
        {
            foreach (Room item in roomList)
            {
                if (item.SceneIndex == sceneIndex)
                {
                    int seat = item.EmptySeat();
                    if (seat != -1) // 找到空位了坐下
                    {
                        Console.WriteLine("用户坐下: " + seat);
                        item.AddClient(client, seat);
                        return item;
                    }
                }
            }

            // 没有空房间，创建一个
            Room room = this.CreateRoom(client, sceneIndex);
            return room;
        }

        // 用户重复连接 用guid检测登陆过期
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

using System;
using System.Net;
using System.Net.Sockets;
using MySql.Data.MySqlClient;
using Serv4Fish3.Tools;
using FishCommon3;
using Serv4Fish3.Model;

namespace Serv4Fish3.ServerSide
{
    public class Client
    {
        string clientAddress;
        Socket clientSocket;
        Server server;
        Message msg = new Message();
        MySqlConnection mySqlConn;
        User user;
        Result result;
        Room room;

        public MySqlConnection MySQlConn
        {
            get
            {
                return mySqlConn;
            }
        }

        public void SetUserData(User user, Result result)
        {
            this.user = user;
            this.result = result;
        }

        public string GetUserData()
        {
            return user.id + "," + user.username + "," + result.Totalresult + "," + result.Winresult;
        }

        public int GetUserId()
        {
            return user.id;
        }

        public Room Room
        {
            set { room = value; }
            get { return room; }
        }

        public Client(Socket socket, Server server)
        {
            this.clientSocket = socket;
            this.server = server;

            this.clientAddress = clientSocket.RemoteEndPoint.ToString();
            Console.WriteLine("新用户[{0}]连接", this.clientAddress);

            mySqlConn = ConnetHelper.Connect(); // 每个用户 keep 一个 sql 连接。
        }

        public void Start()
        {
            if (clientSocket == null || !clientSocket.Connected) return;
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
        }

        void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                int count = clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    Close();
                }

                msg.ReadMessage(count, OnProcessMessage);

                Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Client ReceiveCallback]" + ex.Message);
                Close();
            }
        }

        void OnProcessMessage(RequestCode requestCode, ActionCode actionCode, string data)
        {
            Console.WriteLine("[Client - 接收] RequestCode:{0} ActionCode:{1} data:{2}",
                              requestCode,
                              actionCode,
                              data);
            server.HandleRequest(requestCode, actionCode, data, this);
        }

        void Close()
        {
            Console.WriteLine("用户[{0}]断开连接", this.clientAddress);
            ConnetHelper.CloseConnection(this.mySqlConn);

            if (clientSocket != null)
                clientSocket.Close();

            if (this.room != null)
                room.Close(this); // 用户退出检测关闭房间

            server.RemoveClient(this);

        }

        //public void Send(RequestCode requestCode, string data)
        public void Send(ActionCode actionCode, string data)
        {
            Console.WriteLine("[Client - 发送] ActionCode:{0} data:{1}",
                              actionCode,
                              data);
            byte[] bytes = Message.PackData(actionCode, data);
            clientSocket.Send(bytes);
        }

        public bool IsRoomOwner()
        {
            return room.IsRoomOwner(this);
        }

    }
}

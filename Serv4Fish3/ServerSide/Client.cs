using System;
using System.Net;
using System.Net.Sockets;
using MySql.Data.MySqlClient;
using Serv4Fish3.Tools;
using FishCommon3;
using Serv4Fish3.Model;
using Serv4Fish3.DAO;

namespace Serv4Fish3.ServerSide
{
    public class Client
    {
        public string ipaddress;
        public long LastTickTime = Util.GetTimeStamp();
        Socket clientSocket;
        Server server;
        Message msg = new Message();
        MySqlConnection mySqlConn;
        User user;
        Wallet wallet;
        // 是否是房主 // 房主是每个房间的第一个客户端[负责发鱼 和 鱼的帧同步]
        public int isMaster = 0;

        Room room;

        public MySqlConnection MySQlConn
        {
            get
            {
                return mySqlConn;
            }
        }

        public void SetUserData(User user, Wallet wallet)
        {
            this.user = user;
            this.wallet = wallet;

        }

        public string GetUserData()
        {
            if (this.user == null)
                return "";
            return user.Username + ","
                + user.Nickname + ","
                + user.Corner + ","
                + user.CannonLvCurr + ","
                + wallet.Money + ","
                + wallet.Diamond + ","
                + this.isMaster;
        }

        public int GetUserId()
        {
            return user.Id;
        }

        public User GetUser()
        {
            return user;
        }

        public Wallet GetWallet()
        {
            return wallet;
        }

        public void SaveSQL()
        {
            WalletDAO walletDAO = new WalletDAO();
            Console.WriteLine("[73]开始保存钱包数据");
            Console.WriteLine(mySqlConn);
            Console.WriteLine(wallet);
            if (wallet != null)
            {
                if (wallet != null)
                    walletDAO.UpdateWalletMoney(mySqlConn,
                                                wallet.Username,
                                                wallet.OldMoney,
                                                wallet.Money,
                                                wallet.OldDiamond,
                                                wallet.Diamond);
            }
            Console.WriteLine("[83]开始保存用户数据");
            Console.WriteLine(mySqlConn);
            Console.WriteLine(user);
            UserDAO userDAO = new UserDAO();
            if (userDAO != null)
            {
                if (user != null)
                    userDAO.UpdateUser(mySqlConn, user.Username, user.CannonLvOpen);
            }

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

            this.ipaddress = clientSocket.RemoteEndPoint.ToString();
            Console.WriteLine("[" + DateTime.Now + "] " + "新用户[{0}]连接", this.ipaddress);

            mySqlConn = ConnectHelper.Connect(); // 每个用户 keep 一个 sql 连接。
        }

        public void Start()
        {
            if (clientSocket == null || !clientSocket.Connected)
                return;

            this.IsClosed = false;

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
                //Console.WriteLine("EndReceive Count:" + count);
                msg.ReadMessage(count, OnProcessMessage);
                Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] " + "[Client ReceiveCallback] 异常 " + ex.Message);

                if (!IsClosed)
                    Close();
            }
        }

        void OnProcessMessage(RequestCode requestCode, ActionCode actionCode, string data)
        {
            /*
            Console.WriteLine("[" + DateTime.Now + "] " + "[Client - 接收 {0} ] " +
                "\n\tRequestCode: {1} " +
                "\n\tActionCode: {2} " +
                "\n\tdata: {3}",
                clientSocket.RemoteEndPoint.ToString(),
                requestCode,
                actionCode,
                data);
                */
            server.HandleRequest(requestCode, actionCode, data, this);
        }

        bool IsClosed = false; // 连接已断开，初始化时设置为 true

        void Close()
        {
            Console.WriteLine("[" + DateTime.Now + "] " + "用户[{0}]断开连接", this.ipaddress);


            if (this.room != null)
                room.QuitRoom(this); // 用户退出 检测关闭房间

            // 退出时保存钱包
            this.SaveSQL();

            // 关闭 sql 连接
            ConnectHelper.CloseConnection(this.mySqlConn);


            if (clientSocket != null)
                clientSocket.Close();

            //if (this.room != null)
            //room.QuitRoom(this); // 用户退出 检测关闭房间

            server.RemoveClient(this);

            this.IsClosed = true;

        }

        public void Send(ActionCode actionCode, string data)
        {
            //Console.WriteLine("[" + DateTime.Now + "] " + "[Client - 发送 {0}]" +
            //"\n\tActionCode: {1} " +
            //"\n\tdata: {2}",
            //clientSocket.RemoteEndPoint.ToString(),
            //actionCode,
            //data);
            byte[] bytes = Message.PackData(actionCode, data);
            clientSocket.Send(bytes);
        }

        //public void HeartBeat()
        //{
        //    //long timeNow = Util.GetTimeStamp();
        //    //if (this.LastTickTime < timeNow - 20) // 20 秒超时
        //    //{
        //    //    Console.WriteLine("心跳超时, 关闭连接");
        //    //    this.Close();
        //    //}
        //    //else // 继续发心跳
        //    //{
        //    Console.WriteLine("发出心跳: " + DateTime.Now);
        //    this.Send(ActionCode.PingFromServ, "a");
        //    //}
        //}


    }
}

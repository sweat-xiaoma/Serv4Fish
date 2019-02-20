//#define DEBUG_VIEW
using System;
using System.Diagnostics;
using System.Linq;
using Serv4Fish3.ServerSide;
using Serv4Fish3.Controller;
using Serv4Fish3.DAO;
using Serv4Fish3.Model;
using Serv4Fish3.Tools;
using MySql.Data.MySqlClient;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
//using Protobuf;
using FishCommon3;
using FishProto3;
using System.IO;
using Google.Protobuf;
using System.Diagnostics;
using Serv4Fish3.Tools.ObjectPool;

namespace Serv4Fish3
{
    class MainClass
    {

        public static void Main(string[] args)
        {
            //new EmptyClassC();
            //return;
            //Console.WriteLine(Util.GetTimeStamp());
            //return;
            //new EmptyClassA();
            //////Thread.Sleep(1000);
            ////Thread.Sleep(TimeSpan.FromSeconds(1));

            //Console.WriteLine("------------------------");
            //new EmptyClassB();


            //return;
            Console.WriteLine("------------------------");
            Console.WriteLine("PID: " + Process.GetCurrentProcess().Id);
            Console.WriteLine("------------------------");
            Console.WriteLine("------------------------");


#if DEBUG_VIEW 

            ////Stopwatch stopWatch = new Stopwatch();
            ////stopWatch.Start();

            ////for (int i = 0; i < 100000; i++)
            ////{
            ////    string guid = Guid.NewGuid().ToString();
            ////    Console.WriteLine(guid);
            ////}
            ////stopWatch.Stop();
            ////Console.WriteLine("pb用时: " + stopWatch.Elapsed + "秒");
            //return;

#endif
            Server server = new Server("0.0.0.0", 1234);
            server.Start();
            Console.WriteLine("< 输入 print 回车, 查看服务状态~ > ");
            Console.WriteLine("< 输入 clear 回车, 清除控制台~ > ");
            Console.WriteLine("< 输入 quit/exit 回车, 关闭服务~ > ");

            while (true)
            {
                string str = Console.ReadLine();
                switch (str)
                {
                    case "print":
                        Console.WriteLine("当前活跃房间数： " + server.ListRoom().Count);
                        Console.WriteLine("当前在线人数：" + server.ListClient().Count);
                        //for (int i = 0; i < max; i++)
                        foreach (Room room in server.ListRoom())
                        {
                            Console.WriteLine("room: " + room.GetHashCode());
                            //Console.WriteLine("fishCount: " + room.fishDic.Count);
                            Console.WriteLine("room index: " + room.SceneIndex);
                        }
                        break;

                    case "quit":
                    case "exit":
                        return;
                    case "clear":
                        Console.Clear();
                        break;
                }
            }
        }
    }

}

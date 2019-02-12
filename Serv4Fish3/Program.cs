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

namespace Serv4Fish3
{
    class Yao
    {

    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("------------------------");
            Console.WriteLine("PID: " + Process.GetCurrentProcess().Id);
            Console.WriteLine("------------------------");
            Console.WriteLine("------------------------");

#if DEBUG_VIEW
            //DateTime DateTime1970 = new DateTime(1970, 1, 1);
            //TimeSpan ts = DateTime.UtcNow - DateTime1970;
            //Console.WriteLine(ts.TotalSeconds);

            //TimeSpan ts2 = DateTime.Now - DateTime1970;
            //Console.WriteLine(ts2.TotalSeconds);

            //Console.WriteLine(ts.GetHashCode());
            //return Convert.ToInt64(ts.TotalSeconds);
            //return Convert.ToInt64(ts.TotalMilliseconds);

            DebugBalabala.Test();
            return;

            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();
            //for (int i = 0; i < 100000; i++)
            //{
            //    //Console.WriteLine(Util.TimeInterval1970()); /
            //    //long a = Util.TimeInterval1970();
            //    string span1 = Util.GetTimeStamp().ToString();
            //}
            //stopWatch.Stop();
            //Console.WriteLine("method1 用时: " + stopWatch.Elapsed + "秒");


            //////Console.WriteLine(Util.GetTimeStamp());

            ////Console.WriteLine(Util.TimeInterval1970());
            ////Console.WriteLine(Util.GetTimeStamp());
            ////Console.WriteLine(Util.GetTimeStamp());
            ////return;

            //Stopwatch stopWatch2 = new Stopwatch();
            //stopWatch2.Start();
            //for (int i = 0; i < 100000; i++)
            //{
            //    //Console.WriteLine(Util.GetTimeStamp()); 
            //    //long a = Util.GetTimeStamp();
            //    string span2 = Util.

            //}
            //stopWatch2.Stop();
            //Console.WriteLine("method2 用时: " + stopWatch2.Elapsed + "秒");
            //return;

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
                            Console.WriteLine("fishCount: " + room.fishDic.Count);
                        }
                        break;

                    case "quit":
                    case "exit":
                        return;
                }
            }
        }



        /**
         * 解码
         */
        static string urlsafeB64Decode(string data)
        {
            int remainder = data.Length % 4;
            int padlen = 4 - remainder;
            for (int i = 0; i < padlen; i++)
            {
                data += "=";
            }

            data = data.Replace("-", "+");
            data = data.Replace("_", "/");

            return data;
        }

        static IEnumerator Test01()
        {

            Console.WriteLine(DateTime.Now.Millisecond);


            Console.WriteLine("start Test01" + " [" + DateTime.Now + "]");
            //Console.WriteLine("start Test01" + " [" + DateTime.Now.ToLongDateString() + "]");
            //Console.WriteLine("start Test01" + " [" + DateTime.Now.ToLongTimeString() + "]");
            //Console.WriteLine("start Test01" + " [" + DateTime.Now.ToShortDateString() + "]");
            //Console.WriteLine("start Test01" + " [" + DateTime.Now.ToShortTimeString() + "]");
            //DateTime 当前时间 = DateTime.Now;
            //string 毫秒 = 当前时间.ToString(@"ss\:fff"); //显示2位秒数和秒数后面3位
            //Console.WriteLine(毫秒);

            yield return new WaitForSeconds(5);
            Console.WriteLine("等五秒" + " [" + DateTime.Now + "]");
            Console.WriteLine(DateTime.Now.Millisecond);
            yield return new WaitForSeconds(5);
            Console.WriteLine("再等五秒" + " [" + DateTime.Now + "]");
            Console.WriteLine(DateTime.Now.Millisecond);


        }

        //static IEnumerator Test02()
        //{
        //    Console.WriteLine("start test 02");
        //    yield return new WaitForFrames(500);
        //    Console.WriteLine("after 500 frames");
        //}

        static void debugSql()
        {
            //var t1 = Test01();
            //var t2 = Test02();
            //CoroutineManager.Instance.StartCoroutine(t1);
            //CoroutineManager.Instance.StartCoroutine(t2);

            CoroutineManager.Instance.StartCoroutine(Test01());

            while (true)// 模拟update
            {
                // 帧率 1000 / 20 = 50
                Thread.Sleep(Time.deltaMilliseconds);
                CoroutineManager.Instance.UpdateCoroutine();
            }

            return;


            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(GetRequireExpByLevel(i));
            }
            return;
            string guid = "V4BqJJuh7OpSoKm79ShUQg7LeUEoIkRrTd63PhlAZmHAKD_NZRkhTzvF7KhCbBHmUjVXtlZXErTVdXT8ocPak4UvCA3HYm3xt8effunyNUqgUWvBinIWwy7_8ZQQBRKHNJEbjvvk-7_uYKNbK8DGrdLFZuMXAIBai916_rnwNksF3gVVOaTekYwoTQ7rWL3UhYkIRctweiKyzgOrvT-RUoQtc5MgyJnMh9WHXzvXGMJzD2TQ8wM7meKTE_nG5pgzUsaXTUg_cIgU9LK-dkRuDrAplGruZA_vbKsFMFeR5gq4LiDXhJ6M76AtdgVkRScJRK-nNNVzY22-ZYgRxFgOzg";
            //string re = urlsafeB64Decode(guid);
            //Console.WriteLine(re);

            string re = urlsafeB64Decode(guid);

            string reutf8 = Util.DecodeBase64(Encoding.UTF8, re);
            Console.WriteLine(reutf8); ;

            string reascii = Util.DecodeBase64(Encoding.ASCII, re);
            Console.WriteLine(reascii);

            return;
        }


        static int GetRequireExpByLevel(int level)
        {
            return (int)((level - 1) * (100f + (100f + 10f * (level - 2f))) / 2);
        }
    }

}

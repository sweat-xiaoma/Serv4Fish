//#define DEBUG_VIEW
using System;
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
using Protobuf;

namespace Serv4Fish3
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            //Console.WriteLine("Hello World! 10");

            //int[] arr1 = { 10, 20, 30 };
            //int[] arr2 = { 100, 200, 300 };
            //int[] arr3 = { 7, 8, 9 };

            //int[] re = arr1.Concat(arr2).ToArray<int>()
            //               .Concat(arr3).ToArray<int>();
            //Console.WriteLine(re.Length);

            //Console.ReadLine();

            //int[] arr = { 100, 200, 300, 400 };
            //foreach (var item in arr)
            //{
            //    //UnityEngine.Debug.Log(item);
            //    Console.WriteLine(item);
            //}

            ////return;

            //string user = "a|b|c";
            //string[] strs = user.Split('|');
            //Console.WriteLine(strs[0]);
            //Console.WriteLine(strs[1]);

            //Console.WriteLine("------ 2 ------");
            //string user2 = "a|";
            //string[] strs2 = user2.Split('|');
            //Console.WriteLine(strs2[0]);

            //Console.WriteLine("------ 3 ------");
            //string user3 = "A|B";
            //string[] strs3 = user3.Split('|');
            //Console.WriteLine(strs3[0]);
            //Console.WriteLine(strs3[1]);

            //return;

#if DEBUG_VIEW
            return;

            //Dictionary<string, int> testDic = new Dictionary<string, int>();
            //testDic.Add("a", 1000);
            //testDic.Add("b", 2000);
            //string guid1 = "a";
            //if (testDic.ContainsKey(guid1))
            //{
            //    Console.WriteLine(testDic[guid1]);
            //}

            //string guid2 = "c";
            //if (testDic.ContainsKey(guid2))
            //{
            //    Console.WriteLine(testDic[guid2]);
            //}

            int a = 100;
            a -= 5;
            a -= 5;
            Console.WriteLine(a);
            return;


            //PersonMc personMc = new PersonMc();
            //Console.WriteLine(DateTime.Now.Millisecond);
            //Thread.Sleep(20);
            //Console.WriteLine(DateTime.Now.Millisecond);
            //Thread.Sleep(20);
            //Console.WriteLine(DateTime.Now.Millisecond);
            //Thread.Sleep(20);
            //Console.WriteLine(DateTime.Now.Millisecond);


            //debugSql();

            //Console.WriteLine(Guid.NewGuid());
            //Console.WriteLine(Guid.NewGuid());
            //Console.WriteLine(Guid.NewGuid());

            //Console.WriteLine(DateTime.Now.Second);
            //Console.WriteLine(DateTime.Now.Second);
            //Console.WriteLine(DateTime.Now.Second);

            //Console.WriteLine(DateTime.Now.Ticks);
            //Console.WriteLine(DateTime.Now.Ticks);
            //Console.WriteLine(DateTime.Now.Ticks);

            //Console.WriteLine(DateTime.Now.ToUniversalTime());
            //Console.WriteLine(DateTime.Now.ToUniversalTime());
            //Console.WriteLine(DateTime.Now.ToUniversalTime());

            //Console.WriteLine(DateTime.Now.ToUniversalTime().Ticks);
            //Console.WriteLine(DateTime.Now.ToUniversalTime().Ticks);
            //Console.WriteLine(DateTime.Now.ToUniversalTime().Ticks);

            return;
#endif

            //Server server = new Server("127.0.0.1", 1234);
            Server server = new Server("0.0.0.0", 1234);
            server.Start();
            //Console.ReadLine();
            while (true)
            {
                string str = Console.ReadLine();
                switch (str)
                {
                    case "print":
                        //Console.WriteLine(server.ListRoom());
                        List<Room> rooms = server.ListRoom();
                        foreach (Room room in rooms)
                        {

                        }


                        break;

                    case "quit":
                    case "exit":
                        break;
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

            //string re = Util.DecodeBase64(Encoding.UTF8, data);
            //return re;

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
            ////string[] strs = new string[4];
            //Client[] clients = new Client[4];
            //Console.WriteLine(clients[1] == null);
            //Console.WriteLine(clients[1]);
            //return;

            //string guid = "YAvIe2cQdf74sd8m3t-Ogehkvwf6E7zWx_5q4j_TNFopiUl8Tso6O1bMvwMkmQ0T_fvTc8VtHc04yXm7EGs0-_fYUW3byLxlMPmOCbAr5MH4dTFfuUW-Pz7Vt8rOQSrnh5Y4i_SA7tRreDndnSPlUy2Q3Joj3Ymogiv9GnInQnK-7HeRWWi6_eJY8WHwOasid1n0_hIPy9CvF-tHpPIKzHlDlpISGe-04FLxhBmnVXaSzcRdDZmoVDb7A1QNRXyR8QYrWtcL-gR0w2J-LM2oe4cU0hzqF8utmkGi11O2cA6KNxbnop0YiKKw0BH-2t_Cw7DivxzOfV5bHIqoSBOV2g";

            MySqlConnection connection = new MySqlConnection(ConnectHelper.CONNECTION_STRING);
            connection.Open();
            MySqlCommand command = new MySqlCommand("select username from user where guid = @guid", connection);
            command.Parameters.AddWithValue("guid", guid);

            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                int id = reader.GetInt32("id");
                string username = reader.GetString("username");
                string nickname = reader.GetString("nickname");
                User user = new User(id, username, nickname);
                Console.WriteLine(id);
                Console.WriteLine(username);
                Console.WriteLine(user.Username);
            }
            reader.Close();
            return;
        }


        static int GetRequireExpByLevel(int level)
        {
            return (int)((level - 1) * (100f + (100f + 10f * (level - 2f))) / 2);
        }
    }

}

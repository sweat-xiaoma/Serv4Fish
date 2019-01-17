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
#if DEBUG_VIEW
            return;
#endif
            //Server server = new Server("127.0.0.1", 1234);
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
                        Console.WriteLine("当钱活跃房间数： " + server.ListRoom().Count);
                        Console.WriteLine("当前在线人数：" + server.ListClient().Count);
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

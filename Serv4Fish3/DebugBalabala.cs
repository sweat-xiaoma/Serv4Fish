using System;
using System.Diagnostics;
using System.Text;
using FishProto3;
using Google.Protobuf;
using System.Timers;
using Serv4Fish3.Tools;
using System.Threading.Tasks;
using System.Threading;
using MySql.Data.MySqlClient;

namespace Serv4Fish3
{
    public class DebugBalabala
    {
        //static Timer heartbeatTimer = new Timer(1000f);

        public static void Test()
        {

            MySqlConnection conn = new MySqlConnection("Database=lgl_fish;Data Source=192.168.0.160;port=3306;user=lgl;pwd=lgl0303");
            conn.Open();

            MySqlDataReader reader = null;
            try
            {

                MySqlCommand cmd = new MySqlCommand("select * from game_fishes", conn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //string fishname = reader.GetString("name");
                    string fishname = reader.GetString("nameaaaaa");
                    Console.WriteLine(fishname);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now + "] " + "[VerifyFishStaticData] 出现异常" + ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (conn != null)
                    conn.Close();
            }







            return;


            //            //Task<Int32> t = new Task<Int32>(n => Sum((Int32)n), 1000000000);
            //            //t.Start();
            //            //Task cwt = t.ContinueWith(task => Console.WriteLine("The sum is: " + task.Result));
            //            //return;
            //            //Task.Factory.StartNew(() =>
            //            //});
            //            //Console.WriteLine("22: " + DateTime.Now);
            //            Console.WriteLine("22: " + DateTime.Now + DateTime.Now.Millisecond);

            //            /*
            ////for（int i = 0;i<100000;i++) 改为
            //Parallel.For(0, 100000, i => 
            //            */

            //            /*
            //Task<Int32> t = new Task<Int32>(n => Sum((Int32)n), 1000000000);
            ////  可以在以后某个时间启动任务
            //t.Start();
            //// ContinueWith 返回一个 Task，但一般都不再关心这个对象
            //Task cwt = t.ContinueWith(task => Console.WriteLine("The sum is: " + task.Result))
            //*/


            //Task t = new Task(() =>
            //{
            //    Thread.Sleep(1000);
            //    Console.WriteLine("26: " + DateTime.Now + DateTime.Now.Millisecond);
            //});
            //Console.WriteLine(t.Status);
            //t.Wait(1000);
            //t.Start();
            //return;



            ////Console.WriteLine();
            ////heartbeatTimer.Elapsed += HeartbeatTimer_Elapsed;
            ////heartbeatTimer.AutoReset = false;
            ////heartbeatTimer.Enabled = true;
            ////Console.WriteLine("20");
            ////return;
            ////string dic = AppDomain.CurrentDomain.BaseDirectory;
            ////Console.WriteLine(dic);
            ////Console.WriteLine(File.Exists(dic));


            ////Console.WriteLine(File.Exists("a.txt"));
            ////Console.WriteLine(File.Exists("FishCommon3.dll"));
            //////创建文件流
            ////FileStream myfs = new FileStream("路径", FileMode.打开方式);
            //////打开方式
            //////1:Create  用指定的名称创建一个新文件,如果文件已经存在则改写旧文件
            //////2:CreateNew 创建一个文件,如果文件存在会发生异常,提示文件已经存在
            //////3:Open 打开一个文件 指定的文件必须存在,否则会发生异常
            //////4:OpenOrCreate 打开一个文件,如果文件不存在则用指定的名称新建一个文件并打开它.
            //////5:Append 打开现有文件,并在文件尾部追加内容.

            //////创建写入器
            ////StreamWriter mySw = new StreamWriter(myfs);//将文件流给写入器
            ////                                           //将录入的内容写入文件
            ////mySw.Write("内容");
            //////关闭写入器
            ////mySw.Close();
            //////关闭文件流
            ////myfs.Close();

            ////Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            ////Console.WriteLine(Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory));
            ////Console.WriteLine(Path.GetFullPath("a.txt"));
            ////Console.ReadLine();
            ////return;

            ////Stopwatch stopWatch = new Stopwatch();
            ////stopWatch.Start();

            ////for (int i = 0; i < 100000; i++)
            ////{
            ////    PersonMc p1 = new PersonMc();
            ////    p1.Age = 18;
            ////    p1.Name = "TomTomTomTomTom";
            ////    p1.Hobbies.Add("吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭");
            ////    p1.Hobbies.Add("睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉");

            ////    byte[] databytes = p1.ToByteArray();
            ////    //UnityEngine.Debug.Log(databytes);

            ////    IMessage imPerson = new PersonMc();
            ////    PersonMc personDe = new PersonMc();
            ////    personDe = (PersonMc)imPerson.Descriptor.Parser.ParseFrom(databytes);
            ////    //Console.WriteLine("p1.bytearray length:" + databytes.Length);

            ////    //Console.WriteLine(personDe.Name);
            ////    int age = personDe.Age;
            ////    string name = personDe.Name;
            ////}



            ////stopWatch.Stop();
            ////Console.WriteLine("pb用时: " + stopWatch.Elapsed + "秒");




            ///*
            //10 3 
            //84 -- T
            //111 -- o 
            //109 -- m
            //16 
            //18 
            //26 6 
            //229 144 131 -- 吃
            //233 165 173 -- 饭
            //26 6 
            //231 157 161 -- 睡
            //232 167 137 -- 觉
            //*/
            ////byte[] b = { 10, 3 };
            ////byte[] b2 = { 16, 18, 26, 6 };
            ////byte[] b3 = { 26, 6 };

            ////Stopwatch stopWatch2 = new Stopwatch();
            ////stopWatch2.Start();
            ////for (int i = 0; i < 100000; i++)
            ////{
            ////    string data = "18,TomTomTomTomTom,吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭" +
            ////        "|睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉";
            ////    byte[] databytesStr = Encoding.UTF8.GetBytes(data);
            ////    //string[] strs = 

            ////    string strData = Encoding.UTF8.GetString(databytesStr);
            ////    string[] strs = strData.Split(',');
            ////    int age = int.Parse(strs[0]);
            ////    string name = strs[1];
            ////}
            //////Console.WriteLine("stringdata length:" + databytesStr.Length);
            ////stopWatch2.Stop();
            ////Console.WriteLine("byte 用时: " + stopWatch2.Elapsed + "秒");

            ///*
            //49 -- 1
            //56 -- 8
            //44 -- ,
            //84 -- T
            //111 -- o
            //109 -- m
            //44 -- ,
            //229 144 131 -- 吃
            //233 165 173 -- 饭
            //124         -- |
            //231 157 161 -- 睡
            //232 167 137 -- 觉
            //*/

            //return;



            //Thread thread = new Thread(printNumbers1);
            Thread thread = new Thread(printNumbers1WithDelay);
            thread.Start();
            //Console.WriteLine("thread.Join 183");
            //thread.Join(); // 同步等待，只有该方法返回时(或 Thread.ThreadState属性为 Aborted),
            // 才表示线程真正结束了
            //Console.WriteLine("thread.Join 185");
            //Thread.Sleep(TimeSpan.FromSeconds(6));
            //thread.Abort(); // 向线程抛出一个 ThreadAbortException 异常, 

            //Console.WriteLine("abort");
            /////*
            //thread.Abort();
            //Console.WriteLine("aborted");
            //while(thread.ThreadState!= ThreadState.)
            //ThreadState.
            //Console.WriteLine(thread.ThreadState);

            //Console.WriteLine((int)System..ThreadState.Initialized);
            Console.WriteLine((int)System.Threading.ThreadState.Aborted);

            //thread.ThreadState

            Console.WriteLine("state1:  " + thread.ThreadState + DateTime.Now);


            Thread.Sleep(TimeSpan.FromSeconds(1));
            Console.WriteLine("state2:  " + thread.ThreadState + DateTime.Now);
            //ThreadState.Terminated

            Thread.Sleep(TimeSpan.FromSeconds(30));
            Console.WriteLine("state3:  " + thread.ThreadState + DateTime.Now);


            //printNumbers1();
            Console.WriteLine("EndTest " + DateTime.Now);
            Console.ReadLine();


        }


        static void printNumbers1()
        {
            Console.WriteLine("Starting" + DateTime.Now);
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(i);
            }
        }


        static void printNumbers1WithDelay()
        {
            Console.WriteLine("Starting... withDelay" + DateTime.Now);
            for (int i = 0; i < 100; i++)
            {
                //Thread.Sleep(TimeSpan.FromSeconds(2)); // 暂停两秒
                Thread.Sleep(TimeSpan.FromMilliseconds(1));
                Console.WriteLine(i + "   " + DateTime.Now + ":" + DateTime.Now.Millisecond);
            }
        }


    }
}

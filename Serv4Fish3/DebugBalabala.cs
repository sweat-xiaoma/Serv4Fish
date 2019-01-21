using System;
using System.Diagnostics;
using System.Text;
using FishProto3;
using Google.Protobuf;
using System.Timers;
using Serv4Fish3.Tools;

namespace Serv4Fish3
{
    public class DebugBalabala
    {
        static Timer heartbeatTimer = new Timer(1000f);

        public static void Test()
        {

            //Console.WriteLine();
            //heartbeatTimer.Elapsed += HeartbeatTimer_Elapsed;
            //heartbeatTimer.AutoReset = false;
            //heartbeatTimer.Enabled = true;
            //Console.WriteLine("20");



            return;
            //string dic = AppDomain.CurrentDomain.BaseDirectory;
            //Console.WriteLine(dic);
            //Console.WriteLine(File.Exists(dic));


            //Console.WriteLine(File.Exists("a.txt"));
            //Console.WriteLine(File.Exists("FishCommon3.dll"));
            ////创建文件流
            //FileStream myfs = new FileStream("路径", FileMode.打开方式);
            ////打开方式
            ////1:Create  用指定的名称创建一个新文件,如果文件已经存在则改写旧文件
            ////2:CreateNew 创建一个文件,如果文件存在会发生异常,提示文件已经存在
            ////3:Open 打开一个文件 指定的文件必须存在,否则会发生异常
            ////4:OpenOrCreate 打开一个文件,如果文件不存在则用指定的名称新建一个文件并打开它.
            ////5:Append 打开现有文件,并在文件尾部追加内容.

            ////创建写入器
            //StreamWriter mySw = new StreamWriter(myfs);//将文件流给写入器
            //                                           //将录入的内容写入文件
            //mySw.Write("内容");
            ////关闭写入器
            //mySw.Close();
            ////关闭文件流
            //myfs.Close();

            //Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            //Console.WriteLine(Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory));
            //Console.WriteLine(Path.GetFullPath("a.txt"));
            //Console.ReadLine();
            //return;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            for (int i = 0; i < 100000; i++)
            {
                PersonMc p1 = new PersonMc();
                p1.Age = 18;
                p1.Name = "TomTomTomTomTom";
                p1.Hobbies.Add("吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭");
                p1.Hobbies.Add("睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉");

                byte[] databytes = p1.ToByteArray();
                //UnityEngine.Debug.Log(databytes);

                IMessage imPerson = new PersonMc();
                PersonMc personDe = new PersonMc();
                personDe = (PersonMc)imPerson.Descriptor.Parser.ParseFrom(databytes);
                //Console.WriteLine("p1.bytearray length:" + databytes.Length);

                //Console.WriteLine(personDe.Name);
                int age = personDe.Age;
                string name = personDe.Name;
            }



            stopWatch.Stop();
            Console.WriteLine("pb用时: " + stopWatch.Elapsed + "秒");




            /*
            10 3 
            84 -- T
            111 -- o 
            109 -- m
            16 
            18 
            26 6 
            229 144 131 -- 吃
            233 165 173 -- 饭
            26 6 
            231 157 161 -- 睡
            232 167 137 -- 觉
            */
            //byte[] b = { 10, 3 };
            //byte[] b2 = { 16, 18, 26, 6 };
            //byte[] b3 = { 26, 6 };

            Stopwatch stopWatch2 = new Stopwatch();
            stopWatch2.Start();
            for (int i = 0; i < 100000; i++)
            {
                string data = "18,TomTomTomTomTom,吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭吃饭" +
                    "|睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉睡觉";
                byte[] databytesStr = Encoding.UTF8.GetBytes(data);
                //string[] strs = 

                string strData = Encoding.UTF8.GetString(databytesStr);
                string[] strs = strData.Split(',');
                int age = int.Parse(strs[0]);
                string name = strs[1];
            }
            //Console.WriteLine("stringdata length:" + databytesStr.Length);
            stopWatch2.Stop();
            Console.WriteLine("byte 用时: " + stopWatch2.Elapsed + "秒");

            /*
            49 -- 1
            56 -- 8
            44 -- ,
            84 -- T
            111 -- o
            109 -- m
            44 -- ,
            229 144 131 -- 吃
            233 165 173 -- 饭
            124         -- |
            231 157 161 -- 睡
            232 167 137 -- 觉
            */



            return;
        }

        static void HeartbeatTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // 处理心跳
            HeartBeat();
            heartbeatTimer.Start();
        }

        static void HeartBeat()
        {
            Console.WriteLine("Now: " + DateTime.Now);
        }

    }
}

//#define ST
using System;
using System.Collections.Concurrent;
using System.Threading;
using Serv4Fish3.Tools;
using System.Collections;
using System.Collections.Generic;


namespace Serv4Fish3
{
    public class EmptyClassC
    {
#if ST
        Dictionary<string, string> fishDic = new Dictionary<string, string>();
#else
        ConcurrentDictionary<string, string> fishCDic = new ConcurrentDictionary<string, string>();
#endif
        public EmptyClassC()
        {
            Thread thread = new Thread(genFishWithDelay);
            thread.Start();
            Console.WriteLine("测试插入 thread 开启...");


            Thread thread2 = new Thread(removeFishWithDelay);
            thread2.Start();
            Console.WriteLine("测试remove thread 开启...");


            while (true)
            {
                //string str = Console.ReadLine();
                switch (Console.ReadLine())
                {
                    case "quit":
#if ST
                        Console.WriteLine("关闭数量: " + this.fishDic.Count);
                        foreach (KeyValuePair<string, string> pair in this.fishDic)
                        {
                            Console.WriteLine(pair.Key + " = " + pair.Value);
                        }
#else
                        Console.WriteLine("关闭数量: " + this.fishCDic.Count);
                        foreach (KeyValuePair<string, string> pair in this.fishCDic)
                        {
                            Console.WriteLine(pair.Key + " = " + pair.Value);
                        }
#endif
                        //break;
                        return;
                }
            }
            //Console.ReadLine();
        }


        int a = 0;

        void genFishWithDelay()
        {
            while (true)
            {
                Thread.Sleep(TimeSpan.FromSeconds(0.1f));
                Console.WriteLine("--- gen: " + Util.GetTimeStampMs);

                a++;
#if ST
                try
                {
                    this.fishDic.Add("a" + a, "value" + a);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("添加 异常: " + ex.Message + " stacktrace:" + ex.StackTrace);
                }
#else
                if (this.fishCDic.TryAdd("a" + a, "value" + a))
                {
                    Console.WriteLine("37 Add key: " + a);
                }
                else
                {
                    Console.WriteLine("添加失败!");
                }
#endif
            }
        }


        void removeFishWithDelay()
        {
            while (true)
            {
                Thread.Sleep(TimeSpan.FromSeconds(0.1f));
                Console.WriteLine("=== removeFishWithDelay: " + Util.GetTimeStampMs);

#if ST
                try
                {
                    foreach (string key in this.fishDic.Keys)
                    {
                        fishDic.Remove(key);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("删除 异常: " + ex.Message + " stacktrace:" + ex.StackTrace);
                }
#else
                foreach (string key in this.fishCDic.Keys)
                {
                    if (fishCDic.TryRemove(key, out string a))
                    {
                        Console.WriteLine("43: key: " + key + " value: " + a);
                    }
                    else
                    {
                        Console.WriteLine("移除失败!");
                    }
                }
#endif
            }
        }


    }
}


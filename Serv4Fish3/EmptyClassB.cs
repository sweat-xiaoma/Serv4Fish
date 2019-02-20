using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Serv4Fish3
{
    public class EmptyClassB
    {

        //todo test start
        static int GetLogger(string cmdId)
        {
            if (!cDic.ContainsKey(cmdId))
            {
                Random random = new Random();
                cDic.TryAdd(cmdId, random.Next(0, 1000));
            }
            return cDic[cmdId];
        }
        static ConcurrentDictionary<string, int> cDic;
        //todo test end

        public EmptyClassB()
        {
            //todo test start
            cDic = new ConcurrentDictionary<string, int>();
            for (int i = 0; i < 100; i++)
            {
                ThreadPool.QueueUserWorkItem(obj =>
                {
                    try
                    {
                        int logger = GetLogger("AAA" + i);
                        Console.WriteLine("i: " + i + "  B: " + logger);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("B - 第{0}个线程出现问题: " + ex.Message, obj));
                    }

                }, i);

                //ThreadPool.QueueUserWorkItem
            }

            //ThreadPool.QueueUserWorkItem((obj) =>
            //{

            //});
            Console.ReadKey();
            //todo test end
        }
    }
}

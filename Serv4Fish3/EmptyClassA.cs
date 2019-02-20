using System;
using System.Collections.Generic;
using System.Threading;

namespace Serv4Fish3
{
    public class EmptyClassA
    {

        //todo test start
        static int GetLogger(string cmdId)
        {
            if (!dic.ContainsKey(cmdId))
            {
                Random random = new Random();
                dic.Add(cmdId, random.Next(0, 1000));
            }
            return dic[cmdId];
        }
        static Dictionary<string, int> dic;
        //todo test end

        public EmptyClassA()
        {
            //todo test start
            dic = new Dictionary<string, int>();
            for (int i = 0; i < 100; i++)
            {
                ThreadPool.QueueUserWorkItem(obj =>
                {
                    try
                    {
                        int logger = GetLogger("AAA" + i);
                        //Console.WriteLine("A: " + logger);
                        Console.WriteLine("i: " + i + "  A: " + logger);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("A - 第{0}个线程出现问题: " + ex.Message, obj));
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

using System;
using System.Linq;
using Serv4Fish3.ServerSide;

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

            Server server = new Server("127.0.0.1", 1234);
            server.Start();
            Console.ReadLine();
        }
    }
}

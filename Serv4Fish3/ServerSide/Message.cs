using System;
using System.Text;
using System.Linq;
//using Serv4Fish.Common_framework_2_0;
using FishCommon3;

namespace Serv4Fish3.ServerSide
{
    public class Message
    {
        //public Message()
        //{
        //}

        //byte[] data = new byte[1024];
        byte[] data = new byte[10240];
        int startIndex = 0;

        public byte[] Data
        {
            get
            {
                return data;
            }
        }

        public int StartIndex
        {
            get
            {
                return startIndex;
            }
        }

        public int RemainSize
        {
            get
            {
                return data.Length - startIndex;
            }
        }

        //void AddCount(int count)
        //{
        //    startIndex += count;
        //}

        public void ReadMessage(int newDataAmount, Action<RequestCode, ActionCode, string> processDataCallback)
        {
            startIndex += newDataAmount;

            //Console.WriteLine("读取接收 长度:" + newDataAmount);

            while (true)
            {
                if (startIndex <= 4) return; // 长度字节都不完整
                int count = BitConverter.ToInt32(data, 0); // 数据长度
                if ((startIndex - 4) >= count) // 数据是否完整
                {
                    //string s = Encoding.UTF8.GetString(data, 4, count); // 0 - 3 是长度字节，从 4 开始
                    //Console.WriteLine("解析出来数据: " + s);

                    // 1) 解析 RequestCode (int32)
                    RequestCode requestCode = (RequestCode)BitConverter.ToInt32(data, 4);
                    // 2) 解析 ActionCode (int32)
                    ActionCode actionCode = (ActionCode)BitConverter.ToInt32(data, 8);
                    // 3) 解析数据
                    string strData = Encoding.UTF8.GetString(data, 12, count - 8);



                    //Console.WriteLine("[" + DateTime.Now + "] " +
                    //    "[socket] [读取] Request: {0}  Action: {1}  data: {2}",
                    //    requestCode,
                    //    actionCode,
                    //    strData
                    //);

                    processDataCallback(requestCode, actionCode, strData);

                    //if (newDataAmount > 100)
                    //if (actionCode == ActionCode.FishGenerate)
                    //{
                    //    //Console.WriteLine("长数据: " + strData);
                    //    Console.WriteLine("生成: " + strData);
                    //}

                    // 把已解析完成的数据抛弃，把剩余的数据往前移动
                    Array.Copy(data, count + 4, data, 0, startIndex - 4 - count);
                    startIndex -= (count + 4);
                }
                else
                {
                    // 数据不足长度
                    return;
                }
            }
        }

        // 服务端打包给客户端
        //public static byte[] PackData(RequestCode requestCode, string data)
        public static byte[] PackData(ActionCode actionCode, string data)
        {
            //byte[] requestCodeBytes = BitConverter.GetBytes((int)requestCode);
            byte[] actionCodeBytes = BitConverter.GetBytes((int)actionCode);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int amount = actionCodeBytes.Length + dataBytes.Length;
            byte[] amountBytes = BitConverter.GetBytes(amount);

            return amountBytes.Concat(actionCodeBytes).ToArray<byte>().Concat(dataBytes).ToArray<byte>();
        }
    }
}

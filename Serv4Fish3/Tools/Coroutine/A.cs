using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;

/// <summary>
/// 等待接口
/// </summary>
public interface IWait
{
    /// <summary>
    /// 每帧检测是否等待结束
    /// </summary>
    /// <returns></returns>
    bool Tick();
}

/// <summary>
/// 按秒等待
/// </summary>
public class WaitForSeconds : IWait
{
    float _seconds = 0f;

    public WaitForSeconds(float seconds)
    {
        _seconds = seconds;
    }

    public bool Tick()
    {
        _seconds -= Time.deltaTime;
        return _seconds <= 0;
    }
}


/// <summary>
/// 按帧等待
/// </summary>
public class WaitForFrames : IWait
{
    private int _frames = 0;
    public WaitForFrames(int frames)
    {
        _frames = frames;
    }

    public bool Tick()
    {
        _frames -= 1;
        return _frames <= 0;
    }
}

public class CoroutineManager
{
    private static CoroutineManager _instance = null;
    public static CoroutineManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CoroutineManager();
            }
            return _instance;
        }
    }

    private LinkedList<IEnumerator> coroutineList = new LinkedList<IEnumerator>();

    public void StartCoroutine(IEnumerator ie)
    {
        coroutineList.AddLast(ie);
    }

    public void StopCoroutine(IEnumerator ie)
    {
        try
        {
            coroutineList.Remove(ie);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public void UpdateCoroutine()
    {
        var node = coroutineList.First;
        while (node != null)
        {
            IEnumerator ie = node.Value;
            bool ret = true;
            if (ie.Current is IWait)
            {
                IWait wait = (IWait)ie.Current;
                //检测等待条件，条件满足，跳到迭代器的下一元素 （IEnumerator方法里的下一个yield）
                if (wait.Tick())
                {
                    ret = ie.MoveNext();
                }
            }
            else
            {
                ret = ie.MoveNext();
            }
            //迭代器没有下一个元素了，删除迭代器（IEnumerator方法执行结束）
            if (!ret)
            {
                coroutineList.Remove(node);
            }
            //下一个迭代器
            node = node.Next;
        }
    }
}


public class Time
{
    //每帧时间(秒)
    public static float deltaTime
    {
        get
        {
            return (float)deltaMilliseconds / 1000;
        }
    }
    //每帧时间（毫秒）
    public static int deltaMilliseconds
    {
        get
        {
            return 20;
        }
    }
}
//using System;
//using System.Threading;

//namespace MultithreadingApplication
//{
//    class ThreadCreationProgram
//    {
//        public static void CallToChildThread()
//        {
//            try
//            {

//                Console.WriteLine("Child thread starts");
//                // 计数到 10
//                for (int counter = 0; counter <= 10; counter++)
//                {
//                    Thread.Sleep(500);
//                    Console.WriteLine(counter);
//                }
//                Console.WriteLine("Child Thread Completed");

//            }
//            catch (ThreadAbortException e)
//            {
//                Console.WriteLine("Thread Abort Exception");
//            }
//            finally
//            {
//                Console.WriteLine("Couldn't catch the Thread Exception");
//            }

//        }

//        static void Main(string[] args)
//        {
//            ThreadStart childref = new ThreadStart(CallToChildThread);
//            Console.WriteLine("In Main: Creating the Child thread");
//            Thread childThread = new Thread(childref);
//            childThread.Start();
//            // 停止主线程一段时间
//            Thread.Sleep(2000);
//            // 现在中止子线程
//            Console.WriteLine("In Main: Aborting the Child thread");
//            childThread.Abort();
//            Console.ReadKey();
//        }
//    }
//}

//public class DataUtils
//{
//    public static byte[] ObjectToBytes<T>(T instance)
//    {
//        try
//        {
//            byte[] array;
//            if (instance == null)
//            {
//                array = new byte[0];
//            }
//            else
//            {
//                MemoryStream memoryStream = new MemoryStream();
//                Serializer.Serialize(memoryStream, instance);
//                array = new byte[memoryStream.Length];
//                memoryStream.Position = 0L;
//                memoryStream.Read(array, 0, array.Length);
//                memoryStream.Dispose();
//            }

//            return array;

//        }
//        catch (Exception ex)
//        {

//            return new byte[0];
//        }
//    }

//    public static T BytesToObject<T>(byte[] bytesData, int offset, int length)
//    {
//        if (bytesData.Length == 0)
//        {
//            return default(T);
//        }
//        try
//        {
//            MemoryStream memoryStream = new MemoryStream();
//            memoryStream.Write(bytesData, 0, bytesData.Length);
//            memoryStream.Position = 0L;
//            T result = Serializer.Deserialize<T>(memoryStream);
//            memoryStream.Dispose();
//            return result;
//        }
//        catch (Exception ex)
//        {
//            return default(T);
//        }
//    }
//}

//[ProtoContract]
//public class Test
//{
//    [ProtoMember(1)]
//    public string S { get; set; }

//    [ProtoMember(2)]
//    public string Type { get; set; }

//    [ProtoMember(3)]
//    public int I { get; set; }

//    /// <summary>
//    /// 默认构造函数必须有，否则反序列化会报 No parameterless constructor found for x 错误！
//    /// </summary>
//    public Test() { }

//    public static Test Data => new Test
//    {
//        I = 222,
//        S = "xxxxxx",
//        Type = "打开的封口费"
//    };
//}
////}
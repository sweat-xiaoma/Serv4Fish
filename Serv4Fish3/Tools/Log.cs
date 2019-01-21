using System;
using System.IO;
using System.Text;

namespace Serv4Fish3.Tools
{
    public class Log
    {

        public static void ReadLog()
        {
            try
            {

                // 读取日志 输出
                FileStream myfs = new FileStream("a.txt", FileMode.Open);
                //FileStream myfs = new FileStream("a.txt", FileMode.OpenOrCreate);

                //File.Exists("");
                //myfs.is
                //my
                byte[] bytes = new byte[myfs.Length];
                int count = myfs.Read(bytes, 0, bytes.Length);
                string readStr = Encoding.UTF8.GetString(bytes, 0, count);
                Console.WriteLine(readStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine("读 log 失败，exception: " + ex.Message);
            }
        }

        public static void Write()
        {
            try
            {
                FileStream myfs = new FileStream("a.txt", FileMode.Append);
                ////FileStream myfs = new FileStream("a.txt", FileMode.OpenOrCreate);

                ////File.Exists("");
                ////myfs.is
                ////my
                //byte[] bytes = new byte[myfs.Length];
                //int count = myfs.Read(bytes, 0, bytes.Length);
                //string readStr = Encoding.UTF8.GetString(bytes, 0, count);
                //Console.WriteLine(readStr);

                StreamWriter mysw = new StreamWriter(myfs);

            }
            catch (Exception ex)
            {
                Console.WriteLine("[Write Log] 异常，exception: " + ex.Message);
            }
        }
    }
}





















//using system;
//using system.collections.generic;
//using system.linq;
//using system.text;
//using system.io;

//namespace pathtest
//{
//    class program
//    {
//        static void main(string[] args)
//        {
//            //使用appdomain获取当前应用程序集的执行目录
//            string dir = appdomain.currentdomain.basedirectory;
//            string info = string.format("appdomain方法获取当前程序集目录：{0}", dir);
//            console.writeline(info);
//            //使用path获取当前应用程序集的执行的上级目录
//            dir = path.getfullpath("..");
//            info = string.format("path方法获取当前程序集上级目录：{0}", dir); (www.111cn.net)
//console.writeline(info);
//            //使用path获取当前应用程序集的执行目录的上级的上级目录
//            dir = path.getfullpath(@"....");
//            info = string.format("path方法获取当前程序集目录的级的上级目录：{0}", dir);
//            console.writeline(info);
//            //使用path获取当前应用程序集的执行目录的上级目录
//            dir = path.getfullpath(@"......");
//            info = string.format("path方法获取当前程序集目录的上级目录的上级目录：{0}", dir);
//            console.writeline(info);
//            //在当前程序集目录中添加指定目录
//            dir = path.getfullpath(@"io");
//            info = string.format("在当前程序集目录中添加指定目录：{0}", dir);
//            console.writeline(info);
//            console.read();
//        }
//    }
//}


















//读操作
////1.创建文件流
//FileStream fsRead =new FileStream("1.txt", FileMode.Open);
////2.创建缓冲区，正常情况下，是不会直接等于文件大小的。这里只有读，所以就这么干了。
//byte[] bytes = new byte[fsRead.Length];
////3.开始读取， 返回值是读取到的长度。
//int r = fsRead.Read(bytes, 0, bytes.Lenght);
////4.关闭释放流
//fsRead.Close();
//fsRead.Dispose();
//写操作
////1.创建写入的文件流
//FileStream fsWrite fsWrite = new FileStream(@"xxx", FileMode.OpenOrCreate);
////2.创建缓冲区
//String msg = "HelloWorld";
//byte[] bytes = Enconding.UTF8.GetBytes(msg);
////3.开始写入
//fsWrite.Write(bytes,0,bytes.Length);
////4.关闭
//fsWrite.Close();
//fsWrite.Dispose();
//byte数组与string之间的转换
///*在文件流写入的时候，经常需要string 和 byte数组之间的转换。
//   这里简单的描述一下，这方面的做法。*/
//1.string 到 byte[] 数组。
//string msg = "HelloWorld";
////使用UTF8编码
//byte[] bytes = System.Text.Encoding.UTF8.GetByte(msg);
////使用系统默认编码
//byte[] bytes = System.Text.Encoding.Default.GetByte(msg);

//2.byte[] 到string
//string newMsg = System.Text.Encoding.UTF8.GetString(bytes);
//编码问题
//为什么中文会乱码？
//UTF8 编码中，一个中文字符占用两个字节。
//GBK 编码中，一个中文字符占用三个字节。
//UTF8 编码中，用两个字节保存一个汉字，如果你用GBK读取，按照三个字节一个字的格式去读。当然乱码了。反之也是一样的。
//总结起来，无论是36码的鞋子，穿在50码的脚丫子上。还是36码的脚丫子，穿50码的鞋。看起来都不会很舒服。
//所以，按照什么格式写入，就按照什么格式读取。才是正解。

//PS:1.Utf8是国际标准。
//     2.GB2312 是国标编码，支持中文的。
//     3.GBK是对GB2312的扩展，支持繁体中文。
//什么类可以Dispose()?
//1.Dispose()表示释放资源，.NET中对Dispose()有一个统一的约定或者叫描述。这个约定表现为一个接口。
//或者说这个接口，是一个红头文件，红头文件中约定了如何释放资源。
//所有实现了IDisposable接口的类都可以释放，可以Dispose();
//那么类库中什么样的类会实现IDisposable接口呢？
//我的理解是这样的，一般仅占用托管堆中内存资源的类或对象。一般不需要Dispose()。垃圾回收就搞定了。
//但是对于，文件句柄，网络端口号，数据库连接等，CLR的垃圾回收机制是不管的。
//所以一般这部分内容需要实现IDisposable接口。
//文件流操作的异常处理
////只有把fs定义在这里，finally中才能引用得到。
//FileStream fs =null;
//try
//{
//     fs =new FileStream(@"文件路径", FileMode.Create);
//byte[] bytes = Encoding.Default.GetBytes("HelloWorld");
//fs.Write(bytes,0,byte.Length);
//}
//finally
//{
//     if(fs != null)  //如果fs未赋值，那么直接Dispose就会引发空指针异常。
//     {
//         fs.Dispose();
//     }
//}
//简化上述的写法，虽然严谨但是稍微有点麻烦。Microsoft提供了语法糖。
//就是using的语法
//using(某个可以释放资源的类)
//{
//      操作
//}
////1.操作执行完，会自动释放。
////2.using语句编译完成以后，会形成跟上面类似的代码。就是使用try  finally。
//StreamWriter和StreamReader
////按行写入
//StreamWriter sw =new StreamWriter(@"target",true, Encoding.GetEnconding("GB2312"));
//sw.WriteLine("HelloWorld");

////按行读取
//StreamReader sr = new StreamReader(@"Source");
//sr.ReaderLine();  //每次返回一个字符串
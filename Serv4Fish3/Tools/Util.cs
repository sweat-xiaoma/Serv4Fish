﻿using System;
using System.Text;

namespace Serv4Fish3.Tools
{
    public class Util
    {
        /// <summary>
        /// Encodes the base64.
        /// string val1 = EncodeBase64(Encoding.UTF8, "你好鸭"); // 5L2g5aW96bit
        /// </summary>
        //public static string EncodeBase64(string code_type, string code)
        public static string EncodeBase64(Encoding encoding, string code)
        {
            string encode = "";
            //byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code);
            byte[] bytes = encoding.GetBytes(code);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = code;
            }
            return encode;
        }


        /// <summary>
        /// Decodes the base64.
        /// string val2 = DecodeBase64("utf-8", "5L2g5aW96bit"); // 你好鸭
        /// </summary>
        public static string DecodeBase64(Encoding encoding, string code)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(code);
            try
            {
                //decode = Encoding.GetEncoding(code_type).GetString(bytes);
                decode = encoding.GetString(bytes);
            }
            catch
            {
                decode = code;
            }
            return decode;
        }
    }
}
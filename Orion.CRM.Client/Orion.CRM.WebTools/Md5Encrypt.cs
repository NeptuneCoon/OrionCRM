using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Orion.CRM.WebTools
{
    public class Md5Encrypt
    {
        /// <summary>  
        /// 16位Md5加密
        /// </summary>  
        /// <param name="str"></param>
        /// <returns></returns>  
        //public static string Md5Bit16(string str)
        //{
        //    MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        //    return BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(str)), 4, 8).Replace("-", "").ToLower();
        //}

        /// <summary>
        /// 32位Md5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Md5Bit32(string str)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] bytStr = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            string encryptStr = "";
            for (int i = 0; i < bytStr.Length; i++)
            {
                encryptStr = encryptStr + bytStr[i].ToString("x").PadLeft(2, '0');
            }
            return encryptStr;
        }

    }
}

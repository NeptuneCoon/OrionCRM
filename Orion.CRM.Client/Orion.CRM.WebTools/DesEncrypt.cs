using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Orion.CRM.WebTools
{
    /// <summary>
    /// DES加密解密
    /// </summary>
    public class DesEncrypt
    {
        // 默认密钥向量 
        private static byte[] Keys = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

        /// <summary> 
        /// DES加密
        /// </summary> 
        /// <param name="encryptString">待加密的字符串</param> 
        /// <param name="encryptKey">密钥，要求必须为16位</param> 
        /// <returns>加密后的字符串</returns> 
        public static string Encrypt(string encryptString, string encryptKey)
        {
            if (string.IsNullOrEmpty(encryptString)) {
                throw new ArgumentNullException("encryptString");
            }
            if (string.IsNullOrEmpty(encryptKey)) {
                throw new ArgumentNullException("encryptKey");
            }
            else if (encryptKey.Length != 16) {
                throw new Exception("The length of encryptKey must be 16.");
            }

            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 16));
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            var dcsp = Aes.Create();
            using (MemoryStream memoryStream = new MemoryStream()) {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, dcsp.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write)) {
                    cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                    cryptoStream.FlushFinalBlock();
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }

        /// <summary> 
        /// DES解密字符串 
        /// </summary> 
        /// <param name="decryptString">待解密的字符串</param> 
        /// <param name="decryptKey">密钥，要求必须为16位</param> 
        /// <returns>解密后的字符串</returns> 
        public static string Decrypt(string decryptString, string decryptKey)
        {
            if (string.IsNullOrEmpty(decryptString)) {
                throw new ArgumentNullException("decryptString");
            }
            if (string.IsNullOrEmpty(decryptKey)) {
                throw new ArgumentNullException("decryptKey");
            }
            else if (decryptKey.Length != 16) {
                throw new Exception("The length of decryptKey must be 16.");
            }

            byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 16));
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            var dcsp = Aes.Create();
            using (MemoryStream memoryStream = new MemoryStream()) {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, dcsp.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write)) {
                    Byte[] inputByteArrays = new byte[inputByteArray.Length];
                    cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                    cryptoStream.FlushFinalBlock();
                    return Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
        }
    }
}

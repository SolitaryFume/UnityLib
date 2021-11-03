using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace UnityLib.Cryptography
{
    public class AESHelp
    {
        private static byte[] salt = Encoding.UTF8.GetBytes("一骑孤烟");

        /// <summary>
        /// AEC(RijndaelManaged)加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] input, string key)
        {
            MemoryStream mStream = new MemoryStream();
            RijndaelManaged aes = new RijndaelManaged();
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(key, salt);
            var bKey = pdb.GetBytes(32);
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 128;
            aes.Key = bKey;
            CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            try
            {
                cryptoStream.Write(input, 0, input.Length);
                cryptoStream.FlushFinalBlock();
                return mStream.ToArray();
            }
            finally
            {
                cryptoStream.Close();
                mStream.Close();
                aes.Clear();
            }
        }

        /// <summary>
        ///  AEC(RijndaelManaged)解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] input, string key)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(key, salt);
            var bKey = pdb.GetBytes(32);

            MemoryStream mStream = new MemoryStream(input);
            RijndaelManaged aes = new RijndaelManaged();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 128;
            aes.Key = bKey;

            CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            try
            {
                byte[] tmp = new byte[input.Length + 32];
                int len = cryptoStream.Read(tmp, 0, input.Length + 32);
                byte[] ret = new byte[len];
                Array.Copy(tmp, 0, ret, 0, len);
                return ret;
            }
            finally
            {
                cryptoStream.Close();
                mStream.Close();
                aes.Clear();
            }
        }


        /// <summary>
        /// AEC(RijndaelManaged)加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encrypt(string input, string key)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(input), key));
        }

        /// <summary>
        ///  AEC(RijndaelManaged)解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Decrypt(string input, string key)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(input), key));
        }
    }
}
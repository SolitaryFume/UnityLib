using System.Security.Cryptography;
using System;
using System.Text;

namespace UnityLib.Cryptography
{
    public class MD5Help
    {
        public static string GetMD5(byte[] input)
        {
            using (var md5Hash = MD5.Create())
            {
                var hashbyte = md5Hash.ComputeHash(input);
                var hash = BitConverter.ToString(hashbyte).Replace("-", string.Empty);
                return hash;
            }
        }

        public static string GetMD5(string input)
        {
            var data = Encoding.UTF8.GetBytes(input);
            return GetMD5(data);
        }

        public static string GetFileMD5(String filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }
            var data = System.IO.File.ReadAllBytes(filePath);
            return GetMD5(data);
        }
    }
}
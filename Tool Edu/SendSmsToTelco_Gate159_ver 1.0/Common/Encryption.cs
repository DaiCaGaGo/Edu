using System;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public class Encryption
    {
        public string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }

        /// <summary>
        /// Gets the sha hash.
        /// </summary>
        /// <param name="shahash1">The shahash1.</param>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public string GetShaHash(SHA1 shahash1, string input)
        {
            byte[] data = shahash1.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(data);
        }
    }
}
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace LxSDK.Library
{
    public class LxAESHandler
    {

        private byte[] secretKey;
        private byte[] iv;
        private long bt;
        TripleDESCryptoServiceProvider TDES;

        public LxAESHandler()
        {
            TDES = new TripleDESCryptoServiceProvider();
            //TDES.BlockSize = 16;
            //TDES.FeedbackSize = 16;
            DateTime timeStamp = new DateTime(1970, 1, 1);  //得到1970年的时间戳
            this.bt = (DateTime.UtcNow.Ticks - timeStamp.Ticks) / 10000;  //注意这里有时区问题，用now就要减掉8个小时      
            this.secretKey = key_generator();
            // this.iv = TDES.IV;
            this.iv = UTF8Encoding.UTF8.GetBytes("1234567812345678");
        }

        public LxAESHandler(string key)
        {
            JObject o = JObject.Parse(key);
            this.secretKey = UTF8Encoding.Unicode.GetBytes(Base64DecodeKey(((string)o["key"])));
            this.iv = UTF8Encoding.UTF8.GetBytes(Base64Decode(((string)o["iv"])));
            this.bt = (long)o["bt"];
        }


        public string Encrypt(string toEncrypt)
        {
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            using (RijndaelManaged rDel = new RijndaelManaged())
            {
                rDel.BlockSize = 128;
                rDel.KeySize = 256;
                rDel.Key = this.secretKey;
                rDel.IV = this.iv;
                rDel.Mode = CipherMode.CBC;
                rDel.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = rDel.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
        }

        public string Decrypt(string toDecrypt)
        {
            byte[] toDncryptArray = Convert.FromBase64String(toDecrypt);
            using (RijndaelManaged rDel = new RijndaelManaged())
            {
                rDel.Key = this.secretKey;
                rDel.IV = this.iv;
                rDel.Mode = CipherMode.CBC;
                rDel.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = rDel.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toDncryptArray, 0, toDncryptArray.Length);
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
        }
        private byte[] key_generator()
        {
            return System.Convert.FromBase64String(GetRnd(22) + "==");
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Base64EncodeKey(string plainText)//因为一些很奇怪的问题所以key要用unicode的编码
        {
            var plainTextBytes = System.Text.Encoding.Unicode.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64DecodeKey(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.Unicode.GetString(base64EncodedBytes);
        }
        //随机字符串生成器的主要功能如下： 
        //1、支持自定义字符串长度
        //2、支持自定义是否包含数字
        //3、支持自定义是否包含小写字母
        //4、支持自定义是否包含大写字母
        //5、支持自定义是否包含特殊符号
        //6、支持自定义字符集

        ///<summary>
        ///生成随机字符串
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<returns>指定长度的随机字符串</returns>
        private string GetRnd(int length)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string str = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string s = "";

            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }

        public string toString()
        {
            JObject aesmap = new JObject();
            aesmap["key"] = System.Convert.ToBase64String(this.secretKey);
            aesmap["iv"] = System.Convert.ToBase64String(this.iv);
            aesmap["bt"] = this.bt;
            return aesmap.ToString();
        }

    }
}
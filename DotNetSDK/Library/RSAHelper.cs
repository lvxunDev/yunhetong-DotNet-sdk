using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace LxSDK.Library
{
    /// <summary>
    /// RSA 非对称加解密算法
    /// </summary>
    public class RSAHelper
    {
        private int MAXENCRYPTSIZE = 117;
        private int MAXDECRYPTSIZE = 128;

        public string priKeyXml
        {
            get;
            private set;
        }

        public string pubKeyXml
        {
            get;
            private set;
        }


        private RSAHelper(string privateKey, string publicKey)
        {
            this.priKeyXml = privateKey;
            this.pubKeyXml = publicKey;
        }

        public static RSAHelper Load(string privateKey = "", string publicKey = "")
        {
            if (string.IsNullOrEmpty(privateKey) && string.IsNullOrEmpty(publicKey))
            {
                //无key时生成新密钥
                return Instance;
            }
            return new RSAHelper(privateKey, publicKey);
        }

        /// <summary>
        /// 随机生成公私钥并返回对象
        /// </summary>
        public static RSAHelper Instance
        {
            get
            {
                RSACryptoServiceProvider provider = new RSACryptoServiceProvider(1024);
                var publicKeyXml = provider.ToXmlString(false);
                //publickey:<RSAKeyValue><Modulus>w9u2HfdbNZrmAUmXPbNmrhfy861qX4mzcCn69Ksl03Nz+Fq9gINZeN/vrfcWBzMyYxb2/J2TnGtpCLc0ls6gOTKDPbnQHwHr3oCzfvxNwvT2uoKQUBl4xMFw0TmvufMbheq6q3FCXUkVkAUC1cbQ/S9DqNp/veYcAavRDXtFdD0=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>
                var privateKeyXml = provider.ToXmlString(true);
                //privatekey:<RSAKeyValue><Modulus>w9u2HfdbNZrmAUmXPbNmrhfy861qX4mzcCn69Ksl03Nz+Fq9gINZeN/vrfcWBzMyYxb2/J2TnGtpCLc0ls6gOTKDPbnQHwHr3oCzfvxNwvT2uoKQUBl4xMFw0TmvufMbheq6q3FCXUkVkAUC1cbQ/S9DqNp/veYcAavRDXtFdD0=</Modulus><Exponent>AQAB</Exponent><P>6tzaLZmY+hLLAifunWwcdUSfqTUvKOO5bJ8M1Zt34en40tfBaH9zml9gP8cmXaWyfpiZgHlPS9xlkLngudAiJw==</P><Q>1Xw2E1ufXsCM2JZahB6PH9pCgfD4XPjrqxF9xOWVvfbPmVBZByBIHYRs8ifbjIPvSKuaCfVFVStoIcOYrT9I+w==</Q><DP>mS4iPsuHMtM/BND2mEYC6ZkwaTP+5jRgo6+4tzkHH5lyaFHAG1/FDlJWfEJvi3SezmLI+zojtd6xf4s8PvS40Q==</DP><DQ>I91kMEhaM87hWpmXx05i+RTvy2iyMNxYqzqbCHMRfwJxye3npvzTYLIYo23ywl5/2pOJo1ajOTW7nsB/a8uP9Q==</DQ><InverseQ>EtYQvvBViXf7A5bgh+H4xLlBezD0yziBigoP/xcg1mcuI9Kb9rtPq64hQsajDYeNmm0Ibkxz9ihHr8+uWtdi5w==</InverseQ><D>HSivw2RZKvDlv1lSb/gumEqufALcbF7W3SMS3qxAVGvC3z27Ks/jWTCVwWOg3u+LV99KZC+dk1MWbxq/dJhMmBSiHOT6Sg7wvNMmX58zHl7Bhs702henzbr7CkiWrUcy3pVigr4olT9FlkjQkeEu9VfVW4TRGUDUkixTeh9MMC0=</D></RSAKeyValue>

                return new RSAHelper(privateKeyXml, publicKeyXml);
            }
        }



        /// <summary>
        /// RSA公钥加密
        /// </summary>
        /// <param name="content"></param>
        /// <param name="publicKeyXml">公钥xml串</param>
        /// <returns></returns>
        public string Encrypt(string content)
        {
            //string publickey = @"<RSAKeyValue><Modulus>w9u2HfdbNZrmAUmXPbNmrhfy861qX4mzcCn69Ksl03Nz+Fq9gINZeN/vrfcWBzMyYxb2/J2TnGtpCLc0ls6gOTKDPbnQHwHr3oCzfvxNwvT2uoKQUBl4xMFw0TmvufMbheq6q3FCXUkVkAUC1cbQ/S9DqNp/veYcAavRDXtFdD0=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(pubKeyXml);
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);

            return Convert.ToBase64String(cipherbytes);
            //return cipherbytes;
        }
        /// <summary>  
        /// RSA私钥解密  
        /// </summary>  
        /// <param name="encryptData">经过Base64编码的密文</param>  
        /// <param name="privateKeyXml">私钥xml串</param>  
        /// <returns>RSA解密后的数据</returns>  
        public string Decrypt(string encryptData)
        {
            string decryptData = "";
            try
            {
                RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
                provider.FromXmlString(priKeyXml);
                byte[] bEncrypt = Convert.FromBase64String(encryptData);
                int length = bEncrypt.Length;
                int offset = 0;
                string cache;
                int i = 0;
                while (length - offset > 0)
                {
                    if (length - offset > MAXDECRYPTSIZE)
                    {
                        cache = Encoding.UTF8.GetString(provider.Decrypt(GetSplit(bEncrypt, offset, MAXDECRYPTSIZE), false));
                    }
                    else
                    {
                        cache = Encoding.UTF8.GetString(provider.Decrypt(GetSplit(bEncrypt, offset, length - offset), false));
                    }
                    decryptData += cache;
                    i++;
                    offset = i * MAXDECRYPTSIZE;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return decryptData;
        }

        /// <summary>  
        /// 截取字节数组部分字节  
        /// </summary>  
        /// <param name="input"></param>  
        /// <param name="offset">起始偏移位</param>  
        /// <param name="length">截取长度</param>  
        /// <returns></returns>  
        private byte[] GetSplit(byte[] input, int offset, int length)
        {
            byte[] output = new byte[length];
            for (int i = offset; i < offset + length; i++)
            {
                output[i - offset] = input[i];
            }
            return output;
        }

    }

}
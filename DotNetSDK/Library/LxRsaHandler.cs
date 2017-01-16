using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace LxSDK.Library
{
    public class LxRsaHandler
    {
        private string publicKey;
        private string privateKey;

        public LxRsaHandler(string pri_path, string pub_path)//生成公私钥
        {
            RSAKeyConvert Rsakey = new RSAKeyConvert();
            string pub_content = File.ReadAllText(pub_path);
            string pri_content = File.ReadAllText(pri_path);

            publicKey = Rsakey.RSAPublicKeyJava2DotNet(pub_content);
            privateKey = Rsakey.RSAPrivateKeyJava2DotNet(pri_content);

        }

        public string encryptAES(LxAESHandler aesHandler)
        {//公钥加密

            RSACryptoServiceProvider oRSA1 = new RSACryptoServiceProvider();
            string aseHandler_tostring = aesHandler.toString().Replace("\r\n", "").Replace(" ", "");//要加密的信息
            byte[] aseHandler_data = Encoding.UTF8.GetBytes(aseHandler_tostring);//把信息变成byte型
            oRSA1.FromXmlString(this.publicKey); //加密要用到公钥所以导入公钥 
            byte[] AOutput = oRSA1.Encrypt(aseHandler_data, false); //AOutput 加密以后的数据 
            string encrypted = Convert.ToBase64String(AOutput);

            return encrypted;
        }

        public string decryptRSA(string data)
        {
            if (String.IsNullOrEmpty(data))
            {
                return "";
            }
            RSACryptoServiceProvider oRSA1 = new RSACryptoServiceProvider();
            oRSA1.FromXmlString(this.privateKey);
            byte[] AOutput = oRSA1.Decrypt(Convert.FromBase64String(data), false);
            return System.Text.Encoding.UTF8.GetString(AOutput);
        }



        public string sign_data(string data)//私钥签名
        {
            RSACryptoServiceProvider orsa = new RSACryptoServiceProvider();
            orsa.FromXmlString(this.privateKey);
            byte[] b_data = Encoding.UTF8.GetBytes(data);
            byte[] b_sign = orsa.SignData(b_data, "SHA1");
            string sign = Convert.ToBase64String(b_sign);
            return sign;
        }

    }
}
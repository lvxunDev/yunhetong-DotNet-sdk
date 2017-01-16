using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace LxSDK.Library
{
    public class LxSecretManager
    {
        private string appId;
        private LxAESHandler lxAESHandler;
        private LxRsaHandler lxRSAHandler;

        public LxSecretManager(string appId, string pri_path, string pub_path)
        {
            this.appId = appId;
            this.lxAESHandler = new LxAESHandler();
            this.lxRSAHandler = new LxRsaHandler(pri_path,pub_path);
        }

        //对字符串进行加密

        public string encrypt(string json)//return @string json
        {
            string key=this.lxRSAHandler.encryptAES(this.lxAESHandler);//return ok
            string json_tostring = json.ToString().Replace("\r\n", "").Replace(" ", "").Replace("\"", "").Replace("\\", "\"");
            string content = this.lxAESHandler.Encrypt(json_tostring);
            string sign=sign_data(json_tostring);
            string sign_aes = this.lxAESHandler.Encrypt(sign);
            //make map
            JObject map = new JObject();
            map["key"] = key;
            map["content"] = content;
            map["sign"] = sign_aes;
            return map.ToString();
        }

        public string decrypt(string s)
        {
            JObject o = JObject.Parse(s);
            if ((string)(o["key"]) == null)
            {
                return s;
            }
            string sk = this.lxRSAHandler.decryptRSA((string)(o["key"]));
            this.lxAESHandler = new LxAESHandler(sk);
            return this.lxAESHandler.Decrypt((string)o["content"]);
        }


        public string sign_data(string data)//data:jsontostring user
        {
            string sign = this.lxRSAHandler.sign_data(data);
            return sign;
        }
  

    }
}
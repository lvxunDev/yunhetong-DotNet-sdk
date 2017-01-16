using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace LxSDK.Library
{
    public class LxSDKManager
    {
        private string appId;
        private string pubPath;
        private string priPath;
        private string host;
        private LxSecretManager lx_secret_manager;


        public LxSDKManager(string appId, string pubPath, string priPath)
        {
            this.host = "http://sdk.yunhetong.com/sdk";
            this.appId = appId;
            this.pubPath = pubPath;
            this.priPath = priPath;
            this.lx_secret_manager = new LxSecretManager(this.appId, this.priPath, this.pubPath);
        }

        public string syncGetToken(LxUser currentUser)
        {
            string url = this.host + "/third/tokenWithUser";
            JObject JsonInfo = new JObject();
            JsonInfo["currentUser"] = JsonConvert.SerializeObject(currentUser);
            string secret = this.lx_secret_manager.encrypt(JsonInfo.ToString()).Replace("\r\n", "").Replace(" ", "").Trim();
            WebClient webClient = new WebClient();
            NameValueCollection list = new NameValueCollection();

            list["appid"] = this.appId;
            list["secret"] = secret;

            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
            byte[] responseData = webClient.UploadValues(url, list);
            string srcString = Encoding.UTF8.GetString(responseData);//解码  

            // return srcString;
            return this.lx_secret_manager.decrypt(srcString);

        }

        public String updateUserInfo(LxUser user)
        {
            string url = this.host + "/third/userUpdate";
            JObject JsonInfo = new JObject();
            JsonInfo["currentUser"] = JsonConvert.SerializeObject(user);
            string secret = this.lx_secret_manager.encrypt(JsonInfo.ToString()).Replace("\r\n", "").Replace(" ", "").Trim();
            WebClient webClient = new WebClient();
            NameValueCollection list = new NameValueCollection();

            list["appid"] = this.appId;
            list["secret"] = secret;

            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
            byte[] responseData = webClient.UploadValues(url, list);
            string srcString = Encoding.UTF8.GetString(responseData);//解码  

            return this.lx_secret_manager.decrypt(srcString);
        }

        public string syntGetTokenWithContract(LxUser currentUser, LxContract contract, LxContractActor[] actors)
        {
            string url = this.host + "/third/tokenWithContract";
            JObject contract_form_vo = new JObject();
            contract_form_vo["vo"] = JsonConvert.SerializeObject(contract);
            contract_form_vo["attendUser"] = JsonConvert.SerializeObject(actors);
            JObject contractInfo = new JObject();
            contractInfo["currentUser"] = JsonConvert.SerializeObject(currentUser);
            contractInfo["contractFormVo"] = contract_form_vo;
            string secret = this.lx_secret_manager.encrypt(contractInfo.ToString().Replace("contractPrams", "params")).Replace("\r\n", "").Replace(" ", "")/*这个地方因为params是关键字，所以这样替换，可能会产生迷之bug*/.Trim();
            WebClient webClient = new WebClient();
            NameValueCollection list = new NameValueCollection();

            list["appid"] = this.appId;
            list["secret"] = secret;

            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
            byte[] responseData = webClient.UploadValues(url, list);
            string srcString = Encoding.UTF8.GetString(responseData);//解码  

            // return srcString;
            return this.lx_secret_manager.decrypt(srcString);
        }


        public string syntCreateContract(LxContract contract, LxContractActor[] actors)
        {
            string url = this.host + "/third/autoContract";
            JObject contract_form_vo = new JObject();
            contract_form_vo["vo"] = JsonConvert.SerializeObject(contract);
            contract_form_vo["attendUser"] = JsonConvert.SerializeObject(actors);
            JObject contractInfo = new JObject();
            contractInfo["contractFormVo"] = contract_form_vo;
            string secret = this.lx_secret_manager.encrypt(contractInfo.ToString().Replace("contractPrams", "params")).Replace("\r\n", "").Replace(" ", "")/*这个地方因为params是关键字，所以这样替换，可能会产生迷之bug*/.Trim();
            WebClient webClient = new WebClient();
            NameValueCollection list = new NameValueCollection();

            list["appid"] = this.appId;
            list["secret"] = secret;

            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
            byte[] responseData = webClient.UploadValues(url, list);
            string srcString = Encoding.UTF8.GetString(responseData);//解码  

            // return srcString;
            return this.lx_secret_manager.decrypt(srcString);
        }

        public string queryContractList(int pageSize, int pageNum)
        {
            string url = this.host + "/third/listContract";
            JObject queryParam = new JObject();
            queryParam["flag"] = (DateTime.UtcNow.Ticks - (new DateTime(1970, 1, 1).Ticks)) / 10000; ;
            queryParam["pageSize"] = pageSize;
            queryParam["pageNum"] = pageNum;
            string secret = this.lx_secret_manager.encrypt(queryParam.ToString()).Replace("\r\n", "").Replace(" ", "").Trim();
            WebClient webClient = new WebClient();
            NameValueCollection list = new NameValueCollection();

            list["appid"] = this.appId;
            list["secret"] = secret;

            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
            byte[] responseData = webClient.UploadValues(url, list);
            string srcString = Encoding.UTF8.GetString(responseData);//解码  

            // return srcString;
            return this.lx_secret_manager.decrypt(srcString);
        }

        public string signData(string data)
        {
            JObject resJson = new JObject();
            resJson["response"] = true;
            resJson["msg"] = "OK";
            JObject content = JObject.Parse(data);
            if ((int)content["noticeType"] == 2)
            {//2时表示合同签署完成
                string sign = this.lx_secret_manager.sign_data((string)content["signDigest"]);
                resJson["signDigest"] = this.lx_secret_manager.encrypt(sign).Replace("\r\n", "").Replace(" ", "").Trim();
                return this.lx_secret_manager.encrypt(resJson.ToString());
            }

            return resJson.ToString(); ;
        }
        public string decrypt(string data)
        {
            return this.lx_secret_manager.decrypt(data);
        }
        public Dictionary<string, Object> downloadContract(string contractId)
        {
            string url = this.host + "/third/download";
            JObject queryParam = new JObject();
            queryParam["timestamp"] = (DateTime.UtcNow.Ticks - (new DateTime(1970, 1, 1).Ticks)) / 10000; ;
            queryParam["contractId"] = contractId;
            string secret = this.lx_secret_manager.encrypt(queryParam.ToString()).Replace("\r\n", "").Replace(" ", "").Trim();
            NameValueCollection list = new NameValueCollection();

            list["appid"] = this.appId;
            list["secret"] = secret;
            WebClient webClient = new WebClient();
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
            byte[] responseData = webClient.UploadValues(url, list);

            string contentType = webClient.ResponseHeaders.Get("Content-Type");
            bool success = false;
            if (contentType == "application/octet-stream;charset=UTF-8")
            {
                success = true;
            }
            var retDict = new Dictionary<string, Object>();
            retDict.Add("success", success);
            retDict.Add("body", responseData);
            return retDict;
        }


        public string getLastNotice()
        {
            string url = this.host + "/third/getLastNotice?appid=" + this.appId;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            String s = String.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                s = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
            }
            return this.lx_secret_manager.decrypt(s);
        }



    }
}
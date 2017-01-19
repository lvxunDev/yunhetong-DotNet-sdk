using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
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

        public string createContractFromFile(LxContract contract, LxContractActor[] actors,FileStream fs)
        {
            string url = this.host + "/third/fileContract";
            JObject contract_form_vo = new JObject();   
            contract_form_vo["vo"] = JsonConvert.SerializeObject(contract);
            contract_form_vo["attendUser"] = JsonConvert.SerializeObject(actors);
            contract_form_vo["fileType"] = JsonConvert.SerializeObject(Path.GetExtension(fs.Name).Substring(1));
            JObject contractInfo = new JObject();
            contractInfo["filecontractModel"] = contract_form_vo;
            string secret = this.lx_secret_manager.encrypt(contractInfo.ToString().Replace("contractPrams", "params")).Replace("\r\n", "").Replace(" ", "")/*这个地方因为params是关键字，所以这样替换，可能会产生迷之bug*/.Trim();
            
            
            WebClient webClient = new WebClient();

            var file = new UploadFile{
                        Name = "file",
                        Filename = Path.GetFileName(fs.Name),
                        ContentType = "text/plain",
                        Stream = fs
                    };
            var values = new NameValueCollection{
                                            { "appid", this.appId },
                                            { "secret", secret },
                                            { "hasEn", "0" }
                                         };

            byte[] retByte = UploadFiles(url, file, values);

            string result = System.Text.Encoding.UTF8.GetString(retByte);
            // return srcString;
            return this.lx_secret_manager.decrypt(result);
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

        public byte[] UploadFiles(string address, UploadFile file, NameValueCollection values)
        {
            var request = WebRequest.Create(address);
            request.Method = "POST";
            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            boundary = "--" + boundary;

            using (var requestStream = request.GetRequestStream())
            {
                // Write the values
                foreach (string name in values.Keys)
                {
                    var buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", name, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(values[name] + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                }

                // Write the files

                var fileBuffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                requestStream.Write(fileBuffer, 0, fileBuffer.Length);
                fileBuffer = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"{2}", file.Name, file.Filename, Environment.NewLine));
                requestStream.Write(fileBuffer, 0, fileBuffer.Length);
                fileBuffer = Encoding.ASCII.GetBytes(string.Format("Content-Type: {0}{1}{1}", file.ContentType, Environment.NewLine));
                requestStream.Write(fileBuffer, 0, fileBuffer.Length);
                file.Stream.CopyTo(requestStream);
                fileBuffer = Encoding.ASCII.GetBytes(Environment.NewLine);
                requestStream.Write(fileBuffer, 0, fileBuffer.Length);

                var boundaryBuffer = Encoding.ASCII.GetBytes(boundary + "--");
                requestStream.Write(boundaryBuffer, 0, boundaryBuffer.Length);
            }

            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var stream = new MemoryStream())
            {
                responseStream.CopyTo(stream);
                return stream.ToArray();
            }
        }

        public class UploadFile
        {
            public UploadFile()
            {
                ContentType = "application/octet-stream";
            }
            public string Name { get; set; }
            public string Filename { get; set; }
            public string ContentType { get; set; }
            public Stream Stream { get; set; }
        }

    }
}
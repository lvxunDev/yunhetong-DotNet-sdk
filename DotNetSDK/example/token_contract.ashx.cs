using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using LxSDK.Library;
using LxSDK.example;

namespace LxSDK.Example
{
    /// <summary>
    /// TokenWithContract 的摘要说明
    /// </summary>
    public class TokenWithContract : IHttpHandler{
    

        public void ProcessRequest(HttpContext context)
        {
            LxSDKManager sdkManager = R.getLxSDKManager();
            LxUser currentUser = R.getUserA();
            LxContract contract = R.getTestContract();
            LxContractActor[] actors = R.getActor();

            string test = sdkManager.syntGetTokenWithContract(currentUser, contract, actors);

            context.Response.ContentType = "Content-type: application/json;charset=utf-8";
            context.Response.Write(test);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
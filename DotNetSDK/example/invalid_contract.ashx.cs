using LxSDK.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LxSDK.example
{
    /// <summary>
    /// queryContractList 的摘要说明
    /// </summary>
    public class InvalidContract : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            LxSDKManager sdkManager = R.getLxSDKManager();
            string contractId = context.Request.Params.Get("contractId");
            string test = sdkManager.invalidContract(contractId);
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
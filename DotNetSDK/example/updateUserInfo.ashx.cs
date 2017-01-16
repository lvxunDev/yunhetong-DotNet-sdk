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
    /// UpdateUserInfo 的摘要说明
    /// </summary>
    public class UpdateUserInfo : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            LxSDKManager sdkManager = R.getLxSDKManager();

            string test = sdkManager.updateUserInfo(R.getUserA());

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
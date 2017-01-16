using LxSDK.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LxSDK.example
{
    /// <summary>
    /// messageRecive 的摘要说明
    /// </summary>
    public class messageRecive : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            LxSDKManager sdkManager = R.getLxSDKManager();
            string secret = context.Request.Params["notice"];
            context.Response.ContentType = "Content-type: application/json;charset=utf-8";

            //  确定签署结果之后用这个返回 string s = sdk_manager.signData(secret);
            //  这里只展示了解密后的内容
            string s = sdkManager.decrypt(secret);
            context.Response.Write(s);
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
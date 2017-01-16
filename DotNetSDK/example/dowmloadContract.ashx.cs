using LxSDK.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LxSDK.example
{
    /// <summary>
    /// dowmloadContract 的摘要说明
    /// </summary>
    public class dowmloadContract : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            LxSDKManager sdkManager = R.getLxSDKManager();
            string cid = context.Request.Params.Get("contractId");
            
            if (cid == null)
            {
                context.Response.Write("合同id不能为空");
                return;
            }

            Dictionary<string, Object> result = sdkManager.downloadContract(cid);
            if ((bool)result["success"])
            {
                context.Response.ContentType = "application/octet-stream";
                context.Response.AddHeader("Accept-Ranges", "bytes");
                context.Response.AddHeader("Accept-Length", ((byte[])result["body"]).Length.ToString());
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + cid + ".zip");   
                context.Response.BinaryWrite((byte[])result["body"]);
            }
            else
            {
                context.Response.ContentType = "Content-type: application/json;charset=utf-8";
                context.Response.Write(result["body"]);
            }

            
            return;
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
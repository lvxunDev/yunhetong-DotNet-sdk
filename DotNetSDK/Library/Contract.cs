using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace LxSDK.Library
{
    public class LxContract
    {
        public string title;    // 合同标题

        public string appId;    // 应用Id

        public long overtime;

        public string defContractNo;  // 自定义合同号

        public string templateId;     // 合同模板Id

        public Dictionary<String, String> contractPrams;     // 合同模板相关占位符，array类型

    }
}
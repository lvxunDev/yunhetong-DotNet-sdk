using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LxSDK.Library
{
    public class LxContractActor
    {
        public LxUser user;
        public string locationName; // 签名暂未符id，通过上传模板获取
        public AUTO_SIGN autoSign;        //自动签署，1自动签署，0不自动签
        public enum AUTO_SIGN : int { autoSign = 1, noAutoSign = 0 }
    }

    
}
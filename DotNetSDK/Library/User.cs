using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LxSDK.Library
{
    public class LxUser
    {
        public string appId;    // 应用Id
        public string appUserId; // 用户在第三方应用的唯一标识
        public USER_TYPE userType;// 用户类型
        public string cellNum; // 电话号码
        public string userName;// 用户名称
        public CERTIFY_TYPE certifyType; // 实名认证类型
        public string certifyNumber; // 实名认证号码

        public enum USER_TYPE : int { /*个人用户*/USER = 1, /*企业用户*/COMPANY = 2 };
        public enum CERTIFY_TYPE : int { /*身份证*/ID_CARD = 1,/*护照*/ PASSPORT = 2, /*军官证*/OFFICIAL_CARD = 3, /*营业执照*/BUSINESS_LICENCE = 4, /*组织机构代码证*/ORGANIZATION_CODE = 5, /*三证合一*/TYPE_6 = 6 };

    }
}
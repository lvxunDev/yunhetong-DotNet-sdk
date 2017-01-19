using LxSDK.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LxSDK.example
{
    public class R
    {
        // 第三方应用的appId
        public static String appId = "2016121514373700002";
        /**
        * 初始化 LxSDKManager
        * @return 正常的话返回 LxSDKManager ，有异常的话返回 null
        */
        public static LxSDKManager getLxSDKManager()
        {
            // 初始化 SDKManager(appId,云合同公钥地址,第三方应用的私钥地址)
            return new LxSDKManager(appId, "C:\\Users\\Seanwu\\Desktop\\dotnetSDK\\DotNetSDK\\example\\resource\\yhtSK.pem", "C:\\Users\\Seanwu\\Desktop\\dotnetSDK\\DotNetSDK\\example\\resource\\rsa_private_key_pkcs8.pem");
        }

        /**
         * 生成测试用 userA
         *
         * @return 返回一个 userA
         */
        public static LxUser getUserA()
        {
            LxUser lxUser = new LxUser();
            lxUser.appId = R.appId;
            lxUser.appUserId = "DNetTestUserA";                            // 设置用户id
            lxUser.cellNum = "15267131111";                               // 设置手机号码
            lxUser.certifyNumber = "123";                                 // 设置证件号码
            lxUser.certifyType = LxUser.CERTIFY_TYPE.BUSINESS_LICENCE;    // 设置实名认证类型
            lxUser.userName = "测试甲有限公司";                           // 设置用户名
            lxUser.userType = LxUser.USER_TYPE.COMPANY;                   // 设置用户类型

            return lxUser;
        }
        /**
         * 生成测试用 userA
         *
         * @return 返回用户 B
         */
        public static LxUser getUserB()
        {
            LxUser lxUser = new LxUser();
            lxUser.appId = R.appId;
            lxUser.appUserId = "DNetTestUserB";                            // 设置用户id
            lxUser.cellNum = "15267132222";                               // 设置手机号码
            lxUser.certifyNumber = "123";                                 // 设置证件号码
            lxUser.certifyType = LxUser.CERTIFY_TYPE.BUSINESS_LICENCE;    // 设置实名认证类型
            lxUser.userName = "测试乙";                                   // 设置用户名
            lxUser.userType = LxUser.USER_TYPE.USER;                      // 设置用户类型
            return lxUser;
        }

        /**
         * 创建合同参与方
         * @return 返回合同参与方
         */
        public static LxContractActor[] getActor()
        {
            LxContractActor actorA = new LxContractActor();
            actorA.user = getUserA();
            actorA.autoSign = LxContractActor.AUTO_SIGN.noAutoSign;
            actorA.locationName = "signA";

            LxContractActor actorB = new LxContractActor();
            actorB.user = getUserB();
            actorB.autoSign = LxContractActor.AUTO_SIGN.noAutoSign;
            actorB.locationName = "signB";
            return new LxContractActor[] { actorA, actorB };
        }


        private static Dictionary<String, String> getContractParams()
        {
            Dictionary<String, String> d = new Dictionary<String, String>();
            d.Add("${nameA}", "nameA");
            return d;
        }

        /**
         * 创建测试合同
         * @return 测试合同
         */
        public static LxContract getTestContract() {
        LxContract lxContract = new LxContract();
            lxContract.contractPrams = getContractParams();    // 这是模板占位符
            lxContract.appId = R.appId;                        // 第三方应用的 appId
            lxContract.templateId = "123456";                  // 设置合同模板 Id
            lxContract.title = "测试合同标题";                 // 设置合同标题
            lxContract.defContractNo = "随便写";               // 设置自定义合同编号                      
        ;
        return lxContract;
    }
    }
}
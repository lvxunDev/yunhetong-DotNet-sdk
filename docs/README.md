# yunhetong-DotNet-java
欢迎使用 yunhetong java SDK

- 我们为您编写了一份详细的[Demo](https://github.com/lvxunDev/yunhetong-DotNet-sdk/tree/master/DotNetSDK/example)
- 你可以查看详细的[文档](https://github.com/lvxunDev/yunhetong-DotNet-sdk/wiki)
- 遇到问题可以先去看看我们的 [Issue](https://github.com/lvxunDev/yunhetong-DotNet-sdk/issues)

# 快速上手

快速上手要求您有一定的 C# 基础，如果没有就假装自己有。。。

# 0x00 目录结构

```
phpSDK
|
|-------docs     // 一些说明文档
|-------DotNetSDK
|         |------example  // php SDK Demo 地址
|         |------library  // yunhetong php SDK 核心包
|                 |---- model   // 一些实体类
|                 |---- Http.class.php
|                 |---- LxAESHandler.cs             // AES 加密相关的一个类，客户一般不需要使用
|                 |---- LxRsaHandler.cs             // RSA 加密相关的一个类，客户一般不需要使用
|                 |---- SDKManager.cs               // 客户最主要使用的一个类
|                 |---- LxSecretManager.cs          // 加解密管理类，客户一般不需要调用
|                 |---- StringUtils.php             // 字符串处理的工具类
```

# 0x01 初始化 LxSDKManager
为了方便，这里我们建一个资源类```R```,并添加如下代码：
```C#
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
            return new LxSDKManager(appId, ".\\resource\\yhtSK.pem", ".\\resource\\rsa_private_key_pkcs8.pem");
        }
    }
```


# 0x02 导入用户
我们要导入用户并且获取 token
- 准备用户数据

```C#
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
```

- 导入用户

```C#
LxSDKManager sdkManager = R.getLxSDKManager();
string test = sdkManager.syncGetToken(R.getUserA());
```

- 返回结果
正常会返回如下所示 Json 字符串

```json
{"code":200,"message":"true","subCode":200,"value":{"contractList":[{"id":1701061349385004,"status":"签署中","title":"测试合同标题40"},{"id":1701031046255028,"status":"签署中","title":"测试合同标题25"}],"token":"TGT-31356-4FZDJcQR3yK4IiaWIafnxQY0QAIoAI0SP6jja0VFY65PJ1S2W4-cas01.example.org"}}
```

然后将 token 返回给客户端，客户端再通过这个 token 去调用相应的SDK（比如js SDK 或 Android SDK 或 iOS SDK），去访问合同操作

# 0x03 生成合同
初始化 LxSDKManager 略，参考上面第一条。假设有个 A,B 两个人，A 要发起一份合同合同给 B，此时 A是合同的发起方， 也是合同的参与方。以此为例，代码如下
- 准备用户 B 信息
参考上面第二条用户 A 的信息，用户 B 的代码如下

```C#
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
```

- 准备合同信息

```C#
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
        lxContract.templateId = "123456";                  // 设置合同模板 Id
        lxContract.title = "测试合同标题";                 // 设置合同标题
        lxContract.defContractNo = "随便写";               // 设置自定义合同编号                      
    ;
    return lxContract;
}
```
- 准备合同参与方

在刚才的用户A、B的基础上，我们可以生成合同的参与方

```C#
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
    actorA.user = getUserA();
    actorA.autoSign = LxContractActor.AUTO_SIGN.noAutoSign;
    actorA.locationName = "signB";
    return new LxContractActor[] { actorA, actorB };
}
```

- 生成合同

```C#
    LxSDKManager sdkManager = R.getLxSDKManager();
    LxContract contract = R.getTestContract();
    LxContractActor[] actors = R.getActor();
    string test = sdkManager.syntCreateContract(contract, actors);
```

- 返回结果
正常的话会返回如下所示字符串

```json
{"code":200,"message":"true","subCode":200,"value":{"contractId":1701061352090008}}
```
将上一步得到的 token 和这里的 contractId 返回给客户端，即可用相应的 SDK（比如js SDK 或 Android SDK 或 iOS SDK），去进行合同的相关操作。

# 0x04 通过创建合同获取 token
有时候我们想在创建合同的同时也获取 Token，我们可以像下面这样
```C#
    string test = sdkManager.syntGetTokenWithContract(currentUser, contract, actors);
```

正常的话会返回如下所示字符串

```json
{"code":200,"message":"true","subCode":200,"value":{"contractId":1701061349385004,"token":"TGT-31353-vpnotTbYFJ5wXoTUDzjSD9eVqZfzx9RZIsUhqGcEL5kjRcS6V6-cas01.example.org"}}

```


# 0x05 End
就是这么简单方便
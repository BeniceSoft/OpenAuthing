# OpenAuthing SSO

## 页面

![Login Page](../../screenshots/login.png)

## 配置

### Openiddict 加密和签名证书

参考：https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html#registering-a-certificate-recommended-for-production-ready-scenarios

1. 创建证书
   > 可是使用 OpenAuthing Tools 创建加密和签名证书

   ```bash
   $ dotnet run --project src/BeniceSoft.OpenAuthing.Tools/ certificate && mkdir certs && mv *.pfx ./certs
   ```

   证书文件存放在当前目录的 `certs` 文件夹内

2. 启动时配置加密和签名证书文件位置
   > 使用环境变量或者配置文件等方式配置

    * `OPENAUTHING_SIGNING_CERTIFICATE_FILE`: 签名证书文件位置
    * `OPENAUTHING_SIGNING_CERTIFICATE_PASSWORD`: 签名证书密码
    * `OPENAUTHING_ENCRYPTION_CERTIFICATE_FILE`: 加密证书文件位置
    * `OPENAUTHING_ENCRYPTION_CERTIFICATE_PASSWORD`: 加密证书密码

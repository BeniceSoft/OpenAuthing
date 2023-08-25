using BeniceSoft.OpenAuthing.Models;

namespace BeniceSoft.OpenAuthing;

public interface IAmDingTalkClient
{
    /// <summary>
    /// 根据授权码获取用户信息（扫码登录）
    /// </summary>
    /// <param name="authCode">免登授权码</param>
    /// <returns></returns>
    Task<DingTalkUserInfo> GetUserInfoByAuthCodeAsync(string authCode);

    /// <summary>
    /// 根据免登码获取用户信息（微应用、小程序免登）
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    Task<DingTalkUserInfo> GetUserInfoByCodeAsync(string code);
}
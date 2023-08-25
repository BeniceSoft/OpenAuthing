using AlibabaCloud.SDK.Dingtalkcontact_1_0;
using AlibabaCloud.SDK.Dingtalkcontact_1_0.Models;
using BeniceSoft.OpenAuthing.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing;

public class AmDingTalkClient : IAmDingTalkClient, ITransientDependency
{
    private const string AccessTokenCacheKey = "am:dingtalk:access_token";

    private readonly DingTalkOptions _options;
    private readonly ILogger<AmDingTalkClient> _logger;
    private readonly AlibabaCloud.OpenApiClient.Models.Config _config;

    public AmDingTalkClient(IOptions<DingTalkOptions> options,
        ILogger<AmDingTalkClient> logger,
        AlibabaCloud.OpenApiClient.Models.Config config)
    {
        _logger = logger;
        _config = config;
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<DingTalkUserInfo> GetUserInfoByAuthCodeAsync(string authCode)
    {
        var accessToken = await GetUserAccessTokenAsync(authCode);

        var client = new Client(_config);
        var req = new GetUserHeaders
        {
            XAcsDingtalkAccessToken = accessToken
        };

        var rsp = await client.GetUserWithOptionsAsync("me", req, new());
        if (rsp is null)
        {
            throw new UserFriendlyException("未找到钉钉用户信息");
        }

        return new()
        {
            AvatarUrl = rsp.Body.AvatarUrl,
            Email = rsp.Body.Email,
            Mobile = rsp.Body.Mobile,
            NickName = rsp.Body.Nick,
            StateCode = rsp.Body.StateCode,
            UnionId = rsp.Body.UnionId
        };
    }

    public async Task<DingTalkUserInfo> GetUserInfoByCodeAsync(string code)
    {
        var accessToken = await GetAccessToken();

        var client = new DingTalk.Api.DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/v2/user/getuserinfo");
        var req = new DingTalk.Api.Request.OapiV2UserGetuserinfoRequest
        {
            Code = code
        };
        var rsp = client.Execute(req, accessToken);
        if (rsp?.Result is null)
        {
            throw new UserFriendlyException("未找到钉钉用户信息");
        }

        return new()
        {
            UnionId = rsp.Result.Unionid
        };
    }

    /// <summary>
    /// 获取企业内部 access token
    /// </summary>
    /// <returns></returns>
    private async Task<string> GetAccessToken()
    {
        var client = new AlibabaCloud.SDK.Dingtalkoauth2_1_0.Client(_config);
        var request = new AlibabaCloud.SDK.Dingtalkoauth2_1_0.Models.GetAccessTokenRequest
        {
            AppKey = _options.AppKey,
            AppSecret = _options.AppSecret,
        };
        var response = await client.GetAccessTokenAsync(request);
        return response.Body.AccessToken;
    }

    private async Task<string> GetUserAccessTokenAsync(string authCode)
    {
        var client = new AlibabaCloud.SDK.Dingtalkoauth2_1_0.Client(_config);

        var req = new AlibabaCloud.SDK.Dingtalkoauth2_1_0.Models.GetUserTokenRequest
        {
            ClientId = _options.AppKey,
            ClientSecret = _options.AppSecret,
            Code = authCode,
            GrantType = "authorization_code",
        };

        var rsp = await client.GetUserTokenAsync(req);
        return rsp.Body.AccessToken;
    }
}
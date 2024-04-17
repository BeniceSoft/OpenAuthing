using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing;

public class JwtBearerPostConfigureOptions : IPostConfigureOptions<JwtBearerOptions>, ISingletonDependency
{
    private readonly AuthingOptions _authingOptions;

    public JwtBearerPostConfigureOptions(IOptions<AuthingOptions> authingOptions)
    {
        _authingOptions = authingOptions.Value;
    }

    public void PostConfigure(string? name, JwtBearerOptions options)
    {
        options.TokenValidationParameters.ValidAudiences = [AuthingConstants.Audience];
        options.TokenValidationParameters.ValidIssuers = [_authingOptions.SsoHost];
    }
}
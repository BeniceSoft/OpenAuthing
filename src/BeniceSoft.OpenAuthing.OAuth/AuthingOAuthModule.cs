using BeniceSoft.OpenAuthing.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace BeniceSoft.OpenAuthing;

public class AuthingOAuthModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAuthentication().AddDynamic()
            .AddDingTalk()
            .AddFeishu();
    }
}
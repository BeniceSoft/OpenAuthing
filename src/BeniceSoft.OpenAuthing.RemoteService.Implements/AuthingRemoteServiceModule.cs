using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace BeniceSoft.OpenAuthing;

[DependsOn(
)]
public class AuthingRemoteServiceModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        Configure<DingTalkOptions>(configuration.GetSection("DingTalk"));

        context.Services.AddSingleton(_ => new AlibabaCloud.OpenApiClient.Models.Config
        {
            Protocol = "https",
            RegionId = "central"
        });
    }
}
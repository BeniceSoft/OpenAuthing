using LinkMore.Abp.Http.Client;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace BeniceSoft.OpenAuthing;

[DependsOn(
    typeof(LinkMoreAbpHttpClientModule)
)]
public class AmSdkModule : AbpModule
{
    public const string RemoteServiceName = "LinkMore.AM";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(ApplicationContractsModule).Assembly,
            RemoteServiceName
        );
    }
}
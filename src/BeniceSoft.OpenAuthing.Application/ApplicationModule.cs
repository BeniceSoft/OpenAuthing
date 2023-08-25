using LinkMore.Abp.Ddd.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.BlobStoring;
using Volo.Abp.Modularity;

namespace BeniceSoft.OpenAuthing;

[DependsOn(
    typeof(AbpAutoMapperModule),
    typeof(AbpBlobStoringModule),
    typeof(LinkMoreAbpDddApplicationModule),
    typeof(DomainModule),
    typeof(ApplicationContractsModule)
)]
public class ApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<ApplicationModule>(); });
    }
}
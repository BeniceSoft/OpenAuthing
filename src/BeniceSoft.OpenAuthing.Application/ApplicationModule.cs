using BeniceSoft.Abp.Ddd.Application;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.BlobStoring;
using Volo.Abp.Modularity;

namespace BeniceSoft.OpenAuthing;

[DependsOn(
    typeof(AbpAutoMapperModule),
    typeof(AbpBlobStoringModule),
    typeof(BeniceSoftAbpDddApplicationModule),
    typeof(DomainModule),
    typeof(ApplicationContractsModule)
)]
public class ApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<ApplicationModule>(); });

        context.Services.AddMediatR(config => { config.RegisterServicesFromAssemblyContaining<ApplicationModule>(); });
    }
}
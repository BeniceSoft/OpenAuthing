using BeniceSoft.Abp.Ddd.Application;
using BeniceSoft.OpenAuthing.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.BlobStoring;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace BeniceSoft.OpenAuthing;

[DependsOn(
    typeof(AbpAutoMapperModule),
    typeof(AbpBlobStoringModule),
    typeof(BeniceSoftAbpDddApplicationModule),
    typeof(DomainModule),
    typeof(ApplicationContractsModule),
    typeof(AbpSettingManagementApplicationModule)
)]
public class ApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<ApplicationModule>(); });

        context.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<ApplicationModule>();

            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        context.Services.AddValidatorsFromAssemblyContaining<ApplicationModule>();
    }
}
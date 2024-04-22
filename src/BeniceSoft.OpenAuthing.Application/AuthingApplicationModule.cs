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
    typeof(AuthingDomainModule),
    typeof(AuthingApplicationContractsModule),
    typeof(AbpSettingManagementApplicationModule)
)]
public class AuthingApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<AuthingApplicationModule>(); });

        context.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<AuthingApplicationModule>();

            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        context.Services.AddValidatorsFromAssemblyContaining<AuthingApplicationModule>();
    }
}
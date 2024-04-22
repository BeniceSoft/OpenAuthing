using BeniceSoft.OpenAuthing.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.SettingManagement;
using Volo.Abp.VirtualFileSystem;

namespace BeniceSoft.OpenAuthing;

[DependsOn(
    typeof(AbpDddDomainSharedModule),
    typeof(AbpSettingManagementDomainSharedModule),
    typeof(AbpOpenIddictDomainSharedModule)
)]
public class AuthingDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<AuthingOptions>(options => configuration.GetSection("OpenAuthing").Bind(options));

        Configure<AbpVirtualFileSystemOptions>(options => { options.FileSets.AddEmbedded<AuthingDomainSharedModule>("BeniceSoft.OpenAuthing"); });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AuthingResource>("en-US")
                .AddVirtualJson("/Localization/Authing");

            options.Resources
                .Add<AuthingErrorResource>("en-US")
                .AddVirtualJson("/Localization/AuthingError");

            options.Resources
                .Add<AuthingPermissionResource>("en-US")
                .AddVirtualJson("/Localization/AuthingPermission");

            options.DefaultResourceType = typeof(AuthingResource);
        });
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        var authingConfigures = context.Services.GetPreConfigureActions<AuthingOptions>();
        if (authingConfigures.Any())
        {
            Configure<AuthingOptions>(options => authingConfigures.Configure(options));
        }
    }
}
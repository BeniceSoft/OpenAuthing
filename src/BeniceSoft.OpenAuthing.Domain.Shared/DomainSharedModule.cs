using BeniceSoft.OpenAuthing.Localization;
using BeniceSoft.OpenAuthing.Permissions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
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
public class DomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<AuthingOptions>(options => configuration.GetSection("OpenAuthing").Bind(options));

        Configure<AbpVirtualFileSystemOptions>(options => { options.FileSets.AddEmbedded<DomainSharedModule>("BeniceSoft.OpenAuthing"); });

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
        // Configure<AbpExceptionLocalizationOptions>(options => { options.MapCodeNamespace("MyProjectName", typeof(MyProjectNameResource)); });

        Configure<AbpPermissionOptions>(options => { options.ValueProviders.Insert(0, typeof(SystemAdminPermissionValueProvider)); });
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
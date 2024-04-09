using BeniceSoft.OpenAuthing.Localization;
using Volo.Abp.Domain;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.SettingManagement;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
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
        Configure<AbpVirtualFileSystemOptions>(options => { options.FileSets.AddEmbedded<DomainSharedModule>("BeniceSoft.OpenAuthing"); });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AuthingResource>("en-US")
                // .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/Authing");

            options.DefaultResourceType = typeof(AuthingResource);
        });

        // Configure<AbpExceptionLocalizationOptions>(options => { options.MapCodeNamespace("MyProjectName", typeof(MyProjectNameResource)); });
    }
}
using BeniceSoft.OpenAuthing.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace BeniceSoft.OpenAuthing;

[DependsOn(
    typeof(AbpValidationModule)
)]
public class DomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<DomainSharedModule>("LinkMore.KA.AM");
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AMResource>("zh-CN")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/AM");

            options.DefaultResourceType = typeof(AMResource);
        });

        // Configure<AbpExceptionLocalizationOptions>(options => { options.MapCodeNamespace("MyProjectName", typeof(MyProjectNameResource)); });
    }
}
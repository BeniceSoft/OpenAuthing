using BeniceSoft.OpenAuthing.ObjectExtending;
using BeniceSoft.OpenAuthing.OpenIddict.Applications;
using BeniceSoft.OpenAuthing.OpenIddict.Authorizations;
using BeniceSoft.OpenAuthing.OpenIddict.Scopes;
using BeniceSoft.OpenAuthing.OpenIddict.Tokens;
using BeniceSoft.Abp.Ddd.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;

namespace BeniceSoft.OpenAuthing;

[DependsOn(
    typeof(BeniceSoftAbpDddDomainModule),
    typeof(DomainSharedModule)
)]
public class DomainModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<TokenCleanupOptions>>().Value;
        if (options.IsCleanupEnabled)
        {
            await context.ServiceProvider
                .GetRequiredService<IBackgroundWorkerManager>()
                .AddAsync(context.ServiceProvider.GetRequiredService<TokenCleanupBackgroundWorker>());
        }
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                OpenIddictModuleExtensionConsts.ModuleName,
                OpenIddictModuleExtensionConsts.EntityNames.Application,
                typeof(OpenIddictApplication)
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                OpenIddictModuleExtensionConsts.ModuleName,
                OpenIddictModuleExtensionConsts.EntityNames.Authorization,
                typeof(OpenIddictAuthorization)
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                OpenIddictModuleExtensionConsts.ModuleName,
                OpenIddictModuleExtensionConsts.EntityNames.Scope,
                typeof(OpenIddictScope)
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                OpenIddictModuleExtensionConsts.ModuleName,
                OpenIddictModuleExtensionConsts.EntityNames.Token,
                typeof(OpenIddictToken)
            );
        });
    }
}
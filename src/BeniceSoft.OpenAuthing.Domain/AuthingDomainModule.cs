using BeniceSoft.Abp.Ddd.Domain;
using BeniceSoft.OpenAuthing.Entities.Permissions;
using BeniceSoft.OpenAuthing.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.SettingManagement;

namespace BeniceSoft.OpenAuthing;

[DependsOn(
    typeof(BeniceSoftAbpDddDomainModule),
    typeof(AuthingDomainSharedModule),
    typeof(AbpSettingManagementDomainModule),
    typeof(AbpOpenIddictDomainModule)
)]
public class AuthingDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddTransient<IPasswordHasher<User>, PasswordHasher<User>>();

        Configure<AbpPermissionOptions>(options => { options.ValueProviders.Insert(0, typeof(SystemAdminPermissionValueProvider)); });

        Configure<AuthingPermissionOptions>(options => { options.ManagementProviders.Add<RolePermissionManagementProvider>(); });
    }
}
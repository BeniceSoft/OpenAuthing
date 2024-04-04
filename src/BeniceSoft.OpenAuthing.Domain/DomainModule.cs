using BeniceSoft.Abp.Ddd.Domain;
using BeniceSoft.OpenAuthing.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.SettingManagement;

namespace BeniceSoft.OpenAuthing;

[DependsOn(
    typeof(BeniceSoftAbpDddDomainModule),
    typeof(DomainSharedModule),
    typeof(AbpSettingManagementDomainModule),
    typeof(AbpOpenIddictDomainModule)
)]
public class DomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
    }
}
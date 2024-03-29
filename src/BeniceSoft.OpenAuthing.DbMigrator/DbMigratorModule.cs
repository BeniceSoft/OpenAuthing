using BeniceSoft.OpenAuthing.OpenIddict;
using BeniceSoft.OpenAuthing.PermissionSpaces;
using BeniceSoft.OpenAuthing.Roles;
using BeniceSoft.OpenAuthing.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace BeniceSoft.OpenAuthing.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(EntityFrameworkCoreModule),
    typeof(ApplicationContractsModule)
)]
public class DbMigratorModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDataSeedOptions>(opts =>
        {
            opts.Contributors.Clear();
            opts.Contributors.Add<OpenIddictDataSeedContributor>();
            opts.Contributors.Add<PermissionSpaceDataSeedContributor>();
            opts.Contributors.Add<RoleDataSeedContributor>();
            opts.Contributors.Add<UserDataSeedContributor>();
        });
    }
    
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
    }
}
using BeniceSoft.OpenAuthing.Entities.Departments;
using BeniceSoft.OpenAuthing.Entities.OpenIddict;
using BeniceSoft.OpenAuthing.Entities.PermissionSpaces;
using BeniceSoft.OpenAuthing.Entities.Roles;
using BeniceSoft.OpenAuthing.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace BeniceSoft.OpenAuthing.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AuthingEntityFrameworkCoreModule),
    typeof(AuthingApplicationContractsModule)
)]
public class AuthingDbMigratorModule : AbpModule
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
            opts.Contributors.Add<DepartmentDataSeederContributor>();
        });
    }
    
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
    }
}
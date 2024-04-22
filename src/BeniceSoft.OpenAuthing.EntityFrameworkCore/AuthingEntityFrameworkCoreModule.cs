using BeniceSoft.Abp.EntityFrameworkCore;
using BeniceSoft.OpenAuthing.Entities.IdentityProviders;
using BeniceSoft.OpenAuthing.Entities.Roles;
using BeniceSoft.OpenAuthing.Entities.UserGroups;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace BeniceSoft.OpenAuthing;

[DependsOn(
    typeof(AbpEntityFrameworkCoreMySQLModule),
    typeof(BeniceSoftAbpEntityFrameworkCoreModule),
    typeof(AuthingDomainModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule)
)]
public class AuthingEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        AbpSettingManagementDbProperties.DbSchema = null;
        AbpSettingManagementDbProperties.DbTablePrefix = AuthingDbProperties.DbTablePrefix;
        
        AbpOpenIddictDbProperties.DbSchema = null;
        AbpOpenIddictDbProperties.DbTablePrefix = AuthingDbProperties.OpenIddictDbTablePrefix;

        context.Services.AddAbpDbContext<AuthingDbContext>(options =>
        {
            options.ReplaceDbContext<ISettingManagementDbContext>()
                .ReplaceDbContext<IOpenIddictDbContext>();
            
            /* Remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure<AuthingDbContext>(ctx =>
            {
                ctx.UseMySQL();
#if DEBUG
                ctx.DbContextOptions.EnableSensitiveDataLogging();
#endif
            });
            
        });


        Configure<AbpEntityOptions>(options =>
        {
            options.Entity<Role>(opts => opts.DefaultWithDetailsFunc =
                queryable => queryable.Include(x => x.Subjects));
            options.Entity<UserGroup>(opts => opts.DefaultWithDetailsFunc =
                queryable => queryable.Include(x => x.Members));
            options.Entity<ExternalIdentityProvider>(opts => opts.DefaultWithDetailsFunc =
                queryable => queryable.Include(x => x.Options));
        });
    }
}
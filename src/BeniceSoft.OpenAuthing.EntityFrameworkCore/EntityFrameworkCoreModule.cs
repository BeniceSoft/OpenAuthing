using BeniceSoft.OpenAuthing.OpenIddict.Applications;
using BeniceSoft.OpenAuthing.OpenIddict.Authorizations;
using BeniceSoft.OpenAuthing.OpenIddict.Scopes;
using BeniceSoft.OpenAuthing.OpenIddict.Tokens;
using BeniceSoft.OpenAuthing.Repositories;
using BeniceSoft.OpenAuthing.Roles;
using BeniceSoft.OpenAuthing.UserGroups;
using BeniceSoft.Abp.EntityFrameworkCore;
using BeniceSoft.OpenAuthing.IdentityProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace BeniceSoft.OpenAuthing;

[DependsOn(
    typeof(AbpEntityFrameworkCorePostgreSqlModule),
    typeof(BeniceSoftAbpEntityFrameworkCoreModule),
    typeof(DomainModule)
)]
public class EntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<AuthingDbContext>(options =>
        {
            /* Remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);

            options.AddRepository<OpenIddictApplication, EfCoreOpenIddictApplicationRepository>();
            options.AddRepository<OpenIddictAuthorization, EfCoreOpenIddictAuthorizationRepository>();
            options.AddRepository<OpenIddictScope, EfCoreOpenIddictScopeRepository>();
            options.AddRepository<OpenIddictToken, EfCoreOpenIddictTokenRepository>();
        });

        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure<AuthingDbContext>(ctx =>
            {
                ctx.UseNpgsql();
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
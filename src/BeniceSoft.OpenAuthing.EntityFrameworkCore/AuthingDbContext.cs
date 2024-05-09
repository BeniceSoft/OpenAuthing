using BeniceSoft.OpenAuthing.Entities.DepartmentMembers;
using BeniceSoft.OpenAuthing.Entities.Departments;
using BeniceSoft.OpenAuthing.Entities.IdentityProviders;
using BeniceSoft.OpenAuthing.Entities.IdentityProviderTemplates;
using BeniceSoft.OpenAuthing.Entities.Permissions;
using BeniceSoft.OpenAuthing.Entities.PermissionSpaces;
using BeniceSoft.OpenAuthing.Entities.Positions;
using BeniceSoft.OpenAuthing.Entities.Roles;
using BeniceSoft.OpenAuthing.Entities.UserGroups;
using BeniceSoft.OpenAuthing.Entities.Users;
using BeniceSoft.OpenAuthing.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Tokens;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace BeniceSoft.OpenAuthing;

[ConnectionStringName(AuthingDbProperties.ConnectionStringName)]
[ReplaceDbContext(typeof(ISettingManagementDbContext), typeof(IOpenIddictDbContext))]
public class AuthingDbContext : AbpDbContext<AuthingDbContext>, ISettingManagementDbContext, IOpenIddictDbContext
{
    public AuthingDbContext(DbContextOptions<AuthingDbContext> options) : base(options)
    {
    }


    #region SettingManagement

    public DbSet<Setting> Settings { get; set; }
    public DbSet<SettingDefinitionRecord> SettingDefinitionRecords { get; set; }

    #endregion

    #region Openiddict

    public DbSet<OpenIddictApplication> Applications { get; set; }
    public DbSet<OpenIddictAuthorization> Authorizations { get; set; }
    public DbSet<OpenIddictScope> Scopes { get; set; }
    public DbSet<OpenIddictToken> Tokens { get; set; }

    #endregion

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserToken> UserTokens { get; set; }
    public virtual DbSet<UserLogin> UserLogins { get; set; }

    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<RoleSubject> RoleSubjects { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<UserGroup> UserGroups { get; set; }
    public virtual DbSet<UserGroupMember> UserGroupMembers { get; set; }

    public virtual DbSet<DepartmentMember> DepartmentMembers { get; set; }

    public virtual DbSet<ExternalIdentityProvider> ExternalIdentityProviders { get; set; }
    public virtual DbSet<ExternalIdentityProviderOption> ExternalIdentityProviderOptions { get; set; }

    public virtual DbSet<ExternalIdentityProviderTemplate> ExternalIdentityProviderTemplates { get; set; }
    public virtual DbSet<ExternalIdentityProviderTemplateField> ExternalIdentityProviderTemplateFields { get; set; }

    public virtual DbSet<PermissionSpace> PermissionSpaces { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }
    public virtual DbSet<PermissionGrant> PermissionGrants { get; set; }
    
    public virtual DbSet<Position> Positions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureSettingManagement();
        modelBuilder.ConfigureOpenIddict();

        modelBuilder.ConfigureUserAggregate();
        modelBuilder.ConfigureRoleAggregate();
        modelBuilder.ConfigureDepartmentAggregate();
        modelBuilder.ConfigureUserGroupAggregate();
        modelBuilder.ConfigureDepartmentMemberAggregate();
        modelBuilder.ConfigureExternalIdentityProviderAggregate();
        modelBuilder.ConfigureExternalIdentityProviderTemplateAggregate();
        modelBuilder.ConfigurePermissionSpaceAggregate();
        modelBuilder.ConfigurePermissionAggregate();
        modelBuilder.ConfigurePositionAggregate();
    }
}
using BeniceSoft.OpenAuthing.DepartmentMembers;
using BeniceSoft.OpenAuthing.Departments;
using BeniceSoft.OpenAuthing.EntityTypeConfigurations;
using BeniceSoft.OpenAuthing.Extensions;
using BeniceSoft.OpenAuthing.IdentityProviders;
using BeniceSoft.OpenAuthing.IdentityProviderTemplates;
using BeniceSoft.OpenAuthing.OpenIddict.Applications;
using BeniceSoft.OpenAuthing.OpenIddict.Authorizations;
using BeniceSoft.OpenAuthing.OpenIddict.Scopes;
using BeniceSoft.OpenAuthing.OpenIddict.Tokens;
using BeniceSoft.OpenAuthing.PermissionSpaces;
using BeniceSoft.OpenAuthing.Roles;
using BeniceSoft.OpenAuthing.UserGroups;
using BeniceSoft.OpenAuthing.Users;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace BeniceSoft.OpenAuthing;

[ConnectionStringName("Default")]
public class AuthingDbContext : AbpDbContext<AuthingDbContext>
{
    public AuthingDbContext(DbContextOptions<AuthingDbContext> options) : base(options)
    {
    }

    #region OpenIddict

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureOpenIddict();

        modelBuilder.ConfigureUserAggregate();
        modelBuilder.ConfigureRoleAggregate();
        modelBuilder.ConfigureDepartmentAggregate();
        modelBuilder.ConfigureUserGroupAggregate();
        modelBuilder.ConfigureDepartmentMemberAggregate();
        modelBuilder.ConfigureExternalIdentityProviderAggregate();
        modelBuilder.ConfigureExternalIdentityProviderTemplateAggregate();
        modelBuilder.ConfigurePermissionSpaceAggregate();
    }
}
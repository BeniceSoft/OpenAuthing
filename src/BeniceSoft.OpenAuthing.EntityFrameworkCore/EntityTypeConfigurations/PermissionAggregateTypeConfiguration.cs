using BeniceSoft.OpenAuthing.Entities.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace BeniceSoft.OpenAuthing.EntityTypeConfigurations;

internal static class PermissionAggregateTypeConfiguration
{
    internal static void ConfigurePermissionAggregate(this ModelBuilder modelBuilder)
    {
        ConfigurePermission(modelBuilder.Entity<Permission>());
        ConfigurePermissionGrant(modelBuilder.Entity<PermissionGrant>());
    }

    private static void ConfigurePermission(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(AuthingDbProperties.DbTablePrefix + "Permissions", AuthingDbProperties.DbSchema);

        builder.ConfigureByConvention();

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).HasMaxLength(128).IsRequired();
        builder.Property(x => x.DisplayName).HasMaxLength(256).IsRequired();
        builder.Property(x => x.SystemId).IsRequired();
        builder.Property(x => x.SystemCode).HasMaxLength(64).IsRequired();
        builder.Property(x => x.ParentName).IsRequired(false).HasMaxLength(128);

        builder.HasIndex(x => new { x.SystemId, x.SystemCode, x.Name }).IsUnique();
        builder.HasIndex(x => x.ParentName);

        builder.ApplyObjectExtensionMappings();
    }

    private static void ConfigurePermissionGrant(EntityTypeBuilder<PermissionGrant> builder)
    {
        builder.ToTable(AuthingDbProperties.DbTablePrefix + "PermissionGrants", AuthingDbProperties.DbSchema);

        builder.ConfigureByConvention();

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).HasMaxLength(128).IsRequired();
        builder.Property(x => x.ProviderName).HasMaxLength(64).IsRequired();
        builder.Property(x => x.ProviderKey).HasMaxLength(64).IsRequired();
        builder.Property(x => x.SystemCode).HasMaxLength(64).IsRequired();

        builder.HasIndex(x => new { x.SystemCode, x.Name, x.ProviderName, x.ProviderKey }).IsUnique();

        builder.ApplyObjectExtensionMappings();
    }
}
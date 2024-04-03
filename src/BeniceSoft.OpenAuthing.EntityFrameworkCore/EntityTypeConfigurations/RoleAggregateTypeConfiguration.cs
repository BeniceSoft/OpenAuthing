using BeniceSoft.OpenAuthing.Entities.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace BeniceSoft.OpenAuthing.EntityTypeConfigurations;

internal static class RoleAggregateTypeConfiguration
{
    internal static void ConfigureRoleAggregate(this ModelBuilder modelBuilder)
    {
        ConfigureRole(modelBuilder.Entity<Role>());
        ConfigureRoleSubject(modelBuilder.Entity<RoleSubject>());
    }

    private static void ConfigureRoleSubject(EntityTypeBuilder<RoleSubject> builder)
    {
        builder.ToTable(AuthingDbProperties.DbTablePrefix + "RoleSubjects", x => x.HasComment("角色主体"));
        builder.ConfigureByConvention();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        builder.Property(x => x.RoleId)
            .IsRequired()
            .HasComment("角色id");
        builder.Property(x => x.SubjectType)
            .IsRequired()
            .HasComment("主体类型(0:用户 1:用户组)");
        builder.Property(x => x.SubjectId)
            .IsRequired()
            .HasComment("主体id");

        builder.HasIndex(x => new { x.SubjectType, x.SubjectId, x.RoleId })
            .IsUnique();
    }

    private static void ConfigureRole(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(AuthingDbProperties.DbTablePrefix + "Roles", x => x.HasComment("角色"));
        builder.ConfigureByConvention();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("角色名");
        builder.Property(x => x.NormalizedName)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("归一化后的角色名");
        builder.Property(x => x.DisplayName)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("显示名");
        builder.Property(x => x.Description)
            .IsRequired(false)
            .HasMaxLength(500)
            .HasComment("描述");
        builder.Property(x => x.Enabled)
            .IsRequired()
            .HasDefaultValue(true)
            .HasComment("是否启用");
        builder.Property(x => x.IsSystemBuiltIn)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("是否系统内置");
        builder.Property(x => x.PermissionSpaceId)
            .IsRequired()
            .HasComment("所属权限空间");

        builder.Metadata.FindNavigation(nameof(Role.Subjects))?.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(x => x.PermissionSpaceId);
    }
}
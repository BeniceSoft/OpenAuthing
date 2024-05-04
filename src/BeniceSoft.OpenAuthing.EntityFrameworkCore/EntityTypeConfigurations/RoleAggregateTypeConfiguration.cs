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
        builder.ToTable(AuthingDbProperties.DbTablePrefix + "RoleSubjects", x => x.HasComment("Role Subjects"));
        builder.ConfigureByConvention();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.RoleId).IsRequired();
        builder.Property(x => x.SubjectType).IsRequired().HasComment("0:User 1:UserGroup");
        builder.Property(x => x.SubjectId).IsRequired();

        builder.HasIndex(x => new {x.SubjectType, x.SubjectId, x.RoleId})
            .IsUnique();
    }

    private static void ConfigureRole(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(AuthingDbProperties.DbTablePrefix + "Roles", x => x.HasComment("Roles"));
        builder.ConfigureByConvention();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.NormalizedName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Description).IsRequired(false).HasMaxLength(500);
        builder.Property(x => x.Enabled).IsRequired().HasDefaultValue(true);
        builder.Property(x => x.IsSystemBuiltIn).IsRequired().HasDefaultValue(false);

        builder.Metadata.FindNavigation(nameof(Role.Subjects))?.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(x => x.NormalizedName).IsUnique();
    }
}
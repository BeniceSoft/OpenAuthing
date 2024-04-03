using BeniceSoft.OpenAuthing.Entities.PermissionSpaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace BeniceSoft.OpenAuthing.EntityTypeConfigurations;

internal static class PermissionSpaceAggregateTypeConfiguration
{
    internal static void ConfigurePermissionSpaceAggregate(this ModelBuilder modelBuilder)
    {
        ConfigurePermissionSpace(modelBuilder.Entity<PermissionSpace>());
    }

    private static void ConfigurePermissionSpace(EntityTypeBuilder<PermissionSpace> builder)
    {
        builder.ToTable(AuthingDbProperties.DbTablePrefix + "PermissionSpaces", b => b
            .HasComment("权限空间"));
        builder.ConfigureByConvention();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("名称");
        builder.Property(x => x.NormalizedName)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("标准化名称");
        builder.Property(x => x.DisplayName)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("现实名称");
        builder.Property(x => x.Description)
            .IsRequired(false)
            .HasMaxLength(500)
            .HasComment("描述");
        builder.Property(x => x.IsSystemBuiltIn)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("是否系统内置");
    }
}
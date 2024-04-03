using BeniceSoft.OpenAuthing.Entities.Departments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace BeniceSoft.OpenAuthing.EntityTypeConfigurations;

internal static class DepartmentAggregateTypeConfiguration
{
    internal static void ConfigureDepartmentAggregate(this ModelBuilder modelBuilder)
    {
        ConfigureDepartment(modelBuilder.Entity<Department>());
    }

    private static void ConfigureDepartment(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable(AuthingDbProperties.DbTablePrefix + "Departments", x => x.HasComment("部门"));
        builder.ConfigureByConvention();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("名称");
        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("编码");
        builder.Property(x => x.ParentId)
            .HasComment("父级Id");
        builder.Property(x => x.Seq)
            .HasComment("排序");
        builder.Property(x => x.Path)
            .HasMaxLength(1000)
            .HasComment("路径");

        builder.Metadata.FindNavigation(nameof(Department.Children))?
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne(s => s.Parent)
            .WithMany(s => s.Children)
            .HasForeignKey(s => s.ParentId);
    }
}
using BeniceSoft.OpenAuthing.Entities.DepartmentMembers;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace BeniceSoft.OpenAuthing.EntityTypeConfigurations;

internal static class DepartmentMemberAggregateTypeConfiguration
{
    internal static void ConfigureDepartmentMemberAggregate(this ModelBuilder modelBuilder)
    {
        var builder = modelBuilder.Entity<DepartmentMember>();

        builder.ToTable(AuthingDbProperties.DbTablePrefix + "DepartmentMembers", x => x.HasComment("部门成员"));
        builder.ConfigureByConvention();
        builder.HasKey(x => new {x.DepartmentId, x.UserId});
        builder.Property(x => x.DepartmentId)
            .IsRequired()
            .HasComment("部门ID");
        builder.Property(x => x.UserId)
            .IsRequired()
            .HasComment("用户ID");
        builder.Property(x => x.IsLeader)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("是否负责人");
        builder.Property(x => x.IsMain)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("是否主部门");
    }
}
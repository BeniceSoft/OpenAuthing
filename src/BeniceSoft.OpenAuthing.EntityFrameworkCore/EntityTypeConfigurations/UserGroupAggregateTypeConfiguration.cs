﻿using BeniceSoft.OpenAuthing.UserGroups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace BeniceSoft.OpenAuthing.EntityTypeConfigurations;

internal static class UserGroupAggregateTypeConfiguration
{
    internal static void ConfigureUserGroupAggregate(this ModelBuilder modelBuilder)
    {
        ConfigureUserGroup(modelBuilder.Entity<UserGroup>());
        ConfigureUserGroupMember(modelBuilder.Entity<UserGroupMember>());
    }

    private static void ConfigureUserGroupMember(EntityTypeBuilder<UserGroupMember> builder)
    {
        builder.ToTable(EfConstants.TablePrefix + "UserGroupMembers", x => x.HasComment("用户组成员"));
        builder.ConfigureByConvention();
        builder.HasKey(x => new { x.UserGroupId, x.UserId });
        builder.Property(x => x.UserGroupId)
            .IsRequired();
        builder.Property(x => x.UserId)
            .IsRequired();
    }

    private static void ConfigureUserGroup(EntityTypeBuilder<UserGroup> builder)
    {
        builder.ToTable(EfConstants.TablePrefix + "UserGroups", x => x.HasComment("用户组"));
        builder.ConfigureByConvention();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("名称");
        builder.Property(x => x.DisplayName)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("显示名称");
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500)
            .HasComment("描述");
        builder.Property(x => x.Enabled)
            .HasComment("是否启用");

        builder.Metadata.FindNavigation(nameof(UserGroup.Members))?
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
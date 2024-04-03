using BeniceSoft.OpenAuthing.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace BeniceSoft.OpenAuthing.EntityTypeConfigurations;

internal static class UserAggregateTypeConfiguration
{
    internal static void ConfigureUserAggregate(this ModelBuilder modelBuilder)
    {
        ConfigureUser(modelBuilder.Entity<User>());
        ConfigureUserLogin(modelBuilder.Entity<UserLogin>());
        ConfigureUserToken(modelBuilder.Entity<UserToken>());
    }

    private static void ConfigureUserToken(EntityTypeBuilder<UserToken> builder)
    {
        builder.ToTable(AuthingDbProperties.DbTablePrefix + "UserTokens", x => x.HasComment("用户token"));

        builder.ConfigureByConvention();

        builder.HasKey(l => new { l.UserId, l.LoginProvider, l.Name });

        builder.Property(ul => ul.LoginProvider)
            .HasMaxLength(64)
            .IsRequired();
        builder.Property(ul => ul.Name)
            .HasMaxLength(128)
            .IsRequired();

        builder.ApplyObjectExtensionMappings();
    }

    private static void ConfigureUserLogin(EntityTypeBuilder<UserLogin> builder)
    {
        builder.ToTable(AuthingDbProperties.DbTablePrefix + "UserLogins", x => x.HasComment("用户第三方登录信息"));
        builder.ConfigureByConvention();
        builder.HasKey(x => new { x.UserId, x.LoginProvider });
        builder.Property(x => x.UserId)
            .IsRequired()
            .HasComment("用户id");
        builder.Property(x => x.LoginProvider)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("登录提供程序名称");
        builder.Property(x => x.ProviderKey)
            .IsRequired()
            .HasMaxLength(500)
            .HasComment("此登录程序的唯一标识");
        builder.Property(x => x.ProviderDisplayName)
            .IsRequired(false)
            .HasMaxLength(500)
            .HasComment("此登录程序的显示名");
    }

    private static void ConfigureUser(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(AuthingDbProperties.DbTablePrefix + "Users", x => x.HasComment("用户"));
        builder.ConfigureByConvention();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserName)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("用户名");
        builder.Property(x => x.NormalizedUserName)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("归一化后的用户名");
        builder.Property(x => x.Nickname)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("昵称");
        builder.Property(x => x.PasswordHash)
            .IsRequired()
            .HasMaxLength(500)
            .HasComment("哈希后的密码");
        builder.Property(x => x.PhoneNumber)
            .IsRequired(false)
            .HasMaxLength(50)
            .HasComment("手机号码");
        builder.Property(x => x.PhoneNumberConfirmed)
            .IsRequired()
            .HasComment("手机号码是否确认");
        builder.Property(x => x.Avatar)
            .IsRequired(false)
            .HasMaxLength(1000)
            .HasComment("头像");
        builder.Property(x => x.Gender)
            .IsRequired(false)
            .HasMaxLength(10)
            .HasComment("性别");
        builder.Property(x => x.LockoutEnd)
            .IsRequired(false)
            .HasComment("锁定结束时间");
        builder.Property(x => x.LockoutEnabled)
            .IsRequired()
            .HasComment("锁定状态");
        builder.Property(x => x.AccessFailedCount)
            .IsRequired()
            .HasDefaultValue(0)
            .HasComment("访问错误次数");
        builder.Property(x => x.SecurityStamp)
            .IsRequired(false)
            .HasMaxLength(1000)
            .HasComment("安全凭证");
        builder.Property(x => x.JobTitle)
            .IsRequired(false)
            .HasMaxLength(200)
            .HasComment("职务");
        builder.Property(x => x.TwoFactorEnabled)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("是否启用2FA");
        builder.Property(x => x.IsSystemBuiltIn)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("是否系统内置");

        builder.Metadata.FindNavigation(nameof(User.Logins))?.SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.Metadata.FindNavigation(nameof(User.Tokens))?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
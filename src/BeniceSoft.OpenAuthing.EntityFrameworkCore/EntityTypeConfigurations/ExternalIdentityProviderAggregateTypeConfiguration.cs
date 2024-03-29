using BeniceSoft.OpenAuthing.IdentityProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace BeniceSoft.OpenAuthing.EntityTypeConfigurations;

internal static class ExternalIdentityProviderAggregateTypeConfiguration
{
    internal static void ConfigureExternalIdentityProviderAggregate(this ModelBuilder modelBuilder)
    {
        ConfigureExternalIdentityProvider(modelBuilder.Entity<ExternalIdentityProvider>());
        ConfigureExternalIdentityProviderOption(modelBuilder.Entity<ExternalIdentityProviderOption>());
    }

    private static void ConfigureExternalIdentityProvider(EntityTypeBuilder<ExternalIdentityProvider> builder)
    {
        builder.ToTable(AuthingDbProperties.DbTablePrefix + "ExternalIdentityProviders", b => b.HasComment("外部身份提供者"));
        builder.ConfigureByConvention();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        builder.Property(x => x.ProviderName)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("提供者名称");
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("名称（唯一）");
        builder.Property(x => x.DisplayName)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("显示名称");
        builder.Property(x => x.Enabled)
            .IsRequired()
            .HasComment("是否启用");

        builder.Metadata.FindNavigation(nameof(ExternalIdentityProvider.Options))?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureExternalIdentityProviderOption(EntityTypeBuilder<ExternalIdentityProviderOption> builder)
    {
        builder.ToTable(AuthingDbProperties.DbTablePrefix + "ExternalIdentityProviderOptions", b => b.HasComment("外部身份提供者配置"));
        builder.ConfigureByConvention();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        builder.Property(x => x.Key)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("键");
        builder.Property(x => x.Value)
            .IsRequired(false)
            .HasComment("值");
    }
}
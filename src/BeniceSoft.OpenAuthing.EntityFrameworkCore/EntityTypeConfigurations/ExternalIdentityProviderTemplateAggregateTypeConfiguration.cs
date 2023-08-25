using BeniceSoft.OpenAuthing.IdentityProviderTemplates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace BeniceSoft.OpenAuthing.EntityTypeConfigurations;

internal static class ExternalIdentityProviderTemplateAggregateTypeConfiguration
{
    internal static void ConfigureExternalIdentityProviderTemplateAggregate(this ModelBuilder modelBuilder)
    {
        ConfigureExternalIdentityProviderTemplate(modelBuilder.Entity<ExternalIdentityProviderTemplate>());
        ConfigureExternalIdentityProviderTemplateField(modelBuilder.Entity<ExternalIdentityProviderTemplateField>());
    }

    private static void ConfigureExternalIdentityProviderTemplate(EntityTypeBuilder<ExternalIdentityProviderTemplate> builder)
    {
        builder.ToTable(EfConstants.TablePrefix + "ExternalIdentityProviderTemplates", b => b.HasComment("外部身份提供者模板"));
        builder.ConfigureByConvention();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("名称（唯一）");
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("标题");
        builder.Property(x => x.Description)
            .IsRequired(false)
            .HasMaxLength(500)
            .HasComment("描述");
        builder.Property(x => x.Logo)
            .IsRequired(false)
            .HasMaxLength(500)
            .HasComment("LOGO");

        builder.HasIndex(x => x.Name)
            .IsUnique()
            .HasDatabaseName("IDX_ExternalIdentityProviderTemplates_Name");
    }

    private static void ConfigureExternalIdentityProviderTemplateField(EntityTypeBuilder<ExternalIdentityProviderTemplateField> builder)
    {
        builder.ToTable(EfConstants.TablePrefix + "ExternalIdentityProviderTemplateFields", b => b.HasComment("外部身份提供者模板字段"));
        builder.ConfigureByConvention();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("字段名");
        builder.Property(x => x.Label)
            .IsRequired(false)
            .HasMaxLength(200)
            .HasComment("显示文本");
        builder.Property(x => x.Placeholder)
            .IsRequired(false)
            .HasMaxLength(500)
            .HasComment("站位文本");
        builder.Property(x => x.Required)
            .IsRequired()
            .HasComment("是否必填");
        builder.Property(x => x.Type)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("类型");
        builder.Property(x => x.HelpText)
            .IsRequired(false)
            .HasMaxLength(500)
            .HasComment("帮助文本");
        builder.Property(x => x.ExtraData)
            .IsRequired(false)
            .HasComment("扩展数据");
        builder.Property(x => x.Order)
            .IsRequired()
            .HasComment("排序（正序）");
    }
}
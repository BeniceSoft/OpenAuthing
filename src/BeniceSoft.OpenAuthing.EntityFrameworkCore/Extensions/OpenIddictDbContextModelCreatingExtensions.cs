using BeniceSoft.OpenAuthing.OpenIddict.Applications;
using BeniceSoft.OpenAuthing.OpenIddict.Authorizations;
using BeniceSoft.OpenAuthing.OpenIddict.Scopes;
using BeniceSoft.OpenAuthing.OpenIddict.Tokens;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace BeniceSoft.OpenAuthing.Extensions;

public static class OpenIddictDbContextModelCreatingExtensions
{
    public static void ConfigureOpenIddict(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        if (builder.IsTenantOnlyDatabase())
        {
            return;
        }

        builder.Entity<OpenIddictApplication>(b =>
        {
            b.ToTable(EfConstants.OpenIddictTablePrefix + "Applications");

            b.ConfigureByConvention();

            b.HasIndex(x => x.ClientId);
                //.IsUnique();

            b.Property(x => x.ClientId)
                .HasMaxLength(100);

            b.Property(x => x.ConsentType)
                .HasMaxLength(50);

            b.Property(x => x.Type)
                .HasMaxLength(50);

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<OpenIddictAuthorization>(b =>
        {
            b.ToTable(EfConstants.OpenIddictTablePrefix + "Authorizations");

            b.ConfigureByConvention();

            b.HasIndex(x => new
            {
                x.ApplicationId,
                x.Status,
                x.Subject,
                x.Type
            });

            b.Property(x => x.Status)
                .HasMaxLength(50);

            b.Property(x => x.Subject)
                .HasMaxLength(400);

            b.Property(x => x.Type)
                .HasMaxLength(50);

            b.HasOne<OpenIddictApplication>().WithMany().HasForeignKey(x => x.ApplicationId).IsRequired(false);

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<OpenIddictScope>(b =>
        {
            b.ToTable(EfConstants.OpenIddictTablePrefix + "Scopes");

            b.ConfigureByConvention();

            b.HasIndex(x => x.Name);
                //.IsUnique();

            b.Property(x => x.Name)
                .HasMaxLength(200);

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<OpenIddictToken>(b =>
        {
            b.ToTable(EfConstants.OpenIddictTablePrefix + "Tokens");

            b.ConfigureByConvention();

            b.HasIndex(x => x.ReferenceId);
                //.IsUnique();

            b.HasIndex(x => new
            {
                x.ApplicationId,
                x.Status,
                x.Subject,
                x.Type
            });

            b.Property(x => x.ReferenceId)
                .HasMaxLength(100);

            b.Property(x => x.Status)
                .HasMaxLength(50);

            b.Property(x => x.Subject)
                .HasMaxLength(400);

            b.Property(x => x.Type)
                .HasMaxLength(50);

            b.HasOne<OpenIddictApplication>().WithMany().HasForeignKey(x => x.ApplicationId).IsRequired(false);
            b.HasOne<OpenIddictAuthorization>().WithMany().HasForeignKey(x => x.AuthorizationId).IsRequired(false);

            b.ApplyObjectExtensionMappings();
        });

    }
}

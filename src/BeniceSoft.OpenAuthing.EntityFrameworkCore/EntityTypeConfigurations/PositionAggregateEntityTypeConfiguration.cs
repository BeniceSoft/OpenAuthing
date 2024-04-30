using BeniceSoft.OpenAuthing.Entities.Positions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeniceSoft.OpenAuthing.EntityTypeConfigurations;

internal static class PositionAggregateEntityTypeConfiguration
{
    internal static void ConfigurePositionAggregate(this ModelBuilder modelBuilder)
    {
        ConfigurePosition(modelBuilder.Entity<Position>());
    }

    private static void ConfigurePosition(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable(AuthingDbProperties.DbTablePrefix + "Positions", AuthingDbProperties.DbSchema);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Description).HasMaxLength(512);
    }
}
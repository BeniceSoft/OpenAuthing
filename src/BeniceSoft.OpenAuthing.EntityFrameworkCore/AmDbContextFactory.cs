using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BeniceSoft.OpenAuthing;

public class AmDbContextFactory : IDesignTimeDbContextFactory<AmDbContext>
{
    public AmDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<AmDbContext>()
            .UseNpgsql(configuration.GetConnectionString("Default"));

        return new AmDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../BeniceSoft.OpenAuthing.Web/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
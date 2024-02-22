using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BeniceSoft.OpenAuthing;

public class AuthingDbContextFactory : IDesignTimeDbContextFactory<AuthingDbContext>
{
    public AuthingDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        var connectionString = configuration.GetConnectionString("Default")!;

        var builder = new DbContextOptionsBuilder<AuthingDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new AuthingDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../BeniceSoft.OpenAuthing.API/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
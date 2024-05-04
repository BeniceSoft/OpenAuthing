using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Volo.Abp.OpenIddict;
using Volo.Abp.SettingManagement;

namespace BeniceSoft.OpenAuthing;

public class AuthingDesignTimeDbContextFactory : IDesignTimeDbContextFactory<AuthingDbContext>
{
    public AuthingDbContext CreateDbContext(string[] args)
    {
        AbpSettingManagementDbProperties.DbSchema = null;
        AbpSettingManagementDbProperties.DbTablePrefix = AuthingDbProperties.DbTablePrefix;
        
        AbpOpenIddictDbProperties.DbSchema = null;
        AbpOpenIddictDbProperties.DbTablePrefix = AuthingDbProperties.OpenIddictDbTablePrefix;
        
        var configuration = BuildConfiguration();
        var connectionString = configuration.GetConnectionString("Default")!;

        var builder = new DbContextOptionsBuilder<AuthingDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new AuthingDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../BeniceSoft.OpenAuthing.AdminApi/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.override.json", optional: true);

        return builder.Build();
    }
}
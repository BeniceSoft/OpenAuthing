using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BeniceSoft.OpenAuthing.Configuration.Extensions;

public static class HostingHostBuilderExtensions
{
    public const string AppSettingsSecretJsonPath = "appsettings.override.json";

    public static IHostBuilder AddAppSettingsOverrideJson(
        this IHostBuilder hostBuilder,
        bool optional = true,
        bool reloadOnChange = false,
        string path = AppSettingsSecretJsonPath)
    {
        return hostBuilder.ConfigureAppConfiguration((_, builder) =>
        {
            builder.AddJsonFile(
                path: path,
                optional: optional,
                reloadOnChange: reloadOnChange
            );
        });
    }
}
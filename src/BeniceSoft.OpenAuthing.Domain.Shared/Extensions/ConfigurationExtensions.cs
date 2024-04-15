using Microsoft.Extensions.Configuration;

namespace BeniceSoft.OpenAuthing.Extensions;

public static class ConfigurationExtensions
{
    public static T EnsureGetValue<T>(this IConfiguration configuration, string key)
    {
        var value = configuration.GetValue<T>(key) ??
                    throw new ArgumentException($"{key} can not be null or empty!", key);

        return value;
    }
}
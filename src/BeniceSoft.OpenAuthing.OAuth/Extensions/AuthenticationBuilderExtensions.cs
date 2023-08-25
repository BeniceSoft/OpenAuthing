using BeniceSoft.OpenAuthing.DynamicAuth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Extensions;

/// <summary>
/// AuthenticationBuilder extensions
/// </summary>
public static class AuthenticationBuilderExtensions
{
    /// <summary>
    /// Configures the DI for dynamic scheme management.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static DynamicAuthenticationBuilder AddDynamic(this AuthenticationBuilder builder)
    {
        var dynamicAuthenticationBuilder = new DynamicAuthenticationBuilder(builder.Services);

        builder.Services
            .AddTransient<IDynamicAuthenticationManager>(provider => new DynamicAuthenticationManager
            (
                provider.GetRequiredService<IAuthenticationSchemeProvider>(),
                provider.GetRequiredService<OAuthOptionsMonitorCacheWrapperFactory>(),
                dynamicAuthenticationBuilder.HandlerTypes
            ));

        return dynamicAuthenticationBuilder;
    }
}
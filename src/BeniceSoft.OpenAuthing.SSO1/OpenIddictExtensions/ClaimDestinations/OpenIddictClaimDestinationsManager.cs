using System.Security.Claims;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.OpenIddictExtensions.ClaimDestinations;

public class OpenIddictClaimDestinationsManager : ISingletonDependency
{
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected IOptions<OpenIddictClaimDestinationsOptions> Options { get; }

    public OpenIddictClaimDestinationsManager(IServiceScopeFactory serviceScopeFactory, IOptions<OpenIddictClaimDestinationsOptions> options)
    {
        ServiceScopeFactory = serviceScopeFactory;
        Options = options;
    }

    public virtual async Task SetAsync(ClaimsPrincipal principal)
    {
        using var scope = ServiceScopeFactory.CreateScope();
        foreach (var providerType in Options.Value.ClaimDestinationsProvider)
        {
            var provider = (IOpenIddictClaimDestinationsProvider)scope.ServiceProvider.GetRequiredService(providerType);
            await provider.SetDestinationsAsync(
                new OpenIddictClaimDestinationsProviderContext(scope.ServiceProvider, principal, principal.Claims.ToArray()));
        }
    }
}
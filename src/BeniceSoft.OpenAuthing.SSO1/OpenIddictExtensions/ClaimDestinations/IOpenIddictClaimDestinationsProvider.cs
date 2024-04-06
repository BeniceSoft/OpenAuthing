namespace BeniceSoft.OpenAuthing.OpenIddictExtensions.ClaimDestinations;

public interface IOpenIddictClaimDestinationsProvider
{
    Task SetDestinationsAsync(OpenIddictClaimDestinationsProviderContext context);
}

using Volo.Abp.Collections;

namespace BeniceSoft.OpenAuthing.OpenIddictExtensions.ClaimDestinations;

public class OpenIddictClaimDestinationsOptions
{
    public ITypeList<IOpenIddictClaimDestinationsProvider> ClaimDestinationsProvider { get; }

    public OpenIddictClaimDestinationsOptions()
    {
        ClaimDestinationsProvider = new TypeList<IOpenIddictClaimDestinationsProvider>();
    }
}

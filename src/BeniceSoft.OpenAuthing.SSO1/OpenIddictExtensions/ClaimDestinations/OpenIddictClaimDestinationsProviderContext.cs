using System.Security.Claims;

namespace BeniceSoft.OpenAuthing.OpenIddictExtensions.ClaimDestinations;

public class OpenIddictClaimDestinationsProviderContext
{
     public IServiceProvider ScopeServiceProvider { get; }

     public ClaimsPrincipal Principal{ get;}

     public Claim[] Claims { get; }

     public OpenIddictClaimDestinationsProviderContext(IServiceProvider scopeServiceProvider, ClaimsPrincipal principal, Claim[] claims)
     {
          ScopeServiceProvider = scopeServiceProvider;
          Principal = principal;
          Claims = claims;
     }
}

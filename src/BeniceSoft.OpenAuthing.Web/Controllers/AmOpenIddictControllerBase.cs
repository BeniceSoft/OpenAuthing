using System.Collections.Immutable;
using System.Security.Claims;
using BeniceSoft.OpenAuthing.OpenIddictExtensions.ClaimDestinations;
using BeniceSoft.OpenAuthing.Users;
using Microsoft.AspNetCore;
using OpenIddict.Abstractions;

namespace BeniceSoft.OpenAuthing.Controllers;

public abstract class AmOpenIddictControllerBase : AmControllerBase
{
    protected IOpenIddictApplicationManager ApplicationManager => LazyServiceProvider.LazyGetRequiredService<IOpenIddictApplicationManager>();
    protected IOpenIddictAuthorizationManager AuthorizationManager => LazyServiceProvider.LazyGetRequiredService<IOpenIddictAuthorizationManager>();
    protected IOpenIddictScopeManager ScopeManager => LazyServiceProvider.LazyGetRequiredService<IOpenIddictScopeManager>();
    protected IOpenIddictTokenManager TokenManager => LazyServiceProvider.LazyGetRequiredService<IOpenIddictTokenManager>();
    protected OpenIddictClaimDestinationsManager OpenIddictClaimDestinationsManager => LazyServiceProvider.LazyGetRequiredService<OpenIddictClaimDestinationsManager>();
    
    protected virtual Task<OpenIddictRequest> GetOpenIddictServerRequestAsync(HttpContext httpContext)
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
                      throw new InvalidOperationException(L["TheOpenIDConnectRequestCannotBeRetrieved"]);

        return Task.FromResult(request);
    }
    
    protected virtual async Task SetClaimsDestinationsAsync(ClaimsPrincipal principal)
    {
        await OpenIddictClaimDestinationsManager.SetAsync(principal);
    }
    
    protected virtual async Task<IEnumerable<string>> GetResourcesAsync(ImmutableArray<string> scopes)
    {
        var resources = new List<string>();
        if (!scopes.Any())
        {
            return resources;
        }

        await foreach (var resource in ScopeManager.ListResourcesAsync(scopes))
        {
            resources.Add(resource);
        }
        return resources;
    }
    
    protected virtual async Task<bool> HasFormValueAsync(string name)
    {
        if (Request.HasFormContentType)
        {
            var form = await Request.ReadFormAsync();
            if (!string.IsNullOrEmpty(form[name]))
            {
                return true;
            }
        }

        return false;
    }
    
    protected virtual async Task<bool> PreSignInCheckAsync(User user)
    {
        if (!user.Enabled)
        {
            return false;
        }

        if (!await SignInManager.CanSignInAsync(user))
        {
            return false;
        }

        if (await UserManager.IsLockedOutAsync(user))
        {
            return false;
        }

        return true;
    }
}
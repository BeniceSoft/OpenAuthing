using BeniceSoft.OpenAuthing.OpenIddictExtensions;
using BeniceSoft.Abp.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using Volo.Abp;

namespace BeniceSoft.OpenAuthing.Controllers;

[Route("connect/token")]
[IgnoreAntiforgeryToken]
[ApiExplorerSettings(IgnoreApi = true)]
public partial class TokenController : AuthOpenIddictControllerBase
{
    [HttpGet, HttpPost, Produces("application/json"), IgnoreJsonFormat]
    public virtual async Task<IActionResult> HandleAsync()
    {
        var request = await GetOpenIddictServerRequestAsync(HttpContext);
        
        if (request.IsAuthorizationCodeGrantType())
        {
            return await HandleAuthorizationCodeAsync(request);
        }

        if (request.IsRefreshTokenGrantType())
        {
            return await HandleRefreshTokenAsync(request);
        }

        if (request.IsDingTalkCodeGrantType())
        {
            return await HandleDingTalkCodeAsync(request);
        }

        var extensionGrantsOptions = HttpContext.RequestServices.GetRequiredService<IOptions<AmOpenIddictExtensionGrantsOptions>>();
        var extensionTokenGrantHandler = extensionGrantsOptions.Value.Find<ITokenExtensionGrantHandler>(request.GrantType!);
        if (extensionTokenGrantHandler is not null)
        {
            return await extensionTokenGrantHandler.HandleAsync(new ExtensionGrantContext(HttpContext, request));
        }

        throw new AbpException(string.Format(L["TheSpecifiedGrantTypeIsNotImplemented"], request.GrantType));
    }
}
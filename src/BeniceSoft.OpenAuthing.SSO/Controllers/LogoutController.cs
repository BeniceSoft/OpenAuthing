﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server.AspNetCore;

namespace BeniceSoft.OpenAuthing.Controllers;

[Route("connect/logout")]
[ApiExplorerSettings(IgnoreApi = true)]
public class LogoutController : AuthOpenIddictControllerBase
{
    // GET: /connect/logout
    [HttpGet]
    public virtual async Task<IActionResult> GetAsync(string? returnUrl = null)
    {
        // Ask ASP.NET Core Identity to delete the local and external cookies created
        // when the user agent is redirected from the external identity provider
        // after a successful authentication flow (e.g Google or Facebook).
        await SignInManager.SignOutAsync();

        // Returning a SignOutResult will ask OpenIddict to redirect the user agent
        // to the post_logout_redirect_uri specified by the client application or to
        // the RedirectUri specified in the authentication properties if none was set.
        return SignOut(
            authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
            properties: new AuthenticationProperties { RedirectUri = returnUrl });
    }
}
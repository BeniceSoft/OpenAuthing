using BeniceSoft.OpenAuthing.Models.Accounts;
using BeniceSoft.Abp.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeniceSoft.OpenAuthing.Controllers;

public partial class AccountController
{
    // GET: /api/account/getexternalloginproviders
    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> GetExternalLoginProvidersAsync()
    {
        var schemes = await SignInManager.GetExternalAuthenticationSchemesAsync();
        var idPs = schemes
            .Where(x => !string.IsNullOrWhiteSpace(x.Name))
            .Select(x => new { x.Name, x.DisplayName })
            .ToList();
        var results = new List<ExternalLoginProviderViewModel>(idPs.Count());

        if (idPs.Any())
        {
            var idPNames = idPs.Select(x => x.Name);
            var externalIdentityProviders = await _idPRepository.GetQueryableAsync();
            results = await externalIdentityProviders.Where(x => idPNames.Contains(x.Name))
                .Select(x => new ExternalLoginProviderViewModel
                {
                    Name = x.Name, DisplayName = x.DisplayName, ProviderName = x.ProviderName
                })
                .ToListAsync();
        }
        
        return Ok(results.ToSucceed());
    }

    // POST: /api/account/externallogin
    [HttpPost, HttpGet, AllowAnonymous]
    public async Task<IActionResult> ExternalLogin(string provider, string? returnUrl = null)
    {
        // Request a redirect to the external login provider.
        var redirectUrl = Url.Action(nameof(ExternalLoginCallback), new { returnUrl });
        var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return new ChallengeResult(provider, properties);
    }

    // GET: /api/account/externallogincallback
    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
    {
        returnUrl ??= Url.Content("~/");

        var info = await SignInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            // ErrorMessage = "Error loading external login information.";
            return RedirectToAction(nameof(Login), new { returnUrl });
        }

        // Sign in the user with this external login provider if the user already has a login.
        var result = await SignInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        if (result.Succeeded)
        {
            _logger.LogInformation("User logged in with {LoginProvider} provider.", info.LoginProvider);
            return LocalRedirect(returnUrl);
        }

        // if (result.IsLockedOut)
        // {
        //     return RedirectToPage("./Lockout");
        // }

        // If the user does not have an account, then ask the user to bind an account.
        return RedirectToAction(nameof(Login), new { returnUrl });
    }
}
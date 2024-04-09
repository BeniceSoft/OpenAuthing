using System.Net;
using BeniceSoft.OpenAuthing.Models.Accounts;
using BeniceSoft.Abp.Core.Extensions;
using BeniceSoft.Abp.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace BeniceSoft.OpenAuthing.Controllers;

public partial class AccountController
{
    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> LoginWith2Fa([FromBody] LoginWith2FaViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Ok(new ResponseResult(HttpStatusCode.BadRequest, L["InvalidParameter"]));
        }

        model.ReturnUrl ??= Url.Content("~/");

        var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();
        if (user is null)
        {
            throw new UserFriendlyException($"Unable to load two-factor authentication user");
        }

        // Strip spaces and hyphens
        var authenticatorCode = model.TwoFactorCode
            .Replace(" ", string.Empty)
            .Replace("-", string.Empty);

        var result = await SignInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, model.RememberMe, model.RememberMachine);
        await UserManager.GetUserIdAsync(user);

        if (result.Succeeded)
        {
            _logger.LogInformation("User logged in with 2FA");
            if (!Url.IsLocalUrl(model.ReturnUrl))
            {
                model.ReturnUrl = "/";
            }

            return Ok(new { model.ReturnUrl, UserInfo = user.ToViewModel() }.ToSucceed());
        }

        _logger.LogWarning("Invalid authenticator code entered");

        return Ok(new ResponseResult(HttpStatusCode.BadRequest, L["InvalidAuthenticatorCode"]));
    }

    [HttpGet]
    public async Task<IActionResult> TowFactorAuthentication()
    {
        var user = await UserManager.GetUserAsync(User);
        if (user is null)
        {
            throw new InvalidOperationException($"Unable to load user with Id '{UserManager.GetUserId(User)}'");
        }

        var model = new TowFactorAuthenticationViewModel
        {
            HasAuthenticator = await UserManager.GetAuthenticatorKeyAsync(user) is not null,
            Is2FaEnabled = await UserManager.GetTwoFactorEnabledAsync(user),
            IsMachineRemembered = await SignInManager.IsTwoFactorClientRememberedAsync(user),
            RecoveryCodesLeft = await UserManager.CountRecoveryCodesAsync(user)
        };

        return Ok(model.ToSucceed());
    }

    [HttpPost]
    public async Task<IActionResult> Disable2Fa()
    {
        var user = await UserManager.GetUserAsync(User);
        if (user is null)
        {
            throw new InvalidOperationException($"Unable to load user with Id '{UserManager.GetUserId(User)}'");
        }

        await UserManager.SetTwoFactorEnabledAsync(user, false);

        return Ok(true.ToSucceed());
    }
}
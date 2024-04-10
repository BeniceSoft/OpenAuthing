using System.Net;
using BeniceSoft.OpenAuthing.Models.Accounts;
using BeniceSoft.Abp.Core.Extensions;
using BeniceSoft.Abp.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeniceSoft.OpenAuthing.Controllers;

public partial class AccountController
{
    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> LoginWithRecoveryCode([FromBody] LoginWithRecoveryCodeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Ok(new ResponseResult(HttpStatusCode.BadRequest, L["InvalidParameter"]));
        }

        model.ReturnUrl ??= Url.Content("~/");

        var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();
        ThrowUnauthorizedIfUserIsNull(user, L["UnableToLoadTwoFactorAuthenticationUser"]);

        var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);
        var result = await SignInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

        if (result.Succeeded)
        {
            _logger.LogInformation("User logged in with a recovery code.");

            return Ok(new { model.ReturnUrl, UserInfo = user!.ToViewModel() }.ToSucceed());
        }

        _logger.LogWarning("Invalid recovery code entered.");
        return Ok(new ResponseResult(HttpStatusCode.BadRequest, L["InvalidRecoveryCode"]));
    }

    [HttpGet]
    public async Task<IActionResult> GetRecoveryCodes()
    {
        var user = await UserManager.GetUserAsync(User);
        if (user is null)
        {
            throw new InvalidOperationException($"Unable to load user with Id '{UserManager.GetUserId(User)}'");
        }

        var tokens = await UserManager.GetTwoFactorRecoveryCodesAsync(user) ?? Enumerable.Empty<string>();

        return Ok(tokens.ToList().ToSucceed());
    }
}
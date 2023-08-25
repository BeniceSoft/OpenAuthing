using System.Net;
using BeniceSoft.OpenAuthing.Models.Accounts;
using LinkMore.Abp.Core.Extensions;
using LinkMore.Abp.Core.Models;
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
        if (user is null)
        {
            throw new InvalidOperationException("Unable to load tow factor authentication user.");
        }

        var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);
        var result = await SignInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);
        await UserManager.GetUserIdAsync(user);

        if (result.Succeeded)
        {
            _logger.LogInformation("User logged in with a recovery code.");
            if (!Url.IsLocalUrl(model.ReturnUrl))
            {
                model.ReturnUrl = "/";
            }

            return Ok(new { model.ReturnUrl, UserInfo = user.ToViewModel() }.ToSucceed());
        }

        _logger.LogWarning("Invalid recovery code entered.");
        return Ok(new ResponseResult(HttpStatusCode.BadRequest, L["InvalidRecoveryCode"]));
    }
}
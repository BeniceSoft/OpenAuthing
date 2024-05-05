using System.Net;
using System.Text;
using BeniceSoft.Abp.Core.Extensions;
using BeniceSoft.Abp.Core.Models;
using BeniceSoft.OpenAuthing.Entities.Users;
using BeniceSoft.OpenAuthing.Exceptions;
using BeniceSoft.OpenAuthing.Models.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace BeniceSoft.OpenAuthing.Controllers;

public partial class AccountController
{
    private IEmailSender<User> EmailSender => LazyServiceProvider.GetRequiredService<IEmailSender<User>>();

    // POST: /api/account/login
    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        model.ReturnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user is null)
            {
                return Ok(new ResponseResult(HttpStatusCode.BadRequest, L["IncorrectUsernameOrPassword"]));
            }

            var result = await SignInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

            if (result.Succeeded)
            {
                return Ok(new
                {
                    LoginSuccess = true,
                    model.ReturnUrl,
                    UserInfo = user.ToViewModel()
                }.ToSucceed());
            }

            // 被锁定
            if (result.IsLockedOut)
            {
                return Ok(new ResponseResult(HttpStatusCode.Locked, L["AccountIsLockedOut"]));
            }

            if (result.RequiresTwoFactor)
            {
                return Ok(new
                {
                    result.RequiresTwoFactor,
                    model.ReturnUrl,
                    model.RememberMe
                }.ToSucceed());
            }
        }

        return Ok(new ResponseResult(HttpStatusCode.BadRequest, L["IncorrectUsernameOrPassword"]));
    }

    // POST: /api/account/forgotpassword
    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordInputModel input)
    {
        var user = await UserManager.FindByEmailAsync(input.Email);
        if (user is null) ThrowLocalizedAuthingBizException(ErrorCodes.UserNotFound);

        var code = await UserManager.GeneratePasswordResetTokenAsync(user!);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var link = AppUrl.EnsureEndsWith('/') + $"account/reset-password?uid={user.Id}&code={code}";
        var emailAddress = await UserManager.GetEmailAsync(user);
        await EmailSender.SendPasswordResetLinkAsync(user, emailAddress!, link);

        return Ok();
    }

    // POST: /api/account/resetpassword
    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordInputModel input)
    {
        var user = await UserManager.FindByIdAsync(input.Uid);
        if (user is null) ThrowLocalizedAuthingBizException(ErrorCodes.UserNotFound);

        var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(input.Code));
        var result = await UserManager.ResetPasswordAsync(user!, token, input.Password);
        if (result.Succeeded)
        {
            return Ok();
        }

        return BadRequest();
    }

    // POST: /api/account/changepassword
    [HttpPost]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordInputModel input)
    {
        var user = await UserManager.GetUserAsync(User);
        ThrowUnauthorizedIfUserIsNull(user);

        var result = await UserManager.ChangePasswordAsync(user!, input.CurrentPassword, input.NewPassword);
        if (!result.Succeeded)
        {
            ThrowLocalizedAuthingBizException(ErrorCodes.ChangePasswordFailed);
        }

        return Ok();
    }
}
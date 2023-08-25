using System.Globalization;
using System.Net;
using System.Text;
using BeniceSoft.OpenAuthing.Models.Accounts;
using BeniceSoft.Abp.Core.Extensions;
using BeniceSoft.Abp.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BeniceSoft.OpenAuthing.Controllers;

public partial class AccountController
{
    private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

    [HttpGet]
    public async Task<IActionResult> GenerateAuthenticatorUri()
    {
        var user = await UserManager.GetUserAsync(User);
        if (user is null)
        {
            throw new InvalidOperationException($"Unable to load user with Id '{UserManager.GetUserId(User)}'");
        }

        // Load the authenticator key and QR code URI to display on the form
        var unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrWhiteSpace(unformattedKey))
        {
            await UserManager.ResetAuthenticatorKeyAsync(user);
            unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
        }

        var phoneNumber = await UserManager.GetPhoneNumberAsync(user);
        return Ok(new
        {
            SharedKey = FormatKey(unformattedKey!),
            AuthenticatorUri = GenerateQrCodeUri(phoneNumber!, unformattedKey!)
        });
    }

    [HttpPost]
    public async Task<IActionResult> EnableAuthenticator([FromBody] EnableAuthenticatorViewModel model)
    {
        var user = await UserManager.GetUserAsync(User);
        if (user is null)
        {
            throw new InvalidOperationException($"Unable to load user with Id '{UserManager.GetUserId(User)}'");
        }

        // Strip spaces and hyphens
        var verificationCode = model.Code
            .Replace(" ", string.Empty)
            .Replace("-", string.Empty);

        var is2FaTokenValid = await UserManager.VerifyTwoFactorTokenAsync(user,
            UserManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

        if (!is2FaTokenValid)
        {
            return Ok(new ResponseResult(HttpStatusCode.BadRequest, L["TwoFactorTokenInvalid"]));
        }

        await UserManager.SetTwoFactorEnabledAsync(user, true);
        await UserManager.GetUserIdAsync(user);
        _logger.LogInformation("User has enabled 2FA with an authenticator app");

        var recoveryCodes = await UserManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
        return Ok(new
        {
            RecoveryCodes = recoveryCodes
        }.ToSucceed());
    }

    private string GenerateQrCodeUri(string phoneNumber, string unformattedKey)
    {
        return string.Format(
            CultureInfo.InvariantCulture,
            AuthenticatorUriFormat,
            _urlEncoder.Encode("BeniceSoft OpenAuthing"),
            _urlEncoder.Encode(phoneNumber),
            unformattedKey);
    }

    private static string FormatKey(string unformattedKey)
    {
        var result = new StringBuilder();
        var currentPosition = 0;
        while (currentPosition + 4 < unformattedKey.Length)
        {
            result.Append(unformattedKey.AsSpan(currentPosition, 4)).Append(' ');
            currentPosition += 4;
        }

        if (currentPosition < unformattedKey.Length)
        {
            result.Append(unformattedKey.AsSpan(currentPosition));
        }

        return result.ToString().ToLowerInvariant();
    }
}
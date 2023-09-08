using BeniceSoft.OpenAuthing.OpenIddictExtensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace BeniceSoft.OpenAuthing.Controllers;

public partial class TokenController
{
    protected async Task<IActionResult> HandleDingTalkCodeAsync(OpenIddictRequest request)
    {
        var code = request.GetParameter(DingTalkCodeGrantConstants.CodeParameterName)?.ToString();

        if (string.IsNullOrWhiteSpace(code))
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The code cannot be null or empty."
                }!));
        }

        var client = LazyServiceProvider.LazyGetRequiredService<IAmDingTalkClient>();
        var dingTalkUserInfo = await client.GetUserInfoByCodeAsync(code);

        var user = await UserManager.FindByLoginAsync(AmConstants.LoginProviders.DingTalk, dingTalkUserInfo.UnionId);
        if (user is null)
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = $"User not found, UnionId:{dingTalkUserInfo.UnionId}"
                }!));
        }
        
        // Ensure the user is still allowed to sign in.
        if (!await PreSignInCheckAsync(user))
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is no longer allowed to sign in."
                }!));
        }

        var userPrincipal = await SignInManager.CreateUserPrincipalAsync(user);
        
        await SetClaimsDestinationsAsync(userPrincipal);

        return SignIn(userPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}
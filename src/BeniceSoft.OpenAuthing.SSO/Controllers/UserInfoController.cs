using BeniceSoft.Abp.Core.Attributes;
using BeniceSoft.OpenAuthing.Entities.Roles;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace BeniceSoft.OpenAuthing.Controllers;

[Route("connect/userinfo")]
[IgnoreAntiforgeryToken]
[Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
[ApiExplorerSettings(IgnoreApi = true)]
public class UserInfoController : AuthOpenIddictControllerBase
{
    [HttpGet, HttpPost, Produces("application/json"), IgnoreJsonFormat]
    public virtual async Task<IActionResult> Userinfo()
    {
        var user = await UserManager.GetUserAsync(User);
        if (user is null)
        {
            return Challenge(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidToken,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "The specified access token is bound to an account that no longer exists."
                }!));
        }

        var claims = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            // Note: the "sub" claim is a mandatory claim and must be included in the JSON response.
            [OpenIddictConstants.Claims.Subject] = await UserManager.GetUserIdAsync(user)
        };

        if (User.HasScope(OpenIddictConstants.Scopes.Profile))
        {
            claims[OpenIddictConstants.Claims.PreferredUsername] = user.UserName;
            claims[OpenIddictConstants.Claims.Nickname] = user.Nickname;
            claims[OpenIddictConstants.Claims.Picture] = user.Avatar ?? string.Empty;
        }

        // if (User.HasScope(OpenIddictConstants.Scopes.Email))
        // {
        //     claims[OpenIddictConstants.Claims.Email] = await UserManager.GetEmailAsync(user) ?? string.Empty;
        //     claims[OpenIddictConstants.Claims.EmailVerified] = await UserManager.IsEmailConfirmedAsync(user);
        // }

        if (User.HasScope(OpenIddictConstants.Scopes.Phone))
        {
            claims[OpenIddictConstants.Claims.PhoneNumber] = await UserManager.GetPhoneNumberAsync(user) ?? string.Empty;
            claims[OpenIddictConstants.Claims.PhoneNumberVerified] = await UserManager.IsPhoneNumberConfirmedAsync(user);
        }

        if (User.HasScope(OpenIddictConstants.Scopes.Roles))
        {
            var roleRepository = LazyServiceProvider.GetRequiredService<IRoleRepository>();

            var roles = await roleRepository.GetUserRolesAsync(user.Id);

            claims[OpenIddictConstants.Claims.Role] = roles.Select(x => new
            {
                RoleId = x.Id,
                RoleName = x.NormalizedName,
                RoleDisplayName = x.DisplayName
            });
        }

        // Note: the complete list of standard claims supported by the OpenID Connect specification
        // can be found here: http://openid.net/specs/openid-connect-core-1_0.html#StandardClaims

        return Ok(claims);
    }
}
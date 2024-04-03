using System.Security.Claims;
using BeniceSoft.Abp.Auth.Core;
using BeniceSoft.OpenAuthing.Entities.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace BeniceSoft.OpenAuthing.Entities.Users;

public class UserClaimsPrincipalFactory : IUserClaimsPrincipalFactory<User>, IScopedDependency
{
    private readonly UserManager<User> _userManager;
    private readonly IdentityOptions _options;
    private readonly IRoleRepository _roleRepository;

    public UserClaimsPrincipalFactory(UserManager<User> userManager, IOptions<IdentityOptions> optionsAccessor, IRoleRepository roleRepository)
    {
        if (optionsAccessor == null || optionsAccessor.Value == null)
        {
            throw new ArgumentNullException(nameof(optionsAccessor));
        }

        _userManager = userManager;
        _roleRepository = roleRepository;
        _options = optionsAccessor.Value;
    }

    public async Task<ClaimsPrincipal> CreateAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var id = await GenerateClaimsAsync(user);
        return new ClaimsPrincipal(id);
    }

    /// <summary>
    /// Generate the claims for a user.
    /// </summary>
    /// <param name="user">The user to create a <see cref="ClaimsIdentity"/> from.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous creation operation, containing the created <see cref="ClaimsIdentity"/>.</returns>
    [UnitOfWork(true)]
    protected virtual async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {
        var userId = await _userManager.GetUserIdAsync(user);
        var userName = await _userManager.GetUserNameAsync(user);
        var id = new ClaimsIdentity("Identity.Application", // REVIEW: Used to match Application scheme
            _options.ClaimsIdentity.UserNameClaimType,
            _options.ClaimsIdentity.RoleClaimType);

        id.AddClaim(new Claim(_options.ClaimsIdentity.UserIdClaimType, userId));
        id.AddClaim(new Claim(_options.ClaimsIdentity.UserNameClaimType, userName));
        id.AddClaim(new Claim(OpenIddictConstants.Claims.Name, user.Nickname));
        id.AddClaim(new Claim(BeniceSoftAuthConstants.ClaimTypes.Avatar, user.Avatar ?? string.Empty));
        id.AddClaim(new Claim(OpenIddictConstants.Claims.PhoneNumber, user.PhoneNumber ?? string.Empty));
        id.AddClaim(new Claim(OpenIddictConstants.Claims.Gender, user.Gender ?? string.Empty));
        id.AddClaim(new Claim(OpenIddictConstants.Claims.Nickname, user.Nickname));

        var roles = await _roleRepository.GetUserRolesAsync(user.Id);

        foreach (var role in roles)
        {
            id.AddClaim(OpenIddictConstants.Claims.Role, role.Name);
            id.AddClaim(BeniceSoftAuthConstants.ClaimTypes.RoleId, role.Id.ToString());
        }

        if (_userManager.SupportsUserEmail)
        {
            var email = await _userManager.GetEmailAsync(user);
            if (!string.IsNullOrEmpty(email))
            {
                id.AddClaim(new Claim(_options.ClaimsIdentity.EmailClaimType, email));
            }
        }

        if (_userManager.SupportsUserSecurityStamp)
        {
            id.AddClaim(new Claim(_options.ClaimsIdentity.SecurityStampClaimType,
                await _userManager.GetSecurityStampAsync(user)));
        }

        if (_userManager.SupportsUserClaim)
        {
            id.AddClaims(await _userManager.GetClaimsAsync(user));
        }

        return id;
    }
}
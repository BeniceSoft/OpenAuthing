using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Users;

public class UserManager : UserManager<User>, IScopedDependency
{
    public UserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
    }

    public virtual async Task<IdentityResult> ResetPasswordAsync(User user, string newPassword)
    {
        return await UpdatePasswordHash(user, newPassword, false);
    }

    public override string GetUserId(ClaimsPrincipal principal)
    {
        return base.GetUserId(principal) ??
               principal.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
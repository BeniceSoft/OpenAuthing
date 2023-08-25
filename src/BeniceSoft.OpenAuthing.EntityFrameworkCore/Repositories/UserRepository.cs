using BeniceSoft.OpenAuthing.Extensions;
using BeniceSoft.OpenAuthing.Users;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace BeniceSoft.OpenAuthing.Repositories;

public class UserRepository : EfCoreRepository<AmDbContext, User, Guid>, IUserRepository
{
    public UserRepository(IDbContextProvider<AmDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<User?> FindByNormalizedUserNameAsync(
        string normalizedUserName,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(
                u => u.NormalizedUserName == normalizedUserName,
                GetCancellationToken(cancellationToken)
            );
    }

    public virtual async Task<User?> FindByLoginAsync(
        string loginProvider,
        string providerKey,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(u => u.Logins.Any(login => login.LoginProvider == loginProvider && login.ProviderKey == providerKey))
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }
}
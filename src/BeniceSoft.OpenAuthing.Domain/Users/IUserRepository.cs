using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Users;

public interface IUserRepository : IRepository<User, Guid>
{
    Task<User?> FindByNormalizedUserNameAsync(string normalizedUserName, bool includeDetails, CancellationToken cancellationToken);
    Task<User?> FindByLoginAsync(string loginProvider, string providerKey, bool includeDetails = true,
        CancellationToken cancellationToken = default);
}
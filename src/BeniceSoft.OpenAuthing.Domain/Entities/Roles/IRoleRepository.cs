using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Entities.Roles;

public interface IRoleRepository : IRepository<Role, Guid>
{
    Task<Role?> FindByNormalizedNameAsync(string normalizedRoleName, CancellationToken cancellationToken);

    Task<List<Role>> GetUserRolesAsync(Guid userId);
}
using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Entities.Permissions;

public interface IPermissionGrantRepository : IRepository<PermissionGrant, Guid>
{
    Task<PermissionGrant?> FindAsync(
        Guid permissionSpaceId,
        string name,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default
    );

    Task<List<PermissionGrant>> GetListAsync(
        Guid permissionSpaceId,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default
    );

    Task<List<PermissionGrant>> GetListAsync(
        Guid permissionSpaceId,
        string[] names,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default
    );
}
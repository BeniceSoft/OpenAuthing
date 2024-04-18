using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Entities.Permissions;

public interface IPermissionGrantRepository : IRepository<PermissionGrant, Guid>
{
    Task<PermissionGrant?> FindAsync(
        string systemCode,
        string name,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default
    );

    Task<List<PermissionGrant>> GetListAsync(
        string systemCode,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default
    );

    Task<List<PermissionGrant>> GetListAsync(
        string systemCode,
        string[] names,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default
    );
}
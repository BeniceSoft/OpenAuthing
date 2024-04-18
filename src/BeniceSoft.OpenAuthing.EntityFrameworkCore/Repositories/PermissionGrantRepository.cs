using BeniceSoft.OpenAuthing.Entities.Permissions;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace BeniceSoft.OpenAuthing.Repositories;

public class PermissionGrantRepository : EfCoreRepository<AuthingDbContext, PermissionGrant, Guid>, IPermissionGrantRepository
{
    public PermissionGrantRepository(IDbContextProvider<AuthingDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<PermissionGrant?> FindAsync(string systemCode, string name, string providerName, string providerKey, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        return await (await GetQueryableAsync())
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(s =>
                    s.SystemCode == systemCode &&
                    s.Name == name &&
                    s.ProviderName == providerName &&
                    s.ProviderKey == providerKey,
                cancellationToken
            );
    }

    public async Task<List<PermissionGrant>> GetListAsync(string systemCode, string providerName, string providerKey, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        return await (await GetQueryableAsync())
            .Where(s =>
                s.SystemCode == systemCode &&
                s.ProviderName == providerName &&
                s.ProviderKey == providerKey
            ).ToListAsync(cancellationToken);
    }

    public async Task<List<PermissionGrant>> GetListAsync(string systemCode, string[] names, string providerName, string providerKey,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        return await (await GetQueryableAsync())
            .Where(s =>
                s.SystemCode == systemCode &&
                names.Contains(s.Name) &&
                s.ProviderName == providerName &&
                s.ProviderKey == providerKey
            ).ToListAsync(cancellationToken);
    }
}
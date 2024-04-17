using BeniceSoft.OpenAuthing.Entities.Permissions;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace BeniceSoft.OpenAuthing.Repositories;

public class PermissionRepository : EfCoreRepository<AuthingDbContext, Permission, Guid>, IPermissionRepository
{
    public PermissionRepository(IDbContextProvider<AuthingDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
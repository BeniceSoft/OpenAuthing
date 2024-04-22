using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Entities.Permissions;

public interface IPermissionRepository : IRepository<Permission, Guid>
{
    
}
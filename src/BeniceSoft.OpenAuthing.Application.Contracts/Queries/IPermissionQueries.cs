using BeniceSoft.OpenAuthing.Dtos.Permissions;

namespace BeniceSoft.OpenAuthing.Queries;

public interface IPermissionQueries
{
    Task<List<PermissionRes>> GetAllAsync(Guid permissionSpaceId);
}
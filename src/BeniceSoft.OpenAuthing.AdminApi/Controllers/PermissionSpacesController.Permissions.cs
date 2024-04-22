using BeniceSoft.OpenAuthing.Dtos.Permissions;
using BeniceSoft.OpenAuthing.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BeniceSoft.OpenAuthing.Controllers;

/// <summary>
/// 权限
/// </summary>
public partial class PermissionSpacesController
{
    private IPermissionQueries PermissionQueries => LazyServiceProvider.LazyGetRequiredService<IPermissionQueries>();

    [HttpGet("{permissionSpaceId}/permissions")]
    public async Task<List<PermissionRes>> GetPermissionsAsync(Guid permissionSpaceId)
    {
        return await PermissionQueries.GetAllAsync(permissionSpaceId);
    }
}
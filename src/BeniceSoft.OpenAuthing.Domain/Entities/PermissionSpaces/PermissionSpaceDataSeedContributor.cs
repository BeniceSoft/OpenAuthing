using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace BeniceSoft.OpenAuthing.Entities.PermissionSpaces;

public class PermissionSpaceDataSeedContributor(PermissionSpaceManager permissionSpaceManager) : IDataSeedContributor, ITransientDependency
{
    public const string SystemPermissionSpaceNameProperty = "SystemPermissionSpaceName";
    public const string SystemPermissionSpaceIdProperty = "SystemPermissionSpaceId";

    [UnitOfWork]
    public async Task SeedAsync(DataSeedContext context)
    {
        var spaceId = (Guid)context[SystemPermissionSpaceIdProperty]!;
        var spaceName = (string)context[SystemPermissionSpaceNameProperty]!;
        var systemPermissionSpace = await permissionSpaceManager.FindByNameAsync(spaceName);
        if (systemPermissionSpace is null)
        {
            systemPermissionSpace = new(
                spaceId,
                AuthingConstants.SystemPermissionSpaceName,
                "系统内置权限空间",
                "OpenAuthing 系统内置权限空间",
                true);

            await permissionSpaceManager.CreateAsync(systemPermissionSpace);
        }
    }
}
namespace BeniceSoft.OpenAuthing.Entities.Permissions;

public interface IPermissionManagementProvider
{
    string Name { get; }

    Task<PermissionGrantInfo> CheckAsync(
        Guid permissionSpaceId,
        string name,
        string providerName,
        string providerKey
    );

    Task<MultiplePermissionGrantInfo> CheckAsync(
        Guid permissionSpaceId,
        string[] names,
        string providerName,
        string providerKey
    );

    Task SetAsync(
        Guid permissionSpaceId,
        string name,
        string providerKey,
        bool isGranted
    );
}
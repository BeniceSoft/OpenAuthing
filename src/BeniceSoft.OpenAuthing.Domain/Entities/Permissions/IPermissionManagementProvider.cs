namespace BeniceSoft.OpenAuthing.Entities.Permissions;

public interface IPermissionManagementProvider
{
    string Name { get; }

    Task<PermissionGrantInfo> CheckAsync(
        string systemCode,
        string name,
        string providerName,
        string providerKey
    );

    Task<MultiplePermissionGrantInfo> CheckAsync(
        string systemCode,
        string[] names,
        string providerName,
        string providerKey
    );

    Task SetAsync(
        string systemCode,
        string name,
        string providerKey,
        bool isGranted
    );
}
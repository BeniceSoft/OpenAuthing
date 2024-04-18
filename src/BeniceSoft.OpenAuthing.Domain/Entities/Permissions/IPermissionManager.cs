namespace BeniceSoft.OpenAuthing.Entities.Permissions;

public interface IPermissionManager
{
    Task<PermissionWithGrantedProviders> GetAsync(string systemCode, string permissionName, string providerName, string providerKey);

    Task<MultiplePermissionWithGrantedProviders> GetAsync(string systemCode, string[] permissionNames, string provideName, string providerKey);

    Task<List<PermissionWithGrantedProviders>> GetAllAsync(string systemCode, string providerName, string providerKey);

    Task SetAsync(string systemCode, string permissionName, string providerName, string providerKey, bool isGranted);

    Task<PermissionGrant> UpdateProviderKeyAsync(string systemCode, PermissionGrant permissionGrant, string providerKey);

    Task DeleteAsync(string systemCode, string providerName, string providerKey);
}
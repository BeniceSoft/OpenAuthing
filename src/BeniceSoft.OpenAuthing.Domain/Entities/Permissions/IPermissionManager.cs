namespace BeniceSoft.OpenAuthing.Entities.Permissions;

public interface IPermissionManager
{
    Task<PermissionWithGrantedProviders> GetAsync(Guid permissionSpaceId, string permissionName, string providerName, string providerKey);

    Task<MultiplePermissionWithGrantedProviders> GetAsync(Guid permissionSpaceId, string[] permissionNames, string provideName, string providerKey);

    Task<List<PermissionWithGrantedProviders>> GetAllAsync(Guid permissionSpaceId, string providerName, string providerKey);

    Task SetAsync(Guid permissionSpaceId, string permissionName, string providerName, string providerKey, bool isGranted);

    Task<PermissionGrant> UpdateProviderKeyAsync(Guid permissionSpaceId, PermissionGrant permissionGrant, string providerKey);

    Task DeleteAsync(Guid permissionSpaceId, string providerName, string providerKey);
}
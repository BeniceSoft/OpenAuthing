using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Entities.Permissions;

public class SystemPermissionStore : IPermissionStore, ITransientDependency
{
    private readonly IPermissionManager _permissionManager;

    public SystemPermissionStore(IPermissionManager permissionManager)
    {
        _permissionManager = permissionManager;
    }

    public async Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
    {
        var permissionWithGranted = await _permissionManager.GetAsync(AuthingConstants.SystemPermissionSpaceName, name, providerName, providerKey);
        return permissionWithGranted.IsGranted;
    }

    public Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names, string providerName, string providerKey)
    {
        throw new NotImplementedException();
    }
}
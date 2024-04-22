using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace BeniceSoft.OpenAuthing.Entities.Permissions;

public abstract class AbstractPermissionManagementProvider(IAbpLazyServiceProvider lazyServiceProvider)
    : IPermissionManagementProvider
{
    public abstract string Name { get; }

    protected IPermissionGrantRepository PermissionGrantRepository => lazyServiceProvider.LazyGetRequiredService<IPermissionGrantRepository>();

    protected IGuidGenerator GuidGenerator => lazyServiceProvider.LazyGetRequiredService<IGuidGenerator>();


    public virtual async Task<PermissionGrantInfo> CheckAsync(string systemCode, string name, string providerName, string providerKey)
    {
        var multiplePermissionValueProviderGrantInfo = await CheckAsync(systemCode, new[] { name }, providerName, providerKey);

        return multiplePermissionValueProviderGrantInfo.Result.First().Value;
    }

    public virtual async Task<MultiplePermissionGrantInfo> CheckAsync(string systemCode, string[] names, string providerName, string providerKey)
    {
        var multiplePermissionValueProviderGrantInfo = new MultiplePermissionGrantInfo(names);
        if (providerName != Name)
        {
            return multiplePermissionValueProviderGrantInfo;
        }

        var permissionGrants = await PermissionGrantRepository.GetListAsync(systemCode, names, providerName, providerKey);

        foreach (var permissionName in names)
        {
            var isGrant = permissionGrants.Any(x => x.Name == permissionName);
            multiplePermissionValueProviderGrantInfo.Result[permissionName] = new PermissionGrantInfo(isGrant, providerKey);
        }

        return multiplePermissionValueProviderGrantInfo;
    }

    public virtual Task SetAsync(string systemCode, string name, string providerKey, bool isGranted)
    {
        return isGranted
            ? GrantAsync(systemCode, name, providerKey)
            : RevokeAsync(systemCode, name, providerKey);
    }

    protected virtual async Task GrantAsync(string systemCode, string name, string providerKey)
    {
        var permissionGrant = await PermissionGrantRepository.FindAsync(systemCode, name, Name, providerKey);
        if (permissionGrant != null)
        {
            return;
        }

        await PermissionGrantRepository.InsertAsync(
            new PermissionGrant(
                GuidGenerator.Create(),
                systemCode,
                name,
                Name,
                providerKey
            )
        );
    }

    protected virtual async Task RevokeAsync(string systemCode, string name, string providerKey)
    {
        var permissionGrant = await PermissionGrantRepository.FindAsync(systemCode, name, Name, providerKey);
        if (permissionGrant == null)
        {
            return;
        }

        await PermissionGrantRepository.DeleteAsync(permissionGrant);
    }
}
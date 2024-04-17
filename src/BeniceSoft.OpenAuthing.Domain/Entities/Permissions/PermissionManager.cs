using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.SimpleStateChecking;

namespace BeniceSoft.OpenAuthing.Entities.Permissions;

public class PermissionManager(IAbpLazyServiceProvider lazyServiceProvider) : IPermissionManager, ISingletonDependency
{
    private IPermissionGrantRepository PermissionGrantRepository => lazyServiceProvider.LazyGetRequiredService<IPermissionGrantRepository>();
    private IPermissionDefinitionManager PermissionDefinitionManager => lazyServiceProvider.LazyGetRequiredService<IPermissionDefinitionManager>();

    private ISimpleStateCheckerManager<PermissionDefinition> SimpleStateCheckerManager =>
        lazyServiceProvider.LazyGetRequiredService<ISimpleStateCheckerManager<PermissionDefinition>>();

    private IGuidGenerator GuidGenerator => lazyServiceProvider.LazyGetRequiredService<IGuidGenerator>();

    private IDistributedCache<PermissionGrantCacheItem> Cache => lazyServiceProvider.LazyGetRequiredService<IDistributedCache<PermissionGrantCacheItem>>();
    private IReadOnlyList<IPermissionManagementProvider> ManagementProviders => _lazyProviders.Value;
    private AuthingPermissionOptions Options => lazyServiceProvider.LazyGetRequiredService<IOptions<AuthingPermissionOptions>>().Value;

    private readonly Lazy<List<IPermissionManagementProvider>> _lazyProviders = new(
        () => lazyServiceProvider.LazyGetRequiredService<IOptions<AuthingPermissionOptions>>().Value
            .ManagementProviders
            .Select(c => lazyServiceProvider.GetRequiredService(c) as IPermissionManagementProvider)
            .ToList()!,
        true
    );


    public async Task<PermissionWithGrantedProviders> GetAsync(Guid permissionSpaceId, string permissionName, string providerName, string providerKey)
    {
        var permission = await PermissionDefinitionManager.GetOrNullAsync(permissionName);
        if (permission == null)
        {
            return new PermissionWithGrantedProviders(permissionName, false);
        }

        return await GetInternalAsync(
            permissionSpaceId,
            permission,
            providerName,
            providerKey
        );
    }

    public async Task<MultiplePermissionWithGrantedProviders> GetAsync(Guid permissionSpaceId, string[] permissionNames, string providerName, string providerKey)
    {
        var permissions = new List<PermissionDefinition>();
        var undefinedPermissions = new List<string>();

        foreach (var permissionName in permissionNames)
        {
            var permission = await PermissionDefinitionManager.GetOrNullAsync(permissionName);
            if (permission != null)
            {
                permissions.Add(permission);
            }
            else
            {
                undefinedPermissions.Add(permissionName);
            }
        }

        if (!permissions.Any())
        {
            return new MultiplePermissionWithGrantedProviders(undefinedPermissions.ToArray());
        }

        var result = await GetInternalAsync(
            permissionSpaceId,
            permissions.ToArray(),
            providerName,
            providerKey
        );

        foreach (var undefinedPermission in undefinedPermissions)
        {
            result.Result.Add(new PermissionWithGrantedProviders(undefinedPermission, false));
        }

        return result;
    }

    public async Task<List<PermissionWithGrantedProviders>> GetAllAsync(Guid permissionSpaceId, string providerName, string providerKey)
    {
        var permissionDefinitions = (await PermissionDefinitionManager.GetPermissionsAsync()).ToArray();

        var multiplePermissionWithGrantedProviders = await GetInternalAsync(permissionSpaceId, permissionDefinitions, providerName, providerKey);

        return multiplePermissionWithGrantedProviders.Result;
    }

    public async Task SetAsync(Guid permissionSpaceId, string permissionName, string providerName, string providerKey, bool isGranted)
    {
        var permission = await PermissionDefinitionManager.GetOrNullAsync(permissionName);
        if (permission == null)
        {
            /* Silently ignore undefined permissions,
               maybe they were removed from dynamic permission definition store */
            return;
        }

        if (!permission.IsEnabled || !await SimpleStateCheckerManager.IsEnabledAsync(permission))
        {
            //TODO: BusinessException
            throw new ApplicationException($"The permission named '{permission.Name}' is disabled!");
        }

        if (permission.Providers.Any() && !permission.Providers.Contains(providerName))
        {
            //TODO: BusinessException
            throw new ApplicationException($"The permission named '{permission.Name}' has not compatible with the provider named '{providerName}'");
        }

        var currentGrantInfo = await GetInternalAsync(permissionSpaceId, permission, providerName, providerKey);
        if (currentGrantInfo.IsGranted == isGranted)
        {
            return;
        }

        var provider = ManagementProviders.FirstOrDefault(m => m.Name == providerName);
        if (provider == null)
        {
            //TODO: BusinessException
            throw new AbpException("Unknown permission management provider: " + providerName);
        }

        await provider.SetAsync(permissionSpaceId, permissionName, providerKey, isGranted);
    }

    public async Task<PermissionGrant> UpdateProviderKeyAsync(Guid permissionSpaceId, PermissionGrant permissionGrant, string providerKey)
    {
        //Invalidating the cache for the old key
        await Cache.RemoveAsync(
            PermissionGrantCacheItem.CalculateCacheKey(
                permissionGrant.Name,
                permissionGrant.ProviderName,
                permissionGrant.ProviderKey
            )
        );

        permissionGrant.ProviderKey = providerKey;
        return await PermissionGrantRepository.UpdateAsync(permissionGrant);
    }

    public async Task DeleteAsync(Guid permissionSpaceId, string providerName, string providerKey)
    {
        var permissionGrants = await PermissionGrantRepository.GetListAsync(permissionSpaceId, providerName, providerKey);
        foreach (var permissionGrant in permissionGrants)
        {
            await PermissionGrantRepository.DeleteAsync(permissionGrant);
        }
    }

    protected virtual async Task<PermissionWithGrantedProviders> GetInternalAsync(
        Guid permissionSpaceId,
        PermissionDefinition permission,
        string providerName,
        string providerKey)
    {
        var multiplePermissionWithGrantedProviders = await GetInternalAsync(
            permissionSpaceId,
            [permission],
            providerName,
            providerKey
        );

        return multiplePermissionWithGrantedProviders.Result.First();
    }

    protected virtual async Task<MultiplePermissionWithGrantedProviders> GetInternalAsync(
        Guid permissionSpaceId,
        PermissionDefinition[] permissions,
        string providerName,
        string providerKey)
    {
        var permissionNames = permissions.Select(x => x.Name).ToArray();
        var multiplePermissionWithGrantedProviders = new MultiplePermissionWithGrantedProviders(permissionNames);

        var neededCheckPermissions = new List<PermissionDefinition>();

        foreach (var permission in permissions
                     .Where(x => x.IsEnabled)
                     .Where(x => !x.Providers.Any() || x.Providers.Contains(providerName)))
        {
            if (await SimpleStateCheckerManager.IsEnabledAsync(permission))
            {
                neededCheckPermissions.Add(permission);
            }
        }

        if (!neededCheckPermissions.Any())
        {
            return multiplePermissionWithGrantedProviders;
        }

        foreach (var provider in ManagementProviders)
        {
            permissionNames = neededCheckPermissions.Select(x => x.Name).ToArray();
            var multiplePermissionValueProviderGrantInfo = await provider.CheckAsync(permissionSpaceId, permissionNames, providerName, providerKey);

            foreach (var providerResultDict in multiplePermissionValueProviderGrantInfo.Result)
            {
                if (providerResultDict.Value.IsGranted)
                {
                    var permissionWithGrantedProvider = multiplePermissionWithGrantedProviders.Result
                        .First(x => x.Name == providerResultDict.Key);

                    permissionWithGrantedProvider.IsGranted = true;
                    permissionWithGrantedProvider.Providers.Add(new PermissionValueProviderInfo(provider.Name, providerResultDict.Value.ProviderKey!));
                }
            }
        }

        return multiplePermissionWithGrantedProviders;
    }
}
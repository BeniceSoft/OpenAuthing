using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.SimpleStateChecking;

namespace BeniceSoft.OpenAuthing.Entities.Permissions;

public class PermissionManager : IPermissionManager, ISingletonDependency
{
    public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = null!;

    private IPermissionGrantRepository PermissionGrantRepository => LazyServiceProvider.LazyGetRequiredService<IPermissionGrantRepository>();
    private IPermissionDefinitionManager PermissionDefinitionManager => LazyServiceProvider.LazyGetRequiredService<IPermissionDefinitionManager>();

    private ISimpleStateCheckerManager<PermissionDefinition> SimpleStateCheckerManager =>
        LazyServiceProvider.LazyGetRequiredService<ISimpleStateCheckerManager<PermissionDefinition>>();

    private IGuidGenerator GuidGenerator => LazyServiceProvider.LazyGetRequiredService<IGuidGenerator>();

    private IDistributedCache<PermissionGrantCacheItem> Cache => LazyServiceProvider.LazyGetRequiredService<IDistributedCache<PermissionGrantCacheItem>>();
    private IReadOnlyList<IPermissionManagementProvider> ManagementProviders => _lazyProviders.Value;
    private AuthingPermissionOptions Options => LazyServiceProvider.LazyGetRequiredService<IOptions<AuthingPermissionOptions>>().Value;

    private readonly Lazy<List<IPermissionManagementProvider>> _lazyProviders;

    public PermissionManager()
    {
        _lazyProviders = new(
            () => Options
                .ManagementProviders
                .Select(c => LazyServiceProvider.GetRequiredService(c) as IPermissionManagementProvider)
                .ToList()!,
            true
        );
    }

    public async Task<PermissionWithGrantedProviders> GetAsync(string systemCode, string permissionName, string providerName, string providerKey)
    {
        var permission = await PermissionDefinitionManager.GetOrNullAsync(permissionName);
        if (permission == null)
        {
            return new PermissionWithGrantedProviders(permissionName, false);
        }

        return await GetInternalAsync(
            systemCode,
            permission,
            providerName,
            providerKey
        );
    }

    public async Task<MultiplePermissionWithGrantedProviders> GetAsync(string systemCode, string[] permissionNames, string providerName, string providerKey)
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
            systemCode,
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

    public async Task<List<PermissionWithGrantedProviders>> GetAllAsync(string systemCode, string providerName, string providerKey)
    {
        var permissionDefinitions = (await PermissionDefinitionManager.GetPermissionsAsync()).ToArray();

        var multiplePermissionWithGrantedProviders = await GetInternalAsync(systemCode, permissionDefinitions, providerName, providerKey);

        return multiplePermissionWithGrantedProviders.Result;
    }

    public async Task SetAsync(string systemCode, string permissionName, string providerName, string providerKey, bool isGranted)
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

        var currentGrantInfo = await GetInternalAsync(systemCode, permission, providerName, providerKey);
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

        await provider.SetAsync(systemCode, permissionName, providerKey, isGranted);
    }

    public async Task<PermissionGrant> UpdateProviderKeyAsync(string systemCode, PermissionGrant permissionGrant, string providerKey)
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

    public async Task DeleteAsync(string systemCode, string providerName, string providerKey)
    {
        var permissionGrants = await PermissionGrantRepository.GetListAsync(systemCode, providerName, providerKey);
        foreach (var permissionGrant in permissionGrants)
        {
            await PermissionGrantRepository.DeleteAsync(permissionGrant);
        }
    }

    protected virtual async Task<PermissionWithGrantedProviders> GetInternalAsync(
        string systemCode,
        PermissionDefinition permission,
        string providerName,
        string providerKey)
    {
        var multiplePermissionWithGrantedProviders = await GetInternalAsync(
            systemCode,
            [permission],
            providerName,
            providerKey
        );

        return multiplePermissionWithGrantedProviders.Result.First();
    }

    protected virtual async Task<MultiplePermissionWithGrantedProviders> GetInternalAsync(
        string systemCode,
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
            var multiplePermissionValueProviderGrantInfo = await provider.CheckAsync(systemCode, permissionNames, providerName, providerKey);

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
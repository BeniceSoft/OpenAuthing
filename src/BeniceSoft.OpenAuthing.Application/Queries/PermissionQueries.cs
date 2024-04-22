using BeniceSoft.OpenAuthing.Dtos.Permissions;
using BeniceSoft.OpenAuthing.Entities.Permissions;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Queries;

public class PermissionQueries : BaseQueries, IPermissionQueries, ITransientDependency
{
    public PermissionQueries(IAbpLazyServiceProvider lazyServiceProvider)
        : base(lazyServiceProvider)
    {
    }

    private IPermissionManager PermissionManager => LazyServiceProvider.LazyGetRequiredService<IPermissionManager>();
    private IPermissionRepository PermissionRepository => LazyServiceProvider.LazyGetRequiredService<IPermissionRepository>();

    public async Task<List<PermissionRes>> GetAllAsync(Guid systemId)
    {
        var queryable =
            from permission in await PermissionRepository.GetQueryableAsync()
            where permission.SystemId == systemId
            select new PermissionRes
            {
                Id = permission.Id,
                Name = permission.Name,
                DisplayName = permission.DisplayName,
                IsEnabled = permission.IsEnabled
            };

        return await AsyncExecuter.ToListAsync(queryable);
    }
}
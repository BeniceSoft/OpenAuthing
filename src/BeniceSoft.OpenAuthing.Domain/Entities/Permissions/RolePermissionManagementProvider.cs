using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Entities.Permissions;

public class RolePermissionManagementProvider(IAbpLazyServiceProvider lazyServiceProvider) 
    : AbstractPermissionManagementProvider(lazyServiceProvider), ITransientDependency
{
    public override string Name => "R";
}
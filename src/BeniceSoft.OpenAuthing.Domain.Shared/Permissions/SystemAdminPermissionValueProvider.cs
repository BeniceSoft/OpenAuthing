using Volo.Abp.Authorization.Permissions;

namespace BeniceSoft.OpenAuthing.Permissions;

public class SystemAdminPermissionValueProvider : PermissionValueProvider
{
    public SystemAdminPermissionValueProvider(IPermissionStore permissionStore) : base(permissionStore)
    {
    }

    public override async Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
    {
        if (context.Principal?.IsInRole(AuthingConstants.AdminRoleName) ?? false)
        {
            return PermissionGrantResult.Granted;
        }

        return PermissionGrantResult.Undefined;
    }

    public override Task<MultiplePermissionGrantResult> CheckAsync(PermissionValuesCheckContext context)
    {
        var isGranted = context.Principal?.IsInRole(AuthingConstants.AdminRoleName) ?? false;
        var result = new MultiplePermissionGrantResult(
            context.Permissions.Select(x => x.Name).ToArray(),
            isGranted ? PermissionGrantResult.Granted : PermissionGrantResult.Undefined);

        return Task.FromResult(result);
    }

    public override string Name => nameof(SystemAdminPermissionValueProvider);
}
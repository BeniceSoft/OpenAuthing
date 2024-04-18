using BeniceSoft.OpenAuthing.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace BeniceSoft.OpenAuthing.Permissions;

public class AuthingPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        context.AddGroup(AuthingPermissions.Dashboard)
            .AddPermission(AuthingPermissions.ViewDashboard);

        context.AddGroup(AuthingPermissions.Organization)
            .AddPermission(AuthingPermissions.ManageDepartment)
            .AddChild(AuthingPermissions.CreateDepartment)
            .AddChild(AuthingPermissions.UpdateDepartment);
    }

    private static ILocalizableString L(string name)
    {
        return LocalizableString.Create<AuthingPermissionResource>(name);
    }

    private static ILocalizableString F(string name)
    {
        return new FixedLocalizableString(name);
    }
}
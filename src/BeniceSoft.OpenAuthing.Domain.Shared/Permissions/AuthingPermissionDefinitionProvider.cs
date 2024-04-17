using BeniceSoft.OpenAuthing.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace BeniceSoft.OpenAuthing.Permissions;

public class AuthingPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var moduleGroup = context.AddGroup(AuthingPermissions.GroupName, F("OpenAuthing"));
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
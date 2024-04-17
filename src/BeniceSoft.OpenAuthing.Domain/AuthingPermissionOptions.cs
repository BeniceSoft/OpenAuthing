using BeniceSoft.OpenAuthing.Entities.Permissions;
using Volo.Abp.Collections;

namespace BeniceSoft.OpenAuthing;

public class AuthingPermissionOptions
{
    public ITypeList<IPermissionManagementProvider> ManagementProviders { get; }

    public AuthingPermissionOptions()
    {
        ManagementProviders = new TypeList<IPermissionManagementProvider>();
    }
}
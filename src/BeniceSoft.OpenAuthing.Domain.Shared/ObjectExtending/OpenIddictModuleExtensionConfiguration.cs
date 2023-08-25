using Volo.Abp.ObjectExtending.Modularity;

namespace BeniceSoft.OpenAuthing.ObjectExtending;

public class OpenIddictModuleExtensionConfiguration : ModuleExtensionConfiguration
{
    public OpenIddictModuleExtensionConfiguration ConfigureApplication(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            OpenIddictModuleExtensionConsts.EntityNames.Application,
            configureAction
        );
    }

    public OpenIddictModuleExtensionConfiguration ConfigureAuthorization(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            OpenIddictModuleExtensionConsts.EntityNames.Authorization,
            configureAction
        );
    }

    public OpenIddictModuleExtensionConfiguration ConfigureScope(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            OpenIddictModuleExtensionConsts.EntityNames.Scope,
            configureAction
        );
    }

    public OpenIddictModuleExtensionConfiguration ConfigureToken(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            OpenIddictModuleExtensionConsts.EntityNames.Token,
            configureAction
        );
    }
}

using Volo.Abp.ObjectExtending.Modularity;

namespace BeniceSoft.OpenAuthing.ObjectExtending;

public static class OpenIddictModuleExtensionConfigurationDictionaryExtensions
{
    public static ModuleExtensionConfigurationDictionary ConfigureOpenIddict(
        this ModuleExtensionConfigurationDictionary modules,
        Action<OpenIddictModuleExtensionConfiguration> configureAction)
    {
        return modules.ConfigureModule(
            OpenIddictModuleExtensionConsts.ModuleName,
            configureAction
        );
    }
}

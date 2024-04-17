using Volo.Abp;

namespace BeniceSoft.OpenAuthing.Entities.Permissions;

public class PermissionValueProviderInfo
{
    public string Name { get; }

    public string Key { get; }

    public PermissionValueProviderInfo(string name, string key)
    {
        Check.NotNull(name, nameof(name));
        Check.NotNull(key, nameof(key));

        Name = name;
        Key = key;
    }
}
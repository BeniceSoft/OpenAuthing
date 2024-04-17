using Volo.Abp;

namespace BeniceSoft.OpenAuthing.Entities.Permissions;

public class PermissionWithGrantedProviders
{
    public string Name { get; }

    public bool IsGranted { get; set; }

    public List<PermissionValueProviderInfo> Providers { get; set; }

    public PermissionWithGrantedProviders(string name, bool isGranted)
    {
        Check.NotNull(name, nameof(name));

        Name = name;
        IsGranted = isGranted;

        Providers = new List<PermissionValueProviderInfo>();
    }
}
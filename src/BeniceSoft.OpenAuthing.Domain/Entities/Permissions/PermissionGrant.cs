using Volo.Abp.Domain.Entities;

namespace BeniceSoft.OpenAuthing.Entities.Permissions;

public class PermissionGrant : Entity<Guid>
{
    public string SystemCode { get; private set; }
    public string Name { get; private set; }

    public string ProviderName { get; private set; }

    public string ProviderKey { get; protected internal set; }

    private PermissionGrant(Guid id) : base(id)
    {
    }

    public PermissionGrant(Guid id, string systemCode, string name, string providerName, string providerKey) : this(id)
    {
        SystemCode = systemCode;
        Name = name;
        ProviderName = providerName;
        ProviderKey = providerKey;
    }
}
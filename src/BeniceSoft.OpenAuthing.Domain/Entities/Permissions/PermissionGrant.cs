using Volo.Abp.Domain.Entities;

namespace BeniceSoft.OpenAuthing.Entities.Permissions;

public class PermissionGrant : Entity<Guid>
{
    public Guid PermissionSpaceId { get; private set; }
    public string Name { get; private set; }

    public string ProviderName { get; private set; }

    public string ProviderKey { get; protected internal set; }

    private PermissionGrant(Guid id) : base(id)
    {
    }

    public PermissionGrant(Guid id, Guid permissionSpaceId, string name, string providerName, string providerKey) : this(id)
    {
        PermissionSpaceId = permissionSpaceId;
        Name = name;
        ProviderName = providerName;
        ProviderKey = providerKey;
    }
}
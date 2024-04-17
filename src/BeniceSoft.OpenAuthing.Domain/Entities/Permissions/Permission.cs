using Volo.Abp.Domain.Entities;

namespace BeniceSoft.OpenAuthing.Entities.Permissions;

public class Permission : AggregateRoot<Guid>
{
    /// <summary>
    /// 权限空间
    /// </summary>
    public Guid PermissionSpaceId { get; set; }

    public string Name { get; private set; }

    public string DisplayName { get; private set; }

    public string? ParentName { get; private set; }

    public bool IsEnabled { get; private set; }

    /// <summary>
    /// Comma separated list of provider names.
    /// </summary>
    public string Providers { get; private set; }

    /// <summary>
    /// Serialized string to store info about the state checkers.
    /// </summary>
    public string StateCheckers { get; private set; }

    private Permission(Guid id) : base(id)
    {
        IsEnabled = true;
    }

    public Permission(Guid id, Guid permissionSpaceId, string name, string displayName, string? parentName, bool isEnabled,
        string providers, string stateCheckers)
        : this(id)
    {
        PermissionSpaceId = permissionSpaceId;
        Name = name;
        DisplayName = displayName;
        ParentName = parentName;
        IsEnabled = isEnabled;
        Providers = providers;
        StateCheckers = stateCheckers;
    }
}
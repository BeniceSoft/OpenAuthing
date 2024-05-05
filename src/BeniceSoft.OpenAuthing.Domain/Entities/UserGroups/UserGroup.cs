using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace BeniceSoft.OpenAuthing.Entities.UserGroups;

/// <summary>
/// 用户组
/// </summary>
public class UserGroup : FullAuditedAggregateRoot<Guid>
{
    /// <summary>
    /// Group name
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Description
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Enabled
    /// </summary>
    public bool Enabled { get; private set; }

    /// <summary>
    /// Members
    /// </summary>
    public IReadOnlyCollection<UserGroupMember> Members => _members;

    private readonly List<UserGroupMember> _members;

    private UserGroup(Guid id) : base(id)
    {
        _members = new();
    }

    public UserGroup(Guid id, string name, string description, bool enabled = true)
        : this(id)
    {
        Name = name;
        Description = description;
        Enabled = enabled;
    }

    public void Update(string name, string description)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        Name = name;
        Description = description;
    }
}
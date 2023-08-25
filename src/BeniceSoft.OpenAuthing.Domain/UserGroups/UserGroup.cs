using Volo.Abp.Domain.Entities.Auditing;

namespace BeniceSoft.OpenAuthing.UserGroups;

/// <summary>
/// 用户组
/// </summary>
public class UserGroup : FullAuditedAggregateRoot<Guid>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 显示名称
    /// </summary>
    public string DisplayName { get; private set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; private set; }

    /// <summary>
    /// 成员
    /// </summary>
    public IReadOnlyCollection<UserGroupMember> Members => _members;

    private readonly List<UserGroupMember> _members;

    private UserGroup(Guid id) : base(id)
    {
        _members = new();
    }

    public UserGroup(Guid id, string name, string displayName, string description, bool enabled)
        : this(id)
    {
        Name = name;
        DisplayName = displayName;
        Description = description;
        Enabled = enabled;
    }
}
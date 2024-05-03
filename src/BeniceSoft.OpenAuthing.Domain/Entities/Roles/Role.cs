using BeniceSoft.OpenAuthing.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace BeniceSoft.OpenAuthing.Entities.Roles;

/// <summary>
/// 角色
/// </summary>
public class Role : FullAuditedAggregateRoot<Guid>
{
    /// <summary>
    /// Role Name
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Normalized Role Name
    /// </summary>
    public string NormalizedName { get; private set; }

    /// <summary>
    /// Role Description
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// true: Enabled false: Disabled
    /// </summary>
    public bool Enabled { get; private set; }

    /// <summary>
    /// Is System Built-In
    /// </summary>
    public bool IsSystemBuiltIn { get; private set; }

    /// <summary>
    /// Role Subjects
    /// </summary>
    public IReadOnlyCollection<RoleSubject> Subjects => _subjects;

    private readonly List<RoleSubject> _subjects;

    private Role(Guid id) : base(id)
    {
        _subjects = new();
    }

    public Role(Guid id, string name, string description, bool enabled = true, bool isSystemBuiltIn = false)
        : this(id)
    {
        Name = name;
        Description = description;
        Enabled = enabled;
        IsSystemBuiltIn = isSystemBuiltIn;
    }

    public void SetName(string roleName)
    {
        Name = roleName;
    }

    public void SetNormalizedName(string normalizedName)
    {
        NormalizedName = normalizedName;
    }

    public void ToggleEnabled(bool enabled)
    {
        ThrowIfIsBuiltInSystem();

        Enabled = enabled;
    }

    public void Update(string name, string description)
    {
        ThrowIfIsBuiltInSystem();

        Name = name;
        Description = description;
    }


    private void ThrowIfIsBuiltInSystem()
    {
        if (IsSystemBuiltIn)
        {
            throw new UserFriendlyException("系统内置的角色无法修改");
        }
    }

    public void AddSubject(Guid id, RoleSubjectType subjectType, Guid subjectId)
    {
        if (_subjects.Any(x => x.SubjectType == subjectType && x.SubjectId == subjectId))
        {
            return;
        }

        var subject = new RoleSubject(id, subjectType, subjectId);
        _subjects.Add(subject);
    }

    public void RemoveSubject(Guid roleSubjectId)
    {
        _subjects.RemoveAll(x => x.Id == roleSubjectId);
    }
}
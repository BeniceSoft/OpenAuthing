using BeniceSoft.OpenAuthing.Enums;
using Volo.Abp.Domain.Entities.Auditing;

namespace BeniceSoft.OpenAuthing.Entities.Roles;

/// <summary>
/// 
/// </summary>
public class RoleSubject : CreationAuditedEntity<Guid>
{
    /// <summary>
    /// 角色id
    /// </summary>
    public Guid RoleId { get; private set; }

    /// <summary>
    /// 主体类型
    /// </summary>
    public RoleSubjectType SubjectType { get; private set; }

    /// <summary>
    /// 主体id
    /// </summary>
    public Guid SubjectId { get; private set; }

    private RoleSubject(Guid id) : base(id)
    {
    }

    public RoleSubject(Guid id, RoleSubjectType subjectType, Guid subjectId)
        : this(id)
    {
        SubjectType = subjectType;
        SubjectId = subjectId;
    }
}
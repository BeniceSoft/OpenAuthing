using Volo.Abp.Domain.Entities.Auditing;

namespace BeniceSoft.OpenAuthing.Entities.UserGroups;

public class UserGroupMember : CreationAuditedEntity
{
    /// <summary>
    /// 用户组id
    /// </summary>
    public Guid UserGroupId { get; private set; }

    /// <summary>
    /// 用户id
    /// </summary>
    public Guid UserId { get; private set; }

    public override object[] GetKeys() => new object[] { UserGroupId, UserId };
}
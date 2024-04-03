using Volo.Abp.Domain.Entities.Auditing;

namespace BeniceSoft.OpenAuthing.Entities.DepartmentMembers;

/// <summary>
/// 部门成员
/// </summary>
public class DepartmentMember : AuditedAggregateRoot
{
    /// <summary>
    /// 部门id
    /// </summary>
    public Guid DepartmentId { get; private set; }

    /// <summary>
    /// 用户id
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// 是否是部门负责人
    /// </summary>
    public bool IsLeader { get; private set; }

    /// <summary>
    /// 是否主部门
    /// </summary>
    public bool IsMain { get; private set; }

    private DepartmentMember()
    {
    }

    public DepartmentMember(Guid departmentId, Guid userId, bool isLeader = false, bool isMain = false)
    {
        DepartmentId = departmentId;
        UserId = userId;

        IsLeader = isLeader;
        IsMain = isMain;
    }

    /// <summary>
    /// 设置部门负责人
    /// </summary>
    /// <param name="isLeader"></param>
    public void SetLeader(bool isLeader)
    {
        IsLeader = isLeader;
    }

    /// <summary>
    /// 设置主部门
    /// </summary>
    /// <param name="isMain"></param>
    public void SetMain(bool isMain)
    {
        IsMain = isMain;
    }

    public override object[] GetKeys()
    {
        return new object[] {DepartmentId, UserId};
    }
}
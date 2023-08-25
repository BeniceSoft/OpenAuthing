namespace BeniceSoft.OpenAuthing.Dtos.DepartmentMembers.Responses;

public class UserDepartmentDto
{
    /// <summary>
    /// 部门id
    /// </summary>
    public Guid DepartmentId { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    public string DepartmentName { get; set; }

    /// <summary>
    /// 是否主部门
    /// </summary>
    public bool IsMain { get; set; }

    /// <summary>
    /// 是否负责人
    /// </summary>
    public bool IsLeader { get; set; }
}
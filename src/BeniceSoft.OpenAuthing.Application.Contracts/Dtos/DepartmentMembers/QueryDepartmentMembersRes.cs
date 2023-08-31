using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Dtos.DepartmentMembers;

public class QueryDepartmentMembersRes : EntityDto<Guid>
{
    public string UserName { get; set; }
    public string Nickname { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Gender { get; set; }
    public string? Avatar { get; set; }
    public bool Enabled { get; set; }
    public bool LockoutEnabled { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }

    /// <summary>
    /// 职务
    /// </summary>
    public string? JobTitle { get; set; }

    /// <summary>
    /// 部门列表
    /// </summary>
    public IEnumerable<UserDepartmentDto> Departments { get; set; }

    /// <summary>
    /// 是否负责人
    /// </summary>
    public bool IsLeader { get; set; }
}
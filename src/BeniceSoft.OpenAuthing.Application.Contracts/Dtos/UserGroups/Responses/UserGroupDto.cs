namespace BeniceSoft.OpenAuthing.Dtos.UserGroups.Responses;

/// <summary>
/// 
/// </summary>
public class UserGroupDto
{
    /// <summary>
    /// 
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Desc { get; set; }

    /// <summary>
    /// 排序值
    /// </summary>
    public int Seq { get; set; }

    /// <summary>
    /// 启用/禁用	
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// 部门
    /// </summary>
    public List<AmUserGroupDepartmentDto> Departments { get; set; } = new List<AmUserGroupDepartmentDto>();
     
    /// <summary>
    /// 用户Ids
    /// </summary>
    public List<Guid> UserIds { get; set; } = new List<Guid>();
}

public class AmUserGroupDepartmentDto
{
    public Guid Id { get; set; }

    public string DepartmentName { get; set; } = string.Empty;

    public List<AmUserGroupUserDto> Users { get; set; } = new List<AmUserGroupUserDto>();
}

public class AmUserGroupUserDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string UserName { get; set; } = string.Empty;
}
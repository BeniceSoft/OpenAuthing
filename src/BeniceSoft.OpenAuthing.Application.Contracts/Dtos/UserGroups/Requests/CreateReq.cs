namespace BeniceSoft.OpenAuthing.Dtos.UserGroups.Requests;

/// <summary>
/// 
/// </summary>
public class CreateReq
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 描述
    /// </summary>
    public string Desc { get; set; } = string.Empty;

    /// <summary>
    /// 排序值
    /// </summary>
    public int Seq { get; set; } = 0;

    /// <summary>
    /// 禁用 启用	
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// 角色Id
    /// </summary>
    public Guid? RoleId { get; set; }

    /// <summary>
    /// 用户Ids
    /// </summary>
    public List<Guid> UserIds { get; set; } = new List<Guid>();
}
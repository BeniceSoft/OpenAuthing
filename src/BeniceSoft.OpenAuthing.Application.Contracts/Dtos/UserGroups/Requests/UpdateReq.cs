namespace BeniceSoft.OpenAuthing.Dtos.UserGroups.Requests;

/// <summary>
/// 
/// </summary>
public class UpdateReq
{
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
    /// 启用	/禁用
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// 用户Ids
    /// </summary>
    public List<Guid> UserIds { get; set; } = new List<Guid>();
}
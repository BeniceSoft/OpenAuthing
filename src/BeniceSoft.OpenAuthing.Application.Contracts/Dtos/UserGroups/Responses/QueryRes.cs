using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Dtos.UserGroups.Responses;

/// <summary>
/// 
/// </summary>
public class QueryRes : EntityDto<Guid>
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
    /// 启用/禁用	
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }
}
using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Dtos.Departments;

/// <summary>
/// 
/// </summary>
public class DepartmentDto : EntityDto<Guid>
{
    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 组织架构名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 排序
    /// </summary>
    public int Seq { get; set; }

    /// <summary>
    /// 父级部门
    /// </summary>
    public Guid? ParentId { get; set; }
}
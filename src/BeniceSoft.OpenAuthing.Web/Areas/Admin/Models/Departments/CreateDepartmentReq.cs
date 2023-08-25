using System.ComponentModel.DataAnnotations;

namespace BeniceSoft.OpenAuthing.Areas.Admin.Models.Departments;

/// <summary>
/// 
/// </summary>
public class CreateDepartmentReq
{
    /// <summary>
    /// 编码
    /// </summary>
    [Required]
    public string Code { get; set; }

    /// <summary>
    /// 组织架构名称
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Seq { get; set; }

    /// <summary>
    /// 父级部门
    /// </summary>
    public Guid? ParentId { get; set; }
}
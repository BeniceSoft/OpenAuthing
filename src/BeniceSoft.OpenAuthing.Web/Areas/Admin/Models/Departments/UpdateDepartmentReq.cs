namespace BeniceSoft.OpenAuthing.Areas.Admin.Models.Departments;

/// <summary>
/// 
/// </summary>
public class UpdateDepartmentReq
{
    /// <summary>
    /// 组织架构名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 排序
    /// </summary>
    public int Seq { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; set; } = string.Empty;
}
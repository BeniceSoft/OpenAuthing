using BeniceSoft.OpenAuthing.Dtos.Departments.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BeniceSoft.OpenAuthing.Areas.Admin.Controllers;

/// <summary>
/// 部门
/// </summary>
public partial class DepartmentsController : AdminControllerBase
{
    private readonly IDepartmentAppService _departmentAppService;

    public DepartmentsController(IDepartmentAppService departmentAppService, IDepartmentMemberAppService departmentMemberAppService)
    {
        _departmentAppService = departmentAppService;
        _departmentMemberAppService = departmentMemberAppService;
    }

    /// <summary>
    /// 获取部门列表
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<DepartmentDto>> GetAsync(Guid? parentId = null)
    {
        return await _departmentAppService.GetByParentIdAsync(parentId);
    }

    /// <summary>
    /// 获取部门详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<DepartmentDto> GetAsync(Guid id)
    {
        return await _departmentAppService.GetByIdAsync(id);
    }
}
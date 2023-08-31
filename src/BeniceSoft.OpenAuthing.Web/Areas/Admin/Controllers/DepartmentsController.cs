using BeniceSoft.OpenAuthing.Areas.Admin.Models.Departments;
using BeniceSoft.OpenAuthing.Commands.Departments;
using BeniceSoft.OpenAuthing.Dtos.Departments;
using Microsoft.AspNetCore.Mvc;

namespace BeniceSoft.OpenAuthing.Areas.Admin.Controllers;

/// <summary>
/// 组织/部门
/// </summary>
public partial class DepartmentsController : AdminControllerBase
{
    private readonly IDepartmentQueries _departmentQueries;

    public DepartmentsController(IDepartmentQueries departmentQueries, IDepartmentMemberQueries departmentMemberQueries)
    {
        _departmentQueries = departmentQueries;
        _departmentMemberQueries = departmentMemberQueries;
    }

    /// <summary>
    /// 获取部门列表
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<DepartmentDto>> GetAsync(Guid? parentId = null)
    {
        return await _departmentQueries.GetByParentIdAsync(parentId);
    }

    /// <summary>
    /// 获取部门详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<DepartmentDto> GetAsync(Guid id)
    {
        return await _departmentQueries.GetByIdAsync(id);
    }

    /// <summary>
    /// 创建组织/部门
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<Guid> PostAsync([FromBody] CreateDepartmentReq req)
    {
        var command = new CreateDepartmentCommand(req.Code, req.Name, req.ParentId, req.Seq);
        return await Mediator.Send(command);
    }
}
using BeniceSoft.Abp.Core.Models;
using BeniceSoft.OpenAuthing.Areas.Admin.Models.Departments;
using BeniceSoft.OpenAuthing.Commands.Departments;
using BeniceSoft.OpenAuthing.Dtos.Departments;
using BeniceSoft.OpenAuthing.Queries;
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
    /// 获取列表
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(ResponseResult<List<DepartmentDto>>),200)]
    public async Task<List<DepartmentDto>> GetAsync(Guid? parentId = null)
    {
        return await _departmentQueries.GetByParentIdAsync(parentId);
    }

    /// <summary>
    /// 获取详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseResult<DepartmentDto>),200)]
    public async Task<DepartmentDto> GetAsync(Guid id)
    {
        return await _departmentQueries.GetByIdAsync(id);
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<Guid> PostAsync([FromBody] CreateDepartmentReq req)
    {
        var command = new CreateDepartmentCommand(req.Code, req.Name, req.ParentId, req.Seq);
        return await Mediator.Send(command);
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<bool> PutAsync(Guid id, [FromBody] UpdateDepartmentReq req)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<bool> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
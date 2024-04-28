using BeniceSoft.Abp.Core.Models;
using BeniceSoft.OpenAuthing.Commands.Departments;
using BeniceSoft.OpenAuthing.Dtos.Departments;
using BeniceSoft.OpenAuthing.Models.Departments;
using BeniceSoft.OpenAuthing.Permissions;
using BeniceSoft.OpenAuthing.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeniceSoft.OpenAuthing.Controllers;

/// <summary>
/// 组织/部门
/// </summary>
public partial class DepartmentsController : AuthingApiControllerBase
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
    [Authorize(AuthingPermissions.ManageDepartment)]
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
    [Authorize(AuthingPermissions.ManageDepartment)]
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
    [Authorize(AuthingPermissions.CreateDepartment)]
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
    [Authorize(AuthingPermissions.UpdateDepartment)]
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
    [Authorize(AuthingPermissions.DeleteDepartment)]
    public async Task<bool> DeleteAsync(Guid id)
    {
        var command = new DeleteDepartmentCommand(id);
        return await Mediator.Send(command);
    }
}
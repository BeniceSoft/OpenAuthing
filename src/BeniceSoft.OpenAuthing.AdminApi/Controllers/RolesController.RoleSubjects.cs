using BeniceSoft.Abp.Core.Models;
using BeniceSoft.OpenAuthing.Commands.Roles;
using BeniceSoft.OpenAuthing.Dtos.Roles;
using BeniceSoft.OpenAuthing.Models.Roles;
using Microsoft.AspNetCore.Mvc;

namespace BeniceSoft.OpenAuthing.Controllers;

public partial class RolesController
{
    /// <summary>
    /// 获取角色对象列表
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}/subjects")]
    [ProducesResponseType<ResponseResult<List<RoleSubjectRes>>>(StatusCodes.Status200OK)]
    public async Task<List<RoleSubjectRes>> GetSubjectsAsync(Guid id)
    {
        return await _roleQueries.ListRoleSubjectsAsync(id);
    }

    /// <summary>
    /// 保存角色对象
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut("{id}/subjects")]
    [ProducesResponseType<ResponseResult<bool>>(StatusCodes.Status200OK)]
    public async Task<bool> SaveSubjectsAsync(Guid id, [FromBody] SaveRoleSubjectsReq req)
    {
        var command = new SaveRoleSubjectsCommand(id, req.Subjects);
        return await Mediator.Send(command);
    }

    /// <summary>
    /// 移除角色对象
    /// </summary>
    /// <param name="id"></param>
    /// <param name="roleSubjectId"></param>
    /// <returns></returns>
    [HttpDelete("{id}/subjects/{roleSubjectId}")]
    [ProducesResponseType<ResponseResult<bool>>(StatusCodes.Status200OK)]
    public async Task<bool> RemoveSubjectAsync(Guid id, Guid roleSubjectId)
    {
        var command = new RemoveRoleSubjectCommand(id, roleSubjectId);
        return await Mediator.Send(command);
    }
}
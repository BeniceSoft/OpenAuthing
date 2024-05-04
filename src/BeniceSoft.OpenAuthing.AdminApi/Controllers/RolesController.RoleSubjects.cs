using BeniceSoft.Abp.Core.Models;
using BeniceSoft.OpenAuthing.Commands.Roles;
using BeniceSoft.OpenAuthing.Dtos.Roles;
using BeniceSoft.OpenAuthing.Models.Roles;
using Microsoft.AspNetCore.Mvc;

namespace BeniceSoft.OpenAuthing.Controllers;

public partial class RolesController
{
    /// <summary>
    /// List role subjects
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}/subjects")]
    [ProducesResponseType<ResponseResult<List<RoleSubjectRes>>>(StatusCodes.Status200OK)]
    public Task<List<RoleSubjectRes>> GetSubjectsAsync(Guid id)
    {
        return RoleQueries.ListRoleSubjectsAsync(id);
    }

    /// <summary>
    /// Add subjects
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("{id}/subjects")]
    [ProducesResponseType<ResponseResult<bool>>(StatusCodes.Status200OK)]
    public Task<bool> SaveSubjectsAsync(Guid id, [FromBody] SaveRoleSubjectsReq req)
    {
        var command = new SaveRoleSubjectsCommand(id, req.Subjects);
        return Mediator.Send(command);
    }

    /// <summary>
    /// Remove a subject
    /// </summary>
    /// <param name="id"></param>
    /// <param name="roleSubjectId"></param>
    /// <returns></returns>
    [HttpDelete("{id}/subjects/{roleSubjectId}")]
    [ProducesResponseType<ResponseResult<bool>>(StatusCodes.Status200OK)]
    public Task<bool> RemoveSubjectAsync(Guid id, Guid roleSubjectId)
    {
        var command = new RemoveRoleSubjectCommand(id, roleSubjectId);
        return Mediator.Send(command);
    }
}
using BeniceSoft.OpenAuthing.Areas.Admin.Models.Roles;
using BeniceSoft.OpenAuthing.Commands.Roles;
using BeniceSoft.OpenAuthing.Dtos.Roles;
using BeniceSoft.OpenAuthing.Enums;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace BeniceSoft.OpenAuthing.Areas.Admin.Controllers;

public partial class RolesController
{
    [HttpGet("{id}/subjects")]
    public async Task<List<RoleSubjectRes>> GetSubjectsAsync(Guid id)
    {
        return await _roleQueries.ListRoleSubjectsAsync(id);
    }

    [HttpPut("{id}/subjects")]
    public async Task<bool> SaveSubjectsAsync(Guid id, [FromBody] SaveRoleSubjectsReq req)
    {
        var command = new SaveRoleSubjectsCommand(id, req.Subjects);
        return await Mediator.Send(command);
    }

    [HttpDelete("{id}/subjects/{roleSubjectId}")]
    public async Task<bool> RemoveSubjectAsync(Guid id, Guid roleSubjectId)
    {
        var command = new RemoveRoleSubjectCommand(id, roleSubjectId);
        return await Mediator.Send(command);
    }
}
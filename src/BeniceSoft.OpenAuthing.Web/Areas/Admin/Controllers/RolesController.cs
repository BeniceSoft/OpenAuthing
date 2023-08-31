using BeniceSoft.OpenAuthing.Areas.Admin.Models.Roles;
using BeniceSoft.OpenAuthing.Commands.Roles;
using BeniceSoft.OpenAuthing.Dtos.Roles;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Areas.Admin.Controllers;

public partial class RolesController : AdminControllerBase
{
    private readonly IRoleQueries _roleQueries;

    public RolesController(IRoleQueries roleQueries)
    {
        _roleQueries = roleQueries;
    }

    [HttpGet]
    public async Task<PagedResultDto<RoleSimpleRes>> GetAsync(string? searchKey = null, int pageIndex = 1, int pageSize = 20)
    {
        var req = new RolePageQueryReq
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            SearchKey = searchKey
        };
        return await _roleQueries.PageQueryAsync(req);
    }

    [HttpGet("{id}")]
    public async Task<RoleDetailRes> GetAsync(Guid id)
    {
        return await _roleQueries.GetDetailAsync(id);
    }

    [HttpPost]
    public async Task<Guid> PostAsync([FromBody] InputRoleReq req)
    {
        var command = new CreateRoleCommand(req.Name, req.DisplayName, req.Description ?? string.Empty);
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<bool> PusAsync(Guid id, [FromBody] InputRoleReq req)
    {
        var command = new UpdateRoleCommand(id, req.Name, req.DisplayName, req.Description);
        return await Mediator.Send(command);
    }

    [HttpDelete("{id}")]
    public async Task<bool> DeleteAsync(Guid id)
    {
        var command = new DeleteRoleCommand(id);
        return await Mediator.Send(command);
    }

    [HttpPut("{id}/toggle-enabled")]
    public async Task<bool> ToggleEnabled(Guid id, bool enabled)
    {
        var command = new ToggleRoleEnabledCommand(id, enabled);
        return await Mediator.Send(command);
    }
}
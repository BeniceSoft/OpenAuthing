using BeniceSoft.Abp.Core.Models;
using BeniceSoft.OpenAuthing.Commands.Roles;
using BeniceSoft.OpenAuthing.Dtos.Roles;
using BeniceSoft.OpenAuthing.Models.Roles;
using BeniceSoft.OpenAuthing.Queries;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Controllers;

/// <summary>
/// Roles
/// </summary>
public partial class RolesController : AuthingApiControllerBase
{
    private IRoleQueries RoleQueries => LazyServiceProvider.LazyGetRequiredService<IRoleQueries>();

    /// <summary>
    /// Query roles
    /// </summary>
    /// <param name="searchKey"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType<ResponseResult<PagedResultDto<RoleSimpleRes>>>(StatusCodes.Status200OK)]
    public Task<PagedResultDto<RoleSimpleRes>> GetAsync(string? searchKey = null, int pageIndex = 1, int pageSize = 20)
    {
        var req = new RolePageQueryReq
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            SearchKey = searchKey
        };
        return RoleQueries.PageQueryAsync(req);
    }

    /// <summary>
    /// Get role details
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType<ResponseResult<PagedResultDto<RoleDetailRes>>>(StatusCodes.Status200OK)]
    public Task<RoleDetailRes> GetAsync(Guid id)
    {
        return RoleQueries.GetDetailAsync(id);
    }

    /// <summary>
    /// Create a role
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType<ResponseResult<Guid>>(StatusCodes.Status200OK)]
    public Task<Guid> PostAsync([FromBody] InputRoleReq req)
    {
        var command = new CreateRoleCommand(req.Name, req.Description ?? string.Empty);
        return Mediator.Send(command);
    }

    /// <summary>
    /// Update role
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType<ResponseResult<bool>>(StatusCodes.Status200OK)]
    public Task<bool> PusAsync(Guid id, [FromBody] InputRoleReq req)
    {
        var command = new UpdateRoleCommand(id, req.Name, req.Description);
        return Mediator.Send(command);
    }

    /// <summary>
    /// Delete a role
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType<ResponseResult<bool>>(StatusCodes.Status200OK)]
    public Task<bool> DeleteAsync(Guid id)
    {
        var command = new DeleteRoleCommand(id);
        return Mediator.Send(command);
    }

    /// <summary>
    /// Set role enabled status
    /// </summary>
    /// <param name="id"></param>
    /// <param name="enabled"></param>
    /// <returns></returns>
    [HttpPut("{id}/toggle-enabled")]
    [ProducesResponseType<ResponseResult<bool>>(StatusCodes.Status200OK)]
    public Task<bool> ToggleEnabled(Guid id, bool enabled)
    {
        var command = new ToggleRoleEnabledCommand(id, enabled);
        return Mediator.Send(command);
    }
}
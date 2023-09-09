using BeniceSoft.OpenAuthing.Commands.Roles;
using BeniceSoft.OpenAuthing.Dtos.Roles;
using BeniceSoft.OpenAuthing.Models.Roles;
using BeniceSoft.OpenAuthing.Queries;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Controllers;

/// <summary>
/// 角色
/// </summary>
public partial class RolesController : AuthingApiControllerBase
{
    private readonly IRoleQueries _roleQueries;

    public RolesController(IRoleQueries roleQueries)
    {
        _roleQueries = roleQueries;
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="searchKey"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 获取详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<RoleDetailRes> GetAsync(Guid id)
    {
        return await _roleQueries.GetDetailAsync(id);
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<Guid> PostAsync([FromBody] InputRoleReq req)
    {
        var command = new CreateRoleCommand(req.Name, req.DisplayName, req.Description ?? string.Empty, req.PermissionSpaceId);
        return await Mediator.Send(command);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<bool> PusAsync(Guid id, [FromBody] InputRoleReq req)
    {
        var command = new UpdateRoleCommand(id, req.Name, req.DisplayName, req.Description);
        return await Mediator.Send(command);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<bool> DeleteAsync(Guid id)
    {
        var command = new DeleteRoleCommand(id);
        return await Mediator.Send(command);
    }

    /// <summary>
    /// 修改启用状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="enabled"></param>
    /// <returns></returns>
    [HttpPut("{id}/toggle-enabled")]
    public async Task<bool> ToggleEnabled(Guid id, bool enabled)
    {
        var command = new ToggleRoleEnabledCommand(id, enabled);
        return await Mediator.Send(command);
    }
}
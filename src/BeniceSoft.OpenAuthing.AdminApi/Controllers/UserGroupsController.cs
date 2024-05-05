using BeniceSoft.Abp.Core.Models;
using BeniceSoft.OpenAuthing.Commands.UserGroups;
using BeniceSoft.OpenAuthing.Dtos.UserGroups;
using BeniceSoft.OpenAuthing.Queries;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Controllers;

/// <summary>
/// User Groups
/// </summary>
public class UserGroupsController : AuthingApiControllerBase
{
    private readonly IUserGroupQueries _userGroupQueries;

    public UserGroupsController(IUserGroupQueries userGroupQueries)
    {
        _userGroupQueries = userGroupQueries;
    }

    /// <summary>
    /// List groups
    /// </summary>
    /// <param name="searchKey"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType<ResponseResult<PagedResultDto<UserGroupPagedRes>>>(StatusCodes.Status200OK)]
    public async Task<PagedResultDto<UserGroupPagedRes>> GetAsync(string? searchKey = null, int pageIndex = 1, int pageSize = 20)
    {
        var req = new UserGroupPagedReq
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            SearchKey = searchKey
        };
        return await _userGroupQueries.PageQueryAsync(req);
    }

    /// <summary>
    /// Get group details
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType<ResponseResult<GetUserGroupRes>>(StatusCodes.Status200OK)]
    public async Task<GetUserGroupRes> GetAsync(Guid id)
    {
        return await _userGroupQueries.GetDetailAsync(id);
    }

    /// <summary>
    /// Create a group
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType<ResponseResult<Guid>>(StatusCodes.Status200OK)]
    public async Task<Guid> CreateAsync([FromBody] InputUserGroupReq req)
    {
        var command = new CreateUserGroupCommand(req.Name, req.Description);
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Update a group
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType<ResponseResult<bool>>(StatusCodes.Status200OK)]
    public async Task<bool> UpdateAsync(Guid id, [FromBody] InputUserGroupReq req)
    {
        var command = new UpdateUserGroupCommand(id, req.Name, req.Description);
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Delete a group
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType<ResponseResult<bool>>(StatusCodes.Status200OK)]
    public async Task<bool> DeleteAsync(Guid id)
    {
        var command = new DeleteUserGroupCommand(id);
        return await Mediator.Send(command);
    }
}
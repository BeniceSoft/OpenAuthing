using BeniceSoft.Abp.Core.Models;
using BeniceSoft.OpenAuthing.Dtos.UserGroups;
using BeniceSoft.OpenAuthing.Queries;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Controllers;

/// <summary>
/// 用户组
/// </summary>
public class UserGroupsController : AuthingApiControllerBase
{
    private readonly IUserGroupQueries _userGroupQueries;

    public UserGroupsController(IUserGroupQueries userGroupQueries)
    {
        _userGroupQueries = userGroupQueries;
    }

    /// <summary>
    /// 获取列表
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
    /// 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType<ResponseResult<GetUserGroupRes>>(StatusCodes.Status200OK)]
    public async Task<GetUserGroupRes> GetAsync(Guid id)
    {
        return await _userGroupQueries.GetDetailAsync(id);
    }
}
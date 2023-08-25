using BeniceSoft.OpenAuthing.Areas.Admin.Models.UserGroups;
using BeniceSoft.OpenAuthing.Misc;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Areas.Admin.Controllers;

/// <summary>
/// 用户组
/// </summary>
public class UserGroupsController : AdminControllerBase
{
    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="searchKey"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedResultDto<UserGroupPagedRes>> GetAsync(string? searchKey = null, int pageIndex = 1, int pageSize = 20)
    {
        var queryable = await UserGroupRepository.GetQueryableAsync();
        var queryableWrapper = QueryableWrapperFactory.CreateWrapper(queryable)
            .SearchByKey(searchKey, x => x.Name)
            .AsNoTracking();

        var totalCount = await queryableWrapper.CountAsync();
        var items = new List<UserGroupPagedRes>();

        if (totalCount > 0)
        {
            var userGroups = await queryableWrapper
                .OrderByDescending(x => x.CreationTime)
                .PagedBy(pageIndex, pageSize)
                .ToListAsync();

            items = userGroups.Adapt<List<UserGroupPagedRes>>();
        }

        return new(totalCount, items);
    }

    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<GetUserGroupRes> GetAsync(Guid id)
    {
        var userGroup = await UserGroupRepository.GetAsync(id);
        var userIds = userGroup.Members.Select(x => x.UserId).ToList();

        var result = userGroup.Adapt<GetUserGroupRes>();

        if (userIds.Any())
        {
            var userQueryable = await UserRepository.GetQueryableAsync();
            result.Members = userQueryable
                .Where(x => userIds.Contains(x.Id))
                .Select(x => new UserGroupMemberRes
                {
                    Id = x.Id, Avatar = x.Avatar, UserName = x.UserName, Nickname = x.Nickname
                })
                .ToList();
        }

        return result;
    }
    
}
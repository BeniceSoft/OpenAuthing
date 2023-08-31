using BeniceSoft.OpenAuthing.Dtos.UserGroups;
using BeniceSoft.OpenAuthing.Misc;
using BeniceSoft.OpenAuthing.UserGroups;
using BeniceSoft.OpenAuthing.Users;
using Mapster;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Queries;

public class UserGroupQueries : BaseQueries, IUserGroupQueries
{
    private readonly IRepository<UserGroup, Guid> _userGroupRepository;
    private readonly IUserRepository _userRepository;

    public UserGroupQueries(IAbpLazyServiceProvider lazyServiceProvider, IRepository<UserGroup, Guid> userGroupRepository,
        IUserRepository userRepository)
        : base(lazyServiceProvider)
    {
        _userGroupRepository = userGroupRepository;
        _userRepository = userRepository;
    }

    public async Task<PagedResultDto<UserGroupPagedRes>> PageQueryAsync(UserGroupPagedReq req)
    {
        var queryable = await _userGroupRepository.GetQueryableAsync();
        var queryableWrapper = QueryableWrapperFactory.CreateWrapper(queryable)
            .SearchByKey(req.SearchKey, x => x.Name)
            .AsNoTracking();

        var totalCount = await queryableWrapper.CountAsync();
        var items = new List<UserGroupPagedRes>();

        if (totalCount > 0)
        {
            var userGroups = await queryableWrapper
                .OrderByDescending(x => x.CreationTime)
                .PagedBy(req.PageIndex, req.PageSize)
                .ToListAsync();

            items = userGroups.Adapt<List<UserGroupPagedRes>>();
        }

        return new(totalCount, items);
    }

    public async Task<GetUserGroupRes> GetDetailAsync(Guid id)
    {
        var userGroup = await _userGroupRepository.GetAsync(id);
        var userIds = userGroup.Members.Select(x => x.UserId).ToList();

        var result = userGroup.Adapt<GetUserGroupRes>();

        if (userIds.Any())
        {
            var userQueryable = await _userRepository.GetQueryableAsync();
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
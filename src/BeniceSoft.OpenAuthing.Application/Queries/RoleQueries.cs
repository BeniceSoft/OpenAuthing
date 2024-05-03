using BeniceSoft.OpenAuthing.Dtos.Roles;
using BeniceSoft.OpenAuthing.Entities.PermissionSpaces;
using BeniceSoft.OpenAuthing.Entities.Roles;
using BeniceSoft.OpenAuthing.Entities.UserGroups;
using BeniceSoft.OpenAuthing.Entities.Users;
using BeniceSoft.OpenAuthing.Enums;
using BeniceSoft.OpenAuthing.Misc;
using Mapster;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Queries;

public class RoleQueries : BaseQueries, IRoleQueries
{
    private IRoleRepository RoleRepository => LazyServiceProvider.LazyGetRequiredService<IRoleRepository>();
    private IRepository<RoleSubject, Guid> RoleSubjectRepository => LazyServiceProvider.LazyGetRequiredService<IRepository<RoleSubject, Guid>>();
    private IUserRepository UserRepository => LazyServiceProvider.LazyGetRequiredService<IUserRepository>();
    private IRepository<UserGroup, Guid> UserGroupRepository => LazyServiceProvider.LazyGetRequiredService<IRepository<UserGroup, Guid>>();

    public async Task<PagedResultDto<RoleSimpleRes>> PageQueryAsync(RolePageQueryReq req)
    {
        var roleQueryable = await RoleRepository.GetQueryableAsync();
        var queryable =
            from role in roleQueryable
            select new RoleSimpleRes
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                CreationTime = role.CreationTime,
                Enabled = role.Enabled,
                IsSystemBuiltIn = role.IsSystemBuiltIn
            };
        var queryableWrapper = QueryableWrapperFactory.CreateWrapper(queryable)
            .SearchByKey(req.SearchKey, x => x.Name, x => x.Description)
            .AsNoTracking();

        var totalCount = await queryableWrapper.CountAsync();
        var items = new List<RoleSimpleRes>();
        if (totalCount > 0)
        {
            items = await queryableWrapper
                .OrderByDescending(x => x.CreationTime)
                .PagedBy(req.PageIndex, req.PageSize)
                .ToListAsync();
        }

        return new(totalCount, items);
    }

    public async Task<RoleDetailRes> GetDetailAsync(Guid roleId)
    {
        var roleQueryable = await RoleRepository.GetQueryableAsync();
        var queryable =
            from role in roleQueryable
            where role.Id == roleId
            select new RoleDetailRes
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                Enabled = role.Enabled,
                IsSystemBuiltIn = role.IsSystemBuiltIn
            };

        var entity = await AsyncExecuter.FirstOrDefaultAsync(queryable);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        return entity;
    }

    public async Task<List<RoleSubjectRes>> ListRoleSubjectsAsync(Guid roleId)
    {
        var subjects = await RoleSubjectRepository.GetQueryableAsync();

        var roleSubjects = await AsyncExecuter.ToListAsync(subjects
            .Where(x => x.RoleId == roleId)
            .OrderByDescending(x => x.CreationTime));

        var result = new List<RoleSubjectRes>(roleSubjects.Count);

        var userIds = roleSubjects.Where(x => x.SubjectType == RoleSubjectType.User)
            .Select(x => x.SubjectId)
            .ToList();
        if (userIds.Any())
        {
            var userQueryable = await UserRepository.GetQueryableAsync();
            var users = await AsyncExecuter.ToListAsync(userQueryable
                .Where(x => userIds.Contains(x.Id))
                .Select(x => new {x.Id, x.Nickname, x.UserName, x.Avatar}));
            foreach (var item in roleSubjects.Where(x => x.SubjectType == RoleSubjectType.User))
            {
                var subject = item.Adapt<RoleSubjectRes>();
                var user = users.FirstOrDefault(x => x.Id == item.SubjectId);
                subject.Name = user?.Nickname;
                subject.Description = user?.UserName;
                subject.Avatar = user?.Avatar;

                result.Add(subject);
            }
        }

        var userGroupIds = roleSubjects.Where(x => x.SubjectType == RoleSubjectType.UserGroup)
            .Select(x => x.SubjectId)
            .ToList();
        if (userGroupIds.Any())
        {
            var userGroupQueryable = await UserGroupRepository.GetQueryableAsync();
            var userGroups = await AsyncExecuter.ToListAsync(userGroupQueryable
                .Where(x => userGroupIds.Contains(x.Id))
                .Select(x => new {x.Id, x.Name}));
            foreach (var item in roleSubjects.Where(x => x.SubjectType == RoleSubjectType.UserGroup))
            {
                var subject = item.Adapt<RoleSubjectRes>();
                var userGroup = userGroups.FirstOrDefault(x => x.Id == item.SubjectId);
                subject.Name = userGroup?.Name;

                result.Add(subject);
            }
        }

        return result.OrderByDescending(x => x.CreationTime).ToList();
    }
}
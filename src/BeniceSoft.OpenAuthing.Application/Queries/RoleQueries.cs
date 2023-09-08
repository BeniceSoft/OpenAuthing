using BeniceSoft.OpenAuthing.Dtos.Roles;
using BeniceSoft.OpenAuthing.Enums;
using BeniceSoft.OpenAuthing.Misc;
using BeniceSoft.OpenAuthing.PermissionSpaces;
using BeniceSoft.OpenAuthing.Roles;
using BeniceSoft.OpenAuthing.UserGroups;
using BeniceSoft.OpenAuthing.Users;
using Mapster;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Queries;

public class RoleQueries : BaseQueries, IRoleQueries
{
    private readonly IRoleRepository _roleRepository;
    private readonly IRepository<RoleSubject, Guid> _roleSubjectRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRepository<UserGroup, Guid> _userGroupRepository;
    private readonly IRepository<PermissionSpace, Guid> _spaceRepository;

    public RoleQueries(IAbpLazyServiceProvider lazyServiceProvider, IRoleRepository roleRepository,
        IRepository<RoleSubject, Guid> roleSubjectRepository, IUserRepository userRepository, IRepository<UserGroup, Guid> userGroupRepository, IRepository<PermissionSpace, Guid> spaceRepository)
        : base(lazyServiceProvider)
    {
        _roleRepository = roleRepository;
        _roleSubjectRepository = roleSubjectRepository;
        _userRepository = userRepository;
        _userGroupRepository = userGroupRepository;
        _spaceRepository = spaceRepository;
    }

    public async Task<PagedResultDto<RoleSimpleRes>> PageQueryAsync(RolePageQueryReq req)
    {
        var roleQueryable = await _roleRepository.GetQueryableAsync();
        var spaceQueryable = await _spaceRepository.GetQueryableAsync();
        var queryable =
            from role in roleQueryable
            join space in spaceQueryable on role.PermissionSpaceId equals space.Id
            select new
            {
                role.Id, role.Name, role.DisplayName, role.Description, role.CreationTime, role.Enabled, role.IsSystemBuiltIn,
                PermissionSpaceName = space.DisplayName
            };
        var queryableWrapper = QueryableWrapperFactory.CreateWrapper(queryable)
            .SearchByKey(req.SearchKey, x => x.Name, x => x.DisplayName)
            .AsNoTracking();

        var totalCount = await queryableWrapper.CountAsync();
        var items = new List<RoleSimpleRes>();
        if (totalCount > 0)
        {
            var roles = await queryableWrapper
                .OrderByDescending(x => x.CreationTime)
                .PagedBy(req.PageIndex, req.PageSize)
                .ToListAsync();

            items = roles.Adapt<List<RoleSimpleRes>>();
        }

        return new(totalCount, items);
    }

    public async Task<RoleDetailRes> GetDetailAsync(Guid roleId)
    {
        var roleQueryable = await _roleRepository.GetQueryableAsync();
        var spaceQueryable = await _spaceRepository.GetQueryableAsync();
        var queryable =
            from role in roleQueryable
            join space in spaceQueryable on role.PermissionSpaceId equals space.Id
            where role.Id == roleId
            select new RoleDetailRes
            {
                Id = role.Id,
                Name = role.Name, 
                DisplayName = role.DisplayName,
                Description = role.Description,
                Enabled = role.Enabled, 
                IsSystemBuiltIn = role.IsSystemBuiltIn,
                PermissionSpaceId = role.PermissionSpaceId, 
                PermissionSpaceName = space.DisplayName
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
        var subjects = await _roleSubjectRepository.GetQueryableAsync();

        var roleSubjects = await AsyncExecuter.ToListAsync(subjects
            .Where(x => x.RoleId == roleId)
            .OrderByDescending(x => x.CreationTime));

        var result = new List<RoleSubjectRes>(roleSubjects.Count);

        var userIds = roleSubjects.Where(x => x.SubjectType == RoleSubjectType.User)
            .Select(x => x.SubjectId)
            .ToList();
        if (userIds.Any())
        {
            var userQueryable = await _userRepository.GetQueryableAsync();
            var users = await AsyncExecuter.ToListAsync(userQueryable
                .Where(x => userIds.Contains(x.Id))
                .Select(x => new { x.Id, x.Nickname, x.UserName, x.Avatar }));
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
            var userGroupQueryable = await _userGroupRepository.GetQueryableAsync();
            var userGroups = await AsyncExecuter.ToListAsync(userGroupQueryable
                .Where(x => userGroupIds.Contains(x.Id))
                .Select(x => new { x.Id, x.Name }));
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
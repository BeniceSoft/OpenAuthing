using BeniceSoft.OpenAuthing.Dtos.DepartmentMembers;
using BeniceSoft.OpenAuthing.Dtos.Users;
using BeniceSoft.OpenAuthing.Entities.DepartmentMembers;
using BeniceSoft.OpenAuthing.Entities.Departments;
using BeniceSoft.OpenAuthing.Entities.Roles;
using BeniceSoft.OpenAuthing.Entities.UserGroups;
using BeniceSoft.OpenAuthing.Entities.Users;
using BeniceSoft.OpenAuthing.Enums;
using BeniceSoft.OpenAuthing.Misc;
using Mapster;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Queries;

public class UserQueries : BaseQueries, IUserQueries
{
    private IRepository<DepartmentMember> DepartmentMemberRepository => LazyServiceProvider.LazyGetRequiredService<IRepository<DepartmentMember>>();
    private IUserRepository UserRepository => LazyServiceProvider.LazyGetRequiredService<IUserRepository>();
    private IRepository<Department, Guid> DepartmentRepository => LazyServiceProvider.LazyGetRequiredService<IRepository<Department, Guid>>();
    private IRepository<UserGroupMember> UserGroupMemberRepository => LazyServiceProvider.LazyGetRequiredService<IRepository<UserGroupMember>>();
    private IRoleRepository RoleRepository => LazyServiceProvider.LazyGetRequiredService<IRoleRepository>();
    private IRepository<RoleSubject> RoleSubjectRepository => LazyServiceProvider.LazyGetRequiredService<IRepository<RoleSubject>>();
    private IRepository<UserGroup> UserGroupRepository => LazyServiceProvider.LazyGetRequiredService<IRepository<UserGroup>>();

    public async Task<PagedResultDto<UserPagedRes>> PagedQueryAsync(UserPagedReq req)
    {
        var departmentMembers = await DepartmentMemberRepository.GetQueryableAsync();
        var queryable = await UserRepository.GetQueryableAsync();
        var queryableWrapper = QueryableWrapperFactory.CreateWrapper(queryable)
            .SearchByKey(req.SearchKey, x => x.UserName, x => x.PhoneNumber, x => x.Nickname)
            .WhereIf(req.ExcludeDepartmentId.HasValue, x => departmentMembers
                .Where(y => y.DepartmentId == req.ExcludeDepartmentId)
                .All(y => y.UserId != x.Id))
            .WhereIf(req.OnlyEnabled, x => x.Enabled)
            .AsNoTracking();

        var totalCount = await queryableWrapper.CountAsync();
        var items = new List<UserPagedRes>();
        if (totalCount > 0)
        {
            var users = await queryableWrapper
                .OrderByDescending(x => x.CreationTime)
                .PagedBy(req.PageIndex, req.PageSize)
                .ToListAsync();

            items = users.Adapt<List<UserPagedRes>>();
        }

        return new(totalCount, items);
    }

    public async Task<UserDetailRes> GetDetailAsync(Guid id)
    {
        var user = await UserRepository.GetAsync(id);

        return user.Adapt<UserDetailRes>();
    }

    public async Task<List<UserDepartmentDto>> ListUserDepartmentsAsync(Guid userId)
    {
        var departments = await DepartmentRepository.GetQueryableAsync();
        var departmentMembers = await DepartmentMemberRepository.GetQueryableAsync();
        var queryable =
            from departmentMember in departmentMembers
            join department in departments on departmentMember.DepartmentId equals department.Id
            where departmentMember.UserId == userId
            orderby departmentMember.IsMain descending, departmentMember.IsLeader descending
            select new UserDepartmentDto
            {
                DepartmentId = department.Id,
                DepartmentName = department.Name,
                IsMain = departmentMember.IsMain,
                IsLeader = departmentMember.IsLeader
            };
        return await QueryableWrapperFactory.CreateWrapper(queryable)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<UserRoleRes>> ListUserRolesAsync(Guid userId)
    {
        var userGroups = await UserGroupRepository.GetQueryableAsync();
        var userGroupMembers = await UserGroupMemberRepository.GetQueryableAsync();
        var roleSubjects = await RoleSubjectRepository.GetQueryableAsync();
        var roles = await RoleRepository.GetQueryableAsync();

        // 用户直接与角色绑定
        var userRoles = await AsyncExecuter.ToListAsync(
            from roleSubject in roleSubjects
            join role in roles on roleSubject.RoleId equals role.Id
            where roleSubject.SubjectType == RoleSubjectType.User && roleSubject.SubjectId == userId
            select new UserRoleRes
            {
                RoleId = roleSubject.Id,
                RoleName = role.Name,
                RoleDescription = role.Description,
                AssignmentSubjectType = RoleSubjectType.User,
                AssignmentSubjectId = userId
            }
        );

        // 用户所属用户组与角色绑定
        var userGroupRoles = await AsyncExecuter.ToListAsync(
            from userGroupMember in userGroupMembers
            join userGroup in userGroups on userGroupMember.UserGroupId equals userGroup.Id
            join userGroupRole in roleSubjects.Where(x => x.SubjectType == RoleSubjectType.UserGroup)
                on userGroupMember.UserGroupId equals userGroupRole.SubjectId
            join role in roles on userGroupRole.RoleId equals role.Id
            where userGroupMember.UserId == userId
            select new UserRoleRes
            {
                RoleId = userGroupRole.Id,
                RoleName = role.Name,
                RoleDescription = role.Description,
                AssignmentSubjectType = RoleSubjectType.UserGroup,
                AssignmentSubjectId = userGroup.Id,
                AssignmentSubjectName = userGroup.Name
            }
        );
        
        return [..userRoles, ..userGroupRoles];
    }
}
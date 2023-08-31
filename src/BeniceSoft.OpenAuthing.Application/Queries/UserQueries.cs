using BeniceSoft.OpenAuthing.DepartmentMembers;
using BeniceSoft.OpenAuthing.Departments;
using BeniceSoft.OpenAuthing.Dtos.DepartmentMembers;
using BeniceSoft.OpenAuthing.Dtos.Users;
using BeniceSoft.OpenAuthing.Misc;
using BeniceSoft.OpenAuthing.Users;
using Mapster;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Queries;

public class UserQueries : BaseQueries, IUserQueries
{
    private readonly IRepository<DepartmentMember> _departmentMemberRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRepository<Department, Guid> _departmentRepository;

    public UserQueries(IAbpLazyServiceProvider lazyServiceProvider, IRepository<DepartmentMember> departmentMemberRepository,
        IUserRepository userRepository, IRepository<Department, Guid> departmentRepository)
        : base(lazyServiceProvider)
    {
        _departmentMemberRepository = departmentMemberRepository;
        _userRepository = userRepository;
        _departmentRepository = departmentRepository;
    }

    public async Task<PagedResultDto<UserPagedRes>> PagedQueryAsync(UserPagedReq req)
    {
        var departmentMembers = await _departmentMemberRepository.GetQueryableAsync();
        var queryable = await _userRepository.GetQueryableAsync();
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
        var user = await _userRepository.GetAsync(id);

        return user.Adapt<UserDetailRes>();
    }

    public async Task<List<UserDepartmentDto>> ListUserDepartmentsAsync(Guid userId)
    {
        var departments = await _departmentRepository.GetQueryableAsync();
        var departmentMembers = await _departmentMemberRepository.GetQueryableAsync();
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
}
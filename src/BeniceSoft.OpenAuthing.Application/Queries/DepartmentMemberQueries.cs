using BeniceSoft.OpenAuthing.DepartmentMembers;
using BeniceSoft.OpenAuthing.Departments;
using BeniceSoft.OpenAuthing.Dtos.DepartmentMembers;
using BeniceSoft.OpenAuthing.Users;
using Mapster;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Queries;

public class DepartmentMemberQueries : BaseQueries, IDepartmentMemberQueries
{
    private readonly IRepository<DepartmentMember> _departmentMemberRepository;
    private readonly IRepository<Department, Guid> _departmentRepository;
    private readonly IUserRepository _userRepository;

    public DepartmentMemberQueries(IAbpLazyServiceProvider lazyServiceProvider, IRepository<DepartmentMember> departmentMemberRepository,
        IRepository<Department, Guid> departmentRepository, IUserRepository userRepository) : base(
        lazyServiceProvider)
    {
        _departmentMemberRepository = departmentMemberRepository;
        _departmentRepository = departmentRepository;
        _userRepository = userRepository;
    }

    public async Task<PagedResultDto<QueryDepartmentMembersRes>> QueryDepartmentMembersAsync(Guid departmentId, QueryDepartmentMembersReq req)
    {
        var departmentMembers = await _departmentMemberRepository.GetQueryableAsync();
        var departments = await _departmentRepository.GetQueryableAsync();
        var users = await _userRepository.GetQueryableAsync();

        var currentDepartment = await AsyncExecuter.FirstOrDefaultAsync(departments.Where(x => x.Id == departmentId));
        if (currentDepartment is null) return new();

        List<Guid> matchedDepartmentIds;

        if (req.OnlyDirectUsers)
        {
            matchedDepartmentIds = new() { departmentId };
        }
        else
        {
            matchedDepartmentIds = await AsyncExecuter.ToListAsync(from department in departments
                where department.Paths.StartsWith(currentDepartment.Paths)
                select department.Id);
        }

        var queryable =
            from user in users
            let isLeader = departmentMembers
                .Any(x => x.IsLeader && x.UserId == user.Id && x.DepartmentId == departmentId)
            where departmentMembers.Any(x => x.UserId == user.Id &&
                                             matchedDepartmentIds.Contains(x.DepartmentId))
            orderby isLeader descending
            select new
            {
                entity = user, isLeader
            };

        var queryableWrapper = QueryableWrapperFactory.CreateWrapper(queryable)
            .AsNoTracking();

        var totalCount = await queryableWrapper.CountAsync();
        var items = new List<QueryDepartmentMembersRes>(req.MaxResultCount);
        if (totalCount > 0)
        {
            var userEntities = await queryableWrapper
                .PageBy(req.SkipCount, req.MaxResultCount)
                .ToListAsync();
            var userIds = userEntities.Select(x => x.entity.Id);

            var userDepartmentLookup = (await AsyncExecuter.ToListAsync(
                from departmentMember in departmentMembers
                join department in departments on departmentMember.DepartmentId equals department.Id
                where userIds.Contains(departmentMember.UserId)
                select new
                {
                    departmentMember.UserId, departmentMember.DepartmentId, DepartmentName = department.Name,
                    departmentMember.IsMain, departmentMember.IsLeader
                }
            )).ToLookup(x => x.UserId);
            foreach (var user in userEntities)
            {
                var item = user.entity.Adapt<QueryDepartmentMembersRes>();
                var userDepartments = userDepartmentLookup[user.entity.Id].ToList();
                // item.IsLeader = userDepartments.Any(x => x.IsLeader && x.DepartmentId == departmentId);
                item.IsLeader = user.isLeader;
                item.Departments = userDepartments
                    .Select(x => new UserDepartmentDto()
                    {
                        DepartmentId = x.DepartmentId,
                        DepartmentName = x.DepartmentName,
                        IsMain = x.IsMain,
                        IsLeader = x.IsLeader
                    });
                item.PhoneNumber = user.entity.PhoneNumber;

                items.Add(item);
            }
        }

        return new(totalCount, items);
    }
}
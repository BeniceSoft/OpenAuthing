using BeniceSoft.OpenAuthing.DepartmentMembers;
using BeniceSoft.OpenAuthing.Departments;
using BeniceSoft.OpenAuthing.Dtos.DepartmentMembers.Requests;
using BeniceSoft.OpenAuthing.Dtos.DepartmentMembers.Responses;
using BeniceSoft.OpenAuthing.Extensions;
using BeniceSoft.OpenAuthing.Users;
using Mapster;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Services.DepartmentMembers;

[RemoteService(IsMetadataEnabled = false)]
public class DepartmentMemberAppService : AmAppServiceBase, IDepartmentMemberAppService
{
    private readonly IUserRepository _userRepository;
    private readonly IRepository<DepartmentMember> _departmentMemberRepository;
    private readonly IRepository<Department, Guid> _departmentRepository;

    public DepartmentMemberAppService(IUserRepository userRepository,
        IRepository<DepartmentMember> departmentMemberRepository,
        IRepository<Department, Guid> departmentRepository)
    {
        _userRepository = userRepository;
        _departmentMemberRepository = departmentMemberRepository;
        _departmentRepository = departmentRepository;
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
                item.PhoneNumber = user.entity.PhoneNumber?.Mask(3, 4);

                items.Add(item);
            }
        }

        return new(totalCount, items);
    }

    public async Task<int> AddDepartmentMembersAsync(Guid departmentId, AddDepartmentMembersReq req)
    {
        var queryable = await _departmentMemberRepository.GetQueryableAsync();
        var existedMembers = await QueryableWrapperFactory
            .CreateWrapper(queryable
                .Where(x => x.DepartmentId == departmentId)
                .Where(x => req.UserIds.Contains(x.UserId)))
            .ToListAsync();
        var existedMemberIds = existedMembers.Select(x => x.UserId);

        var newMembers = req.UserIds.Except(existedMemberIds)
            .Select(userId => new DepartmentMember(departmentId, userId))
            .ToList();

        if (newMembers.Any())
        {
            await _departmentMemberRepository.InsertManyAsync(newMembers);
        }

        return newMembers.Count;
    }

    public async Task SetLeaderAsync(Guid departmentId, Guid userId, bool isLeader)
    {
        var departmentMember = await _departmentMemberRepository.GetAsync(x => x.DepartmentId == departmentId && x.UserId == userId);

        departmentMember.SetLeader(isLeader);

        await _departmentMemberRepository.UpdateAsync(departmentMember);
    }

    public async Task SetMainDepartmentAsync(Guid departmentId, Guid userId, bool isMain)
    {
        var departmentMember = await _departmentMemberRepository.GetAsync(x => x.DepartmentId == departmentId && x.UserId == userId);

        departmentMember.SetMain(isMain);

        await _departmentMemberRepository.UpdateAsync(departmentMember);
    }
}
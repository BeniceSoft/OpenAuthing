using BeniceSoft.OpenAuthing.Dtos.DepartmentMembers.Requests;
using BeniceSoft.OpenAuthing.Dtos.DepartmentMembers.Responses;
using LinkMore.Abp.Extensions.DistributedLock.Abstractions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BeniceSoft.OpenAuthing;

public interface IDepartmentMemberAppService : IApplicationService
{
    Task<PagedResultDto<QueryDepartmentMembersRes>> QueryDepartmentMembersAsync(Guid departmentId, QueryDepartmentMembersReq req);
    Task<int> AddDepartmentMembersAsync(Guid departmentId, AddDepartmentMembersReq req);

    [DistributedLock(ResourceId = ApplicationContractsConstants.Lock.DepartmentMemberLeader)]
    Task SetLeaderAsync(Guid departmentId, Guid userId, bool isLeader);

    [DistributedLock(ResourceId = ApplicationContractsConstants.Lock.DepartmentMemberMain)]
    Task SetMainDepartmentAsync(Guid departmentId, Guid userId, bool isMain);
}
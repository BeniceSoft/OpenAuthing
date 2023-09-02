using BeniceSoft.OpenAuthing.Dtos.DepartmentMembers;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Queries;

public interface IDepartmentMemberQueries : ITransientDependency
{
    Task<PagedResultDto<QueryDepartmentMembersRes>> QueryDepartmentMembersAsync(Guid departmentId, QueryDepartmentMembersReq req);
}
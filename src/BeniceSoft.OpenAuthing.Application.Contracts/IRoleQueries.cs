using BeniceSoft.OpenAuthing.Dtos.Roles;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing;

public interface IRoleQueries : ITransientDependency
{
    Task<PagedResultDto<RoleSimpleRes>> PageQueryAsync(RolePageQueryReq req);

    Task<RoleDetailRes> GetDetailAsync(Guid roleId);
    Task<List<RoleSubjectRes>> ListRoleSubjectsAsync(Guid roleId);
}
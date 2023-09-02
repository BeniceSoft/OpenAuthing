using BeniceSoft.OpenAuthing.Dtos.UserGroups;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Queries;

public interface IUserGroupQueries : ITransientDependency
{
    Task<PagedResultDto<UserGroupPagedRes>> PageQueryAsync(UserGroupPagedReq req);

    Task<GetUserGroupRes> GetDetailAsync(Guid id);
}
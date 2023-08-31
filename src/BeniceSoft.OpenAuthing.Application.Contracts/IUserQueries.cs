using BeniceSoft.OpenAuthing.Dtos.DepartmentMembers;
using BeniceSoft.OpenAuthing.Dtos.Users;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing;

public interface IUserQueries : ITransientDependency
{
    Task<PagedResultDto<UserPagedRes>> PagedQueryAsync(UserPagedReq req);

    Task<UserDetailRes> GetDetailAsync(Guid id);

    Task<List<UserDepartmentDto>> ListUserDepartmentsAsync(Guid userId);
}
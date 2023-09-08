using BeniceSoft.OpenAuthing.Dtos.PermissionSpaces;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Queries;

public interface IPermissionSpaceQueries : ITransientDependency
{
    Task<List<ListPermissionSpaceRes>> ListAllAsync(ListPermissionSpaceReq req);
    Task<GetPermissionSpaceRes> GetDetailAsync(Guid id);
}
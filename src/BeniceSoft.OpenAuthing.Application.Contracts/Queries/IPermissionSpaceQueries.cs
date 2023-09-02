using BeniceSoft.OpenAuthing.Dtos.PermissionSpaces;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Queries;

public interface IPermissionSpaceQueries : ITransientDependency
{
    Task<PagedResultDto<PagedPermissionSpaceRes>> PagedListAsync(PagedPermissionSpaceReq req);
}
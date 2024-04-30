using BeniceSoft.OpenAuthing.Dtos.Positions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Queries;

public interface IPositionQueries : ITransientDependency
{
    Task<PagedResultDto<PositionRes>> PagedQueryAsync(PositionPagedReq req);
}
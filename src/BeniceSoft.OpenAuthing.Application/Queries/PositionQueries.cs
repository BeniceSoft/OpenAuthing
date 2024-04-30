using BeniceSoft.OpenAuthing.Dtos.Positions;
using BeniceSoft.OpenAuthing.Entities.Positions;
using BeniceSoft.OpenAuthing.Misc;
using Mapster;
using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Queries;

public class PositionQueries : BaseQueries, IPositionQueries
{
    private IPositionRepository PositionRepository => LazyServiceProvider.LazyGetRequiredService<IPositionRepository>();

    public async Task<PagedResultDto<PositionRes>> PagedQueryAsync(PositionPagedReq req)
    {
        var queryable = await PositionRepository.GetQueryableAsync();
        var queryableWrapper = QueryableWrapperFactory.CreateWrapper(queryable)
            .SearchByKey(req.SearchKey, x => x.Name)
            .AsNoTracking();

        var totalCount = await queryableWrapper.CountAsync();
        var items = new List<PositionRes>();

        if (totalCount > 0)
        {
            var positions = await queryableWrapper
                .OrderByDescending(x => x.Id)
                .PagedBy(req.PageIndex, req.PageSize)
                .ToListAsync();
            items = positions.Adapt<List<PositionRes>>();
        }

        return new(totalCount, items);
    }
}
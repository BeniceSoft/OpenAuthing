using BeniceSoft.OpenAuthing.Dtos.PermissionSpaces;
using BeniceSoft.OpenAuthing.Misc;
using BeniceSoft.OpenAuthing.PermissionSpaces;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Queries;

public class PermissionSpaceQueries : BaseQueries, IPermissionSpaceQueries
{
    private readonly IRepository<PermissionSpace, Guid> _spaceRepository;

    public PermissionSpaceQueries(IAbpLazyServiceProvider lazyServiceProvider, IRepository<PermissionSpace, Guid> spaceRepository)
        : base(lazyServiceProvider)
    {
        _spaceRepository = spaceRepository;
    }

    public async Task<PagedResultDto<PagedPermissionSpaceRes>> PagedListAsync(PagedPermissionSpaceReq req)
    {
        var queryable =
            from space in await _spaceRepository.GetQueryableAsync()
            orderby space.CreationTime descending
            select new PagedPermissionSpaceRes
            {
                Id = space.Id,
                Name = space.Name,
                DisplayName = space.DisplayName,
                Description = space.Description
            };
        var queryableWrapper = QueryableWrapperFactory.CreateWrapper(queryable)
            .SearchByKey(req.SearchKey, x => x.DisplayName);

        var totalCount = await queryableWrapper.CountAsync();
        var items = new List<PagedPermissionSpaceRes>(req.PageSize);
        if (totalCount > 0)
        {
            items = await queryableWrapper
                .PagedBy(req.PageIndex, req.PageSize)
                .ToListAsync();
        }

        return new(totalCount, items);
    }
}
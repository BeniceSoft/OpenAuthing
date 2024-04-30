using BeniceSoft.OpenAuthing.Dtos.PermissionSpaces;
using BeniceSoft.OpenAuthing.Entities.PermissionSpaces;
using Mapster;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Queries;

public class PermissionSpaceQueries : BaseQueries, IPermissionSpaceQueries
{
    private readonly IRepository<PermissionSpace, Guid> _spaceRepository;

    public PermissionSpaceQueries(IRepository<PermissionSpace, Guid> spaceRepository)
    {
        _spaceRepository = spaceRepository;
    }

    public async Task<List<ListPermissionSpaceRes>> ListAllAsync(ListPermissionSpaceReq req)
    {
        var queryable =
            from space in await _spaceRepository.GetQueryableAsync()
            orderby space.CreationTime descending
            select new ListPermissionSpaceRes
            {
                Id = space.Id,
                Name = space.Name,
                DisplayName = space.DisplayName,
                Description = space.Description,
                IsSystemBuiltIn = space.IsSystemBuiltIn
            };
        var queryableWrapper = QueryableWrapperFactory.CreateWrapper(queryable)
            .SearchByKey(req.SearchKey, x => x.DisplayName);

        return await queryableWrapper.ToListAsync();
    }

    public async Task<GetPermissionSpaceRes> GetDetailAsync(Guid id)
    {
        var space = await _spaceRepository.GetAsync(id);
        return space.Adapt<GetPermissionSpaceRes>();
    }
}
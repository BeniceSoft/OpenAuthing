using BeniceSoft.OpenAuthing.Dtos.OpenIddict.Responses;
using Mapster;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.OpenIddict.Applications;

namespace BeniceSoft.OpenAuthing.Queries;

public class ApplicationQueries : BaseQueries, IApplicationQueries
{
    private readonly IRepository<OpenIddictApplication> _applicationRepository;

    public ApplicationQueries(IRepository<OpenIddictApplication> applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    public async Task<List<QueryApplicationRes>> ListQueryAsync(string? searchKey = null)
    {
        var queryable = await _applicationRepository.GetQueryableAsync();
        var application = await AsyncExecuter.ToListAsync(queryable
            .WhereIf(!string.IsNullOrWhiteSpace(searchKey), x => x.DisplayName!.Contains(searchKey!))
            .OrderByDescending(x => x.CreationTime));

        return application.Adapt<List<QueryApplicationRes>>();
    }
}
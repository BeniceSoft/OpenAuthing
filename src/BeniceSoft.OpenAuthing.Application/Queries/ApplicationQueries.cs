using BeniceSoft.OpenAuthing.Dtos.OpenIddict.Responses;
using BeniceSoft.OpenAuthing.OpenIddict.Applications;
using Mapster;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Queries;

public class ApplicationQueries : BaseQueries, IApplicationQueries
{
    private readonly IOpenIddictApplicationRepository _applicationRepository;

    public ApplicationQueries(IAbpLazyServiceProvider lazyServiceProvider, IOpenIddictApplicationRepository applicationRepository) : base(
        lazyServiceProvider)
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
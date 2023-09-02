using BeniceSoft.OpenAuthing.Dtos.OpenIddict.Responses;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Queries;

public interface IApplicationQueries : ITransientDependency
{
    Task<List<QueryApplicationRes>> ListQueryAsync(string? searchKey = null);
}
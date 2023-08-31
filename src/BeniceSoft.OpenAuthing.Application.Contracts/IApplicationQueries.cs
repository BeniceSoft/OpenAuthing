using BeniceSoft.OpenAuthing.Dtos.OpenIddict.Responses;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing;

public interface IApplicationQueries : ITransientDependency
{
    Task<List<QueryApplicationRes>> ListQueryAsync(string? searchKey = null);
}
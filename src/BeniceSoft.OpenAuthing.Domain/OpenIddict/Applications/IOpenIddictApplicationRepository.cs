using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.OpenIddict.Applications;

public interface IOpenIddictApplicationRepository : IReadOnlyRepository<OpenIddictApplication>, IBasicRepository<OpenIddictApplication, Guid>
{
    Task<List<OpenIddictApplication>> GetListAsync(string sorting, int skipCount, int maxResultCount, string filter = null, CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default);
    
    Task<OpenIddictApplication> FindByClientIdAsync(string clientId, CancellationToken cancellationToken = default);

    Task<List<OpenIddictApplication>> FindByPostLogoutRedirectUriAsync(string address, CancellationToken cancellationToken = default);

    Task<List<OpenIddictApplication>> FindByRedirectUriAsync(string address, CancellationToken cancellationToken = default);

    Task<List<OpenIddictApplication>> ListAsync(int? count, int? offset, CancellationToken cancellationToken = default);
}

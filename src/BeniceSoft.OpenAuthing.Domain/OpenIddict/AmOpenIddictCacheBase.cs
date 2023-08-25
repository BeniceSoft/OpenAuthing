using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Caching;

namespace BeniceSoft.OpenAuthing.OpenIddict;

public class AmOpenIddictCacheBase<TEntity, TModel, TStore>
    where TModel : class
    where TEntity : class
{
    public ILogger<AmOpenIddictCacheBase<TEntity, TModel, TStore>> Logger { get; set; }

    protected IDistributedCache<TModel> Cache { get; }

    protected IDistributedCache<TModel[]> ArrayCache { get; }

    protected TStore Store { get; }

    protected AmOpenIddictCacheBase(IDistributedCache<TModel> cache, IDistributedCache<TModel[]> arrayCache, TStore store)
    {
        Cache = cache;
        ArrayCache = arrayCache;
        Store = store;

        Logger = NullLogger<AmOpenIddictCacheBase<TEntity, TModel, TStore>>.Instance;
    }
}
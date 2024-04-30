using BeniceSoft.Abp.Ddd.Domain;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Linq;
using Volo.Abp.ObjectMapping;

namespace BeniceSoft.OpenAuthing.Queries;

public abstract class BaseQueries
{
    public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = null!;

    protected Type? ObjectMapperContext { get; set; }

    protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetService<IObjectMapper>(provider =>
        ObjectMapperContext == null
            ? provider.GetRequiredService<IObjectMapper>()
            : (IObjectMapper)provider.GetRequiredService(typeof(IObjectMapper<>).MakeGenericType(ObjectMapperContext)));

    protected IQueryableWrapperFactory QueryableWrapperFactory => LazyServiceProvider.LazyGetRequiredService<IQueryableWrapperFactory>();

    protected IAsyncQueryableExecuter AsyncExecuter => LazyServiceProvider.GetRequiredService<IAsyncQueryableExecuter>();
}
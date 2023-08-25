using BeniceSoft.Abp.Ddd.Domain;

namespace BeniceSoft.OpenAuthing.Misc;

public static class QueryableWrapperExtensions
{
    public static IQueryableWrapper<TEntity> PagedBy<TEntity>(this IQueryableWrapper<TEntity> queryableWrapper, int pageIndex, int pageSize)
        where TEntity : class
    {
        var skipCount = (pageIndex - 1) * pageSize;
        var maxResultCount = pageSize;
        
        return queryableWrapper.PageBy(skipCount, maxResultCount);
    }
}
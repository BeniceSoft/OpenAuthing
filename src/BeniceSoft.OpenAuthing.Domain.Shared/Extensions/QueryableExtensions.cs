using Volo.Abp;

namespace BeniceSoft.OpenAuthing.Extensions;

public static class QueryableExtensions
{
    public static TQueryable SkipIf<T, TQueryable>(this TQueryable query, bool condition, int? count)
        where TQueryable : IQueryable<T>
    {
        Check.NotNull(query, nameof(query));

        if (condition && count.HasValue)
        {
            return (TQueryable)query.Skip(count.Value);
        }

        return query;
    }

    public static TQueryable TakeIf<T, TQueryable>(this TQueryable query, bool condition, int? count)
        where TQueryable : IQueryable<T>
    {
        Check.NotNull(query, nameof(query));

        if (condition && count.HasValue)
        {
            return (TQueryable)query.Take(count.Value);
        }

        return query;
    }
}
using BeniceSoft.OpenAuthing.Roles;
using BeniceSoft.OpenAuthing.Users;
using Microsoft.EntityFrameworkCore;

namespace BeniceSoft.OpenAuthing.Extensions;

public static class IdentityEfCoreQueryableExtensions
{
    public static IQueryable<User> IncludeDetails(this IQueryable<User> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            .Include(x => x.Tokens)
            .Include(x => x.Logins);
    }

    public static IQueryable<Role> IncludeDetails(this IQueryable<Role> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable;
    }
}
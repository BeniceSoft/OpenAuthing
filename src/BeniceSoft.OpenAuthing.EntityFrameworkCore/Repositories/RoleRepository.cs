using BeniceSoft.OpenAuthing.Enums;
using BeniceSoft.OpenAuthing.Roles;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace BeniceSoft.OpenAuthing.Repositories;

public class RoleRepository : EfCoreRepository<AmDbContext, Role, Guid>, IRoleRepository
{
    public RoleRepository(IDbContextProvider<AmDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<Role?> FindByNormalizedNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        var roleQueryable = await GetQueryableAsync();
        return await roleQueryable
            .FirstOrDefaultAsync(x => x.NormalizedName == normalizedRoleName, cancellationToken: cancellationToken);
    }

    public async Task<List<Role>> GetUserRolesAsync(Guid userId)
    {
        var dbContext = await GetDbContextAsync();

        // 用户直接与角色绑定
        var userRoleIds = await dbContext.RoleSubjects
            .Where(x => x.SubjectType == RoleSubjectType.User && x.SubjectId == userId)
            .Select(x => x.RoleId).ToListAsync();

        // 用户所属用户组与角色绑定
        var userGroupRoleIds = await (from userGroupMember in dbContext.UserGroupMembers
                join userGroupRole in dbContext.RoleSubjects.Where(x => x.SubjectType == RoleSubjectType.UserGroup)
                    on userGroupMember.UserGroupId equals userGroupRole.SubjectId
                where userGroupMember.UserId == userId
                select userGroupRole.RoleId).Distinct()
            .ToListAsync();

        var roleIds = userRoleIds.Concat(userGroupRoleIds).Distinct().ToList();
        if (roleIds.Any())
        {
            return await dbContext.Roles.Where(x => x.Enabled && roleIds.Contains(x.Id)).ToListAsync();
        }

        return new();
    }
}
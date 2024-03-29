using BeniceSoft.OpenAuthing.Enums;
using BeniceSoft.OpenAuthing.PermissionSpaces;
using BeniceSoft.OpenAuthing.Users;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace BeniceSoft.OpenAuthing.Roles;

public class RoleDataSeedContributor(RoleManager roleManager, ILookupNormalizer lookupNormalizer, IGuidGenerator guidGenerator)
    : IDataSeedContributor, ITransientDependency
{
    public const string AdminRoleIdProperty = "AdminRoleId";
    public const string AdminRoleNameProperty = "AdminRoleName";

    [UnitOfWork]
    public async Task SeedAsync(DataSeedContext context)
    {
        var roleId = (Guid)context[AdminRoleIdProperty]!;
        var adminRoleName = (string)context[AdminRoleNameProperty]!;
        //"admin" role
        var normalizedAdminRoleName = lookupNormalizer.NormalizeName(adminRoleName);
        var adminRole = await roleManager.FindByNameAsync(normalizedAdminRoleName);
        if (adminRole is null)
        {
            var spaceId = (Guid)context[PermissionSpaceDataSeedContributor.SystemPermissionSpaceIdProperty]!;
            adminRole = new Role(
                roleId,
                AuthingConstants.AdminRoleName,
                "超级管理员",
                "系统内置超级管理员",
                spaceId,
                isSystemBuiltIn: true
            );

            var adminUserId = (Guid)context[UserDataSeedContributor.AdminUserIdProperty]!;
            adminRole.AddSubject(guidGenerator.Create(), RoleSubjectType.User, adminUserId);

            (await roleManager.CreateAsync(adminRole)).CheckErrors();
        }
    }
}
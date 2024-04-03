using Microsoft.AspNetCore.Identity;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Entities.Users;

public class UserDataSeedContributor(UserManager userManager, ILookupNormalizer lookupNormalizer)
    : IDataSeedContributor, ITransientDependency
{
    public const string AdminUserNameProperty = "AdminUserName";
    public const string AdminUserIdProperty = "AdminUserId";
    public const string AdminUserPassword = "AdminUserPassword";

    public async Task SeedAsync(DataSeedContext context)
    {
        var adminUserId = (Guid)context[AdminUserIdProperty]!;
        var adminUserName = (string)context[AdminUserNameProperty]!;
        var normalizedAdminUserName = lookupNormalizer.NormalizeName(adminUserName);

        var adminUser = await userManager.FindByNameAsync(normalizedAdminUserName);
        if (adminUser is null)
        {
            adminUser = new(
                adminUserId,
                normalizedAdminUserName,
                "超级管理员",
                isSystemBuiltIn: true);

            var password = context[AdminUserPassword] as string ?? "abc123!";
            (await userManager.CreateAsync(adminUser, password)).CheckErrors();
        }
    }
}
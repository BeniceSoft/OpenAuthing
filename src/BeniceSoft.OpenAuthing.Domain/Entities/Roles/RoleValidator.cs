using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Entities.Roles;

[ExposeServices(typeof(IRoleValidator<Role>))]
public class RoleValidator : IRoleValidator<Role>, IScopedDependency
{
    public async Task<IdentityResult> ValidateAsync(RoleManager<Role> manager, Role role)
    {
        var existedRole = await manager.FindByNameAsync(role.Name);
        if (existedRole is not null && existedRole.Id != role.Id)
        {
            return IdentityResult.Failed(new IdentityError()
            {
                Code = "ExistedRoleName",
                Description = $"角色名「{role.Name}」已存在"
            });
        }
        
        return IdentityResult.Success;
    }
}
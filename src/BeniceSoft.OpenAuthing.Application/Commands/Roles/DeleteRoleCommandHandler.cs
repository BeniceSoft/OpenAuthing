using BeniceSoft.OpenAuthing.Entities.Roles;
using MediatR;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Commands.Roles;

public class DeleteRoleCommandHandler
    : IRequestHandler<DeleteRoleCommand, bool>, ITransientDependency
{
    private readonly RoleManager _roleManager;

    public DeleteRoleCommandHandler(RoleManager roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.Id.ToString());
        if (role is null) return true;
        
        if (role.IsSystemBuiltIn)
        {
            throw new UserFriendlyException("系统内置角色无法删除");
        }

        await _roleManager.DeleteAsync(role);
        return true;
    }
}
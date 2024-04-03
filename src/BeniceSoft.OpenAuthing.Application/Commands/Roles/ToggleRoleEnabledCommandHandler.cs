using BeniceSoft.OpenAuthing.Entities.Roles;
using MediatR;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Commands.Roles;

public class ToggleRoleEnabledCommandHandler
    : IRequestHandler<ToggleRoleEnabledCommand, bool>, ITransientDependency
{
    private readonly RoleManager _roleManager;

    public ToggleRoleEnabledCommandHandler(RoleManager roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<bool> Handle(ToggleRoleEnabledCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.Id.ToString());
        if (role is null) return false;
        
        role.ToggleEnabled(request.Enabled);

        await _roleManager.UpdateAsync(role);
        return true;
    }
}
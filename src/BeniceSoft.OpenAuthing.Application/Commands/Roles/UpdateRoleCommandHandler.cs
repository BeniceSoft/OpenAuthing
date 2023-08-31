using BeniceSoft.OpenAuthing.Roles;
using MediatR;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Commands.Roles;

public class UpdateRoleCommandHandler
    : IRequestHandler<UpdateRoleCommand, bool>, ITransientDependency
{
    private readonly RoleManager _roleManager;

    public UpdateRoleCommandHandler(RoleManager roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<bool> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.Id.ToString());
        if (role is null) return false;
        
        role.Update(request.Name, request.DisplayName, request.Description ?? string.Empty);

        var result = await _roleManager.UpdateAsync(role);

        return result.Succeeded;
    }
}
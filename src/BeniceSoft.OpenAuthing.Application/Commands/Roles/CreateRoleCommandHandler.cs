using BeniceSoft.OpenAuthing.Entities.Roles;
using MediatR;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace BeniceSoft.OpenAuthing.Commands.Roles;

public class CreateRoleCommandHandler
    : IRequestHandler<CreateRoleCommand, Guid>, ITransientDependency
{
    private readonly RoleManager _roleManager;
    private readonly IGuidGenerator _guidGenerator;

    public CreateRoleCommandHandler(RoleManager roleManager, IGuidGenerator guidGenerator)
    {
        _roleManager = roleManager;
        _guidGenerator = guidGenerator;
    }

    public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = new Role(_guidGenerator.Create(), request.Name, request.DisplayName, request.Description, request.PermissionSpaceId);
        var result = await _roleManager.CreateAsync(role);
        
        if (!result.Succeeded)
        {
            throw new UserFriendlyException(result.Errors.Select(x => x.Description).JoinAsString(";"));
        }

        return role.Id;
    }
}
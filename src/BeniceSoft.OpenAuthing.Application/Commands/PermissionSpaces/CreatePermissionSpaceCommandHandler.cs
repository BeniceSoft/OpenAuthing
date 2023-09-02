using BeniceSoft.OpenAuthing.PermissionSpaces;
using MediatR;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Commands.PermissionSpaces;

public class CreatePermissionSpaceCommandHandler
    : IRequestHandler<CreatePermissionSpaceCommand, Guid>, ITransientDependency
{
    private readonly PermissionSpaceManager _permissionSpaceManager;

    public CreatePermissionSpaceCommandHandler(PermissionSpaceManager permissionSpaceManager)
    {
        _permissionSpaceManager = permissionSpaceManager;
    }

    public async Task<Guid> Handle(CreatePermissionSpaceCommand request, CancellationToken cancellationToken)
    {
        return await _permissionSpaceManager.CreateAsync(request.Name, request.DisplayName, request.Description);
    }
}
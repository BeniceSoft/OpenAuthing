using BeniceSoft.OpenAuthing.Entities.Roles;
using BeniceSoft.OpenAuthing.Exceptions;
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
        var role = new Role(_guidGenerator.Create(), request.Name, request.Description );
        var result = await _roleManager.CreateAsync(role);
        
        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            throw new AuthingBizException(2000, error.Description);
        }

        return role.Id;
    }
}
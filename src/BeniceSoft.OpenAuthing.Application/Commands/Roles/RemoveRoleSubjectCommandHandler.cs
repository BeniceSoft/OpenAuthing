using BeniceSoft.OpenAuthing.Entities.Roles;
using MediatR;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Commands.Roles;

public class RemoveRoleSubjectCommandHandler
    : IRequestHandler<RemoveRoleSubjectCommand, bool>, ITransientDependency
{
    private readonly IRoleRepository _roleRepository;

    public RemoveRoleSubjectCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<bool> Handle(RemoveRoleSubjectCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetAsync(request.RoleId, cancellationToken: cancellationToken);
        role.RemoveSubject(request.SubjectId);

        await _roleRepository.UpdateAsync(role, cancellationToken: cancellationToken);

        return true;
    }
}
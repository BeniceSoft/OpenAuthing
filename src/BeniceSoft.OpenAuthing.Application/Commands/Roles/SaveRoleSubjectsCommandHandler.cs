using BeniceSoft.OpenAuthing.Entities.Roles;
using MediatR;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace BeniceSoft.OpenAuthing.Commands.Roles;

public class SaveRoleSubjectsCommandHandler(IRoleRepository roleRepository, IGuidGenerator guidGenerator) : IRequestHandler<SaveRoleSubjectsCommand, bool>, ITransientDependency
{
    public async Task<bool> Handle(SaveRoleSubjectsCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetAsync(request.RoleId, cancellationToken: cancellationToken);
        foreach (var subject in request.Subjects)
        {
            role.AddSubject(guidGenerator.Create(), subject.Type, subject.Id);
        }

        await roleRepository.UpdateAsync(role, cancellationToken: cancellationToken);

        return true;
    }
}
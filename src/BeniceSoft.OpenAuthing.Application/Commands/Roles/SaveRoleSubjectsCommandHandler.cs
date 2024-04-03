using BeniceSoft.OpenAuthing.Entities.Roles;
using MediatR;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace BeniceSoft.OpenAuthing.Commands.Roles;

public class SaveRoleSubjectsCommandHandler
    : IRequestHandler<SaveRoleSubjectsCommand, bool>, ITransientDependency
{
    private readonly IRoleRepository _roleRepository;
    private readonly IGuidGenerator _guidGenerator;

    public SaveRoleSubjectsCommandHandler(IRoleRepository roleRepository, IGuidGenerator guidGenerator)
    {
        _roleRepository = roleRepository;
        _guidGenerator = guidGenerator;
    }

    public async Task<bool> Handle(SaveRoleSubjectsCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetAsync(request.RoleId, cancellationToken: cancellationToken);
        foreach (var subject in request.Subjects)
        {
            role.AddSubject(_guidGenerator.Create(), subject.Type, subject.Id);
        }

        await _roleRepository.UpdateAsync(role, cancellationToken: cancellationToken);

        return true;
    }
}
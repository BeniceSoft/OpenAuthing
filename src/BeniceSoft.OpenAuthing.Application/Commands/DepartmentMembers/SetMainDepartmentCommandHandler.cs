using BeniceSoft.OpenAuthing.Entities.DepartmentMembers;
using MediatR;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Commands.DepartmentMembers;

public class SetMainDepartmentCommandHandler
    : IRequestHandler<SetMainDepartmentCommand, bool>, ITransientDependency
{
    private readonly IRepository<DepartmentMember> _departmentMemberRepository;

    public SetMainDepartmentCommandHandler(IRepository<DepartmentMember> departmentMemberRepository)
    {
        _departmentMemberRepository = departmentMemberRepository;
    }

    public async Task<bool> Handle(SetMainDepartmentCommand request, CancellationToken cancellationToken)
    {
        var departmentMember = await _departmentMemberRepository.GetAsync(
            x => x.DepartmentId == request.DepartmentId && x.UserId == request.UserId, 
            cancellationToken: cancellationToken);

        departmentMember.SetMain(request.IsMain);

        await _departmentMemberRepository.UpdateAsync(departmentMember);

        return true;
    }
}
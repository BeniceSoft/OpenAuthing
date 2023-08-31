using BeniceSoft.OpenAuthing.DepartmentMembers;
using MediatR;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Commands.DepartmentMembers;

public class SetLeaderCommandHandler
    : IRequestHandler<SetLeaderCommand, bool>, ITransientDependency
{
    private readonly IRepository<DepartmentMember> _departmentMemberRepository;

    public SetLeaderCommandHandler(IRepository<DepartmentMember> departmentMemberRepository)
    {
        _departmentMemberRepository = departmentMemberRepository;
    }

    public async Task<bool> Handle(SetLeaderCommand request, CancellationToken cancellationToken)
    {
        var departmentMember = await _departmentMemberRepository.GetAsync(
            x => x.DepartmentId == request.DepartmentId && x.UserId == request.UserId,
            cancellationToken: cancellationToken);

        departmentMember.SetLeader(request.IsLeader);

        await _departmentMemberRepository.UpdateAsync(departmentMember, cancellationToken: cancellationToken);

        return true;
    }
}
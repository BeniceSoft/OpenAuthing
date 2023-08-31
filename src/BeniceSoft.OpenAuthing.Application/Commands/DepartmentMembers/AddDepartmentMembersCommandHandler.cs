using BeniceSoft.OpenAuthing.DepartmentMembers;
using MediatR;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;

namespace BeniceSoft.OpenAuthing.Commands.DepartmentMembers;

public class AddDepartmentMembersCommandHandler
    : IRequestHandler<AddDepartmentMembersCommand, int>, ITransientDependency
{
    private readonly IRepository<DepartmentMember> _departmentMemberRepository;
    private readonly IAsyncQueryableExecuter _asyncExecuter;

    public AddDepartmentMembersCommandHandler(IRepository<DepartmentMember> departmentMemberRepository, IAsyncQueryableExecuter asyncExecuter)
    {
        _departmentMemberRepository = departmentMemberRepository;
        _asyncExecuter = asyncExecuter;
    }

    public async Task<int> Handle(AddDepartmentMembersCommand request, CancellationToken cancellationToken)
    {
        var queryable = await _departmentMemberRepository.GetQueryableAsync();
        var existedMembers = await _asyncExecuter.ToListAsync(queryable
            .Where(x => x.DepartmentId == request.DepartmentId)
            .Where(x => request.UserIds.Contains(x.UserId)), cancellationToken);
        var existedMemberIds = existedMembers.Select(x => x.UserId);

        var newMembers = request.UserIds.Except(existedMemberIds)
            .Select(userId => new DepartmentMember(request.DepartmentId, userId))
            .ToList();

        if (newMembers.Any())
        {
            await _departmentMemberRepository.InsertManyAsync(newMembers, cancellationToken: cancellationToken);
        }

        return newMembers.Count;
    }
}
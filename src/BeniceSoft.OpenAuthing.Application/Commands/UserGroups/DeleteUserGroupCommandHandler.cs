using BeniceSoft.OpenAuthing.Entities.UserGroups;
using MediatR;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Commands.UserGroups;

public class DeleteUserGroupCommandHandler(IRepository<UserGroup, Guid> groupRepository)
    : IRequestHandler<DeleteUserGroupCommand, bool>, ITransientDependency
{
    public async Task<bool> Handle(DeleteUserGroupCommand request, CancellationToken cancellationToken)
    {
        await groupRepository.DeleteAsync(request.Id, cancellationToken: cancellationToken);

        return true;
    }
}
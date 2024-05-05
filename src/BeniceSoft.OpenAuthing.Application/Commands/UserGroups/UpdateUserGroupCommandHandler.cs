using BeniceSoft.OpenAuthing.Entities.UserGroups;
using MediatR;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Commands.UserGroups;

public class UpdateUserGroupCommandHandler(IRepository<UserGroup, Guid> groupRepository)
    : IRequestHandler<UpdateUserGroupCommand, bool>, ITransientDependency
{
    public async Task<bool> Handle(UpdateUserGroupCommand request, CancellationToken cancellationToken)
    {
        var userGroup = await groupRepository.GetAsync(request.Id, includeDetails: false, cancellationToken: cancellationToken);
        userGroup.Update(request.Name, request.Description);

        await groupRepository.UpdateAsync(userGroup, cancellationToken: cancellationToken);

        return true;
    }
}
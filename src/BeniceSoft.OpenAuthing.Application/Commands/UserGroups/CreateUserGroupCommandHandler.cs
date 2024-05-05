using BeniceSoft.OpenAuthing.Entities.UserGroups;
using MediatR;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace BeniceSoft.OpenAuthing.Commands.UserGroups;

public class CreateUserGroupCommandHandler(IGuidGenerator guid, IRepository<UserGroup> repository)
    : IRequestHandler<CreateUserGroupCommand, Guid>, ITransientDependency
{
    public async Task<Guid> Handle(CreateUserGroupCommand request, CancellationToken cancellationToken)
    {
        var userGroup = new UserGroup(guid.Create(), request.Name, request.Description);
        await repository.InsertAsync(userGroup, cancellationToken: cancellationToken);

        return userGroup.Id;
    }
}
using MediatR;

namespace BeniceSoft.OpenAuthing.Commands.UserGroups;

public class DeleteUserGroupCommand : IRequest<bool>
{
    public Guid Id { get; private set; }

    public DeleteUserGroupCommand(Guid id)
    {
        Id = id;
    }
}
using MediatR;

namespace BeniceSoft.OpenAuthing.Commands.Roles;

public class DeleteRoleCommand : IRequest<bool>
{
    public DeleteRoleCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private set; }
}
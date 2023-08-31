using MediatR;

namespace BeniceSoft.OpenAuthing.Commands.Roles;

public class ToggleRoleEnabledCommand : IRequest<bool>
{
    public ToggleRoleEnabledCommand(Guid id, bool enabled)
    {
        Id = id;
        Enabled = enabled;
    }

    public Guid Id { get; private set; }
    public bool Enabled { get; private set; }
}
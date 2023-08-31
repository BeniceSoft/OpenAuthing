using MediatR;

namespace BeniceSoft.OpenAuthing.Commands.Roles;

public class UpdateRoleCommand : IRequest<bool>
{
    public UpdateRoleCommand(Guid id, string name, string displayName, string? description)
    {
        Id = id;
        Name = name;
        DisplayName = displayName;
        Description = description;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string DisplayName { get; private set; }
    public string? Description { get; private set; }
}
using MediatR;

namespace BeniceSoft.OpenAuthing.Commands.Roles;

public class CreateRoleCommand : IRequest<Guid>
{
    public CreateRoleCommand(string name, string displayName, string description)
    {
        Name = name;
        DisplayName = displayName;
        Description = description;
    }

    public string Name { get; private set; }
    public string DisplayName { get; private set; }
    public string Description { get; private set; }
}
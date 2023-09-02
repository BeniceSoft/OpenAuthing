using MediatR;

namespace BeniceSoft.OpenAuthing.Commands.PermissionSpaces;

public class CreatePermissionSpaceCommand : IRequest<Guid>
{
    public string Name { get; private set; }

    public string DisplayName { get; private set; }

    public string Description { get; private set; }

    public CreatePermissionSpaceCommand(string name, string displayName, string description)
    {
        Name = name;
        DisplayName = displayName;
        Description = description;
    }
}
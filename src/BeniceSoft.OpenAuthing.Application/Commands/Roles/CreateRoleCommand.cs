using MediatR;

namespace BeniceSoft.OpenAuthing.Commands.Roles;

public class CreateRoleCommand : IRequest<Guid>
{
    public CreateRoleCommand(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
}
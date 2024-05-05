using MediatR;

namespace BeniceSoft.OpenAuthing.Commands.UserGroups;

public class CreateUserGroupCommand : IRequest<Guid>
{
    public string Name { get; private set; }
    public string Description { get; private set; }

    public CreateUserGroupCommand(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
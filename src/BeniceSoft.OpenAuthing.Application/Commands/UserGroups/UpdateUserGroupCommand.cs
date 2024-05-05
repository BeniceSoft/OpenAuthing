using MediatR;

namespace BeniceSoft.OpenAuthing.Commands.UserGroups;

public class UpdateUserGroupCommand : IRequest<bool>
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    public UpdateUserGroupCommand(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}
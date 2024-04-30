using MediatR;

namespace BeniceSoft.OpenAuthing.Commands.Positions;

public class CreatePositionCommand : IRequest<Guid>
{
    public string Name { get; private set; }
    public string? Description { get; private set; }

    public CreatePositionCommand(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}
using MediatR;

namespace BeniceSoft.OpenAuthing.Commands.Positions;

public class DeletePositionCommand : IRequest<bool>
{
    public Guid Id { get; private set; }

    public DeletePositionCommand(Guid id)
    {
        Id = id;
    }
}
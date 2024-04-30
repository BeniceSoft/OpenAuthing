using BeniceSoft.OpenAuthing.Entities.Positions;
using MediatR;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace BeniceSoft.OpenAuthing.Commands.Positions;

public class CreatePositionCommandHandler(IGuidGenerator guidGenerator, IPositionRepository positionRepository)
    : IRequestHandler<CreatePositionCommand, Guid>, ITransientDependency
{
    public async Task<Guid> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
    {
        var id = guidGenerator.Create();
        var position = new Position(id, request.Name, request.Description);
        await positionRepository.InsertAsync(position, cancellationToken: cancellationToken);

        return id;
    }
}
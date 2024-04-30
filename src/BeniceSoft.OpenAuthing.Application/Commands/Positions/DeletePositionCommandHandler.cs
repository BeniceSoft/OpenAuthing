using BeniceSoft.OpenAuthing.Entities.Positions;
using MediatR;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Commands.Positions;

public class DeletePositionCommandHandler(IPositionRepository positionRepository)
    : IRequestHandler<DeletePositionCommand, bool>, ITransientDependency
{
    public async Task<bool> Handle(DeletePositionCommand request, CancellationToken cancellationToken)
    {
        await positionRepository.DeleteAsync(request.Id, cancellationToken: cancellationToken);

        return true;
    }
}
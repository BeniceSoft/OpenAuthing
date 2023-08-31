using BeniceSoft.OpenAuthing.OpenIddict.Applications;
using MediatR;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Commands.Applications;

public class CreateApplicationCommandHandler
    : IRequestHandler<CreateApplicationCommand, Guid>, ITransientDependency
{
    private readonly AmApplicationManger _applicationManager;

    public CreateApplicationCommandHandler(AmApplicationManger applicationManager)
    {
        _applicationManager = applicationManager;
    }

    public async Task<Guid> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        var applicationDescriptor = new AmApplicationDescriptor
        {
            ClientId = request.ClientId,
            DisplayName = request.DisplayName,
            ClientType = request.ClientType
        };
        var application = await _applicationManager.CreateAsync(applicationDescriptor, cancellationToken);

        return application.Id;
    }
}
using MediatR;
using Volo.Abp.DependencyInjection;
using Volo.Abp.OpenIddict.Applications;

namespace BeniceSoft.OpenAuthing.Commands.Applications;

public class CreateApplicationCommandHandler
    : IRequestHandler<CreateApplicationCommand, Guid>, ITransientDependency
{
    private readonly IAbpApplicationManager _applicationManager;

    public CreateApplicationCommandHandler(IAbpApplicationManager applicationManager)
    {
        _applicationManager = applicationManager;
    }

    public async Task<Guid> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        var applicationDescriptor = new AbpApplicationDescriptor
        {
            ClientId = request.ClientId,
            DisplayName = request.DisplayName,
            ClientType = request.ClientType
        };
        var application = (OpenIddictApplicationModel)await _applicationManager.CreateAsync(applicationDescriptor, cancellationToken);

        return application.Id;
    }
}
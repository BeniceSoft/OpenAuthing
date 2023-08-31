using MediatR;

namespace BeniceSoft.OpenAuthing.Commands.Applications;

public class CreateApplicationCommand : IRequest<Guid>
{
    public CreateApplicationCommand(string clientId, string displayName, string clientType)
    {
        ClientId = clientId;
        DisplayName = displayName;
        ClientType = clientType;
    }

    public string ClientId { get; private set; }
    
    public string DisplayName { get; private set; }
    
    public string ClientType { get; private set; }
}
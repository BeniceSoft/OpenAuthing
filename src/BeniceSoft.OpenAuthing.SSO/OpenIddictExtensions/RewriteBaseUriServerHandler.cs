using OpenIddict.Server;

namespace BeniceSoft.OpenAuthing.OpenIddictExtensions;

public class RewriteBaseUriServerHandler : IOpenIddictServerHandler<OpenIddictServerEvents.HandleConfigurationRequestContext>
{
    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.HandleConfigurationRequestContext>()
            .UseSingletonHandler<RewriteBaseUriServerHandler>()
            .SetOrder(0)
            .SetType(OpenIddictServerHandlerType.BuiltIn)
            .Build();

    private readonly IConfiguration _configuration;

    public RewriteBaseUriServerHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ValueTask HandleAsync(OpenIddictServerEvents.HandleConfigurationRequestContext context)
    {
        var url = _configuration.GetValue<string>("AppUrl");
        if (string.IsNullOrWhiteSpace(url) == false)
        {
            context.BaseUri = new(url);
        }

        return ValueTask.CompletedTask;
    }
}
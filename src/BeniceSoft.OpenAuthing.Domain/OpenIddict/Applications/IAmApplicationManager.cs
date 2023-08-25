using OpenIddict.Abstractions;

namespace BeniceSoft.OpenAuthing.OpenIddict.Applications;

public interface IAmApplicationManager : IOpenIddictApplicationManager
{
    ValueTask<string> GetClientUriAsync(object application, CancellationToken cancellationToken = default);

    ValueTask<string> GetLogoUriAsync(object application, CancellationToken cancellationToken = default);
}
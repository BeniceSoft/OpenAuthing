using OpenIddict.Abstractions;

namespace BeniceSoft.OpenAuthing.OpenIddict.Applications;

public interface IAmOpenIddictApplicationStore : IOpenIddictApplicationStore<OpenIddictApplicationModel>
{
    ValueTask<string> GetClientUriAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken = default);

    ValueTask<string> GetLogoUriAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken = default);
}
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using Volo.Abp;

namespace BeniceSoft.OpenAuthing.OpenIddict.Applications;

public class AmApplicationManger : OpenIddictApplicationManager<OpenIddictApplicationModel>, IAmApplicationManager
{
    public AmApplicationManger(
        IOpenIddictApplicationCache<OpenIddictApplicationModel> cache,
        ILogger<AmApplicationManger> logger,
        IOptionsMonitor<OpenIddictCoreOptions> options,
        IOpenIddictApplicationStoreResolver resolver)
        : base(cache, logger, options, resolver)
    {
    }

    public override async ValueTask PopulateAsync(OpenIddictApplicationDescriptor descriptor, OpenIddictApplicationModel application,
        CancellationToken cancellationToken = default)
    {
        await base.PopulateAsync(descriptor, application, cancellationToken);

        if (descriptor is AmApplicationDescriptor model)
        {
            application.ClientUri = model.ClientUri;
            application.LogoUri = model.LogoUri;
            application.ClientType = model.ClientType;
        }
    }

    public override async ValueTask PopulateAsync(OpenIddictApplicationModel application, OpenIddictApplicationDescriptor descriptor,
        CancellationToken cancellationToken = default)
    {
        await base.PopulateAsync(application, descriptor, cancellationToken);

        if (descriptor is AmApplicationDescriptor model)
        {
            application.ClientUri = model.ClientUri;
            application.LogoUri = model.LogoUri;
            application.ClientType = model.ClientType;
        }
    }

    public virtual async ValueTask<string> GetClientUriAsync(object application, CancellationToken cancellationToken = default)
    {
        Check.NotNull(application, nameof(application));
        Check.AssignableTo<IAmOpenIddictApplicationStore>(application.GetType(), nameof(application));

        return await Store.As<IAmOpenIddictApplicationStore>().GetClientUriAsync(application.As<OpenIddictApplicationModel>(), cancellationToken);
    }

    public virtual async ValueTask<string> GetLogoUriAsync(object application, CancellationToken cancellationToken = default)
    {
        Check.NotNull(application, nameof(application));
        Check.AssignableTo<IAmOpenIddictApplicationStore>(application.GetType(), nameof(application));

        return await Store.As<IAmOpenIddictApplicationStore>().GetLogoUriAsync(application.As<OpenIddictApplicationModel>(), cancellationToken);
    }
}
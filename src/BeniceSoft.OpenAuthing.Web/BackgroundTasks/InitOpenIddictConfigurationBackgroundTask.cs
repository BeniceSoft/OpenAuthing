using BeniceSoft.OpenAuthing.OpenIddictExtensions;
using OpenIddict.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace BeniceSoft.OpenAuthing.BackgroundTasks;

public class InitOpenIddictConfigurationBackgroundTask : BackgroundService
{
    private readonly IAbpLazyServiceProvider _lazyServiceProvider;

    public InitOpenIddictConfigurationBackgroundTask(IAbpLazyServiceProvider lazyServiceProvider)
    {
        _lazyServiceProvider = lazyServiceProvider;
    }

    [UnitOfWork]
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await InitApplicationsAsync(stoppingToken);
    }

    private async Task InitApplicationsAsync(CancellationToken cancellationToken)
    {
        var applicationManager = _lazyServiceProvider.LazyGetRequiredService<IOpenIddictApplicationManager>();

        if (await applicationManager.FindByClientIdAsync("linkmore-ka-gateway-admin", cancellationToken) is null)
        {
            await applicationManager.CreateAsync(new()
            {
                ClientId = "linkmore-ka-gateway-admin",
                ClientSecret = "abc123!",
                DisplayName = "LinkMore KA Gateway Admin",
                ConsentType = OpenIddictConstants.ConsentTypes.Implicit,
                RedirectUris = { new("http://localhost:5188/login"), new("http://127.0.0.1:5188/login") },
                PostLogoutRedirectUris = { new("http://localhost:5188/logout"), new("http://127.0.0.1:5188/logout") },
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.Endpoints.Logout,

                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                    DingTalkCodeGrantConstants.Permission,

                    OpenIddictConstants.Permissions.Prefixes.Scope + "api",
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Scopes.Phone,
                    OpenIddictConstants.Permissions.Scopes.Address,
                    OpenIddictConstants.Permissions.Scopes.Roles,

                    OpenIddictConstants.Permissions.ResponseTypes.Code
                }
            }, cancellationToken);
        }

        if (await applicationManager.FindByClientIdAsync("web-js", cancellationToken) is null)
        {
            await applicationManager.CreateAsync(new()
            {
                ClientId = "web-js",
                ClientSecret = "abc123!",
                DisplayName = "Web JS",
                ConsentType = OpenIddictConstants.ConsentTypes.Implicit,
                RedirectUris = { new("http://localhost:9090"), new("http://127.0.0.1:9090") },
                PostLogoutRedirectUris = { new("http://localhost:9090"), new("http://127.0.0.1:9090") },
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.Endpoints.Logout,

                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                    DingTalkCodeGrantConstants.Permission,

                    OpenIddictConstants.Permissions.Prefixes.Scope + "api",
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Scopes.Phone,
                    OpenIddictConstants.Permissions.Scopes.Address,
                    OpenIddictConstants.Permissions.Scopes.Roles,

                    OpenIddictConstants.Permissions.ResponseTypes.Code
                }
            }, cancellationToken);
        }

        if (await applicationManager.FindByClientIdAsync("linkmore-ka-am-admin", cancellationToken) is null)
        {
            await applicationManager.CreateAsync(new()
            {
                ClientId = "linkmore-ka-am-admin",
                ClientSecret = "",
                DisplayName = "LinkMore AM Admin",
                ConsentType = OpenIddictConstants.ConsentTypes.Implicit,
                RedirectUris = { new("http://localhost:8088/login") },
                PostLogoutRedirectUris = { new("http://localhost:8088/logout") },
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.Endpoints.Logout,

                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                    DingTalkCodeGrantConstants.Permission,

                    OpenIddictConstants.Permissions.Prefixes.Scope + "api",
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Scopes.Phone,
                    OpenIddictConstants.Permissions.Scopes.Address,
                    OpenIddictConstants.Permissions.Scopes.Roles,

                    OpenIddictConstants.Permissions.ResponseTypes.Code
                }
            }, cancellationToken);
        }

        if (await applicationManager.FindByClientIdAsync("postman", cancellationToken) is null)
        {
            await applicationManager.CreateAsync(new()
            {
                ClientId = "postman",
                ClientSecret = "postman-secret",
                DisplayName = "Postman",
                ConsentType = OpenIddictConstants.ConsentTypes.Implicit,
                RedirectUris = { new("https://oauth.pstmn.io/v1/callback") },
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.Token,

                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                    DingTalkCodeGrantConstants.Permission,

                    OpenIddictConstants.Permissions.Prefixes.Scope + "api",
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Scopes.Phone,
                    OpenIddictConstants.Permissions.Scopes.Address,
                    OpenIddictConstants.Permissions.Scopes.Roles,

                    OpenIddictConstants.Permissions.ResponseTypes.Code
                }
            }, cancellationToken);
        }
    }
}
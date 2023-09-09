using System.Security.Cryptography.X509Certificates;
using BeniceSoft.OpenAuthing.Localization;
using BeniceSoft.OpenAuthing.OpenIddict.Applications;
using BeniceSoft.OpenAuthing.OpenIddict.Authorizations;
using BeniceSoft.OpenAuthing.OpenIddict.Scopes;
using BeniceSoft.OpenAuthing.OpenIddict.Tokens;
using BeniceSoft.OpenAuthing.OpenIddictExtensions;
using BeniceSoft.OpenAuthing.OpenIddictExtensions.ClaimDestinations;
using BeniceSoft.OpenAuthing.Roles;
using BeniceSoft.OpenAuthing.Users;
using BeniceSoft.Abp.AspNetCore;
using BeniceSoft.Abp.AspNetCore.Localizations;
using BeniceSoft.Abp.AspNetCore.Middlewares;
using BeniceSoft.Abp.Auth;
using BeniceSoft.Abp.Auth.Core;
using BeniceSoft.Abp.Auth.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Logging;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Modularity;

namespace BeniceSoft.OpenAuthing;

[DependsOn(
    typeof(BeniceSoftAbpAspNetCoreModule),
    typeof(AbpAutofacModule),
    typeof(AbpBlobStoringFileSystemModule),
    typeof(BeniceSoftAbpAuthModule),
    typeof(EntityFrameworkCoreModule),
    typeof(ApplicationModule),
    typeof(RemoteServiceModule),
    typeof(OAuthModule)
)]
public class SsoModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(AMResource),
                typeof(DomainModule).Assembly,
                typeof(DomainSharedModule).Assembly,
                typeof(ApplicationModule).Assembly,
                typeof(ApplicationContractsModule).Assembly,
                typeof(SsoModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        IdentityModelEventSource.ShowPII = true;

        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureDefault(container =>
            {
                container.IsMultiTenant = false;
                container.UseFileSystem(fileSystem =>
                {
                    var filePath = Environment.CurrentDirectory + "/wwwroot/uploadFiles";
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    fileSystem.BasePath = filePath;
                });
            });
        });
        
        Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
        });

        // Configure<AbpAspNetCoreMvcOptions>(options =>
        // {
        //     options
        //         .ConventionalControllers
        //         .Create(typeof(ApplicationModule).Assembly);
        // });

        Configure<AbpAntiForgeryOptions>(options => { options.AutoValidate = false; });
        Configure<IdentityOptions>(options => { options.User.AllowedUserNameCharacters = ""; });
        ConfigureOpenIddict(context.Services);

        context.Services.AddDetection();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        app.UseBeniceSoftRequestLocalization();

        app.UseDetection();

        app.UseCorrelationId();

        app.UseStaticFiles();

        // 路由
        app.UseRouting();

        // 跨域
        app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

        app.UseBeniceSoftExceptionHandlingMiddleware();

        // 身份验证
        app.UseBeniceSoftAuthentication();

        // 认证授权
        app.UseBeniceSoftAuthorization();

        app.UseAuditing();

        // 路由映射
        app.UseConfiguredEndpoints(builder =>
        {
            builder.MapDefaultControllerRoute();
            // builder.MapControllerRoute(
            //     name: "AdminArea",
            //     pattern: "{area:exists}/{controller=Console}/{action=Index}/{id?}");
            // builder.MapFallbackToFile("index.html");
            builder.MapFallbackToFile("/", "index.html");
        });
    }

    private void ConfigureOpenIddict(IServiceCollection services)
    {
        var configuration = services.GetConfiguration();

        // see: https://documentation.openiddict.com/configuration/claim-destinations.html
        Configure<OpenIddictClaimDestinationsOptions>(options =>
        {
            options.ClaimDestinationsProvider.Add<DefaultOpenIddictClaimDestinationsProvider>();
        });

        services.AddBeniceSoftAuthentication();

        services
            .AddIdentity<User, Role>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Username;
                options.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;

                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;

                options.SignIn.RequireConfirmedEmail = false;
            })
            // asp.net core identity 2FA/MFA support
            // https://learn.microsoft.com/zh-cn/aspnet/core/security/authentication/mfa?view=aspnetcore-7.0#mfa-2fa
            .AddTokenProvider<AuthenticatorTokenProvider<User>>(TokenOptions.DefaultAuthenticatorProvider);

        // HttpOnly = false
        services.ConfigureApplicationCookie(options =>
        {
            // options.LoginPath = "/#/account/login";
            options.Cookie.HttpOnly = false;
        });

        services.AddOpenIddict()
            // Register the OpenIddict core components.
            .AddCore(builder =>
            {
                builder
                    .SetDefaultApplicationEntity<OpenIddictApplicationModel>()
                    .SetDefaultAuthorizationEntity<OpenIddictAuthorizationModel>()
                    .SetDefaultScopeEntity<OpenIddictScopeModel>()
                    .SetDefaultTokenEntity<OpenIddictTokenModel>();

                builder
                    .AddApplicationStore<AmOpenIddictApplicationStore>()
                    .AddAuthorizationStore<AmOpenIddictAuthorizationStore>()
                    .AddScopeStore<AmOpenIddictScopeStore>()
                    .AddTokenStore<AmOpenIddictTokenStore>();

                builder.ReplaceApplicationManager(typeof(AmApplicationManger));

                builder.Services.TryAddScoped(provider => (IAmApplicationManager)provider.GetRequiredService<IOpenIddictApplicationManager>());
            })
            // Register the OpenIddict server components.
            .AddServer(builder =>
            {
                // register claims
                builder.RegisterClaims(
                    OpenIddictConstants.Claims.Name,
                    OpenIddictConstants.Claims.Nickname,
                    OpenIddictConstants.Claims.Role,
                    OpenIddictConstants.Claims.PhoneNumber,
                    BeniceSoftAuthConstants.ClaimTypes.Avatar,
                    BeniceSoftAuthConstants.ClaimTypes.RoleId
                );

                // register scopes
                builder.RegisterScopes(
                    OpenIddictConstants.Scopes.OpenId,
                    OpenIddictConstants.Scopes.Email,
                    OpenIddictConstants.Scopes.Profile,
                    OpenIddictConstants.Scopes.Phone,
                    OpenIddictConstants.Scopes.Roles,
                    OpenIddictConstants.Scopes.Address,
                    OpenIddictConstants.Scopes.OfflineAccess
                );

                builder
                    .SetAuthorizationEndpointUris("/connect/authorize")
                    .SetIntrospectionEndpointUris("/connect/introspect")
                    .SetLogoutEndpointUris("/connect/logout")
                    .SetRevocationEndpointUris("/connect/revocat")
                    .SetTokenEndpointUris("/connect/token")
                    .SetUserinfoEndpointUris("/connect/userinfo")
                    .SetVerificationEndpointUris("/connect/verify");

                builder
                    .AllowAuthorizationCodeFlow()
                    .AllowRefreshTokenFlow()
                    .AllowCustomFlow(DingTalkCodeGrantConstants.GrantType)
                    .AllowNoneFlow();

                builder
                    .UseAspNetCore()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableTokenEndpointPassthrough()
                    .EnableUserinfoEndpointPassthrough()
                    .EnableLogoutEndpointPassthrough()
                    .EnableVerificationEndpointPassthrough()
                    .EnableStatusCodePagesIntegration()
                    .DisableTransportSecurityRequirement();

                // https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html#registering-a-certificate-recommended-for-production-ready-scenarios
                builder.AddSigningCertificate(new X509Certificate2("signing-certificate.pfx"))
                    .AddEncryptionCertificate(new X509Certificate2("encryption-certificate.pfx"));


                builder.DisableAccessTokenEncryption();

                // token 有效期
                builder.SetAccessTokenLifetime(TimeSpan.FromHours(2));
                // builder.SetAccessTokenLifetime(TimeSpan.FromMinutes(3));

                // 移除CodeVerifier的验证 使用code模式的时候 因为没有文档解释此数据的生成方式
                builder.RemoveEventHandler(OpenIddictServerHandlers.Exchange.ValidateCodeVerifier.Descriptor);
                // 移除通过授权code获取token的时候 验证重定向地址
                builder.RemoveEventHandler(OpenIddictServerHandlers.Exchange.ValidateRedirectUri.Descriptor);
            });
    }

    private List<string> GetXmlCommentsFilePath()
    {
        var basePath = PlatformServices.Default.Application.ApplicationBasePath;
        DirectoryInfo d = new DirectoryInfo(basePath);
        FileInfo[] files = d.GetFiles("*.xml");
        return files.Select(a => Path.Combine(basePath, a.FullName)).ToList();
    }
}
using BeniceSoft.OpenAuthing.Localization;
using BeniceSoft.OpenAuthing.OpenIddictExtensions;
using BeniceSoft.OpenAuthing.Roles;
using BeniceSoft.OpenAuthing.Users;
using BeniceSoft.Abp.AspNetCore;
using BeniceSoft.Abp.AspNetCore.Localizations;
using BeniceSoft.Abp.AspNetCore.Middlewares;
using BeniceSoft.Abp.Auth;
using BeniceSoft.Abp.Auth.Core;
using BeniceSoft.Abp.Auth.Extensions;
using BeniceSoft.OpenAuthing.BackgroundTasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Logging;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;

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
        PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(AuthingResource),
                typeof(DomainModule).Assembly,
                typeof(DomainSharedModule).Assembly,
                typeof(ApplicationModule).Assembly,
                typeof(ApplicationContractsModule).Assembly,
                typeof(SsoModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
#if DEBUG
        IdentityModelEventSource.ShowPII = true;
#endif
        var configuration = context.Services.GetConfiguration();

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

        Configure<RouteOptions>(options => { options.LowercaseUrls = true; });

        Configure<AbpAntiForgeryOptions>(options => { options.AutoValidate = false; });
        Configure<IdentityOptions>(options => { options.User.AllowedUserNameCharacters = ""; });

        ConfigureIdentity(context);

        ConfigureOpeniddict(context, configuration);

        context.Services.AddDetection();
        context.Services.AddHostedService<LoadEnabledExternalIdentityProvidersBackgroundTask>();
    }

    private static void ConfigureIdentity(ServiceConfigurationContext context)
    {
        context.Services.AddBeniceSoftAuthentication();

        context.Services
            .AddIdentity<User, Role>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Username;
                options.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;

                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;

                options.SignIn.RequireConfirmedEmail = false;

                options.User.RequireUniqueEmail = false;

                options.Lockout.MaxFailedAccessAttempts = int.MaxValue;
            })
            // asp.net core identity 2FA/MFA support
            // https://learn.microsoft.com/zh-cn/aspnet/core/security/authentication/mfa?view=aspnetcore-7.0#mfa-2fa
            .AddTokenProvider<AuthenticatorTokenProvider<User>>(TokenOptions.DefaultAuthenticatorProvider);

        // HttpOnly = falsec
        context.Services.ConfigureApplicationCookie(options =>
        {
            // options.LoginPath = "/#/account/login";
            options.Cookie.HttpOnly = false;
        });
    }

    private static void ConfigureOpeniddict(ServiceConfigurationContext context, IConfiguration configuration)
    {
        var appUrl = configuration.GetValue<string>("AppUrl")?.EnsureEndsWith('/') ?? string.Empty;
        context.Services.AddOpenIddict()
            .AddServer(builder =>
            {
                if (!string.IsNullOrWhiteSpace(appUrl))
                {
                    builder.SetIssuer(appUrl);
                    builder.AddEventHandler(RewriteBaseUriServerHandler.Descriptor);
                }

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
                    OpenIddictConstants.Scopes.Address
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

#if DEBUG
                builder.AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();
#else
                // https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html#registering-a-certificate-recommended-for-production-ready-scenarios
                builder.AddSigningCertificate(new System.Security.Cryptography.X509Certificates.X509Certificate2("certs/signing-certificate.pfx"))
                    .AddEncryptionCertificate(new System.Security.Cryptography.X509Certificates.X509Certificate2("certs/encryption-certificate.pfx"));
#endif

                builder.DisableAccessTokenEncryption();

                // token 有效期
                builder.SetAccessTokenLifetime(TimeSpan.FromHours(2));
                // builder.SetAccessTokenLifetime(TimeSpan.FromMinutes(3));

                // if (!string.IsNullOrWhiteSpace(appUrl))
                // {
                //     builder.AddEventHandler<OpenIddictServerEvents.HandleConfigurationRequestContext>(x =>
                //     {
                //         x.UseInlineHandler(ctx =>
                //         {
                //             ctx.BaseUri = new Uri(appUrl);
                //             return ValueTask.CompletedTask;
                //         });
                //     });
                // }

                // 移除CodeVerifier的验证 使用code模式的时候 因为没有文档解释此数据的生成方式
                builder.RemoveEventHandler(OpenIddictServerHandlers.Exchange.ValidateCodeVerifier.Descriptor);
                // 移除通过授权code获取token的时候 验证重定向地址
                builder.RemoveEventHandler(OpenIddictServerHandlers.Exchange.ValidateRedirectUri.Descriptor);
            });
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
            builder.MapFallbackToFile("/", "index.html");
        });
    }
}
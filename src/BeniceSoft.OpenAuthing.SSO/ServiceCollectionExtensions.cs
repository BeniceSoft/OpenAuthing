using BeniceSoft.Abp.Auth.Core;
using BeniceSoft.OpenAuthing.Entities.Roles;
using BeniceSoft.OpenAuthing.Entities.Users;
using BeniceSoft.OpenAuthing.Identity;
using BeniceSoft.OpenAuthing.OpenIddictExtensions;
using BeniceSoft.OpenAuthing.OpenIddictExtensions.ClaimDestinations;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using OpenIddict.Server;

namespace BeniceSoft.OpenAuthing;

internal static class ServiceCollectionExtensions
{
    // ReSharper disable UnusedMember.Local
    private const string SigningCertificateFileConfigKey = "OPENAUTHING_SIGNING_CERTIFICATE_FILE";
    private const string SigningCertificatePasswordConfigKey = "OPENAUTHING_SIGNING_CERTIFICATE_PASSWORD";
    private const string EncryptionCertificateFileConfigKey = "OPENAUTHING_ENCRYPTION_CERTIFICATE_FILE";
    private const string EncryptionCertificatePasswordConfigKey = "OPENAUTHING_ENCRYPTION_CERTIFICATE_PASSWORD";

    internal static void ConfigureIdentity(this IServiceCollection services)
    {
        services.Configure<IdentityOptions>(options => { options.User.AllowedUserNameCharacters = ""; });
        services
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
            .AddUserStore<UserStore>()
            .AddRoleStore<RoleStore>()
            .AddDefaultTokenProviders()
            // asp.net core identity 2FA/MFA support
            // https://learn.microsoft.com/zh-cn/aspnet/core/security/authentication/mfa?view=aspnetcore-8.0#mfa-2fa
            .AddTokenProvider<AuthenticatorTokenProvider<User>>(TokenOptions.DefaultAuthenticatorProvider);

        // HttpOnly = falsec
        services.ConfigureApplicationCookie(options =>
        {
            // options.LoginPath = "/#/account/login";
            options.Cookie.HttpOnly = false;
        });

        services.AddTransient<IEmailSender<User>, SmtpEmailSender>();
    }

    internal static void ConfigureOpeniddict(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpenIddictClaimDestinationsOptions>(options => { options.ClaimDestinationsProvider.Add<DefaultOpenIddictClaimDestinationsProvider>(); });

        services.AddOpenIddict(x =>
        {
            x.AddServer(builder =>
            {
                var appUrl = configuration.GetValue<string>("AppUrl")?.EnsureEndsWith('/') ?? string.Empty;
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
                var signingCertificatePath = configuration.EnsureGetValue<string>(SigningCertificateFileConfigKey);
                var signingCertificatePassword = configuration.GetValue<string>(SigningCertificatePasswordConfigKey);

                var encryptionCertificatePath = configuration.EnsureGetValue<string>(EncryptionCertificateFileConfigKey);
                var encryptionCertificatePassword = configuration.GetValue<string>(EncryptionCertificatePasswordConfigKey);

                // https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html#registering-a-certificate-recommended-for-production-ready-scenarios
                builder.AddSigningCertificate(new System.Security.Cryptography.X509Certificates.X509Certificate2(signingCertificatePath, signingCertificatePassword))
                    .AddEncryptionCertificate(new System.Security.Cryptography.X509Certificates.X509Certificate2(encryptionCertificatePath, encryptionCertificatePassword));
#endif

                builder.DisableAccessTokenEncryption();

                // token 有效期
                builder.SetAccessTokenLifetime(TimeSpan.FromHours(2));

                // 移除CodeVerifier的验证 使用code模式的时候 因为没有文档解释此数据的生成方式
                builder.RemoveEventHandler(OpenIddictServerHandlers.Exchange.ValidateCodeVerifier.Descriptor);
                // 移除通过授权code获取token的时候 验证重定向地址
                builder.RemoveEventHandler(OpenIddictServerHandlers.Exchange.ValidateRedirectUri.Descriptor);
            });
        });
    }

    internal static void ConfigureHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        // var redisOptions = new RedisStorageOptions();
        // configuration.GetSection("Hangfire:Redis").Bind(redisOptions);
        // var connectionString = configuration.GetValue<string>("Hangfire:Redis:ConnectionString");
        services.AddHangfire((sp, config) =>
        {
            config.UseInMemoryStorage();
            // config.UseRedisStorage(connectionString, redisOptions);
        });
    }
}
using BeniceSoft.Abp.AspNetCore;
using BeniceSoft.Abp.AspNetCore.Localizations;
using BeniceSoft.Abp.AspNetCore.Middlewares;
using BeniceSoft.Abp.Auth;
using BeniceSoft.Abp.Auth.Extensions;
using BeniceSoft.OpenAuthing.OpenIddict.Applications;
using BeniceSoft.OpenAuthing.OpenIddict.Authorizations;
using BeniceSoft.OpenAuthing.OpenIddict.Scopes;
using BeniceSoft.OpenAuthing.OpenIddict.Tokens;
using BeniceSoft.OpenAuthing.Roles;
using BeniceSoft.OpenAuthing.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using OpenIddict.Abstractions;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
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
    typeof(RemoteServiceModule)
)]
public class ApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
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
        ConfigureSwaggerServices(context.Services);
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
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1.0/swagger.json", "OpenAuthing API");
        });

        // 路由映射
        app.UseConfiguredEndpoints(builder =>
        {
            builder.MapDefaultControllerRoute();
        });
    }

    private void ConfigureSwaggerServices(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1.0", new OpenApiInfo { Title = "OpenAuthing API", Version = "1.0" });
            // options.DocInclusionPredicate((doc, description) => true);
            options.CustomSchemaIds(type => type.FullName);
            foreach (var item in GetXmlCommentsFilePath())
            {
                options.IncludeXmlComments(item, true);
            }

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Scheme = "Bearer",
                Description = "Specify the authorization token.",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                },
            });
        });
    }

    private void ConfigureOpenIddict(IServiceCollection services)
    {
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
using BeniceSoft.OpenAuthing.Localization;
using BeniceSoft.Abp.AspNetCore;
using BeniceSoft.Abp.AspNetCore.Filters;
using BeniceSoft.Abp.AspNetCore.Localizations;
using BeniceSoft.Abp.AspNetCore.Middlewares;
using BeniceSoft.Abp.Auth;
using BeniceSoft.Abp.Auth.Extensions;
using BeniceSoft.OpenAuthing.BackgroundTasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Logging;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.IO;
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
                    DirectoryHelper.CreateIfNotExists(filePath);

                    fileSystem.BasePath = filePath;
                });
            });
        });

        Configure<RouteOptions>(options => { options.LowercaseUrls = true; });

        Configure<AbpAntiForgeryOptions>(options => { options.AutoValidate = false; });
        
        context.Services.AddBeniceSoftAuthentication();

        context.Services.ConfigureIdentity();
        context.Services.ConfigureOpeniddict(configuration);

        context.Services.AddDetection();
        context.Services.AddHostedService<LoadEnabledExternalIdentityProvidersBackgroundTask>();
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
using BeniceSoft.OpenAuthing.Localization;
using BeniceSoft.Abp.AspNetCore;
using BeniceSoft.Abp.AspNetCore.Localizations;
using BeniceSoft.Abp.AspNetCore.Middlewares;
using BeniceSoft.OpenAuthing.BackgroundTasks;
using BeniceSoft.OpenAuthing.Middlewares;
using Hangfire;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Logging;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs.Hangfire;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.IO;
using Volo.Abp.MailKit;
using Volo.Abp.Modularity;

namespace BeniceSoft.OpenAuthing;

[DependsOn(
    typeof(BeniceSoftAbpAspNetCoreModule),
    typeof(AbpAutofacModule),
    typeof(AbpBlobStoringFileSystemModule),
    typeof(AbpBackgroundJobsHangfireModule),
    typeof(AbpMailKitModule),
    typeof(AuthingEntityFrameworkCoreModule),
    typeof(AuthingApplicationModule),
    typeof(AuthingRemoteServiceModule),
    typeof(AuthingOAuthModule)
)]
public class AuthingSsoModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(AuthingResource),
                typeof(AuthingDomainModule).Assembly,
                typeof(AuthingDomainSharedModule).Assembly,
                typeof(AuthingApplicationModule).Assembly,
                typeof(AuthingApplicationContractsModule).Assembly,
                typeof(AuthingSsoModule).Assembly);
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

        context.Services.AddJsonFormatResponse().AddDesensitizeResponse();

        context.Services.ConfigureIdentity();
        context.Services.ConfigureOpeniddict(configuration);
        context.Services.ConfigureHangfire(configuration);

        context.Services.AddDetection();
        context.Services.AddHostedService<LoadEnabledExternalIdentityProvidersBackgroundTask>();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        app.UseBeniceSoftRequestLocalization(options =>
        {
            options.DefaultRequestCulture = new("en-US", "en-US");
            options.AddSupportedCultures("en-US", "zh-CN");
            options.AddSupportedUICultures("en-US", "zh-CN");
            options.ApplyCurrentCultureToResponseHeaders = true;
            options.CultureInfoUseUserOverride = false;
            options.RequestCultureProviders =
            [
                new AcceptLanguageHeaderRequestCultureProvider(),
                new QueryStringRequestCultureProvider(),
                new CookieRequestCultureProvider(),
            ];
        });

        app.UseDetection();

        app.UseCorrelationId();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

        app.UseBeniceSoftExceptionHandlingMiddleware();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHangfireDashboard();

        // app.UseAuditing();

        app.UseDevelopmentSapProxy(env);

        // 路由映射
        app.UseConfiguredEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            endpoints.MapFallbackToFile("index.html");
        });
    }
}
using BeniceSoft.Abp.AspNetCore;
using BeniceSoft.Abp.AspNetCore.Localizations;
using BeniceSoft.Abp.AspNetCore.Middlewares;
using Microsoft.AspNetCore.Identity;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
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
    typeof(EntityFrameworkCoreModule),
    typeof(ApplicationModule),
    typeof(RemoteServiceModule)
)]
public class AdminApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
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
        Configure<IdentityOptions>(options => { options.User.AllowedUserNameCharacters = ""; });

        context.Services
            .ConfigureSwaggerServices()
            .ConfigureAuthentication(configuration)
            .AddDetection();
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

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseAuditing();
        app.UseSwagger();
        app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1.0/swagger.json", "OpenAuthing API"); });

        // 路由映射
        app.UseConfiguredEndpoints(builder => { builder.MapDefaultControllerRoute(); });
    }
}
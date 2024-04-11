using BeniceSoft.Abp.AspNetCore;
using BeniceSoft.Abp.AspNetCore.Localizations;
using BeniceSoft.Abp.AspNetCore.Middlewares;
using BeniceSoft.Abp.Auth;
using BeniceSoft.Abp.Auth.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
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
    typeof(BeniceSoftAbpAuthModule),
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
        ConfigureSwaggerServices(context.Services);
        
        context.Services.AddBeniceSoftAuthentication();

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

    private List<string> GetXmlCommentsFilePath()
    {
        var basePath = PlatformServices.Default.Application.ApplicationBasePath;
        DirectoryInfo d = new DirectoryInfo(basePath);
        FileInfo[] files = d.GetFiles("*.xml");
        return files.Select(a => Path.Combine(basePath, a.FullName)).ToList();
    }
}
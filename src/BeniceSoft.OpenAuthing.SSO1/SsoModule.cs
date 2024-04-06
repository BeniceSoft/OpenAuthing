using BeniceSoft.OpenAuthing.Components;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace BeniceSoft.OpenAuthing;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAutofacModule),
    typeof(DomainModule),
    typeof(EntityFrameworkCoreModule)
)]
public class SsoModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        context.Services.ConfigureIdentity();

        context.Services.ConfigureOpeniddict(configuration);

        // Add services to the container.
        context.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        context.Services.AddCascadingAuthenticationState();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();
        // Configure the HTTP request pipeline.
        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseCorrelationId();
        app.UseRouting();

        app.UseAuthorization();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();
        });
    }
}
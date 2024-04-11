using Microsoft.AspNetCore.Routing.Template;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Middlewares;

public class DevelopmentSapProxyMiddleware(RequestDelegate next, ILoggerFactory loggerFactory) : ISingletonDependency
{
    private readonly ILogger<DevelopmentSapProxyMiddleware> _logger = loggerFactory.CreateLogger<DevelopmentSapProxyMiddleware>();

    public static string[] ProxyRoutes =
    [
        "/account/login",
        "/account/reset-password"
    ];

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var path = (string)httpContext.Request.Path;

        if (httpContext.Request.Method == HttpMethods.Get &&
            ProxyRoutes.Contains(path, StringComparer.OrdinalIgnoreCase))
        {
            var targetUri = new Uri("http://localhost:8000" + path + httpContext.Request.QueryString);
            httpContext.Response.Redirect(targetUri.ToString());
            
            _logger.LogDebug("Redirect to: {TargetUri}", targetUri);
            return;
        }


        _logger.LogInformation("DevelopmentSapProxyMiddleware");
        await next(httpContext);
    }
}

public static class DevelopmentSapProxyMiddlewareExtensions
{
    public static IApplicationBuilder UseDevelopmentSapProxy(this IApplicationBuilder app, IHostEnvironment env)
    {
        if (env.IsDevelopment() == false) return app;
        return app.UseMiddleware<DevelopmentSapProxyMiddleware>();
    }
}
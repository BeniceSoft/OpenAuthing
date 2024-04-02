using BeniceSoft.OpenAuthing.DynamicAuth;
using Microsoft.EntityFrameworkCore;

namespace BeniceSoft.OpenAuthing.BackgroundTasks;

/// <summary>
/// 加载已启用的外部身份提供者
/// </summary>
public class LoadEnabledExternalIdentityProvidersBackgroundTask : BackgroundService
{
    private readonly ILogger<LoadEnabledExternalIdentityProvidersBackgroundTask> _logger;
    private readonly IServiceProvider _serviceProvider;

    public LoadEnabledExternalIdentityProvidersBackgroundTask(ILogger<LoadEnabledExternalIdentityProvidersBackgroundTask> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogDebug("加载已启用的身份提供者");

        using var scope = _serviceProvider.CreateScope();
        var dynamicAuthenticationManager = scope.ServiceProvider.GetRequiredService<IDynamicAuthenticationManager>();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuthingDbContext>();

        var idps = await dbContext.ExternalIdentityProviders
            .Include(x => x.Options)
            .AsNoTracking()
            .Where(x => x.Enabled)
            .ToListAsync(cancellationToken: stoppingToken);

        foreach (var idp in idps)
        {
            dynamicAuthenticationManager.Add(idp.ProviderName, idp.Name, idp.DisplayName, idp.OptionsDictionary);

            _logger.LogDebug("已加载身份提供者 {0}({1})", idp.ProviderName, idp.Name);
        }
    }
}
using BeniceSoft.OpenAuthing.DynamicAuth;
using BeniceSoft.OpenAuthing.Entities.IdentityProviders;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace BeniceSoft.OpenAuthing.BackgroundTasks;

/// <summary>
/// 加载已启用的外部身份提供者
/// </summary>
public class LoadEnabledExternalIdentityProvidersBackgroundTask : BackgroundService
{
    private readonly ILogger<LoadEnabledExternalIdentityProvidersBackgroundTask> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public LoadEnabledExternalIdentityProvidersBackgroundTask(
        ILogger<LoadEnabledExternalIdentityProvidersBackgroundTask> logger,
        IServiceProvider serviceProvider,
        IUnitOfWorkManager unitOfWorkManager)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _unitOfWorkManager = unitOfWorkManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogDebug("加载已启用的身份提供者");

        var flag = true;
        while (stoppingToken.IsCancellationRequested == false && flag)
        {
            try
            {
                using var _ = _unitOfWorkManager.Begin();
                using var scope = _serviceProvider.CreateScope();
                var dynamicAuthenticationManager = scope.ServiceProvider.GetRequiredService<IDynamicAuthenticationManager>();
                var idpRepository = scope.ServiceProvider.GetRequiredService<IRepository<ExternalIdentityProvider>>();

                var queryable = await idpRepository.WithDetailsAsync(x => x.Options);
                var idps = await queryable.AsNoTracking()
                    .Where(x => x.Enabled)
                    .ToListAsync(cancellationToken: stoppingToken);

                foreach (var idp in idps)
                {
                    dynamicAuthenticationManager.Add(idp.ProviderName, idp.Name, idp.DisplayName, idp.OptionsDictionary);

                    _logger.LogDebug("已加载身份提供者 {0}({1})", idp.ProviderName, idp.Name);
                }

                flag = false;
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "加载已启用的身份提供者失败，稍后重试");
                await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
            }
        }
    }
}
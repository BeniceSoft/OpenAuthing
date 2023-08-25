using BeniceSoft.OpenAuthing.DynamicAuth;
using BeniceSoft.OpenAuthing.IdentityProviders;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace BeniceSoft.OpenAuthing.BackgroundTasks;

public class InitializeConfiguredExternalIdPsBackgroundTask : BackgroundService
{
    private readonly ILogger<InitializeConfiguredExternalIdPsBackgroundTask> _logger;
    private readonly IDynamicAuthenticationManager _dynamicAuthenticationManager;
    private readonly IUnitOfWorkManager _uowManager;
    private readonly IRepository<ExternalIdentityProvider, Guid> _idPRepository;

    public InitializeConfiguredExternalIdPsBackgroundTask(
        ILogger<InitializeConfiguredExternalIdPsBackgroundTask> logger,
        IDynamicAuthenticationManager dynamicAuthenticationManager,
        IUnitOfWorkManager uowManager,
        IRepository<ExternalIdentityProvider, Guid> idPRepository)
    {
        _logger = logger;
        _dynamicAuthenticationManager = dynamicAuthenticationManager;
        _uowManager = uowManager;
        _idPRepository = idPRepository;
    }

    // [UnitOfWork(false)]
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("初始化已配置的外部身份提供程序");

        using var uow = _uowManager.Begin(requiresNew: true, isTransactional: false);
        var queryable = await _idPRepository.WithDetailsAsync(x => x.Options);
        var idPs = await queryable.Where(x => x.Enabled == true)
            .AsNoTracking()
            .ToListAsync(stoppingToken);
        foreach (var idp in idPs)
        {
            _dynamicAuthenticationManager.Add(idp.ProviderName, idp.Name, idp.DisplayName, idp.OptionsDictionary);

            _logger.LogInformation("\t已添加身份提供程序: [{0}] {1}", idp.ProviderName, idp.DisplayName);
        }

        await uow.CompleteAsync(stoppingToken);
    }
}
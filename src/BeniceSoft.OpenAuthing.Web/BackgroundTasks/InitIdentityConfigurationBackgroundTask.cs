using BeniceSoft.OpenAuthing.Roles;
using BeniceSoft.OpenAuthing.Users;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace BeniceSoft.OpenAuthing.BackgroundTasks;

public class InitIdentityConfigurationBackgroundTask : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IGuidGenerator _guidGenerator;
    private readonly ILogger<InitIdentityConfigurationBackgroundTask> _logger;

    public InitIdentityConfigurationBackgroundTask(IServiceProvider serviceProvider,
        IGuidGenerator guidGenerator, ILogger<InitIdentityConfigurationBackgroundTask> logger)
    {
        _serviceProvider = serviceProvider;
        _guidGenerator = guidGenerator;
        _logger = logger;
    }

    [UnitOfWork]
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await InitUsersAsync();
    }

    private async Task InitUsersAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var userManger = scope.ServiceProvider.GetRequiredService<UserManager>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager>();

        if (await roleManager.FindByNameAsync(AmConstants.AdminRoleName) is null)
        {
            var admin = new Role(_guidGenerator.Create(), AmConstants.AdminRoleName, "超级管理员", "系统内置超级管理员角色", true,
                true);
            await roleManager.CreateAsync(admin);
        }

        if (await userManger.FindByNameAsync("admin") is null)
        {
            var admin = new User(_guidGenerator.Create(), "admin", "超级管理员", "13000000000");
            await userManger.AddToRoleAsync(admin, AmConstants.AdminRoleName);
            await userManger.CreateAsync(admin, "123abc!");
        }
    }
}
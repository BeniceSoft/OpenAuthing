using BeniceSoft.Abp.Ddd.Application.Contracts;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace BeniceSoft.OpenAuthing;

[DependsOn(
    typeof(BeniceSoftAbpDddApplicationContractsModule),
    typeof(DomainSharedModule),
    typeof(AbpSettingManagementApplicationContractsModule)
)]
public class ApplicationContractsModule : AbpModule
{
}
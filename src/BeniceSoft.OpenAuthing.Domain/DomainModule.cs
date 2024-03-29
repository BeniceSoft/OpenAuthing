using BeniceSoft.Abp.Ddd.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.SettingManagement;

namespace BeniceSoft.OpenAuthing;

[DependsOn(
    typeof(BeniceSoftAbpDddDomainModule),
    typeof(DomainSharedModule),
    typeof(AbpSettingManagementDomainModule),
    typeof(AbpOpenIddictDomainModule)
)]
public class DomainModule : AbpModule
{
}
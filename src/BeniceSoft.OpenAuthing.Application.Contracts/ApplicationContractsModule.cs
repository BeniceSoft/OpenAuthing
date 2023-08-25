using BeniceSoft.Abp.Ddd.Application.Contracts;
using Volo.Abp.Modularity;

namespace BeniceSoft.OpenAuthing;

[DependsOn(
    typeof(BeniceSoftAbpDddApplicationContractsModule),
    typeof(DomainSharedModule)
)]
public class ApplicationContractsModule : AbpModule
{
}
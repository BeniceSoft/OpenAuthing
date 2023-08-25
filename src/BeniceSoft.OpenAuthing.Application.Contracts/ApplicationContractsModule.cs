using LinkMore.Abp.Ddd.Application.Contracts;
using Volo.Abp.Modularity;

namespace BeniceSoft.OpenAuthing;

[DependsOn(
    typeof(LinkMoreAbpDddApplicationContractsModule),
    typeof(DomainSharedModule)
)]
public class ApplicationContractsModule : AbpModule
{
}
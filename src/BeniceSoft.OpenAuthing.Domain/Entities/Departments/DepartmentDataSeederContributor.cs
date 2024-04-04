using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace BeniceSoft.OpenAuthing.Entities.Departments;

public class DepartmentDataSeederContributor(DepartmentManager departmentManager, IGuidGenerator guidGenerator)
    : IDataSeedContributor, ITransientDependency
{
    public async Task SeedAsync(DataSeedContext context)
    {
        const string defaultDepartmentCode = "ROOT";
        const string defaultDepartmentName = "OpenAuthing";
        if (!await departmentManager.ExistedAsync(defaultDepartmentCode))
        {
            await departmentManager.CreateAsync(new(
                guidGenerator.Create(),
                defaultDepartmentCode,
                defaultDepartmentName));
        }
    }
}
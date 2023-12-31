using BeniceSoft.OpenAuthing.Dtos.Departments;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Queries;

public interface IDepartmentQueries : ITransientDependency
{
    Task<DepartmentDto> GetByIdAsync(Guid id);
    Task<List<DepartmentDto>> GetByParentIdAsync(Guid? parentId = null);
}
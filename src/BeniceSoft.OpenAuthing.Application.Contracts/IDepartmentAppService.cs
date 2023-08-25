using BeniceSoft.OpenAuthing.Dtos.Departments.Requests;
using Volo.Abp.Application.Services;

namespace BeniceSoft.OpenAuthing;

public interface IDepartmentAppService : IApplicationService
{
    Task<List<DepartmentDto>> GetByParentIdAsync(Guid? parentId = null);

    Task<DepartmentDto> GetByIdAsync(Guid id);
}
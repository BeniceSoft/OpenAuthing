using BeniceSoft.OpenAuthing.Departments;
using BeniceSoft.OpenAuthing.Dtos.Departments.Requests;
using Volo.Abp;

namespace BeniceSoft.OpenAuthing.Services.Departments;

/// <summary>
/// 部门
/// </summary>
[RemoteService(IsMetadataEnabled = false)]
public class DepartmentAppService : AmAppServiceBase, IDepartmentAppService
{
    private readonly DepartmentStore _departmentStore;

    public DepartmentAppService(DepartmentStore departmentStore)
    {
        _departmentStore = departmentStore;
    }

    public async Task<List<DepartmentDto>> GetByParentIdAsync(Guid? parentId = null)
    {
        var queryable = await _departmentStore.GetQueryableAsync();
        var departments = await QueryableWrapperFactory.CreateWrapper(queryable
                .Where(x => x.ParentId == parentId))
            .OrderBy(x => x.Seq)
            .AsNoTracking()
            .ToListAsync();

        return ObjectMapper.Map<List<Department>, List<DepartmentDto>>(departments);
    }

    public async Task<DepartmentDto> GetByIdAsync(Guid id)
    {
        var department = await _departmentStore.GetAsync(id);
        return ObjectMapper.Map<Department, DepartmentDto>(department);
    }
}
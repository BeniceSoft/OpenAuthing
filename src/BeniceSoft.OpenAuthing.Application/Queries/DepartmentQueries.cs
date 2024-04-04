using BeniceSoft.OpenAuthing.Dtos.Departments;
using BeniceSoft.OpenAuthing.Entities.Departments;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Queries;

public class DepartmentQueries : BaseQueries, IDepartmentQueries
{
    private readonly DepartmentManager _departmentManager;

    public DepartmentQueries(IAbpLazyServiceProvider lazyServiceProvider, DepartmentManager departmentManager) : base(lazyServiceProvider)
    {
        _departmentManager = departmentManager;
    }

    public async Task<List<DepartmentDto>> GetByParentIdAsync(Guid? parentId = null)
    {
        var queryable = await _departmentManager.GetQueryableAsync();
        var departments = await QueryableWrapperFactory.CreateWrapper(queryable
                .Where(x => x.ParentId == parentId))
            .OrderBy(x => x.Seq)
            .AsNoTracking()
            .ToListAsync();

        return ObjectMapper.Map<List<Department>, List<DepartmentDto>>(departments);
    }

    public async Task<DepartmentDto> GetByIdAsync(Guid id)
    {
        var department = await _departmentManager.GetAsync(id);
        return ObjectMapper.Map<Department, DepartmentDto>(department);
    }
}
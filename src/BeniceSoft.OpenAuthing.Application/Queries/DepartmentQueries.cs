using BeniceSoft.OpenAuthing.Departments;
using BeniceSoft.OpenAuthing.Dtos.Departments;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Queries;

public class DepartmentQueries : BaseQueries, IDepartmentQueries
{
    private readonly DepartmentStore _departmentStore;

    public DepartmentQueries(IAbpLazyServiceProvider lazyServiceProvider, DepartmentStore departmentStore) : base(lazyServiceProvider)
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
using BeniceSoft.OpenAuthing.Entities.TreeServices;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace BeniceSoft.OpenAuthing.Entities.Departments;

public class DepartmentManager : ITransientDependency
{
    private readonly IRepository<Department, Guid> _departmentRepository;
    private readonly ITreeWithCodeService<Department, Guid> _treeService;

    public DepartmentManager(IRepository<Department, Guid> departmentRepository)
    {
        _departmentRepository = departmentRepository;
        _treeService = new TreeWithCodeService<Department, Guid>(departmentRepository);
    }

    public Task<IQueryable<Department>> GetQueryableAsync() => _departmentRepository.GetQueryableAsync();

    [UnitOfWork]
    public async Task CreateAsync(Department department)
    {
        var paths = await _treeService.GetNextChildCodeAsync(department.ParentId);
        department.SavePaths(paths);
        await _treeService.ValidateCodeAsync(department);
        await _departmentRepository.InsertAsync(department);
    }

    /// <summary>
    /// 更新
    /// </summary>
    [UnitOfWork]
    public async Task UpdateAsync(Department department)
    {
        await _treeService.ValidateCodeAsync(department);
        await _departmentRepository.UpdateAsync(department);
    }

    /// <summary>
    /// 删除，同时删除子集
    /// </summary>
    /// <param name="department"></param>
    [UnitOfWork]
    public async Task DeleteAsync(Department department)
    {
        var children = await _treeService.FindChildrenAsync(department.Id, true);

        foreach (var child in children)
        {
            await _departmentRepository.DeleteAsync(child);
        }

        await _departmentRepository.DeleteAsync(department);
    }

    public Task<Department> GetAsync(Guid id)
    {
        return _departmentRepository.GetAsync(id);
    }

    public Task<bool> ExistedAsync(string code)
    {
        return _departmentRepository.AnyAsync(x => x.Code == code);
    }
}
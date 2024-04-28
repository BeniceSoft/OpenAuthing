using BeniceSoft.OpenAuthing.Entities.Departments;
using MediatR;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Commands.Departments;

public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand, bool>, ITransientDependency
{
    private readonly IRepository<Department,Guid> _departmentRepository;

    public DeleteDepartmentCommandHandler(IRepository<Department, Guid> departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<bool> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        // 删除部门及子部门
        var department = await _departmentRepository.GetAsync(request.DepartmentId, cancellationToken: cancellationToken);
        if (department == null)
        {
            throw new EntityNotFoundException(typeof(Department), request.DepartmentId);
        }
        // 删除当前部门
        await _departmentRepository.DeleteAsync(department, cancellationToken: cancellationToken);
        // 删除当前部门下的子部门，使用部门上的 path 字段判断
        var children = await _departmentRepository.GetListAsync(x => x.Path.StartsWith(department.Path), cancellationToken: cancellationToken);
        foreach (var child in children)
        {
            await _departmentRepository.DeleteAsync(child, cancellationToken: cancellationToken);
        }

        return true;
    }
}
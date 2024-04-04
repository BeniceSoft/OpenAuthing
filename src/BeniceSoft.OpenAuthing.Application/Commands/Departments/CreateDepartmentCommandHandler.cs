using BeniceSoft.OpenAuthing.Entities.Departments;
using MediatR;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace BeniceSoft.OpenAuthing.Commands.Departments;

public class CreateDepartmentCommandHandler
    : IRequestHandler<CreateDepartmentCommand, Guid>, ITransientDependency
{
    private readonly IGuidGenerator _guidGenerator;
    private readonly DepartmentManager _departmentManager;

    public CreateDepartmentCommandHandler(IGuidGenerator guidGenerator, DepartmentManager departmentManager)
    {
        _guidGenerator = guidGenerator;
        _departmentManager = departmentManager;
    }

    public async Task<Guid> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = new Department(_guidGenerator.Create(), request.Code, request.Name, request.ParentId, request.Seq);
        await _departmentManager.CreateAsync(department);

        return department.Id;
    }
}
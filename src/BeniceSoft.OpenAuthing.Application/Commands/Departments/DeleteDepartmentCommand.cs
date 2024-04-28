using MediatR;

namespace BeniceSoft.OpenAuthing.Commands.Departments;

public class DeleteDepartmentCommand : IRequest<bool>
{
    public DeleteDepartmentCommand(Guid departmentId)
    {
        DepartmentId = departmentId;
    }

    public Guid DepartmentId { get; private set; }
}
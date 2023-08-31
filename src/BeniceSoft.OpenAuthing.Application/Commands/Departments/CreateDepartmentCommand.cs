using MediatR;

namespace BeniceSoft.OpenAuthing.Commands.Departments;

public class CreateDepartmentCommand : IRequest<Guid>
{
    public string Code { get;  set; }
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public int Seq { get; set; } = 0;
}
using MediatR;

namespace BeniceSoft.OpenAuthing.Commands.Departments;

public class CreateDepartmentCommand : IRequest<Guid>
{
    public CreateDepartmentCommand(string code, string name, Guid? parentId, int seq)
    {
        Code = code;
        Name = name;
        ParentId = parentId;
        Seq = seq;
    }

    public string Code { get; private set; }
    public string Name { get; private set; }
    public Guid? ParentId { get; private set; }
    public int Seq { get; private set; }
}
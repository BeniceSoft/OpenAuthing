using MediatR;

namespace BeniceSoft.OpenAuthing.Commands.DepartmentMembers;

public class AddDepartmentMembersCommand : IRequest<int>
{
    public Guid DepartmentId { get; set; }

    public List<Guid> UserIds { get; set; } = new();
}
using MediatR;

namespace BeniceSoft.OpenAuthing.Commands.DepartmentMembers;

public class SetMainDepartmentCommand : IRequest<bool>
{
    public Guid DepartmentId { get; set; }
    
    public Guid UserId { get; set; }

    public bool IsMain { get; set; }
}
using MediatR;

namespace BeniceSoft.OpenAuthing.Commands.DepartmentMembers;

public class SetLeaderCommand : IRequest<bool>
{
    public Guid DepartmentId { get; set; }
    
    public Guid UserId { get; set; }

    public bool IsLeader { get; set; }
}
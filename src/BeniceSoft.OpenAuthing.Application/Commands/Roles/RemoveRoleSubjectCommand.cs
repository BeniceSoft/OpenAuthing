using MediatR;

namespace BeniceSoft.OpenAuthing.Commands.Roles;

public class RemoveRoleSubjectCommand : IRequest<bool>
{
    public RemoveRoleSubjectCommand(Guid roleId, Guid subjectId)
    {
        RoleId = roleId;
        SubjectId = subjectId;
    }

    public Guid RoleId { get; private set; }
    public Guid SubjectId { get; private set; }
}
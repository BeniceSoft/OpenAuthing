using BeniceSoft.OpenAuthing.Dtos.Roles;
using MediatR;

namespace BeniceSoft.OpenAuthing.Commands.Roles;

public class SaveRoleSubjectsCommand : IRequest<bool>
{
    public SaveRoleSubjectsCommand(Guid roleId, List<RoleSubjectReq> subjects)
    {
        RoleId = roleId;
        Subjects = subjects;
    }

    public Guid RoleId { get; private set; }
    public List<RoleSubjectReq> Subjects { get; private set; }
}
using BeniceSoft.OpenAuthing.Dtos.Roles;

namespace BeniceSoft.OpenAuthing.Models.Roles;

public class SaveRoleSubjectsReq
{
    public List<RoleSubjectReq> Subjects { get; set; } = new();
}

using BeniceSoft.OpenAuthing.Dtos.Roles;

namespace BeniceSoft.OpenAuthing.Areas.Admin.Models.Roles;

public class SaveRoleSubjectsReq
{
    public List<RoleSubjectReq> Subjects { get; set; } = new();
}

using BeniceSoft.OpenAuthing.Enums;

namespace BeniceSoft.OpenAuthing.Areas.Admin.Models.Roles;

public class SaveRoleSubjectsReq
{
    public List<InputRoleSubjectItem> Subjects { get; set; } = new();
}

public class InputRoleSubjectItem
{
    public RoleSubjectType Type { get; set; }

    public Guid Id { get; set; }
}
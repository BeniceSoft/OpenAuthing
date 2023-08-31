using BeniceSoft.OpenAuthing.Enums;

namespace BeniceSoft.OpenAuthing.Dtos.Roles;

public class RoleSubjectReq
{
    public RoleSubjectType Type { get; set; }
    public Guid Id { get; set; }
}
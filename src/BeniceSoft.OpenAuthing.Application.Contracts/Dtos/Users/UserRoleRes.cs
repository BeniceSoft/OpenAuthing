using BeniceSoft.OpenAuthing.Enums;

namespace BeniceSoft.OpenAuthing.Dtos.Users;

public class UserRoleRes
{
    public Guid RoleId { get; set; }

    public string RoleName { get; set; }

    public string RoleDescription { get; set; }

    public RoleSubjectType AssignmentSubjectType { get; set; }
    public Guid AssignmentSubjectId { get; set; }
    public string? AssignmentSubjectName { get; set; }
}
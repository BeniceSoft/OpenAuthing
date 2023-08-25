namespace BeniceSoft.OpenAuthing.Dtos.UserGroups.Responses;

public class UserGroupRoleRes
{
    /// <summary>
    /// 角色Id
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// 角色名称
    /// </summary>
    public string RoleName { get; set; }

    public string RoleNormalizedName { get; set; }
}
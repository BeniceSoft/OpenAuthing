using System.ComponentModel;

namespace BeniceSoft.OpenAuthing.Enums;

[Description("角色主体类型")]
public enum RoleSubjectType
{
    [Description("用户")] User,
    [Description("用户组")] UserGroup
}
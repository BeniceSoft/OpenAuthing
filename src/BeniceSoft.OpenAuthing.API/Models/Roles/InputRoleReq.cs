namespace BeniceSoft.OpenAuthing.Models.Roles;

public class InputRoleReq
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string? Description { get; set; }
    public Guid PermissionSpaceId { get; set; }
}
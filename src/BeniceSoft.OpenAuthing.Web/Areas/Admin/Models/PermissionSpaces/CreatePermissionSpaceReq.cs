namespace BeniceSoft.OpenAuthing.Areas.Admin.Models.PermissionSpaces;

public class CreatePermissionSpaceReq
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; } = string.Empty;
}
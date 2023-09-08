using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Dtos.PermissionSpaces;

public class GetPermissionSpaceRes : EntityDto<Guid>
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public bool IsSystemBuiltIn { get; set; }
}
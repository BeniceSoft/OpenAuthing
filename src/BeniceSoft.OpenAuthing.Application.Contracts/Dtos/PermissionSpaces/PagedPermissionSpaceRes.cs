using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Dtos.PermissionSpaces;

public class PagedPermissionSpaceRes : EntityDto<Guid>
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
}
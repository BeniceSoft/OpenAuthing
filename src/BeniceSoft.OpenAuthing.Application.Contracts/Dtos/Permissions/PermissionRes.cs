using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Dtos.Permissions;

public class PermissionRes : EntityDto<Guid>
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public bool IsEnabled { get; set; }
}
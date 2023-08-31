using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Dtos.Roles;

public class RoleSimpleRes : EntityDto<Guid>
{
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string? Description { get; set; }

    public bool Enabled { get; set; }

    public bool IsSystemBuiltIn { get; set; }
}
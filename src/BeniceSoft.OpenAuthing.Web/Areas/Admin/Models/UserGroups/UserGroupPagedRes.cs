using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Areas.Admin.Models.UserGroups;

public class UserGroupPagedRes : EntityDto<Guid>
{
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public DateTime CreationTime { get; set; }

    public bool Enabled { get; set; }
}
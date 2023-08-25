using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace BeniceSoft.OpenAuthing.Areas.Admin.Models.UserGroups;

public class GetUserGroupRes : Entity<Guid>
{
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public DateTime CreationTime { get; set; }

    public bool Enabled { get; set; }
    
    public List<UserGroupMemberRes> Members { get; set; }
}

public class UserGroupMemberRes : EntityDto<Guid>
{
    public string Nickname { get; set; }

    public string UserName { get; set; }
    public string? Avatar { get; set; }
}
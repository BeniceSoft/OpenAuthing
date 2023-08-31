using BeniceSoft.Abp.Core.Attributes;
using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Dtos.Users;

public class UserPagedRes : EntityDto<Guid>
{
    public string UserName { get; set; }

    public string Nickname { get; set; }

    [Desensitize(Type = DesensitizeType.PhoneNumber)]
    public string? PhoneNumber { get; set; }

    public string Avatar { get; set; }

    public string EmailAddress { get; set; }

    public string JobTitle { get; set; }

    public bool Enabled { get; set; }

    public DateTime CreationTime { get; set; }

    public bool IsSystemBuiltIn { get; set; }
}
using Volo.Abp.Domain.Entities;

namespace BeniceSoft.OpenAuthing.Models.Accounts;

public class ProfileViewModel : Entity<Guid>
{
    public string UserName { get; set; }

    public string Nickname { get; set; }

    public string Avatar { get; set; }

    public string Gender { get; set; }

    public string JobTitle { get; set; }

    public string PhoneNumber { get; set; }

    public DateTime CreationTime { get; set; }
}
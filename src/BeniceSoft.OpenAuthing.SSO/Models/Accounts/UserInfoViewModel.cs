using BeniceSoft.OpenAuthing.Entities.Users;

namespace BeniceSoft.OpenAuthing.Models.Accounts;

public class UserInfoViewModel
{
    public Guid Id { get; set; }

    public string UserName { get; set; }

    public string Nickname { get; set; }

    public string? Avatar { get; set; }
}

public static class AmUserExtensions
{
    public static UserInfoViewModel ToViewModel(this User user) => new()
    {
        Id = user.Id, UserName = user.UserName, Nickname = user.Nickname, Avatar = user.Avatar
    };
}
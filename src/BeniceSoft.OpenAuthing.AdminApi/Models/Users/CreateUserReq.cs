namespace BeniceSoft.OpenAuthing.Models.Users;

public class CreateUserReq
{
    public string UserName { get; set; }

    public string PhoneNumber { get; set; }

    public string Password { get; set; }

    public bool PhoneNumberConfirmed { get; set; }
}
using MediatR;

namespace BeniceSoft.OpenAuthing.Commands.Users;

public class CreateUserCommand : IRequest<Guid>
{
    public CreateUserCommand(string userName, string phoneNumber, string password, bool phoneNumberConfirmed)
    {
        UserName = userName;
        PhoneNumber = phoneNumber;
        Password = password;
        PhoneNumberConfirmed = phoneNumberConfirmed;
    }

    public string UserName { get; private set; }

    public string PhoneNumber { get; private set; }

    public string Password { get; private set; }

    public bool PhoneNumberConfirmed { get; private set; }
}
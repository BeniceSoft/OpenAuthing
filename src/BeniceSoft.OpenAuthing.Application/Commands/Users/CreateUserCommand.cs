using MediatR;

namespace BeniceSoft.OpenAuthing.Commands.Users;

public class CreateUserCommand : IRequest<Guid>
{
    public string UserName { get; set; }

    public string PhoneNumber { get; set; }

    public string Password { get; set; }

    public bool PhoneNumberConfirmed { get; set; }
}
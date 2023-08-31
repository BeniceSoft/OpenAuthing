using BeniceSoft.OpenAuthing.Users;
using MediatR;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace BeniceSoft.OpenAuthing.Commands.Users;

public class CreateUserCommandHandler 
    : IRequestHandler<CreateUserCommand, Guid>, ITransientDependency
{
    private readonly UserManager _userManager;
    private readonly IGuidGenerator _guidGenerator;

    public CreateUserCommandHandler(UserManager userManager, IGuidGenerator guidGenerator)
    {
        _userManager = userManager;
        _guidGenerator = guidGenerator;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User(_guidGenerator.Create(), request.UserName, request.PhoneNumber, request.PhoneNumberConfirmed);
        await _userManager.CreateAsync(user, request.Password);

        return user.Id;
    }
}
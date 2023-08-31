using BeniceSoft.OpenAuthing.Users;
using MediatR;
using Volo.Abp.DependencyInjection;

namespace BeniceSoft.OpenAuthing.Commands.Users;

public class UpdateUserAvatarCommandHandler
    : IRequestHandler<UpdateUserAvatarCommand, bool>, ITransientDependency
{
    private readonly IUserRepository _userRepository;

    public UpdateUserAvatarCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(UpdateUserAvatarCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(request.UserId, cancellationToken: cancellationToken);

        user.UpdateAvatar(request.AvatarFileUrl);

        await _userRepository.UpdateAsync(user, cancellationToken: cancellationToken);
        
        return true;
    }
}
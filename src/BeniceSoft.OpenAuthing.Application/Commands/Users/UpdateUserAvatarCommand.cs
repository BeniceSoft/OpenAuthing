using MediatR;

namespace BeniceSoft.OpenAuthing.Commands.Users;

public class UpdateUserAvatarCommand : IRequest<bool>
{
    public UpdateUserAvatarCommand(Guid userId, string avatarFileUrl)
    {
        UserId = userId;
        AvatarFileUrl = avatarFileUrl;
    }

    public Guid UserId { get; private set; }
    public string AvatarFileUrl { get; private set; }
}
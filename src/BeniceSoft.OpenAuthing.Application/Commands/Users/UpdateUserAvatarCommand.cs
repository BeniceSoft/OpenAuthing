using MediatR;
using Microsoft.AspNetCore.Http;

namespace BeniceSoft.OpenAuthing.Commands.Users;

public class UpdateUserAvatarCommand : IRequest<string>
{
    public IFormFile File { get; set; }
}
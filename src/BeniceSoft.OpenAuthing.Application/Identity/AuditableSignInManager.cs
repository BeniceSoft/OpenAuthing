using BeniceSoft.OpenAuthing.LoginLogs;
using BeniceSoft.OpenAuthing.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Guids;
using Volo.Abp.Timing;
using Wangkanai.Detection.Services;

namespace BeniceSoft.OpenAuthing.Identity;

public class AuditableSignInManager : SignInManager<User>
{
    private readonly IGuidGenerator _guidGenerator;
    private readonly IClock _clock;
    private readonly IDetectionService _detectionService;

    public AuditableSignInManager(UserManager<User> userManager, IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<User> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<User>> logger,
        IAuthenticationSchemeProvider schemes, IUserConfirmation<User> confirmation, IDetectionService detectionService, IGuidGenerator guidGenerator,
        IClock clock)
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
        _detectionService = detectionService;
        _guidGenerator = guidGenerator;
        _clock = clock;
    }

    public override async Task<SignInResult> PasswordSignInAsync(User user, string password, bool isPersistent, bool lockoutOnFailure)
    {
        var result = await base.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        if (result.Succeeded)
        {
            var log = new LoginLog(_guidGenerator.Create(), user.Id, _clock.Now, Context.Connection.RemoteIpAddress?.ToString(),
                _detectionService.Platform.Name.ToString(), _detectionService.Device.Type.ToString(), _detectionService.Browser.Name.ToString(),
                _detectionService.Engine.Name.ToString(), _detectionService.UserAgent.ToString());
            
            // TODO: insert into database
        }

        return result;
    }
}
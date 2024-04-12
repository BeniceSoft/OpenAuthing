using BeniceSoft.Abp.Core.Exceptions;
using BeniceSoft.OpenAuthing.Entities.Users;
using BeniceSoft.OpenAuthing.Exceptions;
using BeniceSoft.OpenAuthing.Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace BeniceSoft.OpenAuthing.Controllers;

[ApiExplorerSettings(GroupName = "auth")]
[Route("api/[controller]/[action]")]
public abstract class AuthControllerBase : AbpController
{
    protected SignInManager<User> SignInManager => LazyServiceProvider.LazyGetRequiredService<SignInManager<User>>();
    protected UserManager UserManager => LazyServiceProvider.LazyGetRequiredService<UserManager>();
    protected IConfiguration Configuration => LazyServiceProvider.LazyGetRequiredService<IConfiguration>();

    protected string AppUrl => Configuration.GetValue<string>("AppUrl")!;

    protected AuthControllerBase()
    {
        LocalizationResource = typeof(AuthingResource);
    }

    protected void ThrowUnauthorizedIfUserIsNull(User? user, string? message = null)
    {
        if (user is null)
        {
            throw new NoAuthorizationException();
        }
    }

    protected void ThrowLocalizedAuthingBizException(int errorCode, params object[] @params)
    {
        var name = errorCode.ToString();
        var message = ErrorMessageLocalizer[name, @params];
        throw new AuthingBizException(errorCode, message);
    }

    private IStringLocalizer? _errorMessageLocalizer = null;
    protected IStringLocalizer ErrorMessageLocalizer => _errorMessageLocalizer ??= StringLocalizerFactory.Create<AuthingErrorResource>();
}
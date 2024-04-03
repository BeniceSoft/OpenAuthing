using BeniceSoft.OpenAuthing.Entities.Users;
using BeniceSoft.OpenAuthing.Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace BeniceSoft.OpenAuthing.Controllers;

[ApiExplorerSettings(GroupName = "auth")]
[Route("api/[controller]/[action]")]
public abstract class AuthControllerBase : AbpController
{
    protected SignInManager<User> SignInManager => LazyServiceProvider.LazyGetRequiredService<SignInManager<User>>();
    protected UserManager UserManager => LazyServiceProvider.LazyGetRequiredService<UserManager>();

    protected AuthControllerBase()
    {
        LocalizationResource = typeof(AuthingResource);
    }
}
using BeniceSoft.OpenAuthing.Localization;
using BeniceSoft.OpenAuthing.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace BeniceSoft.OpenAuthing.Controllers;

[Route("api/[controller]/[action]")]
public abstract class AmControllerBase : AbpController
{
    protected SignInManager<User> SignInManager => LazyServiceProvider.LazyGetRequiredService<SignInManager<User>>();
    protected UserManager UserManager => LazyServiceProvider.LazyGetRequiredService<UserManager>();

    protected AmControllerBase()
    {
        LocalizationResource = typeof(AMResource);
    }
}
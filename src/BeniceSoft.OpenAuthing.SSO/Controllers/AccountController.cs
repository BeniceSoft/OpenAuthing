using System.Net;
using System.Text.Encodings.Web;
using BeniceSoft.OpenAuthing.Models.Accounts;
using BeniceSoft.Abp.Core.Extensions;
using BeniceSoft.Abp.Core.Models;
using BeniceSoft.OpenAuthing.Entities.IdentityProviders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Controllers;

[Authorize]
public partial class AccountController : AuthControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly UrlEncoder _urlEncoder;
    private readonly IRepository<ExternalIdentityProvider, Guid> _idPRepository;
    private readonly IBlobContainer _blobContainer;
    public AccountController(ILogger<AccountController> logger, UrlEncoder urlEncoder, IRepository<ExternalIdentityProvider, Guid> idPRepository, IBlobContainer blobContainer)
    {
        _logger = logger;
        _urlEncoder = urlEncoder;
        _idPRepository = idPRepository;
        _blobContainer = blobContainer;
    }

    [HttpGet, AllowAnonymous]
    [Route("/account/login")]
    public IActionResult Login(string? returnUrl = null)
    {
        if (!string.IsNullOrEmpty(returnUrl))
        {
            returnUrl = _urlEncoder.Encode(returnUrl);
        }
#if DEBUG
        return Redirect($"{Request.Scheme}://{Request.Host}/#/account/login?returnUrl={returnUrl}");
#endif
        return Redirect($"/#/account/login?returnUrl={returnUrl}");
    }

    // POST: /account/login
    [HttpPost]
    [AllowAnonymous]
    // [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        model.ReturnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user is null)
            {
                return Ok(new ResponseResult(HttpStatusCode.BadRequest, L["IncorrectUsernameOrPassword"]));
            }

            var result = await SignInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

            if (result.Succeeded)
            {
                if (!Url.IsLocalUrl(model.ReturnUrl))
                {
                    model.ReturnUrl = "/";
                }

                return Ok(new
                {
                    LoginSuccess = true,
                    ReturnUrl = model.ReturnUrl ?? "/",
                    UserInfo = user.ToViewModel()
                }.ToSucceed());
            }

            // 被锁定
            if (result.IsLockedOut)
            {
                return Ok(new ResponseResult(HttpStatusCode.Locked, L["AccountIsLockedOut"]));
            }

            if (result.RequiresTwoFactor)
            {
                return Ok(new
                {
                    result.RequiresTwoFactor,
                    model.ReturnUrl,
                    model.RememberMe
                }.ToSucceed());
            }
        }

        return Ok(new ResponseResult(HttpStatusCode.BadRequest, L["IncorrectUsernameOrPassword"]));
    }
}
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

#if DEBUG
    [HttpGet, AllowAnonymous, Route("/account/login")]
    public IActionResult Login(string? returnUrl = null)
    {
        if (!string.IsNullOrEmpty(returnUrl))
        {
            returnUrl = _urlEncoder.Encode(returnUrl);
        }

        return Redirect($"http://localhost:8000/account/login?returnUrl={returnUrl}");
    }
#endif
}
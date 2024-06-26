using BeniceSoft.OpenAuthing.Models.Accounts;
using BeniceSoft.Abp.Core.Extensions;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace BeniceSoft.OpenAuthing.Controllers;

public partial class AccountController
{
    // GET: /api/account/me
    [HttpGet]
    public async Task<IActionResult> Me()
    {
        var user = await UserManager.GetUserAsync(User);
        if (user is null)
        {
            return Unauthorized();
        }

        return Ok(user.ToViewModel().ToSucceed());
    }

    // GET: /api/account/profile
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var user = await UserManager.GetUserAsync(User);
        if (user is null)
        {
            return Unauthorized();
        }

        return Ok(user.Adapt<ProfileViewModel>().ToSucceed());
    }

    // PUT: /api/account/uploadavatar
    [HttpPut]
    public async Task<IActionResult> UploadAvatar([FromForm] UploadAvatarViewModel req)
    {
        var user = await UserManager.GetUserAsync(User);
        if (user is null)
        {
            return Unauthorized();
        }

        await using var stream = req.File.OpenReadStream();
        var fileName = Clock.Now.ToString("yyyyMMddHHmmss") + ".jpg";
        var fullFileName = $"avatars/{user.Id}/{fileName}";
        await _blobContainer.SaveAsync(fullFileName, stream);

        var avatar = $"/uploadFiles/host/default/{fullFileName}";
        user.UpdateAvatar(avatar);

        await UserManager.UpdateAsync(user);

        return Ok(avatar.ToSucceed());
    }
}
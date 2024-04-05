using System.Collections.Concurrent;
using BeniceSoft.OpenAuthing.Entities.Users;
using BeniceSoft.OpenAuthing.Misc;
using Microsoft.AspNetCore.Identity;

namespace BeniceSoft.OpenAuthing.Middlewares;

/// <summary>
/// 在 Blazor Server 中使用 SignInManager.PasswordSignInAsync 等需要写入 Cookie 的操作时会抛出异常
/// 因为链接已经升级成了 WebSocket，提示 Headers are read-only, response has already started.
/// 所以使用此办法，具体可查看：
/// 1. https://github.com/dotnet/aspnetcore/issues/13601#issuecomment-679870698
/// 2. https://github.com/dotnet/aspnetcore/issues/34095
/// </summary>
public class BlazorCookieLoginMiddleware(RequestDelegate next, ILoggedInUserTemporaryStore store)
{
    public async Task Invoke(HttpContext context, SignInManager<User> signInManager)
    {
        if ("/account/login".Equals(context.Request.Path, StringComparison.OrdinalIgnoreCase))
        {
            if (context.Request.Query.TryGetValue("token", out var key)
                && !string.IsNullOrWhiteSpace(key))
            {
                var loginInfo = await store.PopAsync(key!);
                if (loginInfo is not null)
                {
                    var returnUrl = loginInfo.ReturnUrl ?? "/";
                    var result = await signInManager.PasswordSignInAsync(loginInfo.User, loginInfo.Password, loginInfo.RememberMe, false);

                    if (result.Succeeded)
                    {
                        context.Response.Redirect(returnUrl);
                        return;
                    }

                    if (result.RequiresTwoFactor)
                    {
                        //TODO: redirect to 2FA razor component
                        context.Response.Redirect($"/account/loginwith2fa/?ReturnUrl={returnUrl}" + key);
                        return;
                    }
                }


                //TODO: Proper error handling
                context.Response.Redirect("/account/loginfailed");
                return;
            }
        }

        await next.Invoke(context);
    }
}
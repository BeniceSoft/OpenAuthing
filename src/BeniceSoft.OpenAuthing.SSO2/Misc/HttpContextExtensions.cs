namespace BeniceSoft.OpenAuthing.Misc;

public static class HttpContextExtensions
{
    public static string? GetReturnUrl(this HttpContext httpContext)
    {
        httpContext.Request.Query.TryGetValue("ReturnUrl", out var returnUrl);

        return returnUrl;
    }
}
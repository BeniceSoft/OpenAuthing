using OpenIddict.Abstractions;

namespace BeniceSoft.OpenAuthing.OpenIddictExtensions;

public class ExtensionGrantContext
{
    public HttpContext HttpContext { get; set; }
    public OpenIddictRequest Request { get; set; }

    public ExtensionGrantContext(HttpContext httpContext, OpenIddictRequest request)
    {
        HttpContext = httpContext;
        Request = request;
    }
}
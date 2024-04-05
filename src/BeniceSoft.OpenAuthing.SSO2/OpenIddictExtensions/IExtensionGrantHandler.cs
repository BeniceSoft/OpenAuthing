using Microsoft.AspNetCore.Mvc;

namespace BeniceSoft.OpenAuthing.OpenIddictExtensions;

public interface IExtensionGrantHandler
{
    string Name { get; }

    Task<IActionResult> HandleAsync(ExtensionGrantContext context);
}
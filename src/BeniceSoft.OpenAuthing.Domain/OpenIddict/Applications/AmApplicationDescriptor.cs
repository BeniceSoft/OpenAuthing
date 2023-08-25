using OpenIddict.Abstractions;

namespace BeniceSoft.OpenAuthing.OpenIddict.Applications;

public class AmApplicationDescriptor : OpenIddictApplicationDescriptor
{
    /// <summary>
    /// URI to further information about client.
    /// </summary>
    public string ClientUri { get; set; }

    /// <summary>
    /// URI to client logo.
    /// </summary>
    public string LogoUri { get; set; }

    /// <summary>
    /// 应用类型
    /// </summary>
    public string ClientType { get; set; }
}
using OpenIddict.Abstractions;

namespace BeniceSoft.OpenAuthing.OpenIddictExtensions;

public static class OpenIddictRequestExtensions
{
    public static bool IsDingTalkCodeGrantType(this OpenIddictRequest request)
        => request != null
            ? string.Equals(request.GrantType, DingTalkCodeGrantConstants.GrantType, StringComparison.Ordinal)
            : throw new ArgumentNullException(nameof(request));
}
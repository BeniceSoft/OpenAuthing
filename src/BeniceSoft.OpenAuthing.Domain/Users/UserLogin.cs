using Microsoft.AspNetCore.Identity;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace BeniceSoft.OpenAuthing.Users;

public sealed class UserLogin : Entity
{
    /// <summary>
    /// Gets or sets the of the primary key of the user associated with this login.
    /// </summary>
    public Guid UserId { get; protected set; }

    /// <summary>
    /// Gets or sets the login provider for the login (e.g. facebook, google)
    /// </summary>
    public string LoginProvider { get; protected set; }

    /// <summary>
    /// Gets or sets the unique provider identifier for this login.
    /// </summary>
    public string ProviderKey { get; protected set; }

    /// <summary>
    /// Gets or sets the friendly name used in a UI for this login.
    /// </summary>
    public string ProviderDisplayName { get; protected set; }

    private UserLogin()
    {
    }

    internal UserLogin(
        Guid userId,
        string loginProvider,
        string providerKey,
        string providerDisplayName)
    {
        Check.NotNull(loginProvider, nameof(loginProvider));
        Check.NotNull(providerKey, nameof(providerKey));

        UserId = userId;
        LoginProvider = loginProvider;
        ProviderKey = providerKey;
        ProviderDisplayName = providerDisplayName;
    }

    internal UserLogin(
        Guid userId,
        UserLoginInfo login)
        : this(
            userId,
            login.LoginProvider,
            login.ProviderKey,
            login.ProviderDisplayName)
    {
    }

    public UserLoginInfo ToUserLoginInfo()
    {
        return new UserLoginInfo(LoginProvider, ProviderKey, ProviderDisplayName);
    }

    public override object[] GetKeys()
    {
        return new object[] { UserId, LoginProvider };
    }
}
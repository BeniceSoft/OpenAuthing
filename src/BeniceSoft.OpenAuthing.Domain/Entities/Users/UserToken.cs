using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace BeniceSoft.OpenAuthing.Entities.Users;

public class UserToken : Entity
{
    /// <summary>
    /// Gets or sets the primary key of the user that the token belongs to.
    /// </summary>
    public Guid UserId { get; protected set; }

    /// <summary>
    /// Gets or sets the LoginProvider this token is from.
    /// </summary>
    public string LoginProvider { get; protected set; }

    /// <summary>
    /// Gets or sets the name of the token.
    /// </summary>
    public string Name { get; protected set; }

    /// <summary>
    /// Gets or sets the token value.
    /// </summary>
    public string Value { get; set; }

    protected UserToken()
    {
    }

    protected internal UserToken(
        Guid userId,
        string loginProvider,
        string name,
        string value)
        : this()
    {
        Check.NotNull(loginProvider, nameof(loginProvider));
        Check.NotNull(name, nameof(name));

        UserId = userId;
        LoginProvider = loginProvider;
        Name = name;
        Value = value;
    }

    public override object[] GetKeys()
    {
        return new object[] { UserId, LoginProvider, Name };
    }
}
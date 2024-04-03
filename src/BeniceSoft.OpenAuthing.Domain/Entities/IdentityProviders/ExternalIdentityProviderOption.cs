using Volo.Abp.Domain.Entities;

namespace BeniceSoft.OpenAuthing.Entities.IdentityProviders;

public class ExternalIdentityProviderOption : Entity<Guid>
{
    public string Key { get; private set; }

    public string Value { get; private set; }

    private ExternalIdentityProviderOption(Guid id)
        : base(id)
    {
    }

    public ExternalIdentityProviderOption(Guid id, string key, string value) 
        : this(id)
    {
        Key = key;
        Value = value;
    }
}
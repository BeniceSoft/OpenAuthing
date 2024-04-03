using Volo.Abp.Domain.Entities.Auditing;

namespace BeniceSoft.OpenAuthing.Entities.IdentityProviders;

/// <summary>
/// 外部身份提供者
/// </summary>
public class ExternalIdentityProvider : FullAuditedAggregateRoot<Guid>
{
    /// <summary>
    /// 提供者名称
    /// </summary>
    public string ProviderName { get; private set; }

    /// <summary>
    /// 名称(唯一)
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 显示名称
    /// </summary>
    public string DisplayName { get; private set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; private set; }

    /// <summary>
    /// 配置
    /// </summary>
    public IReadOnlyCollection<ExternalIdentityProviderOption> Options => _options;

    private readonly List<ExternalIdentityProviderOption> _options;

    private ExternalIdentityProvider(Guid id)
        : base(id)
    {
        _options = new();

        Enabled = true;
    }

    public ExternalIdentityProvider(Guid id, string providerName, string name, string displayName, List<ExternalIdentityProviderOption> options)
        : this(id)
    {
        ProviderName = providerName;
        Name = name;
        DisplayName = displayName;
        _options = options;
    }
    
    public IReadOnlyDictionary<string, string> OptionsDictionary => _options.ToDictionary(x => x.Key, x => x.Value);

    public void SetEnabled(bool enabled)
    {
        Enabled = enabled;
    }
}
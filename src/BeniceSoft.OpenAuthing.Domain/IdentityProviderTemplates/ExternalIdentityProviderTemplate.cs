using Volo.Abp.Domain.Entities;

namespace BeniceSoft.OpenAuthing.IdentityProviderTemplates;

/// <summary>
/// 外部身份提供者模板
/// </summary>
public class ExternalIdentityProviderTemplate : AggregateRoot<Guid>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// logo
    /// </summary>
    public string Logo { get; private set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// 字段
    /// </summary>
    public IReadOnlyCollection<ExternalIdentityProviderTemplateField> Fields => _fields;

    private readonly List<ExternalIdentityProviderTemplateField> _fields;

    private ExternalIdentityProviderTemplate(Guid id) : base(id)
    {
        _fields = new();
    }

    public ExternalIdentityProviderTemplate(Guid id, string name, string logo, string title, string description,
        List<ExternalIdentityProviderTemplateField> fields)
        : this(id)
    {
        Name = name;
        Logo = logo;
        Title = title;
        Description = description;

        _fields = fields;
    }
}
using Volo.Abp.Domain.Entities;

namespace BeniceSoft.OpenAuthing.Entities.IdentityProviderTemplates;

public class ExternalIdentityProviderTemplateField : Entity<Guid>
{
    /// <summary>
    /// 字段名
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 字段显示名
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 站位文本
    /// </summary>
    public string Placeholder { get; private set; }

    /// <summary>
    /// 是否必填
    /// </summary>
    public bool Required { get; private set; }

    /// <summary>
    /// 输入类型
    /// </summary>
    public string Type { get; private set; }

    /// <summary>
    /// 帮助文本
    /// </summary>
    public string HelpText { get; private set; }

    /// <summary>
    /// 扩展数据
    /// </summary>
    public string ExtraData { get; private set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order { get; private set; }

    private ExternalIdentityProviderTemplateField(Guid id) : base(id)
    {
    }

    public ExternalIdentityProviderTemplateField(Guid id, string name, string label, string placeholder, bool required, string type, string helpText,
        string extraData, int order)
        : this(id)
    {
        Name = name;
        Label = label;
        Placeholder = placeholder;
        Required = required;
        Type = type;
        HelpText = helpText;
        ExtraData = extraData;
        Order = order;
    }
}
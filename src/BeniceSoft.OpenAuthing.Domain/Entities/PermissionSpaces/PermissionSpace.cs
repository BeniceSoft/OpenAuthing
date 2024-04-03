using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace BeniceSoft.OpenAuthing.Entities.PermissionSpaces;

/// <summary>
/// 权限空间
/// </summary>
public class PermissionSpace : FullAuditedAggregateRoot<Guid>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 归一化名称
    /// </summary>
    public string NormalizedName { get; private set; }

    /// <summary>
    /// 显示名
    /// </summary>
    public string DisplayName { get; private set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// 是否系统内置
    /// </summary>
    public bool IsSystemBuiltIn { get; private set; }

    private PermissionSpace(Guid id) : base(id)
    {
    }

    public PermissionSpace(Guid id, string name, string displayName, string description, bool isSystemBuiltIn = false)
        : this(id)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        Name = name;
        DisplayName = displayName;
        Description = description;
        IsSystemBuiltIn = isSystemBuiltIn;
    }

    public void SetNormalizedName(string normalizedName)
    {
        NormalizedName = normalizedName;
    }
}
using Volo.Abp.Domain.Entities.Auditing;

namespace BeniceSoft.OpenAuthing.Entities.GeneralResources;

/// <summary>
/// 常规资源
/// </summary>
public class GeneralResource : FullAuditedAggregateRoot<Guid>
{
    /// <summary>
    /// 权限空间id
    /// </summary>
    public Guid PermissionSpaceId { get; private set; }

    /// <summary>
    /// 标识
    /// </summary>
    public string Code { get; private set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 路径
    /// </summary>
    public string Path { get; private set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// 操作
    /// </summary>
    public IReadOnlyCollection<GeneralResourceAction> Actions => _actions;

    private readonly List<GeneralResourceAction> _actions;

    private GeneralResource(Guid id) : base(id)
    {
        _actions = new();
    }

    public GeneralResource(Guid id, Guid permissionSpaceId, string code, string name, string path, string? description,
        List<GeneralResourceAction> actions)
        : this(id)
    {
        _actions = actions;
        PermissionSpaceId = permissionSpaceId;
        Code = code;
        Name = name;
        Path = path;
        Description = description;
    }
}
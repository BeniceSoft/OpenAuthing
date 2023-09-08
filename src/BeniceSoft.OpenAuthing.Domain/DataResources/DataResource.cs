using Volo.Abp.Domain.Entities.Auditing;

namespace BeniceSoft.OpenAuthing.DataResources;

/// <summary>
/// 数据资源
/// </summary>
public class DataResource : FullAuditedAggregateRoot<Guid>
{
    public Guid PermissionSpaceId { get; private set; }
    public string Type { get; private set; }
    public string Code { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public HashSet<string> Actions { get; private set; }
}
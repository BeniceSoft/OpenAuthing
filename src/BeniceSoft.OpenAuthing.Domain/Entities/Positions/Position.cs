using Volo.Abp.Domain.Entities;

namespace BeniceSoft.OpenAuthing.Entities.Positions;

/// <summary>
/// Position
/// </summary>
public sealed class Position : AggregateRoot<Guid>
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    private Position(Guid id) : base(id)
    {
    }

    public Position(Guid id, string name, string? description) : this(id)
    {
        Name = name;
        Description = description;
    }
}
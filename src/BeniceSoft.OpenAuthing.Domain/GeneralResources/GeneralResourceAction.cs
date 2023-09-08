using Volo.Abp.Domain.Entities;

namespace BeniceSoft.OpenAuthing.GeneralResources;

public class GeneralResourceAction : Entity<Guid>
{
    public string Code { get; private set; }
    public string? Description { get; private set; }

    private GeneralResourceAction(Guid id) : base(id)
    {
    }

    public GeneralResourceAction(Guid id, string code, string? description)
        : this(id)
    {
        Code = code;
        Description = description;
    }
}
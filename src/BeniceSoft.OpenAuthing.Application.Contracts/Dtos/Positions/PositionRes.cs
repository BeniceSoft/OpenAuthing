using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Dtos.Positions;

public class PositionRes : EntityDto<Guid>
{
    public string Name { get; set; }
    public string? Description { get; set; }
}
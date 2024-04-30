using System.ComponentModel.DataAnnotations;

namespace BeniceSoft.OpenAuthing.Dtos.Positions;

public class InputPositionReq
{
    [Required]
    public string Name { get; set; }
    
    public string? Description { get; set; }
}
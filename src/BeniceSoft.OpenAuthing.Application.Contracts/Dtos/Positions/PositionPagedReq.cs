namespace BeniceSoft.OpenAuthing.Dtos.Positions;

public class PositionPagedReq
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public string? SearchKey { get; set; }
}
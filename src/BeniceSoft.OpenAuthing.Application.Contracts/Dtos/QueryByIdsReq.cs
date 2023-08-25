namespace BeniceSoft.OpenAuthing.Dtos;

public class QueryByIdsReq
{
    public List<Guid> Ids { get; set; } = new();
}
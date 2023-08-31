namespace BeniceSoft.OpenAuthing.Dtos.Users;

public class UserPagedReq
{
    public string? SearchKey { get; set; }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public Guid? ExcludeDepartmentId { get; set; }

    public bool OnlyEnabled { get; set; } = false;
}
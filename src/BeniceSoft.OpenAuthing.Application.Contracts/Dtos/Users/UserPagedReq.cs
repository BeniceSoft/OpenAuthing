namespace BeniceSoft.OpenAuthing.Dtos.Users;

public class UserPagedReq : BaseQueryReq
{

    public Guid? ExcludeDepartmentId { get; set; }

    public bool OnlyEnabled { get; set; } = false;
}
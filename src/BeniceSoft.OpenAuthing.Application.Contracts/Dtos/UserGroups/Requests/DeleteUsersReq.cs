namespace BeniceSoft.OpenAuthing.Dtos.UserGroups.Requests;

public class DeleteUsersReq
{
    /// <summary>
    /// 
    /// </summary>
    public List<Guid> UserIds { get; set; } = new List<Guid>();
}
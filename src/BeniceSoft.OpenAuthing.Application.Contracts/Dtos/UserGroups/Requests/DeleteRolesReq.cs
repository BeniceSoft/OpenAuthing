namespace BeniceSoft.OpenAuthing.Dtos.UserGroups.Requests;

public class DeleteRolesReq
{
    /// <summary>
    /// 
    /// </summary>
    public List<Guid> RoleIds { get; set; } = new List<Guid>();
}